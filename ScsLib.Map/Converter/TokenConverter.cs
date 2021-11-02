using System;
using System.Collections.Generic;
using System.Text;

namespace ScsLib.Map.Converter
{
	public class TokenConverter : ITokenConverter
	{
		private readonly char[] _letters;

		public TokenConverter()
		{
			_letters = new[]
			{
				'\0', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_'
			};
		}

		public Token FromToken(ulong token)
		{
			StringBuilder stringBuilder = new StringBuilder();

			foreach (char c in ToChars(token))
			{
				stringBuilder.Append(c);
			}

			return new Token
			{
				Value = token,
				StringValue = stringBuilder.ToString()
			};
		}

		public Token FromString(string stringValue)
		{
			stringValue = stringValue.ToLowerInvariant();

			ulong res = 0;

			for (int i = 0; i < stringValue.Length; i++)
			{
				res += (ulong)Math.Pow(_letters.Length, i) * (ulong)GetIdChar(stringValue[i]);
			}

			return new Token
			{
				Value = res,
				StringValue = stringValue
			};
		}

		private int GetIdChar(char letter)
		{
			int index = Array.IndexOf(_letters, letter);

			return index == -1 ? 0 : index;
		}

		private IEnumerable<char> ToChars(ulong token)
		{
			while (token != 0)
			{
				ulong remaining = token % (ulong) _letters.Length;

				yield return _letters[remaining];

				token /= (ulong) _letters.Length;
			}
		}
	}
}
