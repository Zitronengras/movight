using UnityEngine;
using System.Collections;

public class ConstructionDistance : MonoBehaviour {

	float degreeCounter;
	Vector3 wallScanVector;
	Vector3 ceilingScanVector;
	public static float ceilingDistance;
	public static float maxWallDistance;
	public static bool isMaxDistanceDetermined;

	LayerMask onlyWallsLayer;
	LayerMask onlyCeilingLayer;

	RaycastHit hitObject = new RaycastHit();

	public static float wallDistance = Mathf.Infinity;

	// Use this for initialization
	void Start () {

		isMaxDistanceDetermined = false;

		degreeCounter = 0;
		wallScanVector = Vector3.forward; //right; // (1,0,0)
		ceilingScanVector =  Vector3.up;
		maxWallDistance = 0;
		onlyWallsLayer = 1 << LayerMask.NameToLayer ("wall");

		onlyCeilingLayer = 1 << LayerMask.NameToLayer ("ceiling");

	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("isMaxDistanceDetected: " + isMaxDistanceDetermined.ToString ());

		// do once at the beginning
		if (isMaxDistanceDetermined == false) {
			
			determineDistanceHeadCeiling ();
			determineMaxDistanceToWall ();
			//Debug.Log ("maxDistance at the end: " + determineMaxDistanceToWall ());

		}
	}

	float determineMaxDistanceToWall(){

		while (degreeCounter < 360) {

			if (Physics.Raycast (Gestures.handControllerPos, wallScanVector, out hitObject, Mathf.Infinity, onlyWallsLayer)) {
				
				wallDistance = Vector3.Distance (Gestures.handControllerPos, hitObject.point);

				if (wallDistance > maxWallDistance) {
				
					maxWallDistance = wallDistance;
					//Debug.Log ("maxDistance:" + maxWallDistance.ToString ());

				}

				wallScanVector = Quaternion.Euler (0, 1, 0) * wallScanVector; //rotate one degree
				//Debug.Log ("Scan Vector: " + scanVector.ToString ());

				degreeCounter += 1;

			}
		}

		isMaxDistanceDetermined = true;
		return maxWallDistance;

	}

	float determineDistanceHeadCeiling(){

		if (Physics.Raycast (Gestures.handControllerPos, ceilingScanVector, out hitObject, Mathf.Infinity, onlyCeilingLayer)) {

			ceilingDistance = Vector3.Distance (Gestures.handControllerPos, hitObject.point);

		}

		return ceilingDistance;

	}

}
