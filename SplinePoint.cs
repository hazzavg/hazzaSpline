using System;
using UnityEngine;

///<summary>
/// This Script determines the Various Interpretation Points
///<summary>
namespace hazza.Splines
{
	[Serializable]
	public class SplinePoint
	{
		public SplinePoint(Vector3 point)
		{
			this.point = point;
			this.normal = Vector3.up;
		}

		public SplinePoint(Vector3 point, Vector3 normal)
		{
			this.point = point;
			this.normal = normal;
		}

		public Vector3 point;
		public Vector3 normal;
	}
}
