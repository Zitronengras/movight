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
		Bone distalBone = new Bone ();
		float scan = 0.01f;
		Vector3 normRay;

		Transform handController;
		Vector3 handControllerPos;

		int hit = 0;
		int lightLayer;
		bool isHit;

		//Test
		Transform bulp;

		//Raycast
		RaycastHit hitObject = new RaycastHit();




		// Use this for initialization
		void Start () {

			handController = GameObject.Find ("HeadMountedHandController").transform;
			handControllerPos = handController.position;
			//Debug.Log ("UNITY handControllerPos: " + handControllerPos);

			lightLayer = LayerMask.NameToLayer ("light");

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

										if (fingerType == Finger.FingerType.TYPE_INDEX) {
											
											indexFinger = currentExtFinger;
											//Debug.Log ("found extended indexFinger");

											//find distal bone
											distalBone = indexFinger.Bone (Bone.BoneType.TYPE_DISTAL);

											if (distalBone.IsValid) {
												//Debug.Log ("DISTAL Bone of indexFinger!");

												//get center point of distal
												Vector distalBoneCenter = distalBone.Center;
												//Debug.Log ("distalBoneCenter: " + distalBoneCenter);

												//transform into unityWorld
												Vector3 unityDistalBoneCenter = distalBoneCenter.ToUnityScaled();
												//Debug.Log ("UNITY unityDistalBoneCenter: " + unityDistalBoneCenter);

												//rotate because of HMD
												Vector3 rotUnityDistalBoneCenter = Quaternion.Euler(270, 180, 0) * unityDistalBoneCenter;
												//Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

												//get head rotation
												var headRotation = Cardboard.SDK.HeadRotation;
												//rotate rotUnityDistalBoneCenter with headMovement
												Vector3 distalControl = headRotation * rotUnityDistalBoneCenter;

												Debug.DrawRay (handControllerPos, distalControl*10, Color.cyan, 2.0f, true);

												//for test
												bulp = GameObject.Find("bulp").transform;
												Vector3 bulpPos = bulp.position;
												//Debug.Log ("bulp position: " + bulpPos);

												//Debug.Log ("**************");
												//float distanceToObject = 1.0f;


												//LayerMask maskLayer = LayerMaskExtensions.Create("Ignore Raycast");
												//LayerMask layerMask = 1 << LayerMask.NameToLayer ("light"); // only check for collisions with layerX
												//LayerMask layerMask = ~(1 << LayerMask.NameToLayer ("Ignore Raycast")); // ignore collisions with layerX



												if (Physics.Raycast(handControllerPos, distalControl, out hitObject)) {

													if (hitObject.transform.gameObject.layer == lightLayer) {
														
														Debug.Log ("***getroffen***" + hitObject.collider + " *** " + hit);
														isHit = true;

														//for (int sec = 0; sec <= 10000; sec++) {
															
														//if(
														//}

														//hitObject.collider.attachedRigidbody.

														hit += 1;
													} else {
														isHit = false;
													}
												}

											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}