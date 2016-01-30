using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	GameObject selectedLight; //= selectScript.GetSelectedLight();

	float maxY;
	float minY;
	float rangeY;

	float leapMaxX;

	float scaleFactorY;
	float scaleFactorX;

	// Use this for initialization
	void Start () {

		maxY = 500.0f / 1000; //millimeter in meter for unity
		minY = 100.0f / 1000; //millimeter in meter for unity
		rangeY = maxY - minY;


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
			//Debug.Log ("LeapFingerPos.x: " + DetectIndexFinger.leapTipPosition.x);


			//y-axis
			if ((DetectIndexFinger.leapTipPosition.y/1000) <= maxY && (DetectIndexFinger.leapTipPosition.y/1000) >= minY) {

				//Debug.Log ("Finger zwischen 500 und 100");
				//Debug.Log ("rangeY: " + rangeY.ToString());

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



			}

			//x-axis
			/*if (DetectIndexFinger.leapTipPosition.x <= maxX && DetectIndexFinger.leapTipPosition.x >= -(minX)) {

				//Debug.Log ("Finger zwischen 70 und -70");

			}*/

		}


		
	}
}
