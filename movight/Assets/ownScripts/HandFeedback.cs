using UnityEngine;
using System.Collections;

public class HandFeedback : MonoBehaviour {

	// Use this for initialization
	void Start () {

			
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void displayLabel(Vector3 controlPoint, GameObject label){

		label.SetActive(true);

		Vector3 palmCenter = controlPoint;

		float length = palmCenter.magnitude;
		//float onePercentOfLength = length / 100.0f;
		//float newLength = (onePercentOfLength * 0.90f) * 

		//get depth og handpalm
		float palmDepth = 0.13f;
		float newLength = length -  palmDepth; //90%

		Vector3 labelPosition = (palmCenter.normalized) * newLength;
		Debug.Log ("labelPosition.x: " + labelPosition.z.ToString ());


		Debug.Log ("controlPosition.z: " + palmCenter.z.ToString ());

		label.transform.position = labelPosition;

	}
}
