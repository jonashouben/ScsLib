using ScsLib.Map.Converter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ScsLib.Map.Reader
{
	public class BinarySerializer : IBinarySerializer
	{
		class BinaryProperty
		{
			public PropertyInfo Property { get; set; } = default!;
			public BinaryPositionAttribute? Position { get; set; }
			public BinaryFixedArrayAttribute? FixedArray { get; set; }
		}

		private readonly Dictionary<Type, IReadOnlyCollection<BinaryProperty>> _typeProperties;
		private readonly ITokenConverter _tokenConverter;

		public BinarySerializer(ITokenConverter tokenConverter)
		{
			_typeProperties = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(row => row.GetCustomAttribute<BinarySerializableAttribute>() != null)
				.ToDictionary<Type, Type, IReadOnlyCollection<BinaryProperty>>(row => row, row => row
					.GetProperties()
					.Select(r => new BinaryProperty
					{
						Property = r,
						Position = r.GetCustomAttribute<BinaryPositionAttribute>(),
						FixedArray = r.GetCustomAttribute<BinaryFixedArrayAttribute>()
					})
					.Where(r => r.Position != null)
					.OrderBy(r => r.Position!.Position)
					.ToArray());
			_tokenConverter = tokenConverter;
		}

		public T Deserialize<T>(BinaryReader reader) where T : new()
		{
			return (T) DeserializeAsyncInternal(typeof(T), reader);
		}

		public IEnumerable<T> DeserializeMany<T>(BinaryReader reader, uint count) where T : new()
		{
			for (int i = 0; i < count; i++)
			{
				yield return Deserialize<T>(reader);
			}
		}

		private object DeserializeAsyncInternalSimple(TypeCode typeCode, BinaryReader reader)
		{
			switch (typeCode)
			{
				case TypeCode.Byte:
					return reader.ReadByte();
				case TypeCode.Double:
					return reader.ReadDouble();
				case TypeCode.Int16:
					return reader.ReadInt16();
				case TypeCode.Int32:
					return reader.ReadInt32();
				case TypeCode.Int64:
					return reader.ReadInt64();
				case TypeCode.Single:
					return reader.ReadSingle();
				case TypeCode.UInt16:
					return reader.ReadUInt16();
				case TypeCode.UInt32:
					return reader.ReadUInt32();
				case TypeCode.UInt64:
					return reader.ReadUInt64();
				default:
					throw new NotSupportedException($"TypeCode {typeCode} not supported!");
			}
		}

		private object DeserializeAsyncInternal(Type type, BinaryReader reader)
		{
			if (_typeProperties.TryGetValue(type, out IReadOnlyCollection<BinaryProperty>? properties))
			{
				object obj = Activator.CreateInstance(type)!;

				foreach (BinaryProperty property in properties)
				{
					Type propertyType = property.Property.PropertyType;
					TypeCode propertyTypeCode = Type.GetTypeCode(propertyType);

					switch (propertyTypeCode)
					{
						case TypeCode.Object:
							if (typeof(IEnumerable).IsAssignableFrom(propertyType))
							{
								int length = 0;

								if (property.FixedArray != null)
								{
									length = property.FixedArray.Length;
								}
								else
								{
									length = reader.ReadInt32();
								}

								Type propertyElementType = propertyType.GetGenericArguments().First();
								Array array = Array.CreateInstance(propertyElementType, length);

								for (int i = 0; i < length; i++)
								{
									array.SetValue(DeserializeAsyncInternal(propertyElementType, reader), i);
								}

								property.Property.SetValue(obj, array);

								break;
							}
							else
							{
								if (propertyType == typeof(Token))
								{
									property.Property.SetValue(obj, _tokenConverter.FromToken(reader.ReadUInt64()));
								}
								else
								{
									property.Property.SetValue(obj, DeserializeAsyncInternal(propertyType, reader));
								}
								break;
							}
						default:
							property.Property.SetValue(obj, DeserializeAsyncInternalSimple(propertyTypeCode, reader));
							break;
					}
				}

				return obj;
			}
			else
			{
				return DeserializeAsyncInternalSimple(Type.GetTypeCode(type), reader);
			}
		}
	}
}
