using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;
	Vector3 palmCenter;

	float maxWallDistance;
	float onePercentOfWallControllerDistance;

	LayerMask onlyLightLayer;

	int buffer = 0;
	int bufferMax = 30;

	GameObject light;
	Vector3 lightPosition;
	Vector3 controlPoint;

	//calculateDepthRange
	float percentagePosOfLightAtBeginning;
	bool isDepthRangeCalculated = false;
	float fingerControllerDistanceBegin;
	float fingerRangeVolume = 0.40f; //40cm
	float minFingerRange;
	float maxFingerRange;
	float percentagePosOfFingerAtBeginning;
	float onePercentOfFingerRange;

	//Raycaster
	RaycastHit hitObject = new RaycastHit();
	GameObject hitLight;
	public static bool lightShouldMove = false;

	//moveLight
	float selectedLightDistanceToController;
	float currentLightControllerDistance;
	float currentFingerControllerDistance;
	float lastX = 0;
	float lastZ = 0;
	float percentageFingerPosition;
	float depth;
	float lightY;
	Vector3 onlyYVector;
	float onlyYVectorLength;
	float aSquare;
	float bSquare;
	float newLightVectorLength;
	Vector3 normalizedFingerDirection;
	Vector3 newLightVector;

	//bool isPositioningActive = false;

	int startBufferMax = 20;
	int startBuffer;
	//checkForMeaningfulChanges
	//float changeValue = 0.001f;
	int xCounter = 0;
	int zCounter = 0;

	//checkForMeaningfulChangesEntrance
	Vector3 newPosition;
	Vector3 lastPosition;
	int entranceCounter = 0;
	int changeCounter = 0;

	//getPercentageFingerPos
	float currentFingerMinFingerRangeDistance;
	float currentPercentageFingerPosInRange;

	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		labelScriptObject = GameObject.Find("PositionLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		int buffer = bufferMax;


	}
	
	// Update is called once per frame
	void Update () {

		if (ConstructionDistance.isMaxDistanceDetermined == true){

			//get maxWallDistance
			maxWallDistance = ConstructionDistance.maxWallDistance;
			onePercentOfWallControllerDistance = 100 / maxWallDistance; //variable equals one percent of distance between controller and wall

			if (SelectLight.isLightSelected == true) {

				if (Gestures.isPositionGesture == true) { //beeing in selectionsequence

					//TODO
					//palmCenter = Gestures.palmCenter;
					//labelScript.displayLabel (palmCenter, labelScriptObject);

					if (!MainMenu.isGroupAActive) {
						
						if (!ColorTemperature.isTemperatureModusActive) {
							controlPoint = Gestures.controlPoint;

							labelScript.displayLabel (Gestures.palmCenter, labelScriptObject);

							if (lightShouldMove == false) {

								Debug.Log ("buffer");

								buffer -= 1;
								if (buffer <= 0) {
									
									if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

										checkForMeaningfulChangesEntrance (controlPoint);
						
									}
								} 
							} else if (lightShouldMove == true) {

								hitLight = hitObject.collider.gameObject;
								light = SelectLight.light; //selected light
								lightPosition = light.transform.position; //position of selected light

								moveLight (light, lightPosition, controlPoint);
								SelectLight.setHighlighterPosition (light);

							}

						} else if (ColorTemperature.isTemperatureModusActive == true) {
							
							controlPoint = Gestures.controlPoint;

						}
					}
					if (MainMenu.isGroupAActive) {
						

						labelScript.displayLabel (palmCenter, labelScriptObject);
						controlPoint = Gestures.controlPoint;
						palmCenter = Gestures.palmCenter;												

						//Debug.Log ("in position script and gesture == true");
								
						if (lightShouldMove == false) {

							//Debug.Log ("buffer");

							buffer -= 1;
							if (buffer <= 0) {

								if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

									checkForMeaningfulChangesEntrance (controlPoint);

								}
							}
						} else if (lightShouldMove == true) {

							hitLight = hitObject.collider.gameObject;
							light = SelectLight.light; //selected light
							lightPosition = light.transform.position; //position of selected light

							moveLight (light, lightPosition, controlPoint);
							SelectLight.setHighlighterPosition (light);
								
						}
					}
										
				} else {

					labelScriptObject.SetActive(false);

				}

			} else {

				labelScriptObject.SetActive(false);

			}

		} else {
			isDepthRangeCalculated = false;
		}
		
	}

	void calculateDepthRange(float lightControllerDistanceBeginn, Vector3 fingerPosition){ 
		
		//percentage position of light between controller and wall
		percentagePosOfLightAtBeginning = getPercentageLightPosition(lightControllerDistanceBeginn);
		//Debug.Log ("percentage pos of Light at the beginning: " + percentagePosOfLightAtBeginning.ToString());

		//calculate start distance from finger to controller
		fingerControllerDistanceBegin = Vector3.Distance (Gestures.handControllerPos, fingerPosition);
		//Debug.Log ("fingerControllerDistanceBegin: " + fingerControllerDistanceBegin.ToString ());

		percentagePosOfFingerAtBeginning = percentagePosOfLightAtBeginning; //adapt percentage position of light on position of finger
		//Debug.Log ("percentage pos of finger at the beginning: " + percentagePosOfFingerAtBeginning.ToString());

		//get one percent of 40cm range volume
		onePercentOfFingerRange = fingerRangeVolume / 100;
		//Debug.Log ("onePercentOfFingerRange: " + onePercentOfFingerRange.ToString());

		//calculate minFingerRange, starting on current fingerControllerDistance
		minFingerRange = fingerControllerDistanceBegin - (percentagePosOfFingerAtBeginning * onePercentOfFingerRange);
		//Debug.Log ("minFingerRange: " + minFingerRange.ToString ());

		maxFingerRange = fingerControllerDistanceBegin + ((100 - percentagePosOfFingerAtBeginning) * onePercentOfFingerRange);
		//Debug.Log ("maxFingerRange: " + maxFingerRange.ToString ());

		Debug.Log ("range calculated");
	}

	public void moveLight(GameObject light, Vector3 lightPosition, Vector3 controlPoint){

		if (isDepthRangeCalculated == false) {

			selectedLightDistanceToController = Vector3.Distance(Gestures.handControllerPos, SelectLight.lightPosition);

			calculateDepthRange (selectedLightDistanceToController, controlPoint);
			isDepthRangeCalculated = true;

		}

		if (isDepthRangeCalculated == true) {

				checkForMeaningfulChangesZX (lightPosition.x, lightPosition.z);//lastX, lightPosition.x, lastZ, lightPosition.z);

				//checkForMeaningfulChangesZ (lastZ, newLightVector.z);

				//get current distance between finger and controller for getPercentageFingerPosition(...)
				currentFingerControllerDistance = Vector3.Distance (Gestures.handControllerPos, controlPoint);
				//Debug.Log ("currentFingerControllerDistance" + currentFingerControllerDistance.ToString());			

				percentageFingerPosition = getPercentageFingerPosition (currentFingerControllerDistance);

				depth = (maxWallDistance / 100) * percentageFingerPosition;
				//Debug.Log ("depth: " + depth.ToString ());

				//get orthogonal vector to xz-plane, length = light.y
				lightY = SelectLight.light.transform.position.y ;
				onlyYVector = new Vector3(0, lightY, 0);
				onlyYVectorLength = onlyYVector.magnitude;

				//get length for vector to new light position
				aSquare = onlyYVectorLength * onlyYVectorLength;
				//Debug.Log ("aSquare: " + aSquare.ToString());

				bSquare = depth * depth;
				//Debug.Log ("bSquare: " + bSquare);

				// c² = a² + b²
				newLightVectorLength = Mathf.Sqrt(aSquare + bSquare); 
				//Debug.Log ("newLightVectorLength: " + newLightVectorLength.ToString());

				//get direction to fingerTip
				normalizedFingerDirection = controlPoint.normalized;
				//direction multiply with new length
				newLightVector = normalizedFingerDirection * newLightVectorLength;

				SelectLight.light.transform.position = new Vector3(newLightVector.x, lightY, newLightVector.z); //newLightPosition;

				//Debug.Log("new light position: " + light.transform.position.ToString());

		}
	}

	float getPercentageFingerPosition(float distance){

		currentFingerMinFingerRangeDistance = distance - minFingerRange;
		//Debug.Log ("currentFingerMinFingerRangeDistance: " + currentFingerMinFingerRangeDistance.ToString());

		currentPercentageFingerPosInRange = ((100 / fingerRangeVolume) * currentFingerMinFingerRangeDistance); // like 26% ???
		//Debug.Log ("currentPercentageFingerPosInRange: " + currentPercentageFingerPosInRange.ToString());

		//check for range
		if (currentPercentageFingerPosInRange > 100) {

			currentPercentageFingerPosInRange = 100;

		}
		if (currentPercentageFingerPosInRange < 0) {

			currentPercentageFingerPosInRange = 0;

		}

		//Debug.Log ("percentageFingerPosition: " + currentPercentageFingerPosInRange.ToString());

		return currentPercentageFingerPosInRange;
	}

	float getPercentageLightPosition(float distance){

		percentagePosOfLightAtBeginning = ((100 / maxWallDistance) * distance);
		//Debug.Log ("percentagePosOfLightAtBeginning: " + percentagePosOfLightAtBeginning.ToString());

		return percentagePosOfLightAtBeginning;

	}

	void checkForMeaningfulChangesZX (float newX, float newZ){//float lastX, float newX, float lastZ, float newZ){
		Debug.Log ("checkForMeaningfulChangesZX");

		float changeValue = 0.07f; //0.05

		Debug.Log ("lastX: " + lastX.ToString ());
		Debug.Log ("newX " + newX.ToString ());
		Debug.Log ("lastZ " + lastZ.ToString ());
		Debug.Log ("newZ " + newZ.ToString ());

		if (((newX <= (lastX + changeValue)) && (newX >= (lastX - changeValue)))
		   && ((newZ <= (lastZ + changeValue)) && (newZ >= (lastZ - changeValue)))) {
							
			Progressbar.fillProgressbar (zCounter);
			zCounter += 1;

			if (zCounter == SelectLight.waitCountdown) {

				//disable current positioning selection
				lightShouldMove = false;
				isDepthRangeCalculated = false;
				Progressbar.resetProgressbar ();

				buffer = bufferMax;
				zCounter = 0;
				//Debug.Log ("lampe positioniert");

			}
		} else {
			
			zCounter = 0;
			Progressbar.resetProgressbar ();

		}

		lastX = newX;
		lastZ = newZ;

	}

	void checkForMeaningfulChangesEntrance(Vector3 controlPoint){
		//Debug.Log ("checkForMeaningfulChangesEntrance");
		float changeValue = 0.002f; //0.001f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
		    && newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
		    && newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			Progressbar.fillProgressbar (entranceCounter);
			entranceCounter += 1;

			if (entranceCounter == SelectLight.waitCountdown) {

				buffer = bufferMax;
				lightShouldMove = true;
				isDepthRangeCalculated = false;
				Progressbar.resetProgressbar ();
				entranceCounter = 0;

			}
		} else {

			entranceCounter = 0;
			Progressbar.resetProgressbar ();

		}
			
		lastPosition = controlPoint;

	}
}
