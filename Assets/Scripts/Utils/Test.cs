using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		int posy=50;
		if(GUI.Button(new Rect(50,posy,100,40),"Add")){
			
		}
		posy+=60;
		if(GUI.Button(new Rect(50,posy,100,40),"Minus")){

		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
