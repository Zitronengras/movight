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
			
	}

	public void displayLabel(Vector3 controlPoint, GameObject label){

		label.SetActive(true);

		palmCenter = controlPoint;
		length = controlPoint.magnitude;

		//get depth of handpalm
		palmDepth = 0.15f;
		newLength = length -  palmDepth;

		labelPosition = (controlPoint.normalized) * newLength;
		label.transform.position = labelPosition;
		label.transform.LookAt(Gestures.handControllerPos);

	}
}
