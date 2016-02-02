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

					Debug.Log ("*********isLightSelected**********");

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
					if (isDepthRangeCalculated == true && isHorizontalRangeCalculated == false) {

						calculateHorizontalRange ();
						isHorizontalRangeCalculated = true;

					}

					if (isDepthRangeCalculated == true && isHorizontalRangeCalculated == true) {

						//Debug.Log ("*********moveLightDepth start**********");

						//TODO get this both directions together!!!!!
						moveLightInDepth (light, light.transform.position, fingerPos);
						//moveLightHorizontal ();

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

		//calculate start distance from finger to controller
		fingerControllerDistanceBegin = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);
		Debug.Log ("fingerControllerDistanceBegin: " + fingerControllerDistanceBegin.ToString ());

		Vector3 tmp = DetectIndexFinger.fingerPos + DetectIndexFinger.handControllerPos;
		Debug.Log ("tmp: " + tmp.magnitude.ToString ());


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

	void moveLightInDepth(GameObject light, Vector3 lightPosition, Vector3 fingerPosition){
		
		//get current distance between light and controller
		currentLightControllerDistance = Vector3.Distance(DetectIndexFinger.handControllerPos, lightPosition); //selectedLightDistanceToController = 
		Debug.Log ("eingang moveLightDepth********");
		Debug.Log ("1 c currentLightControllerDistance" + currentLightControllerDistance.ToString());

		currentFingerControllerDistance = Vector3.Distance (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos);
		//Debug.Log ("currentFingerControllerDistance" + currentFingerControllerDistance.ToString());


		//TODO what if out of range??
		//check for range
		if (currentFingerControllerDistance >= minFingerRange && currentFingerControllerDistance <= maxFingerRange) {
			//Debug.LogFormat ("in range");
		} else {
			Debug.Log ("++++++++++++++++ out of range +++++++++++++++++++++");
		}
			
		float newLightControllerDistance = (maxWallDistance / 100) * getPercentageFingerPosition (currentFingerControllerDistance);
		//Debug.Log ("newLightControllerDitance" + newLightControllerDistance.ToString());

		//get vector orthogonal to xz-plane, length = light.y
		float lightY = light.transform.position.y ;
		Vector3 onlyYVector = new Vector3(0, lightY, 0);
		float onlyYVectorLength = onlyYVector.magnitude;
		Debug.Log("2 a onlyYVectorLength: " + onlyYVectorLength.ToString());


		//get depth of lightposition
		Vector3 depthVectorAtBeginning = -(onlyYVector) + light.transform.position;
			//just for better understanding
			//depthVectorAtBeginning.y = lightY;
		Debug.Log("depthVectorAtBeginning: " + depthVectorAtBeginning.ToString());

		float depthLengthAtBeginning = depthVectorAtBeginning.magnitude;
		Debug.Log("3 b depthLengthAtBeginning: " + depthLengthAtBeginning.ToString());

		Vector3 normalizedDepthVector = depthVectorAtBeginning.normalized;

		float newDepthVectorLength = (normalizedDepthVector * newLightControllerDistance).magnitude;
		Vector3 newDepthVector = - onlyYVector + (normalizedDepthVector * newLightControllerDistance);
		//Vector3 newDepthVector = onlyYVector + (normalizedMovingVector * newLightControllerDistance);

		float newDepthLength = newDepthVector.magnitude;  //contains length of new depth vector orthogonal to y axis
		Debug.Log("##############################");

		Debug.Log("newDepthLength: " + newDepthLength.ToString());

		Debug.Log("##############################");

		//float newlightY = newLightDepthPosition.y;

		//float newLightPositionMagnitude = newLightPosition.magnitude;

		//get length for vector to new light position
		float aSquare = onlyYVectorLength * onlyYVectorLength;
		Debug.Log ("aSquare: " + aSquare.ToString());

		float bSquare = newDepthLength * newDepthLength;
		Debug.Log ("bSquare: " + bSquare);

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
		Debug.Log ("newLightVectorLength: " + newLightVectorLength.ToString());

		//HORIZONTAL TEST get direction of finger
		Debug.Log("fingerPos: " + fingerPosition.ToString());

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
			
		light.transform.position = new Vector3(newLightVector.x, lightY, newLightVector.z); //newLightPosition;
		Debug.Log("new light position: " + light.transform.position.ToString());

	}

	void moveLightHorizontal(){



	}

	float getPercentageFingerPosition(float distance){

		float currentFingerMinFingerRangeDistance = distance - minFingerRange;
		//Debug.Log ("currentFingerMinFingerRangeDistance: " + currentFingerMinFingerRangeDistance.ToString());

		float currentPercentageFingerPosInRange = ((100 / fingerRangeVolume) * currentFingerMinFingerRangeDistance); // like 26% ???
		Debug.Log ("currentPercentageFingerPosInRange: " + currentPercentageFingerPosInRange.ToString());

		return currentPercentageFingerPosInRange;
	}

	float getPercentageLightPosition(float distance){

		float percentagePosOfLightAtBeginning = ((100 / maxWallDistance) * distance); // like 26%
		Debug.Log ("percentagePosOfLightAtBeginning: " + percentagePosOfLightAtBeginning.ToString());

		return percentagePosOfLightAtBeginning;

	}

}
