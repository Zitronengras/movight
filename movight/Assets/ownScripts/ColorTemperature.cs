using UnityEngine;
using System.Collections;

public class ColorTemperature : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject light;
	Light lightSource;
	Color32 currentColor;
	Vector3 controlPoint;
	Camera camera;

	//checkForMeaningfulChanges
	Vector3 newPosition;
	Vector3 lastPosition;
	int changeCounter = 0;
	public static bool temperatureShouldChange = false;

	float percentagWidthOfOneColumn;

	//range
	bool isHorizontalRangeCalculated = false;
	float minScreenRange;
	float maxScreenRange;

	float numberOfColumns = 39.0f;

	//
	float currentXOnScreen;
	float screenXRange = 280.0f;
	float temperaturePositionRange = 40.0f;
	Color color = new Color32();


	// Use this for initialization
	void Start () {

		camera = Camera.main; // GetComponent<Camera>;
		Debug.Log ("Camera: " + camera.ToString ());
		labelScriptObject = GameObject.Find("TemperatureLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		percentagWidthOfOneColumn = 100 / numberOfColumns;
			
	}
	
	// Update is called once per frame
	void Update () {

		if (SelectLight.isLightSelected) {

			light = SelectLight.light; //selected light		

			if (Gestures.isTemperatureGesture) {

				Debug.Log ("im color temperature script********************");

				labelScript.displayLabel (controlPoint, labelScriptObject);
				lightSource = light.GetComponentInChildren<Light> ();
				controlPoint = Gestures.controlPoint;
				currentColor = lightSource.color;

				//checkForMeaningfulChanges (controlPoint);

				//if (temperatureShouldChange) {
					//changeTemperature (controlPoint, lightSource, currentColor);
				//}
				getPercentagePalmPosition(controlPoint);



			} else {
				labelScriptObject.SetActive(false);
			}

		} else {
			labelScriptObject.SetActive(false);
		}
	
	}

	void calculateHorizontalRange(Vector3 currentPosition){

		//convert currentPosition into 2D (screen)
		Vector3 onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		currentXOnScreen = onScreenPosition.x;

		//get percentagColorValue

		float percentagePosOnScreenAtBeginning = getPercentagePalmPosition (currentPosition);

		//cone percent of range on screen
		float onePercentOfScreenRange = screenXRange / 100;

		//get startpoint of range
		minScreenRange = currentXOnScreen - (percentagePosOnScreenAtBeginning * onePercentOfScreenRange); 
		maxScreenRange = currentXOnScreen + ((100 - percentagePosOnScreenAtBeginning) * onePercentOfScreenRange);
	
		//TODO what if out of range???
	
	}


	void changeTemperature(Vector3 controlPoint, Light lightSource, Color32 currentColor){

		if (isHorizontalRangeCalculated == false) {

			calculateHorizontalRange ();
			isHorizontalRangeCalculated = true;

		} else if (isHorizontalRangeCalculated == true) {

			float percentagePosition = getPercentagePalmPosition (controlPoint);
			lightSource.color = getColor (percentagePosition);

		}
			
	}

	void getPercentagePalmPosition(Vector3 currentPosition){

		//convert currentPosition into 2D (screen)
		Vector3 onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		float currentXOnScreen = onScreenPosition.x;

		float currentMinXOnScreen = currentXOnScreen - minScreenRange; //from range
		

		Debug.Log ("On screen x psotion" + currentXOnScreen.ToString ()); 

		float percentageXPosition = (100 / screenXRange) * currentXOnScreen;
		return percentageXPosition;
	}

	float getPercentageTemperatureValue(Color32 currentColor){

		float percentageTemperaturValue;

		/*
		if (currentColor == Color32 (227, 24, 23, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns);
		} else if (currentColor == Color32 (233, 92, 14, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 2;
		} else if (currentColor == Color32 (239, 131, 1, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 3;
		} else if (currentColor == Color32 (246, 165, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 4;
		} else if (currentColor == Color32 (251, 188, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 5;
		} else if (currentColor == Color32 (253, 202, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 6;
		} else if (currentColor == Color32 (255, 219, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 7;
		} else if (currentColor == Color32 (255, 228, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 8;
		} else if (currentColor == Color32 (255, 235, 0, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 9;
		} else if (currentColor == Color32 (255, 235, 10, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 10;
		} else if (currentColor == Color32 (254, 237, 20, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 11;
		} else if (currentColor == Color32 (254, 237, 27, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 12;
		} else if (currentColor == Color32 (252, 238, 69, 1)) {
			percentageTemperaturValue = (100 / numberOfColumns) * 13;
		}  
		  



		else if (percentagePosition >= (percentagWidthOfOneColumn * 13) && percentagePosition < (percentagWidthOfOneColumn * 14)) {
			color = new Color32 (252, 239, 108, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 14) && percentagePosition < (percentagWidthOfOneColumn * 15)) {
			color = new Color32 (251, 240, 149, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 15) && percentagePosition < (percentagWidthOfOneColumn * 16)) {
			color = new Color32 (250, 242, 178, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 16) && percentagePosition < (percentagWidthOfOneColumn * 17)) {
			color = new Color32 (248, 244, 210, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 17) && percentagePosition < (percentagWidthOfOneColumn * 18)) {
			color = new Color32 (247, 245, 229, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 18) && percentagePosition < (percentagWidthOfOneColumn * 19)) {
			color = new Color32 (246, 245, 242, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 19) && percentagePosition < (percentagWidthOfOneColumn * 20)) {
			color = new Color32 (245, 245, 246, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 20) && percentagePosition < (percentagWidthOfOneColumn * 21)) { //white
			color = new Color32 (255, 255, 255, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 21) && percentagePosition < (percentagWidthOfOneColumn * 22)) {
			color = new Color32 (225, 238, 244, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 22) && percentagePosition < (percentagWidthOfOneColumn * 23)) {
			color = new Color32 (201, 229, 241, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 23) && percentagePosition < (percentagWidthOfOneColumn * 24)) {
			color = new Color32 (168, 217, 239, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 24) && percentagePosition < (percentagWidthOfOneColumn * 25)) {
			color = new Color32 (105, 198, 234, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 25) && percentagePosition < (percentagWidthOfOneColumn * 26)) { //blue
			color = new Color32 (53, 188, 232, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 26) && percentagePosition < (percentagWidthOfOneColumn * 27)) {
			color = new Color32 (2, 172, 228, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 27) && percentagePosition < (percentagWidthOfOneColumn * 28)) {
			color = new Color32 (0, 166, 226, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 28) && percentagePosition < (percentagWidthOfOneColumn * 29)) {
			color = new Color32 (0, 160, 224, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 29) && percentagePosition < (percentagWidthOfOneColumn * 30)) {
			color = new Color32 (0, 158, 224, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 30) && percentagePosition < (percentagWidthOfOneColumn * 31)) {
			color = new Color32 (0, 157, 223, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 31) && percentagePosition < (percentagWidthOfOneColumn * 32)) {
			color = new Color32 (0, 156, 222, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 32) && percentagePosition < (percentagWidthOfOneColumn * 33)) {
			color = new Color32 (0, 154, 220, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 33) && percentagePosition < (percentagWidthOfOneColumn * 34)) {
			color = new Color32 (0, 150, 216, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 34) && percentagePosition < (percentagWidthOfOneColumn * 35)) {
			color = new Color32 (0, 147, 212, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 35) && percentagePosition < (percentagWidthOfOneColumn * 36)) {
			color = new Color32 (0, 142, 207, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 36) && percentagePosition < (percentagWidthOfOneColumn * 37)) {
			color = new Color32 (1, 135, 200, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 37) && percentagePosition < (percentagWidthOfOneColumn * 38)) {
			color = new Color32 (6, 127, 193, 1);
		} else if (percentagePosition >= (percentagWidthOfOneColumn * 38) && percentagePosition <= (percentagWidthOfOneColumn * 39)) { // 100%
			color = new Color32 (12, 118, 185, 1);
		}*/

		//float currentPercent = percentagWidthOfOneColumn * 19.5f;
		return percentageTemperaturValue;
	}


	void checkForMeaningfulChanges(Vector3 controlPoint){

		float changeValue = 0.0001f;
		newPosition = controlPoint;

		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			if (!(newPosition.x == lastPosition.x) && !(newPosition.y == lastPosition.y) && !(newPosition.z == lastPosition.z)) {				

				changeCounter += 1;
				Debug.Log ("changeCounter " + changeCounter.ToString());

				if (changeCounter == SelectLight.waitCountdown) {

					if (temperatureShouldChange == true) {
						temperatureShouldChange = false;
					} else {
						temperatureShouldChange = true;
					}

					changeCounter = 0;

				}
			}
		}

		Debug.Log ("shouldTemperatureChange??: " + temperatureShouldChange.ToString ());

		lastPosition = controlPoint;

	}

	Color32 getColor(float percentagePosition){

		float widthOfOneColumn = temperaturePositionRange / numberOfColumns; //39 columns in 0.40 positionRange	

		if (percentagePosition >= 0 && percentagePosition <= percentagWidthOfOneColumn) {
			color = new Color32 (227, 24, 23, 1);
		} else if (percentagePosition >  percentagWidthOfOneColumn && percentagePosition <= (percentagWidthOfOneColumn * 2)) {
			color = new Color32 (233, 92, 14, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 2) && percentagePosition <= (percentagWidthOfOneColumn * 3)) {
			color = new Color32 (239, 131, 1, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 3) && percentagePosition <= (percentagWidthOfOneColumn * 4)) {
			color = new Color32 (246, 165, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 4) && percentagePosition <= (percentagWidthOfOneColumn * 5)) {
			color = new Color32 (251, 188, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 5) && percentagePosition <= (percentagWidthOfOneColumn * 6)) {
			color = new Color32 (253, 202, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 6) && percentagePosition <= (percentagWidthOfOneColumn * 7)) {
			color = new Color32 (255, 219, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 7) && percentagePosition <= (percentagWidthOfOneColumn * 8)) {
			color = new Color32 (255, 228, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 8) && percentagePosition <= (percentagWidthOfOneColumn * 9)) {
			color = new Color32 (255, 235, 0, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 9) && percentagePosition <= (percentagWidthOfOneColumn * 10)) {
			color = new Color32 (255, 235, 10, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 10) && percentagePosition <= (percentagWidthOfOneColumn * 11)) {
			color = new Color32 (254, 237, 20, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 11) && percentagePosition <= (percentagWidthOfOneColumn * 12)) {
			color = new Color32 (254, 237, 27, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 12) && percentagePosition <= (percentagWidthOfOneColumn * 13)) {
			color = new Color32 (252, 238, 69, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 13) && percentagePosition <= (percentagWidthOfOneColumn * 14)) {
			color = new Color32 (252, 239, 108, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 14) && percentagePosition <= (percentagWidthOfOneColumn * 15)) {
			color = new Color32 (251, 240, 149, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 15) && percentagePosition <= (percentagWidthOfOneColumn * 16)) {
			color = new Color32 (250, 242, 178, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 16) && percentagePosition <= (percentagWidthOfOneColumn * 17)) {
			color = new Color32 (248, 244, 210, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 17) && percentagePosition <= (percentagWidthOfOneColumn * 18)) {
			color = new Color32 (247, 245, 229, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 18) && percentagePosition <= (percentagWidthOfOneColumn * 19)) {
			color = new Color32 (246, 245, 242, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 19) && percentagePosition <= (percentagWidthOfOneColumn * 20)) {
			color = new Color32 (245, 245, 246, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 20) && percentagePosition <= (percentagWidthOfOneColumn * 21)) { //white
			color = new Color32 (255, 255, 255, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 21) && percentagePosition <= (percentagWidthOfOneColumn * 22)) {
			color = new Color32 (225, 238, 244, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 22) && percentagePosition <= (percentagWidthOfOneColumn * 23)) {
			color = new Color32 (201, 229, 241, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 23) && percentagePosition <= (percentagWidthOfOneColumn * 24)) {
			color = new Color32 (168, 217, 239, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 24) && percentagePosition <= (percentagWidthOfOneColumn * 25)) {
			color = new Color32 (105, 198, 234, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 25) && percentagePosition <= (percentagWidthOfOneColumn * 26)) { //blue
			color = new Color32 (53, 188, 232, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 26) && percentagePosition <= (percentagWidthOfOneColumn * 27)) {
			color = new Color32 (2, 172, 228, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 27) && percentagePosition <= (percentagWidthOfOneColumn * 28)) {
			color = new Color32 (0, 166, 226, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 28) && percentagePosition <= (percentagWidthOfOneColumn * 29)) {
			color = new Color32 (0, 160, 224, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 29) && percentagePosition <= (percentagWidthOfOneColumn * 30)) {
			color = new Color32 (0, 158, 224, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 30) && percentagePosition <= (percentagWidthOfOneColumn * 31)) {
			color = new Color32 (0, 157, 223, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 31) && percentagePosition <= (percentagWidthOfOneColumn * 32)) {
			color = new Color32 (0, 156, 222, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 32) && percentagePosition <= (percentagWidthOfOneColumn * 33)) {
			color = new Color32 (0, 154, 220, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 33) && percentagePosition <= (percentagWidthOfOneColumn * 34)) {
			color = new Color32 (0, 150, 216, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 34) && percentagePosition <= (percentagWidthOfOneColumn * 35)) {
			color = new Color32 (0, 147, 212, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 35) && percentagePosition <= (percentagWidthOfOneColumn * 36)) {
			color = new Color32 (0, 142, 207, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 36) && percentagePosition <= (percentagWidthOfOneColumn * 37)) {
			color = new Color32 (1, 135, 200, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 37) && percentagePosition <= (percentagWidthOfOneColumn * 38)) {
			color = new Color32 (6, 127, 193, 1);
		} else if (percentagePosition > (percentagWidthOfOneColumn * 38) && percentagePosition <= (percentagWidthOfOneColumn * 39)) { // 100%
			color = new Color32 (12, 118, 185, 1);
		}

		return color;

	}

}
