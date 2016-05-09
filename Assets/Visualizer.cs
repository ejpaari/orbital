using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visualizer : MonoBehaviour 
{
	private const float SCALE = 0.1f;

	public void Visualize(List<Mathd.Vector3d> satellites, List<Router.Node> graph) 
	{
		for (int i = 0; i < graph.Count; ++i) {
			Vector3 p1 = new Vector3((float)satellites[i].x / 1000.0f, 
			                         (float)satellites[i].y / 1000.0f, 
			                         (float)satellites[i].z / 1000.0f);

			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere.transform.position = p1;
			sphere.transform.localScale = new Vector3(SCALE, SCALE, SCALE);

			for (int j = 0; j < graph[i].nodes.Count; ++j) {
				Vector3 p2 = new Vector3((float)satellites[graph[i].nodes[j]].x / 1000.0f,
				                         (float)satellites[graph[i].nodes[j]].y / 1000.0f,
				                         (float)satellites[graph[i].nodes[j]].z / 1000.0f);
				Debug.DrawLine(p1, p2, Color.red, 999.0f);
			}
		}
	}
}
