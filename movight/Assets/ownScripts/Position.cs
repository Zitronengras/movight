using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	GameObject selectedLight; //= selectScript.GetSelectedLight();

	float maxY;
	float minY;
	float rangeY;

	float relativeUnityTipPositionY;

	float leapMaxX;

	float scaleFactorY;
	float scaleFactorX;

	// Use this for initialization
	void Start () {

		maxY = 500.0f / 1000; //millimeter in meter for unity
		minY = 100.0f / 1000; //millimeter in meter for unity
		rangeY = maxY - minY;

		//if (DetectIndexFinger.isFingerDetected == true) {
			
			//relativeUnityTipPositionY = DetectIndexFinger.leapTipPosition.y / 1000;
			//Debug.Log ("relativeUnityTipPosition set");
		//}


		GameObject selectScriptObject = GameObject.Find ("SelectLight");
		selectScript = selectScriptObject.GetComponent<SelectLight> ();

	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log("controllerPos: " + DetectIndexFinger.handControllerPos);

		//define area in leapArea for depthcontrol

		//define area in LeapArea for horizontal control
		//between x = 70 and x = -70

		//TODO reihenfolge der werteübergaben stimmt nicht

		if (DetectIndexFinger.isFingerDetected == true && SelectLight.isLightHit && SelectLight.isLightSelected == true) {
			
			//Debug.Log ("LeapFingerPos: " + DetectIndexFinger.leapTipPosition);
			//Debug.Log ("LeapFingerPos.y: " + DetectIndexFinger.leapTipPosition.y/1000);

			if (ConstructionDistance.isMaxDistanceDetermined == true) {

				Debug.Log ("*******************");
				setControlRange ();
				Debug.Log ("*******************");

			}


			/*



			//y-axis
			//if (relativeUnityTipPositionY <= maxY && relativeUnityTipPositionY >= minY) {

				//Debug.Log ("Finger zwischen 500 und 100");
				//Debug.Log ("rangeY: " + rangeY.ToString());

			selectedLight = selectScript.GetSelectedLight();
			Debug.Log ("1 returned selectedLight" + selectedLight.ToString());

				
			// 1 percentage position of light on raycast between controller and wallhitpoint
			//walldistance = 100%
			float scaleHelper = 100 / ConstructionDistance.wallDistance; //multiply scaleHelper with yPos
			//
			float selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, selectedLight.transform.position);
			Debug.Log ("2 selectedLightDistanceToController z: " + selectedLightDistanceToController);

			//percentage position of light between controllerand wall
			float percentagePosOfLight = scaleHelper * selectedLightDistanceToController;  //percent
			Debug.Log ("3 percentage pos of Light: " + percentagePosOfLight);


			// 2 adapt percentage position of light to fingerPos on raycast between controller and max range
			*/

			/*
			scaleFactorY = rangeY / ConstructionDistance.wallDistance;

			Debug.Log ("scaleFactor: " + scaleFactorY.ToString());

			//get y value of light
			selectedLight = selectScript.GetSelectedLight();
			float currentY = selectedLight.transform.position.z;
			Debug.Log ("selectedLight.y: " + selectedLight.transform.position.z);

			//change y value of light
			float transformedY = currentY * (DetectIndexFinger.leapTipPosition.y/1000);
			Debug.Log ("Position of light: " + selectedLight.transform.position);
			Vector3 temp = selectedLight.transform.position;
			Debug.Log ("temp: " + temp.ToString());

			temp.z = transformedY;
			Debug.Log ("temp after y: " + temp.ToString());
			selectedLight.transform.position = temp;
			*/



			//}


			//x-axis
			/*if (DetectIndexFinger.leapTipPosition.x <= maxX && DetectIndexFinger.leapTipPosition.x >= -(minX)) {

				//Debug.Log ("Finger zwischen 70 und -70");

			}*/

		}


		
	}

	void setControlRange(){
		
		float controlRangeDepth = 300.0f; // 100 percent

		selectedLight = selectScript.GetSelectedLight();
		Debug.Log ("1 returned selectedLight position" + selectedLight.transform.position.ToString());

		//get maxWallDistance
		float maxWallDistance = ConstructionDistance.maxWallDistance;
		//Debug.Log ("maxwalldistance: " + maxWallDistance.ToString ());

		// 1 percentage position of light on raycast between controller and maxWallDistance
		//walldistance = 100%
		float scaleHelper = 100 / maxWallDistance; //multiply scaleHelper with position of light
		//
		float selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, selectedLight.transform.position);
		Debug.Log ("2 selectedLightDistanceToController absolut: " + selectedLightDistanceToController);
		//
		Vector3 distanceLightContr = selectedLight.transform.position - DetectIndexFinger.handControllerPos;
		Debug.Log ("vector lightContrdistance: " + distanceLightContr.ToString ());
		Debug.Log ("vector lightContrdistance betrag: " + distanceLightContr.magnitude.ToString ());


		//percentage position of light between controllerand wall
		float percentagePosOfLight = scaleHelper * selectedLightDistanceToController;  //percent
		//Debug.Log ("3 percentage pos of Light: " + percentagePosOfLight);


		// 2 
		//get leapFingerPos
		//Leap.Vector leapFingerPosInMeter = (DetectIndexFinger.leapTipPosition / 1000);
		Vector3 fingerPos = DetectIndexFinger.fingerPos;
		Debug.Log ("FingerPos: " + fingerPos.ToString ());
		Debug.Log ("handContr: " + DetectIndexFinger.handControllerPos.ToString ());
		float distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPos);
		Debug.Log ("distanceFingerController: " + distanceFingerController.ToString ());
		//
		Vector3 distance = fingerPos - DetectIndexFinger.handControllerPos;
		Debug.Log ("vector fingercontrdistance: " + distance.ToString ());
		Debug.Log ("vector fingercontrdistance betrag: " + distance.magnitude.ToString ());





	}


}
