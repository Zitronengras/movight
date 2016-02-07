using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	//Color32 color;

	Camera camera;

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject intensityUp;

	GameObject light;
	Light lightSource;
	float intensity;
	Vector3 controlPoint;

	//Test
	GameObject cube;

	string lightSourceTag = "lightSource";

	//range
	//TODO balancing
	float intensityRangeVolume = 0.20f; //20 virtual cm
	float minYIntensityRange;
	float maxYIntensityRange;
	float screenYRange = 300.0f;


	//Vector3 upIntensityRange = new Vector3(0,0,1);
	//Vector3 downIntensityRange = new Vector3(0,0,1);

	//changeIntensity
	float newIntensity;
	int beforeChangeBuffer = 30;
	int buffer = 0;

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

	//checkForMeaningfulYChanges
	float lastYOnScreen;

	//TODO for horizontal

	// Use this for initialization
	void Start () {

		Debug.Log("*****************************************************************");

		camera = Camera.main; // GetComponent<Camera>;

		labelScriptObject = GameObject.Find ("IntensityLabelObjectNew");
		Debug.Log ("*************labelScriptObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		intensityUp = GameObject.Find ("IntensityUp");
		Debug.Log ("*************labelScriptObject: " + intensityUp.ToString ());
		intensityUp.SetActive(false);
	
	}

	//TODO buffer einbauen

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
				//lightSource.color = color;

				controlPoint = Gestures.controlPoint;

				/*Vector3 onScreenPosition = camera.WorldToScreenPoint(controlPoint);
				float currentyOnScreen = onScreenPosition.y;
				Debug.Log("#############################onScreenY " + currentyOnScreen.ToString());*/

				checkForMeaningfulChangesEntrance (controlPoint);

				if (intensityShouldChange) {

					//Debug.Log ("now changeIntensity()");
					intensityUp.SetActive (true);
					changeIntensity (controlPoint, intensity, lightSource);

				} else {

					intensityUp.SetActive(false);

				}
			} else {

				//labelScriptObject.SetActive(false);
				//intensityUp.SetActive(false);

			}
		} else {

			//labelScriptObject.SetActive(false);
			//intensityUp.SetActive(false);

		}
	}

	void calculateVerticalRange(Vector3 currentPosition, float intensity){ 

		float currentIntensity = intensity;

		//convert currentPosition into 2D (screen)
		Vector3 onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		float currentYOnScreen = onScreenPosition.y;

		Debug.Log ("current Y at start" + currentYOnScreen.ToString ());
		//get percentage intensity
		//percentage position of fist between display left and display right

		float percentageIntensityAtBeginning = getPercentageIntensity(currentIntensity);
		Debug.Log ("percentageIntensityAtBeginning at the beginning: " + percentageIntensityAtBeginning.ToString());

		float percentagePosOnScreenAtBeginning = percentageIntensityAtBeginning; //adapt percentage position of light on position of finger
		Debug.Log ("percentagePosOfFistAtBeginning at the beginning: " + percentagePosOnScreenAtBeginning.ToString());

		float onePercentOfScreenRange = screenYRange / 100;

		//	float screenXRange = 280.0f;


		//get one percent of 30cm range volume
		//float onePercentOfIntensityRange = intensityRangeVolume / 100;
		//Debug.Log ("onePercentOfFingerRange: " + onePercentOfFingerRange.ToString());

		//for check
		//get value for range from percentageIntensity
		//float percentageRangeValueAtBeginning = onePercentOfIntensityRange * percentagePosOfFistAtBeginning;
		//Debug.Log ("sollte gleich sein wie anfangs Intensität: " + percentageRangeValueAtBeginning.ToString ());

		//get start y value
		//float startY = currentY;

		//
		maxYIntensityRange = currentYOnScreen + ((100 - percentagePosOnScreenAtBeginning) * onePercentOfScreenRange);
		Debug.Log ("maxYIntensityRange: " + maxYIntensityRange.ToString ());

		minYIntensityRange = currentYOnScreen - (percentagePosOnScreenAtBeginning * onePercentOfScreenRange);
		Debug.Log ("minYIntensityRange: " + minYIntensityRange.ToString ());


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


			buffer += 1;

			Vector3 currentOnScreenPosition = camera.WorldToScreenPoint (controlPoint);

			float currentOnScreenY = currentOnScreenPosition.y;

			float percentageOnScreenPalmPosition = getPercentageFistPosition (currentOnScreenY);
			newIntensity = ((possibleMax / 100.0f) * percentageOnScreenPalmPosition); 
			Debug.Log ("new intensity: " + newIntensity.ToString ());
			lightSource.intensity = newIntensity;

			if (buffer >= beforeChangeBuffer) {
				checkForMeaningfulYChanges (controlPoint);
				//TODO not correct??
			}
		}

	}

	float getPercentageFistPosition(float currentOnScreenY){
		
		//Debug.Log ("currentY: " + currentY.ToString ());
		float currentPositionDistance = currentOnScreenY - minYIntensityRange; 
		Debug.Log ("currentPositionDistance: " + currentPositionDistance.ToString ());

		float percentagFistY = (100 / screenYRange) * currentPositionDistance;

		//check for range
		if (percentagFistY > 100) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentagFistY = 100;

		}
		if (percentagFistY < 0) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentagFistY = 0;

		}

		Debug.Log ("percentageFistPosition: " + percentagFistY.ToString ());
		return percentagFistY;
	}

	float getPercentageIntensity(float intensity){

		//float onePercentOfIntensityMax = possibleMax / 100;
		//float percentageIntensity = onePercentOfIntensityMax * intensity;

		float percentageIntensity = (100f / possibleMax) * intensity;

		//check for range
		if (percentageIntensity > 100) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentageIntensity = 100;

		}
		if (percentageIntensity < 0) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			percentageIntensity = 0;

		}


		Debug.Log ("percentageIntensity: " + percentageIntensity.ToString());

		return percentageIntensity;
	}


	//TODO for x and z values as well???
	void checkForMeaningfulChangesEntrance(Vector3 controlPoint){

		float changeValue = 0.0015f; //0.001f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			//Debug.Log ("no big change***********************");

			if (!(newPosition.x == lastPosition.x) && !(newPosition.y == lastPosition.y) && !(newPosition.z == lastPosition.z)) {

				//Debug.Log ("values " + newPosition.x.ToString () + newPosition.y.ToString () + newPosition.z.ToString ());

				changeCounter += 1;
				//Debug.Log ("changeCounter " + changeCounter.ToString());

				if (changeCounter == SelectLight.waitCountdown) {

					intensityShouldChange = true;
					intensityUp.SetActive (true);

					//Debug.Log("######## intensityShouldChange ###### " + intensityShouldChange.ToString());
					changeCounter = 0;

				}
			}
		}

		Debug.Log ("shouldIntensityChange??????: " + intensityShouldChange.ToString ());

		lastPosition = controlPoint;

	}

	void checkForMeaningfulYChanges(Vector3 controlPoint){

		float changeValue = 3.0f; //0.001f;

		Vector3 onScreenPosition = camera.WorldToScreenPoint (controlPoint);
		float currentYOnScreen = onScreenPosition.y;

		if (currentYOnScreen <= (lastYOnScreen + changeValue) && currentYOnScreen >= (lastYOnScreen - changeValue)) {

			//Debug.Log ("no big change***********************");

			if (!(currentYOnScreen == lastYOnScreen)) { //if hand is out of controller


				Progressbar.fillProgressbar ();

				//Debug.Log ("values " + newPosition.x.ToString () + newPosition.y.ToString () + newPosition.z.ToString ());

				changeCounter += 1;
				Debug.Log ("changeCounter " + changeCounter.ToString ());

				if (changeCounter == SelectLight.waitCountdown) {

					intensityShouldChange = false;
					intensityUp.SetActive (false);
					Progressbar.progressbarObject.SetActive (false);
					//TODO
					//isVerticalRangeCalculated = false;

					//Debug.Log("######## intensityShouldChange ###### " + intensityShouldChange.ToString());
					changeCounter = 0;
					buffer = 0;

				}
			}
		} else {
			changeCounter = 0;
			Progressbar.progressbarObject.SetActive (false);
		}

		lastYOnScreen = currentYOnScreen;

	}
}
