using UnityEngine;
using System.Collections;

public class ColorTemperature : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject light;
	Light lightSource;
	Color32 currentColor;
	Vector3 controlPoint;

	//checkForMeaningfulChanges
	Vector3 newPosition;
	Vector3 lastPosition;
	int changeCounter = 0;
	public static bool temperatureShouldChange = false;

	//
	float temperaturePositionRange = 40.0f;
	Color color = new Color32();


	// Use this for initialization
	void Start () {

		labelScriptObject = GameObject.Find("TemperatureLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);
	
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

			} else {
				labelScriptObject.SetActive(false);
			}

		} else {
			labelScriptObject.SetActive(false);
		}
	
	}

	void calculateHorizontalRange(){

	}


	void changeTemperature(Vector3 controlPoint){

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

	void colorRange(float percentagePosition){

		float numberOfColumns = 39.0f;
		float widthOfOneColumn = temperaturePositionRange / numberOfColumns; //39 columns in 0.40 positionRange	
		float percentagWidthOfOneColumn = 100 / numberOfColumns;

		//float onePercent


		if (percentagePosition <= percentagWidthOfOneColumn) {
			color = new Color32(227, 24, 23, 1);
		}



	}

}
