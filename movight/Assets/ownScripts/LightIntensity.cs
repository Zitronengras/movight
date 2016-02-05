using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	Vector3 controlPoint;

	float intensityRangeVolume = 0.30f; //20 virtual cm

	bool isVerticalRangeCalculated = false;
	bool isControlActive = false;

	//checkForMeaningfulChanges
	Vector3 newPosition;
	Vector3 lastPosition = new Vector3(0,0,0);
	int changeCounter = 0;
	bool intensityShouldChange = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Gestures.isIntensityGesture) {

			//Debug.Log ("in intensity script");

			controlPoint = Gestures.controlPoint;

			checkForMeaningFulChanges (controlPoint);

			if (intensityShouldChange) {

				//Debug.Log ("now changeIntensity()");
				
				//changeIntensity (controlPoint);

			}
				
		}

	}

	/*void calculateVerticalRange(Vector3 controlPoint){ 

		//percentage position of light between controller and wall
		float percentagePosOfLightAtBeginning = getPercentageLightPosition(lightControllerDistanceBeginn);
		//Debug.Log ("percentage pos of Light at the beginning: " + percentagePosOfLightAtBeginning.ToString());

		//calculate start distance from finger to controller
		fingerControllerDistanceBegin = Vector3.Distance (Gestures.handControllerPos, fingerPosition);
		//Debug.Log ("fingerControllerDistanceBegin: " + fingerControllerDistanceBegin.ToString ());

		float percentagePosOfFingerAtBeginning = percentagePosOfLightAtBeginning; //adapt percentage position of light on position of finger
		//Debug.Log ("percentage pos of finger at the beginning: " + percentagePosOfFingerAtBeginning.ToString());

		//get one percent of 40cm range volume
		float onePercentOfFingerRange = fingerRangeVolume / 100;
		//Debug.Log ("onePercentOfFingerRange: " + onePercentOfFingerRange.ToString());

		//calculate minFingerRange, starting on current fingerControllerDistance
		minFingerRange = fingerControllerDistanceBegin - (percentagePosOfFingerAtBeginning * onePercentOfFingerRange);
		//Debug.Log ("minFingerRange: " + minFingerRange.ToString ());

		maxFingerRange = fingerControllerDistanceBegin + ((100 - percentagePosOfFingerAtBeginning) * onePercentOfFingerRange);
		//Debug.Log ("maxFingerRange: " + maxFingerRange.ToString ());

		//TODO was wenn min in negativ bereich???
		/*if (minRangeValue < 0.10) {
			//Debug.LogFormat ("minRangeValue < 0.10");
		}
		if (maxRangeValue > 0.60) {
			//Debug.LogFormat ("maxRangeValue > 0.0");
		}*
		Debug.Log ("range calculated");
	}

	void changeIntensity(){

		if (isVerticalRangeCalculated == false) {
			//calculateVerticalRange ();
		}
		if(isVerticalRangeCalculated == true){

		}

	}*/

	void checkForMeaningFulChanges(Vector3 controlPoint){

		float changeValue = 0.001f;
		newPosition = controlPoint;

		if (newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			changeCounter += 1;
			//Debug.Log ("changeCounter " + changeCounter.ToString());


			if (changeCounter == SelectLight.waitCountdown) {

				if (intensityShouldChange == true) {
					intensityShouldChange = false;
				} else {
					intensityShouldChange = true;
				}

				Debug.Log("######## intensityShouldChange ###### " + intensityShouldChange.ToString());
				changeCounter = 0;

			}
		}

		lastPosition = controlPoint;

	}

}
