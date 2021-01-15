using System;
using UnityEngine;

///<summary>
/// This Script determine the Various Associations of the Spline Interpolation
/// by accessing Variables from other Scripts to carry out the Interpolation
/// ---------------------
/// In this regard, Spline Interpolation uses Polynomials to Oscillate between Various Points; to ensure a Smooth Gameplay Experience to any Framework
///<summary>
namespace hazza.Splines
{
	public class Spline : MonoBehaviour
	{
		public int CurveCount
		{
			get
			{
				return (this.points.Length - 1) / 3; 
			}
		}

		public int ControlPointCount
		{
			get
			{
				return this.points.Length; 
			}
		}

		public bool Closed
		{
			get
			{
				return this.closed; 
			}
			set
			{
				this.closed = value;
				if (value)
				{
					this.points[this.points.Length - 1].point = this.points[0].point;
					this.points[this.points.Length - 1].normal = this.points[0].normal;
					Array.Resize<TangentMode>(ref this.tangentModes, this.CurveCount);
					this.tangentModes[this.tangentModes.Length - 1] = TangentMode.Aligned;
					return;
				}
				Array.Resize<TangentMode>(ref this.tangentModes, this.CurveCount - 1); 
				this.RemoveLastCurve(); 
			}
		}

		public void Reset()
		{
			this.points = new SplinePoint[]
			{
				new SplinePoint(Vector3.forward * 1f, Vector3.up),
				new SplinePoint(Vector3.forward * 2f, Vector3.up),
				new SplinePoint(Vector3.forward * 3f, Vector3.up),
				new SplinePoint(Vector3.forward * 4f, Vector3.up)
			};
			this.tangentModes = new TangentMode[0];
		}

		public Vector3 GetPoint(float t)
		{
			int num;
			if (t >= 1f)
			{
				t = 1f;
				num = this.points.Length - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * (float)this.CurveCount; 
				num = (int)t;
				t -= (float)num; 
				num *= 3;
			}
			return base.transform.TransformPoint(Spline.GetPoint(this.points[num].point, this.points[num + 1].point, this.points[num + 2].point, this.points[num + 3].point, t));
		}

		public Vector3 GetVelocity(float t)
		{
			int num;
			if (t >= 1f)
			{
				t = 1f;
				num = this.points.Length - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * (float)this.CurveCount; 
				num = (int)t; 
				t -= (float)num; 
				num *= 3;
			}
			return base.transform.TransformDirection(Spline.GetDerivative(this.points[num].point, this.points[num + 1].point, this.points[num + 2].point, this.points[num + 3].point, t)); 
		}

		public Vector3 GetDirection(float t)
		{
			return this.GetVelocity(t).normalized;
		}

		public Vector3 GetNormal(float t)
		{
			int num;
			if (t >= 1f)
			{
				t = 1f;
				num = this.points.Length - 4;
			}
			else
			{
				t = Mathf.Clamp01(t) * (float)this.CurveCount;
				num = (int)t;
				t -= (float)num;
				num *= 3;
			}
			return base.transform.TransformDirection(this.GetNormal(this.points[num].point, this.points[num + 1].point, this.points[num + 2].point, this.points[num + 3].point, this.points[num].normal, this.points[num + 3].normal, t));
		}

		public void AddCruve()
		{
			Vector3 vector = base.transform.InverseTransformPoint(this.GetPoint(1f));
			Vector3 normal = this.points[this.points.Length - 1].normal;
			Vector3 direction = this.GetDirection(1f);
			Vector3 b = base.transform.InverseTransformDirection(direction);
			int num = this.points.Length;
			Array.Resize<SplinePoint>(ref this.points, this.points.Length + 3);
			for (int i = 0; i < 3; i++)
			{
				vector += b;
				this.points[num + i] = new SplinePoint(vector, normal);
			}
			Array.Resize<TangentMode>(ref this.tangentModes, this.tangentModes.Length + 1);
			this.tangentModes[this.tangentModes.Length - 1] = TangentMode.Aligned;
		}

		public void RemoveLastCurve()
		{
			if (this.CurveCount < 2)
			{
				return;
			}
			for (int i = 2; i >= 0; i--)
			{
				this.points[this.points.Length - 1 - i] = null;
			}
			Array.Resize<SplinePoint>(ref this.points, this.points.Length - 3);
			Array.Resize<TangentMode>(ref this.tangentModes, this.tangentModes.Length - 1);
		}
		
		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		public static Vector3 GetDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float num = 1f - t;
			return 3f * num * num * (p1 - p0) + 6f * num * t * (p2 - p1) + 3f * t * t * (p3 - p2);
		}

