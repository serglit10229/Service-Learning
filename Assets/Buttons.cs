using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {


	public string MapURL; 
	public string GuideURL;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Info()
	{
		SceneManager.LoadScene("Info");
	}

	public void Map()
	{
		Application.OpenURL(MapURL);
	}

	public void Hours()
	{
		SceneManager.LoadScene("Hours");
	}
}
