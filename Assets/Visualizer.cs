using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour 
{
	public Object groundObj;
	public Object satelliteObj;

	private const float SCALE = 0.1f;

	public void Visualize(List<Mathd.Vector3d> satellites, List<Router.Node> graph, List<Mathd.Vector3d> routes) 
	{
		for (int i = 0; i < graph.Count; ++i) {
			Vector3 p1 = satellites[i].ToVector3() / 1000.0f;

			Object o = Instantiate(satelliteObj, p1, Quaternion.identity);
			o.name = i.ToString();

			for (int j = 0; j < graph[i].nodes.Count; ++j) {
				Vector3 p2 = satellites[graph[i].nodes[j]].ToVector3() / 1000.0f;
				Debug.DrawLine(p1, p2, Color.green, 999.0f);
			}
		}

		Vector3 start = routes[0].ToVector3() / 1000.0f;
		Object startObj = Instantiate(groundObj, start, Quaternion.identity);
		startObj.name = "Start";
		Vector3 end = routes[1].ToVector3() / 1000.0f;
		Object endObj = Instantiate(groundObj, end, Quaternion.identity);
		endObj.name = "End";
	}
}
