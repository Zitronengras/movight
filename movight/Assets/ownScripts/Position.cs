using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;

	float maxWallDistance;
	float onePercentOfWallControllerDistance;

	LayerMask onlyLightLayer;


//	float selectedLightDistanceToController;
	GameObject light;
	Vector3 lightPosition;
	Vector3 controlPoint;

	//calculateDepthRange
	bool isDepthRangeCalculated = false;
	float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;

	//Raycaster
	RaycastHit hitObject = new RaycastHit();
	GameObject hitLight;
	public static bool lightShouldMove = false;

	//moveLight
	float currentLightControllerDistance;
	float currentFingerControllerDistance;
	float lastX;
	float lastZ;

	//checkForMeaningfulChanges
	float changeValue = 0.001f;
	int xCounter = 0;
	int zCounter = 0;

	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		labelScriptObject = GameObject.Find("PositionLabelObject");
		//Debug.Log ("PositionLabelObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		//Debug.Log ("labelScript: " + labelScript.ToString ());
		labelScriptObject.SetActive(false);


		//GameObject selectScriptObject = GameObject.Find ("SelectLight");
		//selectScript = selectScriptObject.GetComponent<SelectLight> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (ConstructionDistance.isMaxDistanceDetermined == true){

			//get maxWallDistance
			maxWallDistance = ConstructionDistance.maxWallDistance;
			onePercentOfWallControllerDistance = 100 / maxWallDistance; //variable equals one percent of distance between controller and wall

			if (SelectLight.isLightSelected == true) {

				light = SelectLight.light; //selected light
				lightPosition = light.transform.position; //position of selected light

				if (Gestures.isPositionGesture == true) { //beeing in selectionsequence

					//Debug.Log ("in position script and gesture == true");

					controlPoint = Gestures.controlPoint;

					labelScript.displayLabel (controlPoint, labelScriptObject);


					if (lightShouldMove == false) {
						
						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							hitLight = hitObject.collider.gameObject;

							Debug.Log ("Licht getroffen");

							if(Equals(hitLight.name, light.name)){

								lightShouldMove = true;
								//TODO do not look for other gesture when moving

							}

						}
					}
					if(lightShouldMove == true) {

						moveLight (light, lightPosition, controlPoint);

					}
				}
			}

		} else {
			isDepthRangeCalculated = false;
		}
		
	}

	void calculateDepthRange(float lightControllerDistanceBeginn, Vector3 fingerPosition){ 
		
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
		}*/
		Debug.Log ("range calculated");
	}

	public void moveLight(GameObject light, Vector3 lightPosition, Vector3 controlPoint){

		if (isDepthRangeCalculated == false) {

			//Debug.Log ("at the beginning: selectedLightDistanceToController: " + selectedLightDistanceToController.ToString ());

			float selectedLightDistanceToController = Vector3.Distance(Gestures.handControllerPos, SelectLight.lightPosition);

			calculateDepthRange (selectedLightDistanceToController, controlPoint);
			isDepthRangeCalculated = true;

		}

		if (isDepthRangeCalculated == true) {

			//get current distance between finger and controller for getPercentageFingerPosition(...)
			currentFingerControllerDistance = Vector3.Distance (Gestures.handControllerPos, controlPoint); // ????????????????????????
			//Debug.Log ("currentFingerControllerDistance" + currentFingerControllerDistance.ToString());			

			float percentageFingerPosition = getPercentageFingerPosition (currentFingerControllerDistance);

			float depth = (maxWallDistance / 100) * percentageFingerPosition;
			//Debug.Log ("depth: " + depth.ToString ());

			//get orthogonal vector to xz-plane, length = light.y
			float lightY = SelectLight.light.transform.position.y ;
			Vector3 onlyYVector = new Vector3(0, lightY, 0);
			float onlyYVectorLength = onlyYVector.magnitude;

			//get length for vector to new light position
			float aSquare = onlyYVectorLength * onlyYVectorLength;
			//Debug.Log ("aSquare: " + aSquare.ToString());

			float bSquare = depth * depth;
			//Debug.Log ("bSquare: " + bSquare);

			// c² = a² + b²
			float newLightVectorLength = Mathf.Sqrt(aSquare + bSquare); 
			//Debug.Log ("newLightVectorLength: " + newLightVectorLength.ToString());

			//get direction to fingerTip
			Vector3 normalizedFingerDirection = controlPoint.normalized;
			//direction multiply with new length
			Vector3 newLightVector = normalizedFingerDirection * newLightVectorLength;

			SelectLight.light.transform.position = new Vector3(newLightVector.x, lightY, newLightVector.z); //newLightPosition;

			checkForMeaningfulChangesX (lastX, newLightVector.x);
			checkForMeaningfulChangesZ (lastZ, newLightVector.z);

			//Debug.Log("new light position: " + light.transform.position.ToString());

			lastX = newLightVector.x;
			lastZ = newLightVector.z;
		}
	}

	float getPercentageFingerPosition(float distance){

		float currentFingerMinFingerRangeDistance = distance - minFingerRange;
		//Debug.Log ("currentFingerMinFingerRangeDistance: " + currentFingerMinFingerRangeDistance.ToString());

		float currentPercentageFingerPosInRange = ((100 / fingerRangeVolume) * currentFingerMinFingerRangeDistance); // like 26% ???
		//Debug.Log ("currentPercentageFingerPosInRange: " + currentPercentageFingerPosInRange.ToString());

		//check for range
		if (currentPercentageFingerPosInRange > 100) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			currentPercentageFingerPosInRange = 100;

		}
		if (currentPercentageFingerPosInRange < 0) {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");

			currentPercentageFingerPosInRange = 0;

		}

		//Debug.Log ("percentageFingerPosition: " + currentPercentageFingerPosInRange.ToString());

		return currentPercentageFingerPosInRange;
	}

	float getPercentageLightPosition(float distance){

		float percentagePosOfLightAtBeginning = ((100 / maxWallDistance) * distance); // like 26%
		//Debug.Log ("percentagePosOfLightAtBeginning: " + percentagePosOfLightAtBeginning.ToString());

		return percentagePosOfLightAtBeginning;

	}

	void checkForMeaningfulChangesX(float lastValue, float newValue){

		if(newValue <= (lastValue + changeValue) && newValue >= (lastValue - changeValue)){

			xCounter += 1;

			if (xCounter == SelectLight.deselectCountdown) {

				//disable current positioning selection
				lightShouldMove = false;

				Debug.Log ("lampe positioniert");

				xCounter = 0;
				zCounter = 0;
			}
		}
	}
	void checkForMeaningfulChangesZ(float lastValue, float newValue){

		if(newValue <= (lastValue + changeValue) && newValue >= (lastValue - changeValue)){

			zCounter += 1;

			if (zCounter == SelectLight.waitCountdown) {

				//disable current positioning selection
				lightShouldMove = false;

				Debug.Log ("lampe positioniert");

				zCounter = 0;
				xCounter = 0;
			}
		}
	}

}
