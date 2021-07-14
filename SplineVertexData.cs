using System;
using UnityEngine;

///<summary>
/// This Script access the Vertex Data for each Point of Interpolation
///<summary>
namespace hazza.Splines
{
	[Serializable]
	public struct SplineVertexData
	{
		public SplineVertexData(Vector3 point)
		{
			this.point = point;
			this.normal = Vector3.up;
			this.direction = Vector3.forward;
			this.bitangent = Vector3.Cross(this.normal, this.direction);
		}

		public SplineVertexData(Vector3 point, Vector3 normal, Vector3 direction)
		{
			this.point = point;
			this.normal = normal;
			this.direction = direction;
			this.bitangent = Vector3.Cross(normal, direction);
		}

		public Vector3 point; // The Position of of Each Point
		public Vector3 normal; // The Normal Position of each Point
		public Vector3 direction; // The Direction of Each Point
		public Vector3 bitangent; // The Line of which that goes through 2 Spline Points
	}
}
