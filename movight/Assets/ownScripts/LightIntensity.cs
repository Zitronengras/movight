﻿using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	//Color32 color;

	Camera camera;

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject intensityUpDown;

	GameObject light;
	Light lightSource;
	float intensity;
	Vector3 controlPoint;

	Vector3 onScreenPosition;
	float currentYOnScreen;

	//Test
	GameObject cube;

	string lightSourceTag = "lightSource";

	//range
	//TODO balancing
	float intensityRangeVolume = 0.20f; //20 virtual cm
	float minYIntensityRange;
	float maxYIntensityRange;
	float screenYRange = 300.0f;
	float currentIntensity;
	float percentageIntensityAtBeginning;
	float percentagePosOnScreenAtBeginning;
	float onePercentOfScreenRange;
	float currentOnScreenY;

	//changeIntensity
	float newIntensity;
	int beforeChangeBuffer = 30;
	int buffer = 0;
	float percentageOnScreenPalmPosition;

	//getPercentageIntensity
	float possibleMax = 8.0f;
	float possibleMin = 0.0f;

	//getPercentageFistPosition
	float currentPositionDistance;
	float percentagFistY;
	float percentageIntensity;

	bool isVerticalRangeCalculated = false;
	bool isControlActive = false;

	//checkForMeaningfulChangesEntrance
	float changeValue;
	Vector3 newPosition;
	Vector3 lastPosition;
	int changeCounter = 0;
	public static bool intensityShouldChange = false;

	//checkForMeaningfulYChanges
	float lastYOnScreen = 0;
	float changeYValue;

	// Use this for initialization
	void Start () {

		camera = Camera.main;

		labelScriptObject = GameObject.Find ("IntensityLabelObjectNew");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		intensityUpDown = GameObject.Find ("IntensityUpDown");
		intensityUpDown.SetActive(false);
	
	}
		
	// Update is called once per frame
	void Update () {

		if (SelectLight.isLightSelected == true) {

			light = SelectLight.light; //selected light		

			if (Gestures.isIntensityGesture) {
				
				Debug.Log ("im intensity script******************************************************");

				labelScriptObject.SetActive(true);
				labelScript.displayLabel (controlPoint, labelScriptObject);
				lightSource = light.GetComponentInChildren<Light> ();
				intensity = lightSource.intensity;

				controlPoint = Gestures.controlPoint;
				checkForMeaningfulChangesEntrance (controlPoint);

				if (intensityShouldChange) {

					intensityUpDown.SetActive (true);
					changeIntensity (controlPoint, intensity, lightSource);

				} else {

					intensityUpDown.SetActive(false);

				}
			} else {

				labelScriptObject.SetActive(false);
				//intensityUp.SetActive(false);

			}
		} else {

			//labelScriptObject.SetActive(false);
			//intensityUp.SetActive(false);

		}
	}

	void calculateVerticalRange(Vector3 currentPosition, float intensity){ 

		currentIntensity = intensity;

		//convert currentPosition into 2D (screen)
		onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		currentYOnScreen = onScreenPosition.y;

		//Debug.Log ("current Y at start" + currentYOnScreen.ToString ());
		//get percentage intensity
		//percentage position of fist between display left and display right
		percentageIntensityAtBeginning = getPercentageIntensity(currentIntensity);
		//Debug.Log ("percentageIntensityAtBeginning at the beginning: " + percentageIntensityAtBeginning.ToString());

		percentagePosOnScreenAtBeginning = percentageIntensityAtBeginning; //adapt percentage position of light on position of finger
		//Debug.Log ("percentagePosOfFistAtBeginning at the beginning: " + percentagePosOnScreenAtBeginning.ToString());

		onePercentOfScreenRange = screenYRange / 100;

		//
		maxYIntensityRange = currentYOnScreen + ((100 - percentagePosOnScreenAtBeginning) * onePercentOfScreenRange);
		//Debug.Log ("maxYIntensityRange: " + maxYIntensityRange.ToString ());

		minYIntensityRange = currentYOnScreen - (percentagePosOnScreenAtBeginning * onePercentOfScreenRange);
		//Debug.Log ("minYIntensityRange: " + minYIntensityRange.ToString ());


		//TODO was wenn min in negativ bereich???
		/*if (minRangeValue < 0.10) {
			//Debug.LogFormat ("minRangeValue < 0.10");
		}
		if (maxRangeValue > 0.60) {
			//Debug.LogFormat ("maxRangeValue > 0.0");
		}*/

		//isVerticalRangeCalculated = true;

		Debug.Log ("range calculated");
	}

	void changeIntensity(Vector3 controlPoint, float intensity, Light lightSource){

		//Debug.Log ("current Y : " + currentY.ToString ());

		if (isVerticalRangeCalculated == false) {
			calculateVerticalRange (controlPoint, intensity);
			isVerticalRangeCalculated = true;
		}
		if (isVerticalRangeCalculated == true) {


			//buffer += 1;

			onScreenPosition = camera.WorldToScreenPoint (controlPoint);

			currentOnScreenY = onScreenPosition.y;

			checkForMeaningfulYChanges (currentOnScreenY);

			percentageOnScreenPalmPosition = getPercentageFistPosition (currentOnScreenY);
			newIntensity = ((possibleMax / 100.0f) * percentageOnScreenPalmPosition); 
			Debug.Log ("new intensity: " + newIntensity.ToString ());
			lightSource.intensity = newIntensity;

			//if (buffer >= beforeChangeBuffer) {
			//}
		}

	}

	float getPercentageFistPosition(float currentOnScreenY){
		
		//Debug.Log ("currentY: " + currentY.ToString ());
		currentPositionDistance = currentOnScreenY - minYIntensityRange; 
		Debug.Log ("currentPositionDistance: " + currentPositionDistance.ToString ());

		percentagFistY = (100 / screenYRange) * currentPositionDistance;

		//check for range
		if (percentagFistY > 100) {

			percentagFistY = 100;

		}
		if (percentagFistY < 0) {

			percentagFistY = 0;

		}

		Debug.Log ("percentageFistPosition: " + percentagFistY.ToString ());
		return percentagFistY;
	}

	float getPercentageIntensity(float intensity){

		percentageIntensity = (100f / possibleMax) * intensity;

		//check for range
		if (percentageIntensity > 100) {

			percentageIntensity = 100;

		}
		if (percentageIntensity < 0) {

			percentageIntensity = 0;

		}


		Debug.Log ("percentageIntensity: " + percentageIntensity.ToString());

		return percentageIntensity;
	}


	//TODO for x and z values as well???
	void checkForMeaningfulChangesEntrance(Vector3 controlPoint){

		changeValue = 0.0015f; //0.001f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			//Debug.Log ("no big change***********************");

			if (!(newPosition.x == lastPosition.x) && !(newPosition.y == lastPosition.y) && !(newPosition.z == lastPosition.z)) {

				changeCounter += 1;

				if (changeCounter == SelectLight.waitCountdown) {

					intensityShouldChange = true;
					intensityUpDown.SetActive (true);

					changeCounter = 0;

				}
			}
		}

		//Debug.Log ("shouldIntensityChange??????: " + intensityShouldChange.ToString ());

		lastPosition = controlPoint;

	}

	void checkForMeaningfulYChanges(float currentPosition){

		changeYValue = 2.0f; //0.001f;

		float compareAddition = lastYOnScreen + changeYValue;
		float compareSubstraction = lastYOnScreen - changeYValue;



		/*onScreenPosition = camera.WorldToScreenPoint (controlPoint);
		currentYOnScreen = onScreenPosition.y;*/
		Debug.Log ("lastY: " + lastYOnScreen.ToString ());
		Debug.Log ("currentPosition: " + currentPosition.ToString ());
		Debug.Log ("compareSubstraction: " + compareSubstraction.ToString ());

		Debug.Log ("lastYOnScreen + changeValue: " + (lastYOnScreen + changeValue).ToString ());


		if ((currentPosition <= compareAddition) && (currentPosition >= compareSubstraction)) {
			Debug.Log ("####################################################");

			//if (!(currentPosition == lastYOnScreen)) { //if hand is out of controller

				changeCounter += 1;
				Progressbar.fillProgressbar ();
				//Debug.Log ("changeCounter " + changeCounter.ToString ());

				if (changeCounter == SelectLight.waitCountdown) {

					Progressbar.resetProgressbar ();
					intensityShouldChange = false;
					intensityUpDown.SetActive (false);
					//TODO
					//isVerticalRangeCalculated = false;

					//Debug.Log("######## intensityShouldChange ###### " + intensityShouldChange.ToString());
					changeCounter = 0;
					buffer = 0;

				}
			//}
		} else {
			changeCounter = 0;
			Progressbar.resetProgressbar ();
		}

		lastYOnScreen = currentPosition;

	}
}
