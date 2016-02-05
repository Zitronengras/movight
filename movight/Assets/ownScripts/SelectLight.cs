using UnityEngine;
using System.Collections;

public class SelectLight : MonoBehaviour{
	
	HandFeedback labelScript;
	GameObject labelScriptObject;

	Vector3 controlPoint;

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	public static GameObject light;
	public static Vector3 lightPosition;
	//public static int castDistance;

	//countdown
	bool firstPassThrough = true;
	public static bool isLightHit = false;
	public static bool isLightSelected = false;
	int hitCounter = 0;
	public static int deselectCountdown = 20;
	public static int waitCountdown = 15;

	int bufferCounter = 0;
	int bufferMax = 50;

	//public static int PositionCountdown = 15;

	//TODO no other seletion possible when light selected


	//GameObject selectedLight;


	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		GameObject controllerObject = GameObject.Find ("HeadMountedHandController");

		labelScriptObject = GameObject.Find("SelectionLabelObject");
		//Debug.Log (" 46 labelScriptObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		//Debug.Log (" 50 labelScript: " + labelScript.ToString ());
		labelScriptObject.SetActive(false);


		//gestureScript = controllerObject.GetComponent<Gestures> ();

		//castDistance = ConstructionDistance.maxWallDistance;

	}

	// Update is called once per frame
	void Update () {

		if (Gestures.isSelectGesture == true) {

			controlPoint = Gestures.controlPoint;

			labelScript.displayLabel (Gestures.palmCenter, labelScriptObject);


			if (firstPassThrough == true) { //select light

				if (isLightSelected == false) {
					//Debug.Log ("bufferCounter" + bufferCounter.ToString ());

					//if (bufferMax) {
					if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

						hitCounter += 1;
						Debug.Log ("hitCounter: " + hitCounter.ToString ());

						if (hitCounter == waitCountdown) {

							light = hitObject.collider.gameObject;
							lightPosition = light.transform.position;

							Debug.Log ("Licht Objekt ausgewählt**************************************************: " + light.ToString ());

							//stop select sequence
							hitCounter = 0;
							isLightSelected = true;
							firstPassThrough = false;
						}
					} else {
						
						hitCounter = 0;
						firstPassThrough = true;

					}
				}
			} else {
				
				bufferCounter += 1;
				//Debug.Log ("bufferCounter: " + bufferCounter.ToString ());

				if (bufferCounter >= bufferMax) {

					if (SelectLight.isLightSelected == false) {

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							hitCounter += 1;
							Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								light = hitObject.collider.gameObject;
								lightPosition = light.transform.position;

								Debug.Log ("Licht Objekt ausgewählt**************************************************: " + light.ToString ());

								//stop select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = true;
							}
						}
					}
					//
					if (isLightSelected == true) { //deselect light

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							hitCounter += 1;
							Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								Debug.Log ("Licht abgewählt ***************************************: " + light.ToString ());

								//stop select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = false;
							}
						} else {
							
							hitCounter = 0;

						}
					}
				}			
			}
		} else {
			
			labelScriptObject.SetActive(false);

			Debug.Log ("should set labelScriptObject false*********************");
				

		}
	}
}