		public Vector3 GetNormal(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 n0, Vector3 n1, float t)
		{
			Vector3 derivative = Spline.GetDerivative(p0, p1, p2, p3, 0f);
			Vector3 vector = Quaternion.FromToRotation(Spline.GetDerivative(p0, p1, p2, p3, 1f), derivative) * n1;
			Quaternion.FromToRotation(derivative, vector);
			Quaternion rotation = Quaternion.AngleAxis(Vector3.SignedAngle(n0, vector, derivative) * t, Spline.GetDerivative(p0, p1, p2, p3, t));
			n0 = Quaternion.FromToRotation(derivative, Spline.GetDerivative(p0, p1, p2, p3, t)) * n0;
			return rotation * n0;
		}

		public Vector3 GetDirection(int index)
		{
			Vector3 result = (index == this.ControlPointCount - 1) ? Spline.GetDerivative(this.GetControlPointPosition(index - 3), this.GetControlPointPosition(index - 2), this.GetControlPointPosition(index - 1), this.GetControlPointPosition(index), 1f) : Spline.GetDerivative(this.GetControlPointPosition(index), this.GetControlPointPosition(index + 1), this.GetControlPointPosition(index + 2), this.GetControlPointPosition(index + 3), 0f);
			result.Normalize();
			return result;
		}

		public Vector3 GetControlPointPosition(int index)
		{
			return this.points[index].point;
		}

		public Vector3 GetControlPointNormal(int index)
		{
			return this.points[index].normal;
		}

		public int GetAttachedPoint(int tangentIndex)
		{
			if (this.closed)
			{
				if (tangentIndex == 1 || tangentIndex == this.points.Length - 2)
				{
					return 0;
				}
				if (tangentIndex % 3 == 2)
				{
					return tangentIndex + 1;
				}
				if (tangentIndex % 3 == 1)
				{
					return tangentIndex - 1;
				}
			}
			if (tangentIndex % 3 == 2)
			{
				return tangentIndex + 1;
			}
			if (tangentIndex % 3 == 1)
			{
				return tangentIndex - 1;
			}
			return -1;
		}

		public TangentMode GetTangentMode(int pointIndex)
		{
			if (!this.closed && (pointIndex <= 1 || pointIndex >= this.points.Length - 2))
			{
				return TangentMode.Broken;
			}
			int num = (pointIndex <= 1) ? (this.closed ? (this.tangentModes.Length - 1) : 0) : ((pointIndex + 1) / 3 - 1);
			return this.tangentModes[num];
		}

		public void SetTangentMode(int pointIndex, TangentMode tangentMode)
		{
			if (!this.closed && (pointIndex <= 1 || pointIndex >= this.points.Length - 2))
			{
				return;
			}
			int num = (pointIndex + 1) / 3 - 1;
			this.tangentModes[num] = tangentMode;
		}

		public void SetControlPointPosition(int index, Vector3 point)
		{
			this.points[index].point = point;
			if (this.closed)
			{
				if (index == 0)
				{
					this.points[this.points.Length - 1].point = point;
				}
				else if (index == this.points.Length - 1)
				{
					this.points[0].point = point;
				}
			}
			this.UpdateLength();
		}

		public void SetControlPointNormal(int index, Vector3 normal)
		{
			this.points[index].normal = normal;
			if (this.closed)
			{
				if (index == 0)
				{
					this.points[this.points.Length - 1].normal = normal;
					return;
				}
				if (index == this.points.Length - 1)
				{
					this.points[0].normal = normal;
				}
			}
		}

		public void SetControlPoint(int index, Vector3 point, Vector3 normal)
		{
			this.SetControlPointPosition(index, point);
			this.SetControlPointNormal(index, normal);
		}

		private void UpdateLength()
		{
			this.length = 0f;
			float num = 0f;
			float a = 0.0001f;
			Vector3 b = this.GetPoint(0f);
			Vector3 vector = Vector3.zero;
			while (num < 1f)
			{
				num += Mathf.Min(a, 1f - num);
				vector = this.GetPoint(num);
				this.length += (vector - b).magnitude;
				b = vector;
			}
		}

		public float GetLength()
		{
			if (this.length < 0f)
			{
				this.UpdateLength();
			}
			return this.length;
		}

		public void OnValidate()
		{
			Spline.ValidateEvent onValidateCall = this.OnValidateCall;
			if (onValidateCall == null)
			{
				return;
			}
			onValidateCall();
		}

		public Spline.ValidateEvent OnValidateCall;

		[SerializeField]
		private SplinePoint[] points;

		[SerializeField]
		private TangentMode[] tangentModes;

		[SerializeField]
		private bool closed;

		[SerializeField]
		private float length = -1f;
		public delegate void ValidateEvent();
	}
}
