using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandFeedback : MonoBehaviour {

	Camera camera;
	Image labelImg;

	// Use this for initialization
	void Start () {

		camera = Camera.main; // GetComponent<Camera>;
		Debug.Log("Camera" + camera.ToString());
			
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	//TODO feedback für Start- und Endpunkt

	public void displayLabel(Vector3 controlPoint, GameObject label){

		label.SetActive(true);

		/*Debug.Log ("label object" + label.ToString ());

		labelImg = label.GetComponentInChildren<Image>();
		Debug.Log ("labelImg: " + labelImg.ToString ());*/
		Vector3 palmCenter = controlPoint;

		float length = controlPoint.magnitude;
		//float onePercentOfLength = length / 100.0f;
		//float newLength = (onePercentOfLength * 0.90f) * 

		//get depth og handpalm
		float palmDepth = 0.01f; //0.05f; ////0.13f;
		float newLength = length -  palmDepth; //90%

		Vector3 labelPosition = (controlPoint.normalized) * newLength;
		//Vector3 screenPosition = camera.WorldToScreenPoint(controlPoint);
		//Debug.Log ("screenPosition labelPosition.x: " + screenPosition.x.ToString ());


		//Debug.Log ("controlPosition.z: " + controlPoint.z.ToString ());

		//float tmp = 300.0f;
		//screenPosition.x = tmp; //Gestures.headRotation * labelPosition;
		label.transform.position = labelPosition; //Gestures.headRotation * labelPosition;
		label.transform.LookAt(Gestures.handControllerPos);

		//Debug.Log("labelImg.transform.position: " + label.transform.position.ToString());



	}
}
