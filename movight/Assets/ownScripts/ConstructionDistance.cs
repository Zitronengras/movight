using UnityEngine;
using System.Collections;

public class ConstructionDistance : MonoBehaviour {

	//TODO: distance to ground

	float degreeCounter;
	Vector3 scanVector;
	public static float maxWallDistance;
	public static bool isMaxDistanceDetermined; // = false;
	//Vector3 rotatedDirection;

	LayerMask onlyWallsLayer;
	//LayerMask onlyGroundLayer;

	RaycastHit hitObject = new RaycastHit();

	public static float wallDistance = Mathf.Infinity;
	//public static float groundDistance = Mathf.Infinity;


	// Use this for initialization
	void Start () {

		isMaxDistanceDetermined = false;

		degreeCounter = 0;
		scanVector = Vector3.forward; //right; // (1,0,0)
		//Debug.Log("initiate scanVector" + scanVector.ToString());
		maxWallDistance = 0;
		onlyWallsLayer = 1 << LayerMask.NameToLayer ("wall");

		//onlyGroundLayer = 1 << LayerMask.NameToLayer ("ground");

	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("isMaxDistanceDetected: " + isMaxDistanceDetermined.ToString ());

		// do once at the beginning
		if (isMaxDistanceDetermined == false) {
			
			Debug.Log ("maxDistance at the end: " + determineMaxDistanceToWall ());

		}


		/*
		//get distance to wall
		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos, out hitObject, Mathf.Infinity, onlyWallsLayer)) {

			//Debug.Log ("hitPoint:" + hitObject.point);

			wallDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, hitObject.point);

			Debug.Log ("Distance to wall:" + wallDistance.ToString());
			//Debug.Log ("*************");

		} else {

			//Debug.Log ("hit nothing \n stop select sequence");

		}
		*/

		//get distance to ground
		//get distance to wall
		/*
		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.controlPoint, out hitObject, Mathf.Infinity, onlyGroundLayer)) {

			//Debug.Log ("hitPoint:" + hitObject.point);
			//Debug.Log ("hit ground");
			groundDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, hitObject.point);

			//Debug.Log ("Distance to ground:" + groundDistance.ToString());

		} else {

			//Debug.Log ("hit nothing \n stop select sequence");

		}
		*/

	
	}

	float determineMaxDistanceToWall(){

		while (degreeCounter < 360) {

			if (Physics.Raycast (DetectIndexFinger.handControllerPos, scanVector, out hitObject, Mathf.Infinity, onlyWallsLayer)) {
				
				//Debug.DrawRay (DetectIndexFinger.handControllerPos, scanVector * 30, Color.red, 50.0f, true);

				wallDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, hitObject.point);

				if (wallDistance > maxWallDistance) {
				
					maxWallDistance = wallDistance;
					//Debug.Log ("maxDistance:" + maxWallDistance.ToString ());

				}

				scanVector = Quaternion.Euler (0, 1, 0) * scanVector; //rotate one degree
				//Debug.Log ("Scan Vector: " + scanVector.ToString ());

				degreeCounter += 1;

			}
		}

		isMaxDistanceDetermined = true;
		return maxWallDistance;

	}

}
