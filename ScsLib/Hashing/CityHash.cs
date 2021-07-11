using System;
using System.Text;

namespace ScsLib.Hashing
{
	public static class CityHash
	{
		private const ulong K0 = 0xc3a5c85c97cb3127UL;
		private const ulong K1 = 0xb492b66fbe98f273UL;
		private const ulong K2 = 0x9ae16a3b2f90404fUL;
		private const ulong K3 = 0xc949d7c7509e6557UL;
		private const ulong KMul = 0x9ddfea08eb382d69UL;

		public static ulong CityHash64(string str)
		{
			return CityHash64(Encoding.UTF8.GetBytes(str), str.Length);
		}

		#region internal hashing
		private static ulong Rotate(ulong val, int shift)
		{
			return shift == 0 ? val : RotateByAtleast1(val, shift);
		}

		private static ulong RotateByAtleast1(ulong val, int shift)
		{
			return (val >> shift) | (val << (64 - shift));
		}

		private static void Swap(ref ulong left, ref ulong right)
		{
			ulong temp = left;
			left = right;
			right = temp;
		}

		private static ulong ShiftMix(ulong val)
		{
			return val ^ (val >> 47);
		}

		private static uint Fetch32(byte[] data, int pos = 0)
		{
			return BitConverter.ToUInt32(data, pos);
		}

		private static ulong Fetch64(byte[] data, int pos = 0)
		{
			return BitConverter.ToUInt64(data, pos);
		}

		private static ulong Hash128To64((ulong, ulong) x)
		{
			ulong a = (x.Item1 ^ x.Item2) * KMul;
			a ^= a >> 47;
			ulong b = (x.Item2 ^ a) * KMul;
			b ^= b >> 47;
			b *= KMul;
			return b;
		}

		private static ulong HashLen0To16(byte[] s, int len)
		{
			if (len > 8)
			{
				ulong a = Fetch64(s);
				ulong b = Fetch64(s, len - 8);
				return Hash128To64((a, RotateByAtleast1(b + (ulong)len, len))) ^ b;
			}

			if (len >= 4)
			{
				ulong a = Fetch32(s);
				return Hash128To64(((ulong)len + (a << 3), Fetch32(s, len - 4)));
			}

			if (len > 0)
			{
				byte a = s[0];
				byte b = s[len >> 1];
				byte c = s[len - 1];
				uint y = a + ((uint)b << 8);
				ulong z = (ulong)len + ((uint)c << 2);
				return ShiftMix((y * K2) ^ (z * K3)) * K2;
			}

			return K2;
		}

		private static ulong HashLen17To32(byte[] s, int len)
		{
			ulong a = Fetch64(s) * K1;
			ulong b = Fetch64(s, 8);
			ulong c = Fetch64(s, len - 8) * K2;
			ulong d = Fetch64(s, len - 16) * K0;
			return Hash128To64((Rotate(a - b, 43) + Rotate(c, 30) + d,
							 a + Rotate(b ^ K3, 20) - c + (ulong)len));
		}

		private static ulong HashLen33To64(byte[] s, int len)
		{
			ulong z = Fetch64(s, 24);
			ulong a = Fetch64(s) + (((ulong)len + Fetch64(s, len - 16)) * K0);
			ulong b = Rotate(a + z, 52);
			ulong c = Rotate(a, 37);
			a += Fetch64(s, 8);
			c += Rotate(a, 7);
			a += Fetch64(s, 16);
			ulong vf = a + z;
			ulong vs = b + Rotate(a, 31) + c;
			a = Fetch64(s, 16) + Fetch64(s, len - 32);
			z = Fetch64(s, len - 8);
			b = Rotate(a + z, 52);
			c = Rotate(a, 37);
			a += Fetch64(s, len - 24);
			c += Rotate(a, 7);
			a += Fetch64(s, len - 16);
			ulong wf = a + z;
			ulong ws = b + Rotate(a, 31) + c;
			ulong r = ShiftMix(((vf + ws) * K2) + ((wf + vs) * K0));
			return ShiftMix((r * K0) + vs) * K2;
		}

		private static (ulong, ulong) WeakHashLen32WithSeeds(ulong w, ulong x, ulong y, ulong z, ulong a, ulong b)
		{
			a += w;
			b = Rotate(b + a + z, 21);
			ulong c = a;
			a += x;
			a += y;
			b += Rotate(a, 44);

			return (a + z, b + c);
		}

		private static (ulong, ulong) WeakHashLen32WithSeeds(byte[] s, int len, ulong a, ulong b)
		{
			return WeakHashLen32WithSeeds(Fetch64(s, len),
										  Fetch64(s, len + 8),
										  Fetch64(s, len + 16),
										  Fetch64(s, len + 24),
										  a,
										  b);
		}
		#endregion

		private static ulong CityHash64(byte[] s, int len)
		{
			if (len <= 16) return HashLen0To16(s, len);
			if (len <= 32) return HashLen17To32(s, len);
			if (len <= 64) return HashLen33To64(s, len);

			ulong x = Fetch64(s, len - 40);
			ulong y = Fetch64(s, len - 16) + Fetch64(s, len - 56);
			ulong z = Hash128To64((Fetch64(s, len - 48) + (ulong)len, Fetch64(s, len - 24)));
			var v = WeakHashLen32WithSeeds(s, len - 64, (ulong)len, z);
			var w = WeakHashLen32WithSeeds(s, len - 32, y + K1, x);
			x = (x * K1) + Fetch64(s);

			int pos = 0;
			len = (len - 1) & ~63;
			do
			{
				x = Rotate(x + y + v.Item1 + Fetch64(s, pos + 8), 37) * K1;
				y = Rotate(y + v.Item2 + Fetch64(s, pos + 48), 42) * K1;
				x ^= w.Item2;
				y += v.Item1 + Fetch64(s, pos + 40);
				z = Rotate(z + w.Item1, 33) * K1;
				v = WeakHashLen32WithSeeds(s, pos, v.Item2 * K1, x + w.Item1);
				w = WeakHashLen32WithSeeds(s, pos + 32, z + w.Item2, y + Fetch64(s, pos + 16));
				Swap(ref z, ref x);

				pos += 64;
				len -= 64;

			} while (len != 0);

			return Hash128To64((Hash128To64((v.Item1, w.Item1)) + (ShiftMix(y) * K1) + z,
							 Hash128To64((v.Item2, w.Item2)) + x));
		}
	}
}
