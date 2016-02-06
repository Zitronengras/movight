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

	Color[] temperatureColors = new Color[39];
	float percentageTemperaturValue;


	// Use this for initialization
	void Start () {

		Debug.Log("oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo");


		camera = Camera.main; // GetComponent<Camera>;
		Debug.Log ("Camera: " + camera.ToString ());
		labelScriptObject = GameObject.Find("TemperatureLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		percentagWidthOfOneColumn = 100 / numberOfColumns;

		fillColorArray (temperatureColors);
			
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

				checkForMeaningfulChanges (controlPoint);

				if (temperatureShouldChange) {
					changeTemperature (controlPoint, lightSource, currentColor);
				}
				//getPercentagePalmPosition(controlPoint);



			} else {
				labelScriptObject.SetActive(false);
			}

		} else {
			labelScriptObject.SetActive(false);
		}
	
	}

	void calculateHorizontalRange(Vector3 currentPosition, Color currentColor){

		//convert currentPosition into 2D (screen)
		Vector3 onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		currentXOnScreen = onScreenPosition.x;

		//get percentagColorValue
		float currentPercentageColorPosition = getPercentageTemperatureValue(currentColor);

		float percentagePosOnScreenAtBeginning = currentPercentageColorPosition;
			
		//getPercentagePalmPosition (currentPosition);

		//cone percent of range on screen
		float onePercentOfScreenRange = screenXRange / 100;

		//get startpoint of range
		minScreenRange = currentXOnScreen - (percentagePosOnScreenAtBeginning * onePercentOfScreenRange); 
		maxScreenRange = currentXOnScreen + ((100 - percentagePosOnScreenAtBeginning) * onePercentOfScreenRange);
	
		//TODO what if out of range???
	
	}


	void changeTemperature(Vector3 controlPoint, Light lightSource, Color32 currentColor){

		if (isHorizontalRangeCalculated == false) {

			calculateHorizontalRange (controlPoint, currentColor);
			isHorizontalRangeCalculated = true;

		} else if (isHorizontalRangeCalculated == true) {

			Vector3 currentOnScreenPosition = camera.WorldToScreenPoint(controlPoint);
			currentXOnScreen = currentOnScreenPosition.x;

			Debug.Log ("currentX: " + currentXOnScreen);

			float percentagePalmPosition = getPercentagePalmPosition (currentXOnScreen);
			//get belonging percent for color
			//float currentPercentageTemperatureValue = getPercentageTemperatureValue (currentColor);


			Debug.Log ("neue Farbe: " + getColor (percentagePalmPosition, temperatureColors).ToString ());
			lightSource.color = getColor (percentagePalmPosition, temperatureColors);

		}
			
	}

	float getPercentagePalmPosition(float currentXOnScreen){

		float currentPalmMinXDistance = currentXOnScreen - minScreenRange;

		//convert currentPosition into 2D (screen)
		//Vector3 onScreenPosition = camera.WorldToScreenPoint(currentPosition);
		//float currentXOnScreen = onScreenPosition.x;

		//float currentMinXOnScreen = currentXOnScreen - minScreenRange; //from range
		//Debug.Log ("On screen x psotion" + currentXOnScreen.ToString ()); 

		float currentPercentagePalmPosition = (100 / screenXRange) * currentPalmMinXDistance;

		Debug.Log ("Percentage Palm Position " + currentPercentagePalmPosition.ToString());

		return currentPercentagePalmPosition;

	}

	float getPercentageTemperatureValue(Color32 currentColor){

		for (int i = 0; i < temperatureColors.Length; i++) {

			if (temperatureColors[i].r == currentColor.r && temperatureColors[i].g == currentColor.g && temperatureColors[i].b == currentColor.b) {
				percentageTemperaturValue = (100 / numberOfColumns) * (i);
			}

		}

		return percentageTemperaturValue;
	}


	void checkForMeaningfulChanges(Vector3 controlPoint){

		float changeValue = 0.001f; //0.0001f;
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

	Color32 getColor(float percentagePosition, Color[]temperatureColors){

		float widthOfOneColumn = temperaturePositionRange / numberOfColumns; //39 columns in 0.40 positionRange	

		if (percentagePosition >= 0 && percentagePosition <= percentagWidthOfOneColumn) {
			color = temperatureColors [0];
		} else if (percentagePosition >  percentagWidthOfOneColumn && percentagePosition <= (percentagWidthOfOneColumn * 2)) {
			color = temperatureColors [1];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 2) && percentagePosition <= (percentagWidthOfOneColumn * 3)) {
			color = temperatureColors [2];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 3) && percentagePosition <= (percentagWidthOfOneColumn * 4)) {
			color = temperatureColors [3];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 4) && percentagePosition <= (percentagWidthOfOneColumn * 5)) {
			color = temperatureColors [4];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 5) && percentagePosition <= (percentagWidthOfOneColumn * 6)) {
			color = temperatureColors [5];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 6) && percentagePosition <= (percentagWidthOfOneColumn * 7)) {
			color = temperatureColors [6];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 7) && percentagePosition <= (percentagWidthOfOneColumn * 8)) {
			color = temperatureColors [7];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 8) && percentagePosition <= (percentagWidthOfOneColumn * 9)) {
			color = temperatureColors [8];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 9) && percentagePosition <= (percentagWidthOfOneColumn * 10)) {
			color = temperatureColors [9];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 10) && percentagePosition <= (percentagWidthOfOneColumn * 11)) {
			color = temperatureColors [10];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 11) && percentagePosition <= (percentagWidthOfOneColumn * 12)) {
			color = temperatureColors [11];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 12) && percentagePosition <= (percentagWidthOfOneColumn * 13)) {
			color = temperatureColors [12];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 13) && percentagePosition <= (percentagWidthOfOneColumn * 14)) {
			color = temperatureColors [13];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 14) && percentagePosition <= (percentagWidthOfOneColumn * 15)) {
			color = temperatureColors [14];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 15) && percentagePosition <= (percentagWidthOfOneColumn * 16)) {
			color = temperatureColors [15];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 16) && percentagePosition <= (percentagWidthOfOneColumn * 17)) {
			color = temperatureColors [16];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 17) && percentagePosition <= (percentagWidthOfOneColumn * 18)) {
			color = temperatureColors [17];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 18) && percentagePosition <= (percentagWidthOfOneColumn * 19)) {
			color = temperatureColors [18];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 19) && percentagePosition <= (percentagWidthOfOneColumn * 20)) {
			color = temperatureColors [19];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 20) && percentagePosition <= (percentagWidthOfOneColumn * 21)) { //white
			color = temperatureColors [20];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 21) && percentagePosition <= (percentagWidthOfOneColumn * 22)) {
			color = temperatureColors [21];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 22) && percentagePosition <= (percentagWidthOfOneColumn * 23)) {
			color = temperatureColors [22];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 23) && percentagePosition <= (percentagWidthOfOneColumn * 24)) {
			color = temperatureColors [23];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 24) && percentagePosition <= (percentagWidthOfOneColumn * 25)) {
			color = temperatureColors [24];
		}  else if (percentagePosition > (percentagWidthOfOneColumn * 25) && percentagePosition <= (percentagWidthOfOneColumn * 26)) { //blue
			color = temperatureColors [25];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 26) && percentagePosition <= (percentagWidthOfOneColumn * 27)) {
			color = temperatureColors [26];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 27) && percentagePosition <= (percentagWidthOfOneColumn * 28)) {
			color = temperatureColors [27];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 28) && percentagePosition <= (percentagWidthOfOneColumn * 29)) {
			color = temperatureColors [28];
		}  else if (percentagePosition > (percentagWidthOfOneColumn * 29) && percentagePosition <= (percentagWidthOfOneColumn * 30)) {
			color = temperatureColors [29];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 30) && percentagePosition <= (percentagWidthOfOneColumn * 31)) {
			color = temperatureColors [30];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 31) && percentagePosition <= (percentagWidthOfOneColumn * 32)) {
			color = temperatureColors [31];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 32) && percentagePosition <= (percentagWidthOfOneColumn * 33)) {
			color = temperatureColors [32];
		}  else if (percentagePosition > (percentagWidthOfOneColumn * 33) && percentagePosition <= (percentagWidthOfOneColumn * 34)) {
			color = temperatureColors [33];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 34) && percentagePosition <= (percentagWidthOfOneColumn * 35)) {
			color = temperatureColors [34];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 35) && percentagePosition <= (percentagWidthOfOneColumn * 36)) {
			color = temperatureColors [35];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 36) && percentagePosition <= (percentagWidthOfOneColumn * 37)) {
			color = temperatureColors [36];
		}  else if (percentagePosition > (percentagWidthOfOneColumn * 37) && percentagePosition <= (percentagWidthOfOneColumn * 38)) {
			color = temperatureColors [37];
		} else if (percentagePosition > (percentagWidthOfOneColumn * 38) && percentagePosition <= (percentagWidthOfOneColumn * 39)) { // 100%
			color = temperatureColors [38];
		}

		return color;

	}

	void fillColorArray(Color[] temperatureColors){


		temperatureColors[0] = new Color32 (227, 24, 23, 1);
		temperatureColors[1] = new Color32 (233, 92, 14, 1);
		temperatureColors[2] = new Color32 (239, 131, 1, 1);
		temperatureColors[3] = new Color32 (246, 165, 0, 1);
		temperatureColors[4] = new Color32 (251, 188, 0, 1);
		temperatureColors[5] = new Color32 (253, 202, 0, 1);
		temperatureColors[6] = new Color32 (255, 219, 0, 1);
		temperatureColors[7] = new Color32 (255, 228, 0, 1);
		temperatureColors[8] = new Color32 (255, 235, 0, 1);
		temperatureColors[9] = new Color32 (255, 235, 10, 1);
		temperatureColors[10] = new Color32 (254, 237, 20, 1);
		temperatureColors[11] = new Color32 (254, 237, 27, 1);
		temperatureColors[12] = new Color32 (252, 238, 69, 1);
		temperatureColors[13] = new Color32 (252, 239, 108, 1);
		temperatureColors[14] = new Color32 (251, 240, 149, 1);
		temperatureColors[15] = new Color32 (250, 242, 178, 1);
		temperatureColors[16] = new Color32 (248, 244, 210, 1);
		temperatureColors[17] = new Color32 (247, 245, 229, 1);
		temperatureColors[18] = new Color32 (246, 245, 242, 1);
		temperatureColors[19] = new Color32 (245, 245, 246, 1);
		temperatureColors[20] = new Color32 (255, 255, 255, 1); //white
		temperatureColors[21] = new Color32 (225, 238, 244, 1);
		temperatureColors[22] = new Color32 (201, 229, 241, 1);
		temperatureColors[23] = new Color32 (168, 217, 239, 1);
		temperatureColors[24] = new Color32 (105, 198, 234, 1);
		temperatureColors[25] = new Color32 (53, 188, 232, 1);
		temperatureColors[26] = new Color32 (2, 172, 228, 1);
		temperatureColors[27] = new Color32 (0, 166, 226, 1);
		temperatureColors[28] = new Color32 (0, 160, 224, 1);
		temperatureColors[29] = new Color32 (0, 158, 224, 1);
		temperatureColors[30] = new Color32 (0, 157, 223, 1);
		temperatureColors[31] = new Color32 (0, 156, 222, 1);
		temperatureColors[32] = new Color32 (0, 154, 220, 1);
		temperatureColors[33] = new Color32 (0, 150, 216, 1);
		temperatureColors[34] = new Color32 (0, 147, 212, 1);
		temperatureColors[35] = new Color32 (0, 142, 207, 1);
		temperatureColors[36] = new Color32 (1, 135, 200, 1);
		temperatureColors[37] = new Color32 (6, 127, 193, 1);
		temperatureColors[38] = new Color32 (12, 118, 185, 1);
	
	}

}
