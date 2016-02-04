using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	//SelectLight selectScript;

	float maxWallDistance;
	float onePercentOfWallControllerDistance;


//	float selectedLightDistanceToController;

	//Vector3 fingerPos;

	//calculateDepthRange
	bool isDepthRangeCalculated = false;
	float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;

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

		//GameObject selectScriptObject = GameObject.Find ("SelectLight");
		//selectScript = selectScriptObject.GetComponent<SelectLight> ();

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

					//fingerPos = DetectIndexFinger.fingerPos; //unity absolute fingerPosition

					//light = SelectLight.GetSelectedLight(); //selected light
					//lightPosition = light.transform.position; //position of selected light

					//selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, SelectLight.lightPosition);

					//TODO isSelectedLight = false at the end of positioning 

					//calculate range at the beginning of the selectionsequence

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
		fingerControllerDistanceBegin = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPosition);
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

	public void moveLight(GameObject light, Vector3 lightPosition, Vector3 fingerPosition){

		if (isDepthRangeCalculated == false) {

			//Debug.Log ("at the beginning: selectedLightDistanceToController: " + selectedLightDistanceToController.ToString ());

			float selectedLightDistanceToController = Vector3.Distance(DetectIndexFinger.handControllerPos, SelectLight.lightPosition);

			calculateDepthRange (selectedLightDistanceToController, fingerPosition);
			isDepthRangeCalculated = true;

		}

		if (isDepthRangeCalculated == true) {

			//get current distance between finger and controller for getPercentageFingerPosition(...)
			currentFingerControllerDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, fingerPosition); // ????????????????????????
			//Debug.Log ("currentFingerControllerDistance" + currentFingerControllerDistance.ToString());			

			float percentageFingerPosition = getPercentageFingerPosition (currentFingerControllerDistance);

			float depth = (maxWallDistance / 100) * percentageFingerPosition;
			//Debug.Log ("depth: " + depth.ToString ());

			//get orthogonal vector to xz-plane, length = light.y
			float lightY = SelectLight.light.transform.position.y ;
			Vector3 onlyYVector = new Vector3(0, lightY, 0);
			float onlyYVectorLength = onlyYVector.magnitude;
			//Debug.Log("2 a onlyYVectorLength: " + onlyYVectorLength.ToString());


			//get depth of lightposition
			//Vector3 depthVectorAtBeginning = -(onlyYVector) + light.transform.position;
			//just for better understanding
			//depthVectorAtBeginning.y = lightY;
			//Debug.Log("depthVectorAtBeginning: " + depthVectorAtBeginning.ToString());

			//float depthLengthAtBeginning = depthVectorAtBeginning.magnitude;
			//Debug.Log("3 b depthLengthAtBeginning: " + depthLengthAtBeginning.ToString());

			//Vector3 normalizedDepthVector = depthVectorAtBeginning.normalized;

			//float newDepthVectorLength = (normalizedDepthVector * newLightControllerDistance).magnitude;
			//Vector3 newDepthVector = - onlyYVector + (normalizedDepthVector * newLightControllerDistance);
			//Vector3 newDepthVector = onlyYVector + (normalizedMovingVector * newLightControllerDistance);

			//float newDepthLength = newDepthVector.magnitude;  //contains length of new depth vector orthogonal to y axis
			//Debug.Log("##############################");

			//Debug.Log("newDepthLength: " + newDepthLength.ToString());

			//Debug.Log("##############################");

			//float newlightY = newLightDepthPosition.y;

			//float newLightPositionMagnitude = newLightPosition.magnitude;

			//get length for vector to new light position
			float aSquare = onlyYVectorLength * onlyYVectorLength;
			//Debug.Log ("aSquare: " + aSquare.ToString());

			float bSquare = depth * depth;
			//Debug.Log ("bSquare: " + bSquare);

			/*float Twoab = 2 * (onlyYVectorLength * newDepthLength);
			Debug.Log ("Twoab: " + Twoab);*/


			//Vector2 rechterWinkel = new Vector3 (1, 0, 0);
			//Debug.Log("Winkel zwischen onlyY and rechterWinkel: " + Vector3.Angle (onlyYVector, rechterWinkel).ToString());

			//double angle = Vector3.Angle (onlyYVector, rechterWinkel);
			//Debug.Log("angle: " + angle.ToString());

			//float cosAngle = 0.0f; // System.Math.Cos(angle); //newDepthVector));
			//Debug.Log ("cosAngle: " + cosAngle);

			// c² = a² + b²
			float newLightVectorLength = Mathf.Sqrt(aSquare + bSquare); 
			//Debug.Log ("newLightVectorLength: " + newLightVectorLength.ToString());

			//get direction to fingerTip
			Vector3 normalizedFingerDirection = fingerPosition.normalized;
			//direction multiply with new length
			Vector3 newLightVector = normalizedFingerDirection * newLightVectorLength;
			//newLightVector.y = lightY;

			//float newLightX = fingerPos.x * 2.0f;
			//float newLightZ = fingerPos.z * 2.0f;

			//lampe gets direction from finger
			//Vector3 newLightPosition = new Vector3(newLightX, lightY , newLightZ); //(-(fingerPos) + onlyYVector + newLightDepthPosition) + fingerPos;


			//light.transform.position = normalizedFingerDirection; //* newLightPositionMagnitude;

			SelectLight.light.transform.position = new Vector3(newLightVector.x, lightY, newLightVector.z); //newLightPosition;

			checkForMeaningfulChangesX (lastX, newLightVector.x);
			checkForMeaningfulChangesZ (lastZ, newLightVector.z);

			//Debug.Log("new light position: " + light.transform.position.ToString());

			lastX = newLightVector.x;
			lastZ = newLightVector.z;
		}

		//Debug.Log ("Eingang moveLightDepth********");



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
			//Debug.Log ("xCounter: " + xCounter.ToString ());

			if (xCounter == SelectLight.waitCountdown) {

				//disable current light selection
				SelectLight.isLightSelected = false;

				Debug.Log ("lampe abgewählt");

				xCounter = 0;
				zCounter = 0;
			}
		}
	}
	void checkForMeaningfulChangesZ(float lastValue, float newValue){

		if(newValue <= (lastValue + changeValue) && newValue >= (lastValue - changeValue)){

			zCounter += 1;
			//Debug.Log ("counter: " + zCounter.ToString ());

			if (zCounter == SelectLight.waitCountdown) {

				//disable current light selection
				SelectLight.isLightSelected = false;

				Debug.Log ("lampe abgewählt");

				zCounter = 0;
				xCounter = 0;
			}
		}
	}

}
