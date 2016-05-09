using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Router : MonoBehaviour 
{
	private struct Route
	{
		public Mathd.Vector3d start;
		public Mathd.Vector3d end;
	};

	public struct Node
	{
		public int id;
		public List<int> nodes;
	};

	private const double EARTH_RADIUS = 6371.0;

	private Route route = new Route();
	private List<Mathd.Vector3d> satellites = new List<Mathd.Vector3d>();
	private List<Node> graph = new List<Node>();
	private int startSat = -1;
	private int endSat = -1;
	private Visualizer visualizer;

	private void CreateGraph()
	{
		Mathd.Vector3d origo = new Mathd.Vector3d();
		for (int i = 0; i < satellites.Count; ++i) {
			Node node = new Node();
			node.nodes = new List<int>();
			node.id = i;
			for (int j = 0; j < satellites.Count; ++j) {
				if (!Mathd.LineSphereIntersection(satellites[i], satellites[j], origo, EARTH_RADIUS)) {
					node.nodes.Add(j);
				}
			}
			graph.Add(node);
		}
	}

	private void FindNearestSatellites()
	{
		double startDistance = double.MaxValue;
		double endDistance = double.MaxValue;
		double dist;
		for (int i = 0; i < satellites.Count; ++i) {
			dist = Mathd.SquaredDistance(satellites[i], route.start);
			if (dist < startDistance) {
				startDistance = dist;
				startSat = i;
			}
			dist = Mathd.SquaredDistance(satellites[i], route.end);
			if (dist < endDistance) {
				endDistance = dist;
				endSat = i;
			}
		}
	}

	void Start() 
	{
		var reader = new StreamReader(File.OpenRead(@"data"));			
		while (!reader.EndOfStream) {
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			if (values == null || values.Length == 0 || values[0][0] == '#') {
				continue;
			}
			if (values[0].Contains("SAT")) {
				Mathd.Vector3d p;
				p.x = System.Convert.ToDouble(values[1]);
				p.y = System.Convert.ToDouble(values[2]);
				p.z = EARTH_RADIUS + System.Convert.ToDouble(values[3]);
				satellites.Add(Mathd.ToCartesian(p));
			}
			if (values[0].Contains("ROUTE")) {
				route.start.x = System.Convert.ToDouble(values[1]);
				route.start.y = System.Convert.ToDouble(values[2]);
				route.start.z = EARTH_RADIUS;
				route.start = Mathd.ToCartesian(route.start);
				route.end.x = System.Convert.ToDouble(values[3]);
				route.end.y = System.Convert.ToDouble(values[4]);
				route.end.z = EARTH_RADIUS;
				route.end = Mathd.ToCartesian(route.end);
			}
		}

		CreateGraph();
		FindNearestSatellites();

		visualizer = GetComponent<Visualizer>();
		visualizer.Visualize(satellites, graph);
	}

	public List<Mathd.Vector3d> Satellites
	{
		get { return satellites; }
	}
}
