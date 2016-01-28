using UnityEngine;
using System.Collections;

public class SelectObject : MonoBehaviour{
	
	DetectIndexFinger fingerScript;

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	int castDistance = 50; //TODO change dynamicly with roomsize

	//countdown
	bool isHit = false;
	int hitCounter = 0;

	GameObject selectedObject;

	//Test
	Transform bulp;

	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		GameObject controllerObject = GameObject.Find ("HeadMountedHandController");
		fingerScript = controllerObject.GetComponent<DetectIndexFinger> ();


	}

	// Update is called once per frame
	void Update () {

		//for test
		bulp = GameObject.Find("bulp").transform;
		Vector3 bulpPos = bulp.position;

		if (Physics.Raycast (fingerScript.GetHandControllerPos(), fingerScript.GetFingerControl(), out hitObject, castDistance, onlyLightLayer)) {

			isHit = true;
			hitCounter += 1;

			Debug.Log ("***hit light***" + hitObject.collider + " *** " + hitCounter);

			if (hitCounter == 15) {
				Debug.Log ("***ausgewählt" + hitObject.collider + " *** " + hitCounter + "\n stop select sequence");
				//fingerScript.SetSelectedObject ();
				//stop select sequence
				hitCounter = 0;
				isHit = false;
			}											

		} else {
			hitCounter = 0;
			isHit = false;

			Debug.Log ("hit nothing \n stop select sequence");
		}
	}
	void SetSelectedObject(GameObject gameObject) {
		selectedObject = gameObject;
	}
}
