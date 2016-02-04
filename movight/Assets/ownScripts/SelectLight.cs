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

	//TODO no other seletion possible when light selected


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

			if (isLightSelected == false) { //select light

				if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

					hitCounter += 1;

					if (hitCounter == waitCountdown) {
						
						light = hitObject.collider.gameObject;
						lightPosition = light.transform.position;

						Debug.Log ("Licht Objekt getroffen**************************************************: " + light.ToString());

						//stop select sequence
						hitCounter = 0;
						isLightSelected = true;
					}
				}
			}
			if (isLightSelected == true) { //deselect light

				if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

					hitCounter += 1;

					if (hitCounter == waitCountdown) {
						
						Debug.Log ("Licht abgewählt ***************************************: " + light.ToString());

						//stop select sequence
						hitCounter = 0;
						isLightSelected = false;
					}
				}
			}
		}
	}
}
