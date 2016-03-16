using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

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
	int entranceChangeCounter = 0;
	public static bool intensityShouldChange = false;

	//checkForMeaningfulYChanges
	float lastYOnScreen = 0;
	float changeYValue;
	float compareAddition;
	float compareSubstraction;

	// Use this for initialization
	void Start () {

		camera = Camera.main;

		labelScriptObject = GameObject.Find ("IntensityLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		intensityUpDown = GameObject.Find ("IntensityUpDown");
		intensityUpDown.SetActive(false);
	
	}
		
	// Update is called once per frame
	void Update () {

		if (SelectLight.isLightSelected == true) {

			light = SelectLight.light; //selected light		

			if (Gestures.isIntensityGesture == true) {
				
				controlPoint = Gestures.controlPoint;

				labelScript.displayLabel (controlPoint, labelScriptObject);

				checkForMeaningfulChangesEntrance (controlPoint);

				if (intensityShouldChange == true) {

					lightSource = light.GetComponentInChildren<Light> ();
					intensity = lightSource.intensity;

					intensityUpDown.SetActive (true);
					changeIntensity (controlPoint, intensity, lightSource);

				} 
			} else {

				labelScriptObject.SetActive(false);

			}
		}
	}

	void calculateVerticalRange(Vector3 currentPosition, float intensity){ 

		currentIntensity = intensity;

		//convert currentPosition into 2D (screen)
		onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		currentYOnScreen = onScreenPosition.y;

		//get percentage intensity
		//percentage position of fist between display left and display right
		percentageIntensityAtBeginning = getPercentageIntensity(currentIntensity);

		percentagePosOnScreenAtBeginning = percentageIntensityAtBeginning; //adapt percentage position of light on position of finger

		onePercentOfScreenRange = screenYRange / 100;

		maxYIntensityRange = currentYOnScreen + ((100 - percentagePosOnScreenAtBeginning) * onePercentOfScreenRange);

		minYIntensityRange = currentYOnScreen - (percentagePosOnScreenAtBeginning * onePercentOfScreenRange);

	}

	void changeIntensity(Vector3 controlPoint, float intensity, Light lightSource){

		if (isVerticalRangeCalculated == false) {
			calculateVerticalRange (controlPoint, intensity);
			isVerticalRangeCalculated = true;
		}
		if (isVerticalRangeCalculated == true) {

			onScreenPosition = camera.WorldToScreenPoint (controlPoint);

			currentOnScreenY = onScreenPosition.y;


			percentageOnScreenPalmPosition = getPercentageFistPosition (currentOnScreenY);
			newIntensity = ((possibleMax / 100.0f) * percentageOnScreenPalmPosition); 
			lightSource.intensity = newIntensity;

			checkForMeaningfulYChanges (currentOnScreenY);

		
		}

	}

	float getPercentageFistPosition(float currentOnScreenY){
		
		currentPositionDistance = currentOnScreenY - minYIntensityRange; 

		percentagFistY = (100 / screenYRange) * currentPositionDistance;

		//check for range
		if (percentagFistY > 100) {

			percentagFistY = 100;

		}
		if (percentagFistY < 0) {

			percentagFistY = 0;

		}

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
			
		return percentageIntensity;
	}

	void checkForMeaningfulChangesEntrance(Vector3 controlPoint){

		changeValue = 0.0019f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {


			if (!(newPosition.x == lastPosition.x) && !(newPosition.y == lastPosition.y) && !(newPosition.z == lastPosition.z)) {

				entranceChangeCounter += 1;

				if (entranceChangeCounter == SelectLight.waitCountdown) {

					intensityShouldChange = true;
					intensityUpDown.SetActive (true);
					isVerticalRangeCalculated = false;

					entranceChangeCounter = 0;

				}
			} else {				
				entranceChangeCounter = 0;
			}
		} else {				
			entranceChangeCounter = 0;
		}
			
		lastPosition = controlPoint;

	}

	void checkForMeaningfulYChanges(float currentPosition){

		changeYValue = 6.0f;

		compareAddition = lastYOnScreen + changeYValue;
		compareSubstraction = lastYOnScreen - changeYValue;

		if ((currentPosition <= compareAddition) && (currentPosition >= compareSubstraction)) {
			
			Progressbar.fillProgressbar (changeCounter);
			changeCounter += 1;

			if (changeCounter == SelectLight.waitCountdown) {

				intensityShouldChange = false;
				intensityUpDown.SetActive (false);
				isVerticalRangeCalculated = false;
				Progressbar.resetProgressbar ();

				changeCounter = 0;

			}
		} else {
			
			changeCounter = 0;
			Progressbar.resetProgressbar ();

		}
			
		lastYOnScreen = currentPosition;

	}
}
