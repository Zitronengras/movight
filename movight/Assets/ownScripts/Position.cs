using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	GameObject light; //= selectScript.GetSelectedLight();

	//calculateFingerRange
	float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;

	//moveLightDepth
	float currentLightControllerDistance;
	float currentFingerControllerDistance;

	//getPercentageFingerPos
	//float currentFingerControllerDistance;

		//float minRangeValue;
		//float maxRangeValue;

	Vector3 lightPosition;

	bool isRangeCalculated = false;
	float selectedLightDistanceToController;

	float maxWallDistance;
	float onePercentOfWallControllerDistance;

	Vector3 fingerPos;

	//int i;

	//float maxY;
	//float minY;
	//float rangeY;

	float relativeUnityTipPositionY;

	float leapMaxX;

	float scaleFactorY;
	float scaleFactorX;

	// Use this for initialization
	void Start () {

		//i = 0;

		//leapRange
		//maxY = 500.0f / 1000; //millimeter in meter for unity
		//minY = 100.0f / 1000; //millimeter in meter for unity
		//rangeY = maxY - minY;


		//if (DetectIndexFinger.isFingerDetected == true) {
			
			//relativeUnityTipPositionY = DetectIndexFinger.leapTipPosition.y / 1000;
			//Debug.Log ("relativeUnityTipPosition set");
		//}


		GameObject selectScriptObject = GameObject.Find ("SelectLight");
		selectScript = selectScriptObject.GetComponent<SelectLight> ();

	}
	
	// Update is called once per frame
	void Update () {

	
		//TODO reihenfolge der werteübergaben stimmt nicht


		if (ConstructionDistance.isMaxDistanceDetermined == true){

			//get maxWallDistance
			maxWallDistance = ConstructionDistance.maxWallDistance;
			onePercentOfWallControllerDistance = 100 / maxWallDistance; //variable equals one percent of distance between controller and wall
			
			//Debug.Log ("*******isMaxDistanceDetermined************");

			if (DetectIndexFinger.isFingerDetected == true) { //&& SelectLight.isLightHit				
			
				//Debug.Log ("LeapFingerPos: " + DetectIndexFinger.leapTipPosition);
				//Debug.Log ("LeapFingerPos.y: " + DetectIndexFinger.leapTipPosition.y/1000);

				//Debug.Log ("*********isFingerDetected**********");

				if (SelectLight.isLightSelected == true) { //beeing in selectionsequence

					//Debug.Log ("*********isLightSelected**********");

					fingerPos = DetectIndexFinger.fingerPos; //unity absolute fingerPosition

					light = selectScript.GetSelectedLight(); //selected light
					lightPosition = light.transform.position; //position of selected light

					selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition);



					//TODO isSelectedLight = false at the end of positioning 

					//calculate range at the beginning of the selectionsequence
					if (isRangeCalculated == false) {

						Debug.Log ("at the beginning: selectedLightDistanceToController: " + selectedLightDistanceToController.ToString ());

						calculateControlRange (selectedLightDistanceToController);
						isRangeCalculated = true;
						//i += 1;
						//Debug.Log ("i: " + i.ToString ());

						/*if (distanceFingerController >= minRangeValue && distanceFingerController <= maxRangeValue) {
							//Debug.Log ("In range");
						}*/
					}
					if (isRangeCalculated == true) {

						Debug.Log ("*********moveLightDepth start**********");

						//position light
						//moveLightDepth();

						//percentageLightPosition (selectedLightDistanceToController, onePercentOfWallControllerDistance);

						//distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);
						//percentageFingerPosition (distanceFingerController);

						moveLightDepth (light, light.transform.position);

						Debug.Log ("*********moveLightDepth end**********");


					}
				}
			}

			//x-axis
			/*if (DetectIndexFinger.leapTipPosition.x <= maxX && DetectIndexFinger.leapTipPosition.x >= -(minX)) {

				//Debug.Log ("Finger zwischen 70 und -70");

			}*/

		} else {
			isRangeCalculated = false;
			//i = 0;
		}


		
	}

	void calculateControlRange(float lightControllerDistanceBeginn){ //, float onePercentOfWallControllerDistance){
		

			//selectedLight = selectScript.GetSelectedLight();
			//Debug.Log ("1 returned selectedLight position" + selectedLight.transform.position.ToString());


			//Debug.Log ("maxwalldistance: " + maxWallDistance.ToString ());

			// 1 percentage position of light on raycast between controller and maxWallDistance
			//walldistance = 100%

			//float absoluteWallDistanceValueEqualsOnePercent = 100 / maxWallDistance; //variable equals one percent of distance between controller and wall //multiply scaleHelper with position of light
			//
			//selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition);
			//Debug.Log ("2 selectedLightDistanceToController absolut: " + selectedLightDistanceToController);
			//
			/*Vector3 distanceLightContr = selectedLight.transform.position - DetectIndexFinger.handControllerPos;
			Debug.Log ("vector lightContrdistance: " + distanceLightContr.ToString ());
			Debug.Log ("vector lightContrdistance betrag: " + distanceLightContr.magnitude.ToString ());
			*/


		//percentage position of light between controller and wall
		float percentagePosOfLightAtBeginning = getPercentageLightPosition(lightControllerDistanceBeginn); //((100 / maxWallDistance) * lightControllerDistanceBeginn); // like 26%
		Debug.Log ("percentage pos of Light at the beginning: " + percentagePosOfLightAtBeginning.ToString());


		// 2 
		//get leapFingerPos
		//Leap.Vector leapFingerPosInMeter = (DetectIndexFinger.leapTipPosition / 1000);

		//get start finger position
		//fingerPos = DetectIndexFinger.fingerPos;
			//Debug.Log ("FingerPos: " + fingerPos.ToString ());
			//Debug.Log ("handContr: " + DetectIndexFinger.handControllerPos.ToString ());

		//calculate start distance fromm finger to controller
		fingerControllerDistanceBegin = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);
			//Debug.Log ("distanceFingerController: " + distanceFingerController.ToString ());
			//
			/*Vector3 distance = fingerPos - DetectIndexFinger.handControllerPos;
			Debug.Log ("vector fingercontrdistance: " + distance.ToString ());
			Debug.Log ("vector fingercontrdistance betrag: " + distance.magnitude.ToString ());
			*/

		float percentagePosOfFingerAtBeginning = percentagePosOfLightAtBeginning; //adapt percentage position of light on position of finger
		Debug.Log ("percentage pos of finger at the beginning: " + percentagePosOfFingerAtBeginning.ToString());

			//devide absolute fingerPos by persentagePosition
			/*float absoluteFingerPosEquaslOnePercent = distanceFingerController / percentagePosOfLight;
			Debug.Log ("absoluteFingerPosEquaslOnePercent: " + absoluteFingerPosEquaslOnePercent.ToString ());
			float absoluteFingerPosEqualsHundretPercent = absoluteFingerPosEquaslOnePercent * 100;
			Debug.Log ("absoluteFingerPosEqualsHundretPercent: " + absoluteFingerPosEqualsHundretPercent.ToString ());*/

		//get one percent of 40cm range volume
		float onePercentOfFingerRange = fingerRangeVolume / 100;
		Debug.Log ("onePercentOfFingerRange: " + onePercentOfFingerRange.ToString());

			//need a final absoluteFingerPosEquaslOnePercent
			//working area 20cm: 10cm to 30cm
			//float absoluteFingerPosEquaslOnePercent = 0.40f / 100; //0,0020
			//Debug.Log ("absoluteFingerPosEquaslOnePercent, 0,0020: " + absoluteFingerPosEquaslOnePercent.ToString ());

		//calculate minFingerRange, starting on current fingerControllerDistance
		minFingerRange = fingerControllerDistanceBegin - (percentagePosOfFingerAtBeginning * onePercentOfFingerRange);
		Debug.Log ("minFingerRange: " + minFingerRange.ToString ());

			//float absoluteRangeValueUponFingerPos = percentagePosOfFinger * absoluteFingerPosEquaslOnePercent; //percentagePos * 0,0015
			//Debug.Log ("absoluteRangeValueUponFingerPos: " + absoluteRangeValueUponFingerPos.ToString ());

			//minRangeValue = distanceFingerController - absoluteRangeValueUponFingerPos;

		maxFingerRange = fingerControllerDistanceBegin + ((100 - percentagePosOfFingerAtBeginning) * onePercentOfFingerRange);
		Debug.Log ("maxFingerRange: " + maxFingerRange.ToString ());

			//distanceFingerController + ((100-percentagePosOfFinger) * absoluteFingerPosEquaslOnePercent);

		/*if (minRangeValue < 0.10) {
			//Debug.LogFormat ("minRangeValue < 0.10");
		}
		if (maxRangeValue > 0.60) {
			//Debug.LogFormat ("maxRangeValue > 0.0");
		}*/
		Debug.Log ("range calculated");
	}

	void moveLightDepth(GameObject light, Vector3 lightPosition){



		//get current distance between light and controller
		currentLightControllerDistance = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition); //selectedLightDistanceToController = 
		Debug.Log ("eingang moveLightDepth********");
		Debug.Log ("currentLightControllerDistance" + currentLightControllerDistance.ToString());


		currentFingerControllerDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);


			//fingerPos = DetectIndexFinger.fingerPos;
			//distanceFingerController = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPos);

		//check for range
		if (currentFingerControllerDistance >= minFingerRange && currentFingerControllerDistance <= maxFingerRange) {
			Debug.LogFormat ("in range");
		} else {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");
		}

			//float tempPercentFinger = getPercentageFingerPosition (currentFingerControllerDistance);

		float newLightControllerDistance = (maxWallDistance / 100) * getPercentageFingerPosition (currentFingerControllerDistance);
		Debug.Log ("newLightControllerDitance" + newLightControllerDistance.ToString());

		//get percentage pos of finger in range
			//float tmp = currentFingerControllerDistance / getPercentageFingerPosition(selectedLightDistanceToController);
		//float finalDistance = tmp * percentageFingerPosition(currentFingerControllerDistance);

		/*
		Vector3 normalizedLightVector = lightPosition.normalized;
		Vector3 newLightPosition = normalizedLightVector * newLightControllerDistance;
		*/

		//get vector orthogonal to xz-plane, length = light.y
		float lightY = light.transform.position.y ;
		Vector3 onlyYVector = new Vector3(0, lightY, 0);

		Vector3 movingVector = -(onlyYVector) + light.transform.position;

		Vector3 normalizedMovingVector = movingVector.normalized;

		Vector3 newLightPosition = onlyYVector + (normalizedMovingVector * newLightControllerDistance);
			
		light.transform.position = newLightPosition;


	}

	float getPercentageFingerPosition(float distance){

		float currentFingerMinFingerRangeDistance = distance - minFingerRange;
		Debug.Log ("currentFingerMinFingerRangeDistance: " + currentFingerMinFingerRangeDistance.ToString());

		float currentPercentageFingerPosInRange = ((100 / fingerRangeVolume) * currentFingerMinFingerRangeDistance); // like 26% ???
		Debug.Log ("currentPercentageFingerPosInRange: " + currentPercentageFingerPosInRange.ToString());

		//float currentPercentageFingerPos = ((100 / maxFingerRange) * currentFingerMinFingerRangeDistance); // like 26% ???

		
		//for test
		//float tmp = maxFingerRange * 1000.0f;
		//float dreisatz1 = maxRangeValue / tmp;

		//
		//float dreisatz1right = 100 / tmp;
		//float percentage = dreisatz1right * distance;

		Debug.Log ("currentPercentageFingerPos: " + currentPercentageFingerPosInRange.ToString());

		return currentPercentageFingerPosInRange;
	}

	float getPercentageLightPosition(float distance){

		float percentagePosOfLightAtBeginning = ((100 / maxWallDistance) * distance); // like 26%

		Debug.Log ("percentagePosOfLightAtBeginning: " + percentagePosOfLightAtBeginning.ToString());

		/*float tmp = maxWallDistance * 1000.0f;

		float dreisatz1right = 100 / tmp;
		float percentage = dreisatz1right * lightDistance;

		Debug.Log ("light percent: " + percentage);*/

		return percentagePosOfLightAtBeginning;

	}





}
