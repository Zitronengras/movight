﻿using UnityEngine;
using System.Collections;
using Leap;

public class detectIndexFinger : MonoBehaviour {

	Frame frame;
	Controller controller = new Controller();
	Hand rightHand = new Hand();
	Hand currentHand = new Hand();
	int indexFingerID = 1;

	// Use this for initialization
	void Start () {
		//Controller controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("update");
	
		if (controller.IsConnected) {  //controller is a Controller object
			frame = controller.Frame ();
			//Debug.Log("controller found");
		
		} else {
			//Debug.Log("no controller found");
		}

		HandList allHandsInFrame = frame.Hands; //gets Hands objects
	
		//Hand firstHand = hands.get (0);

		//iterate hands
		if (!frame.Hands.IsEmpty) {
			for(int i = 0; i < frame.Hands.Count; i++){
				
				//Debug.Log("all hands" + allHandsInFrame[i]); //System.Diagnostics.Debug.Write
				currentHand = allHandsInFrame[i];
				//check for valid data
				if (currentHand.IsValid) {
					if (currentHand.IsRight) {
						rightHand = currentHand;
						Debug.Log ("************************************************Found right hand" + rightHand);
						//FingerList fingers = rightHand.fingers();
						//Finger fingerOnHandsByID = rightHand.finger (indexFingerID);
					}
				}
			}
		}
	}
}
