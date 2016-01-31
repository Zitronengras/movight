using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	GameObject selectedLight; //= selectScript.GetSelectedLight();

	float distanceFingerController;
	float minRangeValue;
	float maxRangeValue;

	bool isRangeCalculated = false;

	Vector3 fingerPos;

	int i;

	float maxY;
	float minY;
	float rangeY;

	float relativeUnityTipPositionY;

	float leapMaxX;

	float scaleFactorY;
	float scaleFactorX;

	// Use this for initialization
	void Start () {

		i = 0;

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


		if (ConstructionDistance.isMaxDistanceDetermined == true){
			
			//Debug.Log ("*******isMaxDistanceDetermined************");

			if (DetectIndexFinger.isFingerDetected == true) { //&& SelectLight.isLightHit				
			
				//Debug.Log ("LeapFingerPos: " + DetectIndexFinger.leapTipPosition);
				//Debug.Log ("LeapFingerPos.y: " + DetectIndexFinger.leapTipPosition.y/1000);

				//Debug.Log ("*********isFingerDetected**********");

				if (SelectLight.isLightSelected == true) {

					//TODO isSelectedLight = false at the end of positioning 

					Debug.Log ("*********isLightSelected**********");

					if (isRangeCalculated == false) {
						calculateControlRange ();
						isRangeCalculated = true;
						i += 1;
						//Debug.Log ("i: " + i.ToString ());

						if (distanceFingerController >= minRangeValue && distanceFingerController <= maxRangeValue) {
							//Debug.Log ("In range");
						}
					}
					if (isRangeCalculated == true) {

						Debug.Log ("*********isRangeCalculated**********");

						//position light
						//moveLightDepth();
						percentagePosition ();

					}
				}
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

		} else {
			isRangeCalculated = false;
			i = 0;
		}


		
	}

	void calculateControlRange(){
		
		float controlRangeDepth = 300.0f; // 100 percent

		selectedLight = selectScript.GetSelectedLight();
		//Debug.Log ("1 returned selectedLight position" + selectedLight.transform.position.ToString());

		//get maxWallDistance
		float maxWallDistance = ConstructionDistance.maxWallDistance;
		//Debug.Log ("maxwalldistance: " + maxWallDistance.ToString ());

		// 1 percentage position of light on raycast between controller and maxWallDistance
		//walldistance = 100%
		float scaleHelper = 100 / maxWallDistance; //multiply scaleHelper with position of light
		//
		float selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, selectedLight.transform.position);
		//Debug.Log ("2 selectedLightDistanceToController absolut: " + selectedLightDistanceToController);
		//
		/*Vector3 distanceLightContr = selectedLight.transform.position - DetectIndexFinger.handControllerPos;
		Debug.Log ("vector lightContrdistance: " + distanceLightContr.ToString ());
		Debug.Log ("vector lightContrdistance betrag: " + distanceLightContr.magnitude.ToString ());
		*/

		//percentage position of light between controllerand wall
		float percentagePosOfLight = (scaleHelper * selectedLightDistanceToController); // / 100;  //percent
		//Debug.Log ("3 percentage pos of Light: " + percentagePosOfLight);


		// 2 
		//get leapFingerPos
		//Leap.Vector leapFingerPosInMeter = (DetectIndexFinger.leapTipPosition / 1000);
		fingerPos = DetectIndexFinger.fingerPos;
		//Debug.Log ("FingerPos: " + fingerPos.ToString ());
		//Debug.Log ("handContr: " + DetectIndexFinger.handControllerPos.ToString ());
		distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPos);
		//Debug.Log ("distanceFingerController: " + distanceFingerController.ToString ());
		//
		/*Vector3 distance = fingerPos - DetectIndexFinger.handControllerPos;
		Debug.Log ("vector fingercontrdistance: " + distance.ToString ());
		Debug.Log ("vector fingercontrdistance betrag: " + distance.magnitude.ToString ());
		*/


		float percentagePosOfFinger = percentagePosOfLight; //adapt position of light on position of finger
		//devide absolute fingerPos by persentagePosition
		/*float absoluteFingerPosEquaslOnePercent = distanceFingerController / percentagePosOfLight;
		Debug.Log ("absoluteFingerPosEquaslOnePercent: " + absoluteFingerPosEquaslOnePercent.ToString ());
		float absoluteFingerPosEqualsHundretPercent = absoluteFingerPosEquaslOnePercent * 100;
		Debug.Log ("absoluteFingerPosEqualsHundretPercent: " + absoluteFingerPosEqualsHundretPercent.ToString ());*/

		//need a final absoluteFingerPosEquaslOnePercent
		//working area 20cm: 10cm to 30cm
		float absoluteFingerPosEquaslOnePercent = 0.40f / 100; //0,0020
		//Debug.Log ("absoluteFingerPosEquaslOnePercent, 0,0020: " + absoluteFingerPosEquaslOnePercent.ToString ());

		float absoluteRangeValueUponFingerPos = percentagePosOfFinger * absoluteFingerPosEquaslOnePercent; //percentagePos * 0,0015
		//Debug.Log ("absoluteRangeValueUponFingerPos: " + absoluteRangeValueUponFingerPos.ToString ());

		minRangeValue = distanceFingerController - absoluteRangeValueUponFingerPos;
		Debug.Log ("minRangeValue: " + minRangeValue.ToString ());

		maxRangeValue = distanceFingerController + ((100-percentagePosOfFinger) * absoluteFingerPosEquaslOnePercent);
		Debug.Log ("maxRangeValue: " + maxRangeValue.ToString ());

		if (minRangeValue < 0.10) {
			//Debug.LogFormat ("minRangeValue < 0.10");
		}
		if (maxRangeValue > 0.60) {
			//Debug.LogFormat ("maxRangeValue > 0.0");
		}
		Debug.Log ("range calculated");
	}

	void moveLightDepth(){
		
		selectedLight = selectScript.GetSelectedLight();

		fingerPos = DetectIndexFinger.fingerPos;
		distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPos);

		//check for range
		if (distanceFingerController >= minRangeValue && distanceFingerController <= maxRangeValue) {
			Debug.LogFormat ("in range");
		} else {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");
		}
		//get percentage pos of finger in range
		//float tmp = 100
	}

	void percentagePosition(){ //float finger){
		
		distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);
		Debug.Log ("distance:" + distanceFingerController.ToString ());

		//for test
		float tmp = maxRangeValue * 1000.0f;
		//float dreisatz1 = maxRangeValue / tmp;

		//
		float dreisatz1right = 100 / tmp;
		float percentage = dreisatz1right * distanceFingerController;

		Debug.Log ("percentage: " + percentage);

		//return percentage;
	}



}
