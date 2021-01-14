using System;
using UnityEngine;

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

		public Vector3 point;
		public Vector3 normal;
		public Vector3 direction;
		public Vector3 bitangent;
	}
}
