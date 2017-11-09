using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public Material mat;
	public float speed;
	public float zoom;
	public float maxZoom;

	void Update ()
	{
		//float zoomIncrease = zoom / speed;
		zoom += zoom * Time.deltaTime * speed;
		zoom = Mathf.Max(zoom % maxZoom, 1.0f);
		mat.SetFloat("_CurrentZoom", zoom);
		mat.SetFloat("_MaxZoom", maxZoom);
	}
}
