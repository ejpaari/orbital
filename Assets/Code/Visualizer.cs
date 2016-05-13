using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour 
{
	public Object groundObj;
	public Object satelliteObj;

	private List<Vector3> Scale(List<Mathd.Vector3d> positions)
	{
		List<Vector3> v = new List<Vector3>();
		foreach (Mathd.Vector3d p in positions) {
			v.Add(p.ToVector3() / 1000.0f);
		}
		return v;
	}

	public void Visualize(List<Mathd.Vector3d> satellites, List<Router.Node> graph, List<Mathd.Vector3d> routes, List<int> shortestPath) 
	{
		List<Vector3> fSatellites = new List<Vector3>();
		fSatellites = Scale(satellites);
		List<Vector3> fRoutes = new List<Vector3>();
		fRoutes = Scale(routes);

		for (int i = 0; i < graph.Count; ++i) {
			Vector3 p1 = fSatellites[i];
			Object o = Instantiate(satelliteObj, p1, Quaternion.identity);
			o.name = i.ToString();
			for (int j = 0; j < graph[i].nodes.Count; ++j) {
				Vector3 p2 = satellites[graph[i].nodes[j]].ToVector3() / 1000.0f;
				Debug.DrawLine(p1, p2, Color.green, 999.0f);
			}
		}

		Vector3 start = fRoutes[0];
		Object startObj = Instantiate(groundObj, start, Quaternion.identity);
		startObj.name = "Start";
		Vector3 end = fRoutes[1];
		Object endObj = Instantiate(groundObj, end, Quaternion.identity);
		endObj.name = "End";

		Debug.DrawLine(fRoutes[0], fSatellites[shortestPath[0]], Color.red, 999.0f);
		for (int i = 0; i < shortestPath.Count - 1; ++i) {
			Debug.DrawLine(fSatellites[shortestPath[i]], fSatellites[shortestPath[i+1]], Color.red, 999.0f);
		}
		Debug.DrawLine(fSatellites[shortestPath[shortestPath.Count-1]], fRoutes[1], Color.red, 999.0f);
	}
}
