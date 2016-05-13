using UnityEngine;
using System.Collections;
using System;

// Unity3D does not support double precision vectors or modern .NET so here's some wheel reinventing.
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

		public void Normalize()
		{
			double l = Length();
			x = x / l;
			y = y / l;
			z = z / l;
		}

		public Vector3 ToVector3()
		{
			return new Vector3((float)x, (float)y, (float)z);
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

	public static bool RaySphereIntersection(Vector3d start, Vector3d end, Vector3d origo, double r) 
	{		
		Vector3d L = origo - start;
		Vector3d dir = (end - start);
		dir.Normalize();
		double t = Dot(L, dir);
		if (t < 0.0) {
			return false;
		}
		double d = Dot(L, L) - t * t;
		return d < r * r;
	}
}
