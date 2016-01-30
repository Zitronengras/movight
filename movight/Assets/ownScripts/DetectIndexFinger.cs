﻿using UnityEngine;
using System.Collections;
using Leap;

//namespace Leap{

public class DetectIndexFinger : MonoBehaviour{

	/*public DetectIndexFinger(){

	}*/

	//GetHandControllerPos();

	Frame frame;
	Controller controller = new Controller();
	Transform handController;
	public static Vector3 handControllerPos; //fingerScript
	Hand rightHand = new Hand();
	Hand currentHand = new Hand();
	Finger indexFinger = new Finger();
	Bone distalBone = new Bone ();		

	public static Vector3 fingerPos;
	public static Leap.Vector leapTipPosition;
	public static bool isFingerDetected = false;

	// Use this for initialization
	void Start () {

		handController = GameObject.Find ("HeadMountedHandController").transform;
		handControllerPos = handController.position;		

	}

	// Update is called once per frame
	void Update () {

		if (controller.IsConnected) {
			frame = controller.Frame ();
		} else {
			Debug.Log("no controller found");
		}

		//gets Hands objects
		HandList allHandsInFrame = frame.Hands;

		//iterate detected hands
		if (!frame.Hands.IsEmpty) {
			for(int i = 0; i < frame.Hands.Count; i++){

				currentHand = allHandsInFrame[i];

				//check for valid data
				if (currentHand.IsValid) {
					if (currentHand.IsRight) {
						rightHand = currentHand;
						//Debug.Log ("Found right hand" + rightHand);

						//check for extended fingers on right hand
						FingerList extendedFingers = rightHand.Fingers.Extended();

						if (!extendedFingers.IsEmpty) {	
							//Debug.Log("extended fingers found");
							foreach (Finger currentExtFinger in extendedFingers) {

								if (currentExtFinger.IsValid) {

									Finger.FingerType fingerType = currentExtFinger.Type;

								//TODO maybe fingertip??

									if (fingerType == Finger.FingerType.TYPE_INDEX) {

										indexFinger = currentExtFinger;
										leapTipPosition = indexFinger.TipPosition;

										isFingerDetected = true;
										//Debug.Log ("found extended indexFinger");

										/*	
									//find distal bone
										distalBone = indexFinger.Bone (Bone.BoneType.TYPE_DISTAL);

										if (distalBone.IsValid) {
											//Debug.Log ("DISTAL Bone of indexFinger!");

											//get center point of distal
											Leap.Vector distalBoneCenter = distalBone.Center;
											Debug.Log ("distalBoneCenter: " + distalBoneCenter);

											//transform into unityWorld
											//Vector3 unityDistalBoneCenter = distalBoneCenter.ToUnityScaled();
										*/
											// work with tipPositon
											Vector3 unityDistalBoneCenter = leapTipPosition.ToUnityScaled ();
												
											//Debug.Log ("UNITY unityDistalBoneCenter: " + unityDistalBoneCenter);

											//rotate because of HMD
											Vector3 rotUnityDistalBoneCenter = Quaternion.Euler (270, 180, 0) * unityDistalBoneCenter;
											//Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

											//get head rotation
											var headRotation = Cardboard.SDK.HeadRotation;
											//rotate rotUnityDistalBoneCenter with headMovement
											fingerPos = headRotation * rotUnityDistalBoneCenter;
											//SetFingerControl (fingerControl);

											Debug.DrawRay (handControllerPos, fingerPos * 10, Color.cyan, 2.0f, true);



										//}
										} 
								}
							}
						}
					}
				//TODO right position for else???			
				}else {
					isFingerDetected = false;
				}
			}
		}
	}
}