using UnityEngine;
using System.Collections;
using System;

// Unity3D does not support double precision vectors or modern .NET so here's some double math.
public static class Mathd
{
	public const double TO_RADIANS = Math.PI / 180.0;

	public struct Vector3d 
	{
		public double x;
		public double y;
		public double z;

		public Vector3d(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector3d operator-(Vector3d a, Vector3d b)
		{
			return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public double Length()
		{
			return Math.Sqrt(x * x + y * y + z * z);
		}

		public Vector3d Normalize()
		{
			double l = Length();
			return new Vector3d(x / l, y / l, z / l);
		}
	};

	public static Vector3d ToCartesian(Vector3d p) 
	{
		Vector3d np = new Vector3d();
		double lat = (90.0 - p.x) * TO_RADIANS;
		double lon = p.y * TO_RADIANS;
		np.z = -p.z * Math.Sin(lat) * Math.Cos(lon);
		np.x = p.z * Math.Sin(lat) * Math.Sin(lon);
		np.y = p.z * Math.Cos(lat);
		return np;
	}

	public static double Dot(Vector3d a, Vector3d b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	public static double SquaredDistance(Vector3d a, Vector3d b)
	{
		return Math.Abs(Math.Pow(a.x - b.x, 2.0) + 
		                Math.Pow(a.y - b.y, 2.0) + 
		                Math.Pow(a.z - b.z, 2.0));
	}

	public static double Distance(Vector3d a, Vector3d b)
	{
		return Math.Sqrt(SquaredDistance(a, b));
	}

	public static bool LineSphereIntersection(Vector3d start, Vector3d end, Vector3d origo, double r) 
	{
		Vector3d n = (end - start).Normalize();
		Vector3d c = start - origo;
		return (Math.Pow(Dot(n, c), 2.0) - Math.Pow(c.Length(), 2.0) + Math.Pow(r, 2.0)) > 0.0;
	}
}
