using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject light;
	Light lightSource;
	float intensity;
	Vector3 controlPoint;

	//Test
	GameObject cube;

	string lightSourceTag = "lightSource";

	//range
	//TODO balancing
	float intensityRangeVolume = 0.60f; //20 virtual cm
	float minYIntensityRange;
	float maxYIntensityRange;


	//Vector3 upIntensityRange = new Vector3(0,0,1);
	//Vector3 downIntensityRange = new Vector3(0,0,1);

	//changeIntensity
	float newIntensity;

	//getPercentageIntensity
	float possibleMax = 8.0f;
	float possibleMin = 0.0f;

	bool isVerticalRangeCalculated = false;
	bool isControlActive = false;

	//checkForMeaningfulChanges
	Vector3 newPosition;
	Vector3 lastPosition;
	int changeCounter = 0;
	public static bool intensityShouldChange = false;

	//TODO for horizontal

	// Use this for initialization
	void Start () {

		labelScriptObject = GameObject.Find("IntensityLabelObject");
		//Debug.Log ("labelScriptObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);



		//prepare vector forhorizontalRange
		//upIntensityRange = Quaternion.Euler (42, 0, 0) * upIntensityRange;
		//downIntensityRange = Quaternion.Euler (-42, 0, 0) * downIntensityRange;
	
	}

	//TODO buffer einbauen

	// Update is called once per frame
	void Update () {

		if (SelectLight.isLightSelected == true) {

			light = SelectLight.light; //selected light		

			if (Gestures.isIntensityGesture) {
				
				//Debug.Log ("im intensity script******************************************************");

				labelScript.displayLabel (controlPoint, labelScriptObject);
				lightSource = light.GetComponentInChildren<Light> ();
				controlPoint = Gestures.controlPoint;

				checkForMeaningfulChanges (controlPoint);

				if (intensityShouldChange) {

					//Debug.Log ("now changeIntensity()");

					changeIntensity (controlPoint, intensity, lightSource);

				}
			} else {

				labelScriptObject.SetActive(false);

			}
		}
	}

	void calculateVerticalRange(float currentY, float intensity){ 

		float currentIntensity = intensity;

		//percentage position of fist between display left and display right
		float percentageIntensityAtBeginning = getPercentageIntensity(currentIntensity);
		//Debug.Log ("percentage pos of Light at the beginning: " + percentagePosOfLightAtBeginning.ToString());

		float percentagePosOfFistAtBeginning = percentageIntensityAtBeginning; //adapt percentage position of light on position of finger
		//Debug.Log ("percentage pos of finger at the beginning: " + percentagePosOfFingerAtBeginning.ToString());

		//get one percent of 30cm range volume
		float onePercentOfIntensityRange = intensityRangeVolume / 100;
		//Debug.Log ("onePercentOfFingerRange: " + onePercentOfFingerRange.ToString());

		//for check
		//get value for range from percentageIntensity
		float percentageRangeValueAtBeginning = onePercentOfIntensityRange * percentagePosOfFistAtBeginning;
		//Debug.Log ("sollte gleich sein wie anfangs Intensität: " + percentageRangeValueAtBeginning.ToString ());

		//get start y value
		float startY = currentY;

		//
		maxYIntensityRange = startY + ((100 - percentagePosOfFistAtBeginning) * onePercentOfIntensityRange);
		//Debug.Log ("maxYIntensityRange: " + maxYIntensityRange.ToString ());

		minYIntensityRange = startY - (percentagePosOfFistAtBeginning * onePercentOfIntensityRange);
		//Debug.Log ("minYIntensityRange: " + minYIntensityRange.ToString ());


		//TODO was wenn min in negativ bereich???
		/*if (minRangeValue < 0.10) {
			//Debug.LogFormat ("minRangeValue < 0.10");
		}
		if (maxRangeValue > 0.60) {
			//Debug.LogFormat ("maxRangeValue > 0.0");
		}*/
		Debug.Log ("range calculated");
	}

	void changeIntensity(Vector3 controlPoint, float intensity, Light lightSource){

		float currentY = controlPoint.y;

		if (isVerticalRangeCalculated == false) {
			calculateVerticalRange (currentY, intensity);
			isVerticalRangeCalculated = true;
		}
		if(isVerticalRangeCalculated == true){

			newIntensity = getPercentageFistPosition(currentY);
			lightSource.intensity = newIntensity;

		}

	}

	float getPercentageFistPosition(float currentY){

		float percentagFistY = (100 / intensityRangeVolume) * currentY;
		return percentagFistY;
	}

	float getPercentageIntensity(float intensity){

		float onePercentOfIntensityMax = possibleMax / 100;
		float percentageIntensity = onePercentOfIntensityMax * intensity;

		//check for range
		if (percentageIntensity > 100) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentageIntensity = 100;

		}
		if (percentageIntensity < 0) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentageIntensity = 0;

		}


		//Debug.Log ("percentageFingerPosition: " + currentPercentageFingerPosInRange.ToString());

		return percentageIntensity;
	}


	//TODO for x and z values as well???
	void checkForMeaningfulChanges(Vector3 controlPoint){

		float changeValue = 0.0001f; //0.001f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			Debug.Log ("no big change***********************");

			if (!(newPosition.x == lastPosition.x) && !(newPosition.y == lastPosition.y) && !(newPosition.z == lastPosition.z)) {

				Debug.Log ("values " + newPosition.x.ToString () + newPosition.y.ToString () + newPosition.z.ToString ());

				changeCounter += 1;
				Debug.Log ("changeCounter " + changeCounter.ToString());

				if (changeCounter == SelectLight.waitCountdown) {

					if (intensityShouldChange == true) {
						intensityShouldChange = false;
					} else {
						intensityShouldChange = true;
					}

					//Debug.Log("######## intensityShouldChange ###### " + intensityShouldChange.ToString());
					changeCounter = 0;

				}

			}
		}

		Debug.Log ("shouldIntensityChange??: " + intensityShouldChange.ToString ());

		lastPosition = controlPoint;

	}

}
