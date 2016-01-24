using UnityEngine;
using System.Collections;
using Leap;

namespace Leap{
		
	public class detectIndexFinger : MonoBehaviour {

		Frame frame;
		Controller controller = new Controller();
		Hand rightHand = new Hand();
		Hand currentHand = new Hand();
		Finger indexFinger = new Finger();
		int indexFingerID = 1;
		Bone indexBoneTip = new Bone ();

		//WaitForEndOfFrame

		//Transform lampTransform = lamp.transform;



		// Use this for initialization
		void Start () {
			Transform lamp = GameObject.Find("lamp").transform; //.transform
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
							FingerList extendedFingers = rightHand.Fingers.Extended();
							if (!extendedFingers.IsEmpty) {	
								Debug.Log("extended fingers found");
								foreach (Finger currentExFinger in extendedFingers) {
									//string fingerDescription = currentFinger.ToString();
									//Debug.Log ("Finger description: " + fingerDescription);

									if (currentExFinger.IsValid) {
										Finger.FingerType fingerType = currentExFinger.Type;
										if (fingerType == Finger.FingerType.TYPE_INDEX) {
											string fingerDescription = currentExFinger.ToString();
											//Debug.Log ("Finger description: " + fingerDescription);
											indexFinger = currentExFinger;
											Vector fingerTipPosition = indexFinger.TipPosition;
											Debug.Log ("LEAP FingerTipPosition: " + fingerTipPosition);
											//Debug.Log ("found extended indexFinger");
											//if (indexFinger.IsExtended) {
												//Debug.Log ("extended indexFinger!");
											//find distal bone
											indexBoneTip = indexFinger.Bone (Bone.BoneType.TYPE_DISTAL);
											//foreach (Bone.BoneType boneType in (Bone.BoneType[]) Enum.GetValues(typeof(Bone.BoneType))) {
											if (indexBoneTip.IsValid) {
												//Debug.Log ("DISTAL Bone of indexFinger!");

												Vector middle = indexBoneTip.Center;
												//Debug.Log ("LEAP Middle of DISTAL: " + middle);
												//leap to unity world
												Vector3 unityMiddle = middle.ToUnityScaled();
												Debug.Log ("UNITY Middle of DISTAL: " + unityMiddle);

												Transform handController = GameObject.Find ("HeadMountedHandController").transform;
												Vector3 handControllerPos = handController.position;

												Debug.DrawRay (handControllerPos, unityMiddle, Color.red, 80.0f, false); 


												Transform lamp = GameObject.Find("lamp").transform;
												Vector3 lampPos = lamp.position; //.transform.position;
												Debug.Log ("lamp position: " + lampPos);



												//TODO: position entweder mit FingerTip oder center of distal
											}
												//}

											//}

										}
										//check for indexFinger

									}
									/*if (finger) {
										String fingerDescription = finger.ToString();
										Debug.Log ("Finger description: " + fingerDescription);
									}*/
									//Finger fingerOnHandsByID = rightHand.finger (indexFingerID);
								}
							}

						}
					}
				}
			}
		}
	}
}