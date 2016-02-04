using UnityEngine;
using System.Collections;

public class SelectLight : MonoBehaviour{
	
	DetectIndexFinger fingerScript;

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	public static GameObject light;
	public static Vector3 lightPosition;
	//public static int castDistance;

	//countdown
	public static bool isLightHit = false;
	public static bool isLightSelected = false;
	int hitCounter = 0;
	public static int deselectCountdown = 15;
	public static int waitCountdown = 15;
	//public static int PositionCountdown = 15;


	//GameObject selectedLight;


	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		GameObject controllerObject = GameObject.Find ("HeadMountedHandController");
		fingerScript = controllerObject.GetComponent<DetectIndexFinger> ();

		//castDistance = ConstructionDistance.maxWallDistance;

	}

	// Update is called once per frame
	void Update () {

		if (DetectIndexFinger.isSelectGesture == true) {

			if (isLightSelected == false) {

				if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

					//isLightHit = true;
					hitCounter += 1;

					Debug.Log ("***hit light***" + hitObject.collider + " *** " + hitCounter);

					if (hitCounter == waitCountdown) {
						
						//Debug.Log ("***ausgewählt" + hitObject.collider.gameObject + " *** " + "\n stop select sequence");
						light = hitObject.collider.gameObject;
						lightPosition = light.transform.position;

						//SetSelectedObject (selectedLight); //sets current Object for following control
						Debug.Log ("getroffenes Licht Objekt: " + light.ToString());


						//stop select sequence
						hitCounter = 0;
						isLightSelected = true;
						//isLightHit = false;
					} else {

						isLightSelected = false;

					}

				} else {
					hitCounter = 0;
					//isLightHit = false;
					isLightSelected = false;

					//Debug.Log ("hit nothing \n stop select sequence");
				}
			}
		} else {
			isLightSelected = false;
		}
	}

	/*void SetSelectedObject(GameObject gameObject) {
		selectedLight = gameObject;
	}
	public static GameObject GetSelectedLight() {
		return selectedLight;
	}*/
}
