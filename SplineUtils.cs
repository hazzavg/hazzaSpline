using System;
using System.Collections.Generic;
using UnityEngine;

namespace hazza.Splines
{
	public static class SplineUtils
	{
		public static List<Vector3> ToEvenPointArray(Spline spline, float spacing, bool includeEdge = false)
		{
			float num = 0f;
			float a = 0.0001f;
			float num2 = 0f;
			List<Vector3> list = new List<Vector3>
			{
				spline.GetPoint(0f)
			};
			Vector3 b = spline.GetPoint(0f);
			Vector3 vector = Vector3.zero;
			while (num < 1f)
			{
				while (num2 < spacing && num < 1f)
				{
					num += Mathf.Min(a, 1f - num);
					vector = spline.GetPoint(num);
					num2 += (vector - b).magnitude;
					b = vector;
				}
				if (num2 >= spacing || includeEdge)
				{
					list.Add(vector);
				}
				num2 -= spacing;
			}
			return list;
		}

		public static List<Vector3> ToEvenPointArray(Spline spline, int count)
		{
			count = Mathf.Max(count, 2);
			if (count == 2)
			{
				return new List<Vector3>
				{
					spline.GetPoint(0f),
					spline.GetPoint(1f)
				};
			}
			return SplineUtils.ToEvenPointArray(spline, spline.GetLength() / (float)(count - 1), false);
		}

		public static List<SplineVertexData> ToEvenSplinePointArray(Spline spline, float spacing)
		{
			float num = 0f;
			float a = 0.0001f;
			float num2 = 0f;
			List<SplineVertexData> list = new List<SplineVertexData>
			{
				new SplineVertexData(spline.GetPoint(0f), spline.GetNormal(0f), spline.GetDirection(0))
			};
			Vector3 b = spline.GetPoint(0f);
			Vector3 vector = Vector3.zero;
			while (num < 1f)
			{
				while (num2 < spacing && num < 1f)
				{
					num += Mathf.Min(a, 1f - num);
					vector = spline.GetPoint(num);
					num2 += (vector - b).magnitude;
					b = vector;
				}
				num2 -= spacing;
				list.Add(new SplineVertexData(spline.GetPoint(num), spline.GetNormal(num), spline.GetDirection(num)));
			}
			return list;
		}

		public static List<SplineVertexData> ToEvenSplinePointArray(Spline spline, int count)
		{
			count = Mathf.Max(count, 2);
			if (count == 2)
			{
				return new List<SplineVertexData>
				{
					new SplineVertexData(spline.GetPoint(0f), spline.GetNormal(0f), spline.GetDirection(0)),
					new SplineVertexData(spline.GetPoint(1f), spline.GetNormal(1f), spline.GetDirection(1))
				};
			}
			return SplineUtils.ToEvenSplinePointArray(spline, spline.GetLength() / (float)(count - 1));
		}
	}
}
