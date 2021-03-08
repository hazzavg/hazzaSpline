using System;

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
