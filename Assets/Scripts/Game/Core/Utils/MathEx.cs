using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utils
{
	public static class MathEx
	{
		//
		// Summary:
		//     The infamous 3.14159265358979... value (Read Only).
		public const float PI = 3.14159274F;
		//
		// Summary:
		//     A representation of positive infinity (Read Only).
		public const float Infinity = float.PositiveInfinity;
		//
		// Summary:
		//     A representation of negative infinity (Read Only).
		public const float NegativeInfinity = float.NegativeInfinity;
		//
		// Summary:
		//     Degrees-to-radians conversion constant (Read Only).
		public const float Deg2Rad = 0.0174532924F;
		//
		// Summary:
		//     Radians-to-degrees conversion constant (Read Only).
		public const float Rad2Deg = 57.29578F;

		const float floatEpsilon = 1e-6f;
		const double doubleEpsilon = 1e-12;

		public static int Clamp(int x, int min, int max)
		{
			return Math.Max(min, Math.Min(x, max));
		}

		public static float Clamp(float x, float min, float max)
		{
			return Math.Max(min, Math.Min(x, max));
		}

		public static float Clamp01(float x)
		{
			return Math.Max(0.0f, Math.Min(x, 1.0f));
		}

		public static float Frac(float x)
		{
			return x - (int)x;
		}

		internal static int RoundToInt(float v)
		{
			return (int)Math.Round(v);
		}

		internal static long RoundToLong(float v)
		{
			return (long)Math.Round(v);
		}

		public static float Lerp(float min, float max, float k)
		{
			return (max - min) * k + min;
		}

		public static int FloorToInt(float k)
		{
			return (int)k;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		public static T RandomWeight<T>(IList<T> items, Random random, Func<T, float> getWeight) where T : class
		{
			if (items == null) return null;

			var summ = items.Sum(getWeight);

			if (summ <= 0) return null;

			var randValue = random.RandomRange(0.0f, summ);

			var totalWeight = 0.0f;
			foreach (var item in items)
			{
				totalWeight += getWeight(item);
				if (totalWeight >= randValue) return item;
			}

			return null;
		}

		public static bool EqualsEpsilon(float a, float b)
		{
			return Math.Abs(a - b) < floatEpsilon;
		}

		public static bool EqualsEpsilon(double a, double b)
		{
			return Math.Abs(a - b) < doubleEpsilon;
		}
	}
}
