using UnityEngine;
using System.Collections;

public class ConstructionDistance : MonoBehaviour {

	//TODO: distance to ground

	LayerMask onlyWallsLayer;
	LayerMask onlyGroundLayer;

	RaycastHit hitObject = new RaycastHit();

	public static float wallDistance = Mathf.Infinity;
	public static float groundDistance = Mathf.Infinity;


	// Use this for initialization
	void Start () {
		
		onlyWallsLayer = 1 << LayerMask.NameToLayer ("wall");

		onlyGroundLayer = 1 << LayerMask.NameToLayer ("ground");

	}
	
	// Update is called once per frame
	void Update () {


		//get distance to wall
		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos, out hitObject, Mathf.Infinity, onlyWallsLayer)) {

			//Debug.Log ("hitPoint:" + hitObject.point);

			wallDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, hitObject.point);

			//Debug.Log ("Distance to wall:" + wallDistance.ToString());
			//Debug.Log ("*************");

		} else {

			//Debug.Log ("hit nothing \n stop select sequence");

		}


		//get distance to ground
		//get distance to wall
		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos, out hitObject, Mathf.Infinity, onlyGroundLayer)) {

			//Debug.Log ("hitPoint:" + hitObject.point);

			groundDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, hitObject.point);

			//Debug.Log ("Distance to ground:" + groundDistance.ToString());

		} else {

			//Debug.Log ("hit nothing \n stop select sequence");

		}

	
	}
}
