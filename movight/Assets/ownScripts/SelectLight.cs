﻿using UnityEngine;
using System.Collections;

public class SelectLight : MonoBehaviour{
	
	DetectIndexFinger fingerScript;

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	int castDistance = 50; //TODO change dynamicly with roomsize

	//countdown
	public static bool isLightHit = false;
	public static bool isLightSelected = false;
	int hitCounter = 0;

	GameObject selectedLight;

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

		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos, out hitObject, castDistance, onlyLightLayer)) {

			isLightHit = true;
			hitCounter += 1;

			//Debug.Log ("***hit light***" + hitObject.collider + " *** " + hitCounter);

			if (hitCounter == 15) {
				isLightSelected = true;
				//Debug.Log ("***ausgewählt" + hitObject.collider.gameObject + " *** " + "\n stop select sequence");
				selectedLight = hitObject.collider.gameObject;
				SetSelectedObject (selectedLight); //sets current Object for following control
				Debug.Log("getroffenes Licht Objekt: " + selectedLight);


				//stop select sequence
				hitCounter = 0;
				isLightHit = false;
			}											

		} else {
			hitCounter = 0;
			isLightHit = false;

			//Debug.Log ("hit nothing \n stop select sequence");
		}
	}
	void SetSelectedObject(GameObject gameObject) {
		selectedLight = gameObject;
	}
	public GameObject GetSelectedLight() {
		return selectedLight;
	}
}
