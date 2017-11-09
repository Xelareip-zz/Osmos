using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	public Material mat;
	public float zoom;
	public float maxZoom;

	void Update()
	{
		zoom = Mathf.Max(zoom % maxZoom, 1.0f);
		mat.SetFloat("_CurrentZoom", zoom);
		mat.SetFloat("_MaxZoom", maxZoom);
	}
}
