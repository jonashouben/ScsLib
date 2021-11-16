using ScsLib.Converter;
using ScsLib.Map;
using ScsLib.Map.SectorItem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScsLib.Reader
{
	public class BinarySerializer : IBinarySerializer
	{
		class BinaryProperty
		{
			public PropertyInfo Property { get; set; } = default!;
			public BinaryPositionAttribute? Position { get; set; }
			public BinaryFixedArrayAttribute? FixedArray { get; set; }
			public BinaryDynamicArrayAttribute? DynamicArray { get; set; }
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
						FixedArray = r.GetCustomAttribute<BinaryFixedArrayAttribute>(),
						DynamicArray = r.GetCustomAttribute<BinaryDynamicArrayAttribute>()
					})
					.Where(r => r.Position != null)
					.OrderBy(r => r.Position!.Position)
					.ToArray());
			_tokenConverter = tokenConverter;
		}

		public T Deserialize<T>(BinaryReader reader)
		{
			return (T)DeserializeInternal(typeof(T), reader);
		}

		public IEnumerable<T> DeserializeMany<T>(BinaryReader reader, uint count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return Deserialize<T>(reader);
			}
		}

		private object DeserializeInternalSimple(Type type, BinaryReader reader)
		{
			switch (Type.GetTypeCode(type))
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
				case TypeCode.String:
					ulong length = reader.ReadUInt64();
					return Encoding.UTF8.GetString(reader.ReadBytes((int) length));
				case TypeCode.Object:
					if (type == typeof(Token))
					{
						return _tokenConverter.FromToken(reader.ReadUInt64());
					}
					else if (type == typeof(SignOverrideAttribute))
					{
						SignOverrideAttribute attribute = new SignOverrideAttribute
						{
							Type = Deserialize<SignOverrideAttributeType>(reader),
							Index = Deserialize<uint>(reader)
						};

						switch (attribute.Type)
						{
							case SignOverrideAttributeType.Byte:
								attribute.Value = Deserialize<byte>(reader);
								break;
							case SignOverrideAttributeType.Integer:
								attribute.Value = Deserialize<int>(reader);
								break;
							case SignOverrideAttributeType.UnsignedInteger:
								attribute.Value = Deserialize<uint>(reader);
								break;
							case SignOverrideAttributeType.Float:
								attribute.Value = Deserialize<float>(reader);
								break;
							case SignOverrideAttributeType.String:
								attribute.Value = Deserialize<string>(reader);
								break;
							case SignOverrideAttributeType.UnsignedLong:
								attribute.Value = Deserialize<ulong>(reader);
								break;
						}

						return attribute;
					}
					else if (type == typeof(TriggerAction))
					{
						TriggerAction action = new TriggerAction
						{
							Name = Deserialize<Token>(reader)
						};

						uint numberParameters = Deserialize<uint>(reader);

						if (numberParameters != 4294967295)
						{
							action.NumberParameters = DeserializeMany<float>(reader, numberParameters).ToArray();
							action.StringParameters = DeserializeMany<string>(reader, Deserialize<uint>(reader)).ToArray();
							action.TargetTags = DeserializeMany<Token>(reader, Deserialize<uint>(reader)).ToArray();
							action.TargetRange = Deserialize<float>(reader);
							action.Flags = Deserialize<uint>(reader);
						}

						return action;
					}
					else if (type == typeof(AbstractSectorItem))
					{
						SectorItemType sectorItemType = Deserialize<SectorItemType>(reader);

						switch (sectorItemType)
						{
							case SectorItemType.City:
								return Deserialize<CitySectorItem>(reader);
							case SectorItemType.Road:
								return Deserialize<RoadSectorItem>(reader);
							case SectorItemType.CutPlane:
								return Deserialize<CutPlaneSectorItem>(reader);
							case SectorItemType.Prefab:
								PrefabSectorItem prefab = Deserialize<PrefabSectorItem>(reader);

								prefab.Nodes = DeserializeMany<PrefabNode>(reader, (uint) prefab.NodeUids.Count).ToArray();
								prefab.SemaphoreProfile = Deserialize<Token>(reader);

								return prefab;
							case SectorItemType.Sign:
								SignSectorItem sign = Deserialize<SignSectorItem>(reader);

								if (!string.IsNullOrWhiteSpace(sign.OverrideTemplate))
								{
									sign.Overrides = DeserializeMany<SignOverride>(reader, reader.ReadUInt32()).ToArray();
								}

								return sign;
							case SectorItemType.MapArea:
								return Deserialize<MapAreaSectorItem>(reader);
							case SectorItemType.TrafficArea:
								return Deserialize<TrafficAreaSectorItem>(reader);
							case SectorItemType.TrajectoryItem:
								return Deserialize<TrajectorySectorItem>(reader);
							case SectorItemType.Trigger:
								TriggerSectorItem trigger = Deserialize<TriggerSectorItem>(reader);

								if (trigger.NodeUids.Count == 1)
								{
									trigger.Range = Deserialize<float>(reader);
								}

								return trigger;
							case SectorItemType.Company:
								return Deserialize<CompanySectorItem>(reader);
							case SectorItemType.Service:
								return Deserialize<ServiceSectorItem>(reader);
							case SectorItemType.MapOverlay:
								return Deserialize<MapOverlaySectorItem>(reader);
							case SectorItemType.Garage:
								return Deserialize<GarageSectorItem>(reader);
							case SectorItemType.Services:
								return Deserialize<ServicesSectorItem>(reader);
							default:
								throw new NotSupportedException($"SectorItemType {sectorItemType} not supported!");
						}
					}

					break;
			}

			throw new NotSupportedException($"Type {type.Name} not supported!");
		}

		private object DeserializeInternal(Type type, BinaryReader reader)
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
								int fixedLength = 0;

								if (property.FixedArray != null)
								{
									length = property.FixedArray.Length;
									fixedLength = length;
								}
								else
								{
									if (property.DynamicArray == null) throw new ArgumentException("Dynamic Array without Attribute!");

									length = (int)Convert.ChangeType(DeserializeInternalSimple(property.DynamicArray.LengthType, reader), typeof(int), CultureInfo.InvariantCulture);
									
									if (property.DynamicArray.FixedLength != 0)
									{
										fixedLength = property.DynamicArray.FixedLength;
									}
									else
									{
										fixedLength = length;
									}

									if (length > fixedLength)
									{
										length = fixedLength;
									}
								}

								Type propertyElementType = propertyType.GetGenericArguments().First();
								Array array = Array.CreateInstance(propertyElementType, length);

								for (int i = 0; i < fixedLength; i++)
								{
									object value = DeserializeInternal(propertyElementType, reader);

									if (i < length)
									{
										array.SetValue(value, i);
									}
								}

								property.Property.SetValue(obj, array);

								break;
							}
							else
							{
								property.Property.SetValue(obj, DeserializeInternal(propertyType, reader));
								break;
							}
						default:
							property.Property.SetValue(obj, DeserializeInternalSimple(propertyType, reader));
							break;
					}
				}

				return obj;
			}
			else
			{
				return DeserializeInternalSimple(type, reader);
			}
		}
	}
}
