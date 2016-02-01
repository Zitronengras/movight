using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	SelectLight selectScript;

	float maxWallDistance;
	float onePercentOfWallControllerDistance;

	GameObject light;
	Vector3 lightPosition;
	float selectedLightDistanceToController;

	Vector3 fingerPos;

	//calculateDepthRange
	bool isDepthRangeCalculated = false;
	float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;

	//calculateHorizontelRange
	bool isHorizontalRangeCalculated = false;
	/*float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;*/

	//moveLightDepth
	float currentLightControllerDistance;
	float currentFingerControllerDistance;

	// Use this for initialization
	void Start () {

		GameObject selectScriptObject = GameObject.Find ("SelectLight");
		selectScript = selectScriptObject.GetComponent<SelectLight> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (ConstructionDistance.isMaxDistanceDetermined == true){

			//get maxWallDistance
			maxWallDistance = ConstructionDistance.maxWallDistance;
			onePercentOfWallControllerDistance = 100 / maxWallDistance; //variable equals one percent of distance between controller and wall
			
			//Debug.Log ("*******isMaxDistanceDetermined************");

			if (DetectIndexFinger.isFingerDetected == true) {				

				//Debug.Log ("*********isFingerDetected**********");

				if (SelectLight.isLightSelected == true) { //beeing in selectionsequence

					//Debug.Log ("*********isLightSelected**********");

					fingerPos = DetectIndexFinger.fingerPos; //unity absolute fingerPosition

					light = selectScript.GetSelectedLight(); //selected light
					lightPosition = light.transform.position; //position of selected light

					selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition);

					//TODO isSelectedLight = false at the end of positioning 

					//calculate range at the beginning of the selectionsequence
					if (isDepthRangeCalculated == false) {

						//Debug.Log ("at the beginning: selectedLightDistanceToController: " + selectedLightDistanceToController.ToString ());

						calculateDepthRange (selectedLightDistanceToController);
						isDepthRangeCalculated = true;

					}

					//calculate range at the beginning of the selectionsequence
					if (isHorizontalRangeCalculated == false) {

						calculateHorizontalRange ();
						isHorizontalRangeCalculated = true;

					}
					if (isDepthRangeCalculated == true && isHorizontalRangeCalculated == true) {

						//Debug.Log ("*********moveLightDepth start**********");

						//TODO get this both directions together!!!!!
						//moveLightInDepth (light, light.transform.position);
						moveLightHorizontal ();

						//Debug.Log ("*********moveLightDepth end**********");


					}
				}
			}

		} else {
			isDepthRangeCalculated = false;
		}


		
	}

	void calculateDepthRange(float lightControllerDistanceBeginn){ 
		
		//percentage position of light between controller and wall
		float percentagePosOfLightAtBeginning = getPercentageLightPosition(lightControllerDistanceBeginn);
		//Debug.Log ("percentage pos of Light at the beginning: " + percentagePosOfLightAtBeginning.ToString());

		//calculate start distance fromm finger to controller
		fingerControllerDistanceBegin = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);

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

	void calculateHorizontalRange(){

	}

	void moveLightInDepth(GameObject light, Vector3 lightPosition){

		//get current distance between light and controller
		currentLightControllerDistance = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition); //selectedLightDistanceToController = 
		//Debug.Log ("eingang moveLightDepth********");
		//Debug.Log ("currentLightControllerDistance" + currentLightControllerDistance.ToString());

		currentFingerControllerDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);

		//TODO what if out of range??
		//check for range
		if (currentFingerControllerDistance >= minFingerRange && currentFingerControllerDistance <= maxFingerRange) {
			Debug.LogFormat ("in range");
		} else {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");
		}
			
		float newLightControllerDistance = (maxWallDistance / 100) * getPercentageFingerPosition (currentFingerControllerDistance);
		//Debug.Log ("newLightControllerDitance" + newLightControllerDistance.ToString());

		//get vector orthogonal to xz-plane, length = light.y
		float lightY = light.transform.position.y ;
		Vector3 onlyYVector = new Vector3(0, lightY, 0);

		Vector3 movingVector = -(onlyYVector) + light.transform.position;

		Vector3 normalizedMovingVector = movingVector.normalized;

		Vector3 newLightPosition = onlyYVector + (normalizedMovingVector * newLightControllerDistance);
			
		light.transform.position = newLightPosition;

	}

	void moveLightHorizontal(){

	}

	float getPercentageFingerPosition(float distance){

		float currentFingerMinFingerRangeDistance = distance - minFingerRange;
		//Debug.Log ("currentFingerMinFingerRangeDistance: " + currentFingerMinFingerRangeDistance.ToString());

		float currentPercentageFingerPosInRange = ((100 / fingerRangeVolume) * currentFingerMinFingerRangeDistance); // like 26% ???
		//Debug.Log ("currentPercentageFingerPosInRange: " + currentPercentageFingerPosInRange.ToString());

		//Debug.Log ("currentPercentageFingerPos: " + currentPercentageFingerPosInRange.ToString());

		return currentPercentageFingerPosInRange;
	}

	float getPercentageLightPosition(float distance){

		float percentagePosOfLightAtBeginning = ((100 / maxWallDistance) * distance); // like 26%
		//Debug.Log ("percentagePosOfLightAtBeginning: " + percentagePosOfLightAtBeginning.ToString());

		return percentagePosOfLightAtBeginning;

	}

}
