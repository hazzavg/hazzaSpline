using System;

///<summary>
/// This Script refers to the Various Instances in which the Spline can be Interpolated
/// An Enum (Enumeration) refers to the Value Type that uses various Constants to determine a particular state
/// This Script uses an Enum to determine the various States classified by the Spline
///<summary>
namespace hazza.Splines
{
	[Serializable] // Serializable refers to how other Scripts are able to Access this Class/Script
	public enum TangentMode
	{
		Broken, // When the Spline is Broken
		Aligned, // When the Spline Points are Aligned accordingly
		Mirrored // When the Spline Points are Equal on Adjacent Sides
	}
}
