using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	GameObject selectedLight; //= selectScript.GetSelectedLight();



	// Use this for initialization
	void Start () {

		GameObject selectScriptObject = GameObject.Find ("SelectLight");
		selectScript = selectScriptObject.GetComponent<SelectLight> ();



	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log("controllerPos: " + DetectIndexFinger.handControllerPos);


		
	}
}
