using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Router : MonoBehaviour 
{
	public struct Node
	{
		public int id;
		public List<int> nodes;
	};

	private const double EARTH_RADIUS = 6371.0;

	private List<Mathd.Vector3d> satellites = new List<Mathd.Vector3d>();
	private List<Mathd.Vector3d> routes = new List<Mathd.Vector3d>();
	private List<Node> graph = new List<Node>();
	private List<int> startSats = new List<int>();
	private List<int> endSats = new List<int>();
	private Visualizer visualizer;

	private void CreateGraph()
	{
		Mathd.Vector3d origo = new Mathd.Vector3d();
		for (int i = 0; i < satellites.Count; ++i) {
			Node node = new Node();
			node.nodes = new List<int>();
			node.id = i;
			for (int j = 0; j < satellites.Count; ++j) {
				if (i != j && !Mathd.RaySphereIntersection(satellites[i], satellites[j], origo, EARTH_RADIUS)) {
					node.nodes.Add(j);
				}
			}
			graph.Add(node);

			if (!Mathd.RaySphereIntersection(routes[0], satellites[i], origo, EARTH_RADIUS)) {
				startSats.Add(i);
			}
			if (!Mathd.RaySphereIntersection(routes[1], satellites[i], origo, EARTH_RADIUS)) {
				endSats.Add(i);
			}
		}
	}

	private bool FindShortestPath(int start, List<int> endSats, ref List<int> path)
	{
		if (start < 0 || start >= graph.Count || endSats.Count == 0) {
			return false;
		}

		List<bool> visited = new List<bool>();
		for (int i = 0; i < graph.Count; ++i) {
			visited.Add(false);
		}

		List<List<int>> paths = new List<List<int>>();
		List<int> startPath = new List<int>();
		startPath.Add(start);
		paths.Add(startPath);

		while (paths.Count > 0) {
			List<int> currentPath = new List<int>(paths[0]);
			int id = currentPath[currentPath.Count - 1];
			visited[id] = true;
			foreach (int i in graph[id].nodes) {
				if (!visited[i]) {
					List<int> newPath = new List<int>(currentPath);
					newPath.Add(i);
					if (endSats.Contains(i)) {
						path = newPath;
						return true;
					}
					paths.Add(newPath);
				}
			}
			paths.RemoveAt(0);
		}
		return false;
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
				Mathd.Vector3d s;
				s.x = System.Convert.ToDouble(values[1]);
				s.y = System.Convert.ToDouble(values[2]);
				s.z = EARTH_RADIUS + 0.1;
				routes.Add(Mathd.ToCartesian(s));

				Mathd.Vector3d e;
				e.x = System.Convert.ToDouble(values[3]);
				e.y = System.Convert.ToDouble(values[4]);
				e.z = EARTH_RADIUS + 0.1;
				routes.Add(Mathd.ToCartesian(e));
			}
		}

		CreateGraph();

		List<int> shortestPath = new List<int>();
		foreach (int sat in startSats) {
			List<int> temp = new List<int>();
			if (FindShortestPath(sat, endSats, ref temp)) {
				if (shortestPath.Count == 0 || temp.Count < shortestPath.Count) {
					shortestPath = temp;
				}
			}
		}

		string text = "";
		foreach (int i in shortestPath) {
			text += "SAT" + i.ToString() + ",";
		}
		text = text.Length > 0 ? text.Remove(text.Length - 1) : "";
		File.WriteAllText("out", text);

		if (shortestPath.Count > 0) {
			visualizer = GetComponent<Visualizer> ();
			visualizer.Visualize (satellites, graph, routes, shortestPath);
		}
	}
}
