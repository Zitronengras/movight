using UnityEngine;
using System.Collections;

public class ColorTemperature : MonoBehaviour {

	HandFeedback labelScript;
	GameObject labelScriptObject;

	GameObject light;
	Light lightSource;
	Vector3 controlPoint;

	//checkForMeaningfulChanges
	Vector3 newPosition;
	Vector3 lastPosition;
	int changeCounter = 0;
	public static bool temperatureShouldChange = false;


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

				//checkForMeaningfulChanges (controlPoint);

			} else {
				labelScriptObject.SetActive(false);
			}

		} else {
			labelScriptObject.SetActive(false);
		}
	
	}

	void checkForMeaningfulChanges(Vector3 controlPoint){

		float changeValue = 0.0001f; //0.001f;
		newPosition = controlPoint;


		if (newPosition.x <= (lastPosition.x + changeValue) && newPosition.x >= (lastPosition.x - changeValue)
			&& newPosition.y <= (lastPosition.y + changeValue) && newPosition.y >= (lastPosition.y - changeValue)
			&& newPosition.z <= (lastPosition.z + changeValue) && newPosition.z >= (lastPosition.z - changeValue)) {

			changeCounter += 1;
			//Debug.Log ("changeCounter " + changeCounter.ToString());

			if (changeCounter == SelectLight.waitCountdown) {

				if (temperatureShouldChange == true) {
					temperatureShouldChange = false;
				} else {
					temperatureShouldChange = true;
				}

				changeCounter = 0;

			}
		}

		lastPosition = controlPoint;

	}

}
