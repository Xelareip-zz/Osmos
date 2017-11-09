using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class Parameters : ParameterBase
{
	private static string SAVE_FILE = Application.persistentDataPath + "/params.json";

	private static Parameters instance;
	public static Parameters Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Parameters();
				instance.Load();
			}
			return instance;
		}
	}

	[Parameter("Absorb speed")]
	public float absorbSpeed = 5.0f;

	[Parameter("Charge speed")]
	public float chargeSpeed = 3.0f;
	[Parameter("Zoom out speed")]
	public float zoomOutSpeed = 0.5f;

	[Parameter("reduce speed")]
	public float reduceSpeed = 0.01f;
	

	private Parameters()
	{
	}

	public override void Load()
	{
		if (File.Exists(SAVE_FILE))
		{
			string content = File.ReadAllText(SAVE_FILE);
			JsonUtility.FromJsonOverwrite(content, this);
		}
	}

	public override void Save()
	{
		File.WriteAllText(SAVE_FILE, JsonUtility.ToJson(this));
	}

	public override void Reset()
	{
		File.Delete(SAVE_FILE);
	}

	public override void DeleteInstance()
	{
		instance = null;
	}
}
