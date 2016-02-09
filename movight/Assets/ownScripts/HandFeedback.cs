using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandFeedback : MonoBehaviour {

	Camera camera;

	Vector3 palmCenter;
	float length;

	float palmDepth;
	float newLength;
	Vector3 labelPosition;

	// Use this for initialization
	void Start () {

		camera = Camera.main;
		//Debug.Log("Camera" + camera.ToString());
			
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void displayLabel(Vector3 controlPoint, GameObject label){

		label.SetActive(true);

		//Debug.Log ("LABEL : " + label.ToString ());

		palmCenter = controlPoint;
		length = controlPoint.magnitude;

		//get depth of handpalm
		palmDepth = 0.001f; //0.05f; ////0.13f;
		newLength = length +  palmDepth;

		labelPosition = (controlPoint.normalized) * newLength;
		label.transform.position = labelPosition;
		label.transform.LookAt(Gestures.handControllerPos);

		//Debug.Log("labelImg.transform.position: " + label.transform.position.ToString());

	}
}
