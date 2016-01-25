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
		float scan = 0.01f;
		Vector3 normRay;

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

					currentHand = allHandsInFrame[i];
					//check for valid data
					if (currentHand.IsValid) {
						if (currentHand.IsRight) {
							rightHand = currentHand;
							//Debug.Log ("************************************************Found right hand" + rightHand);
							FingerList extendedFingers = rightHand.Fingers.Extended();
							if (!extendedFingers.IsEmpty) {	
								//Debug.Log("extended fingers found");
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
											//Debug.Log ("LEAP FingerTipPosition: " + fingerTipPosition);
											//Debug.Log ("found extended indexFinger");
											//if (indexFinger.IsExtended) {
											//Debug.Log ("extended indexFinger!");

											//TODO: position entweder mit FingerTip oder center of distal

											//find distal bone
											indexBoneTip = indexFinger.Bone (Bone.BoneType.TYPE_DISTAL);
											//foreach (Bone.BoneType boneType in (Bone.BoneType[]) Enum.GetValues(typeof(Bone.BoneType))) {
											if (indexBoneTip.IsValid) {
												//Debug.Log ("DISTAL Bone of indexFinger!");

												Transform handController = GameObject.Find ("HeadMountedHandController").transform;
												Vector3 handControllerPos = handController.position;
												Debug.Log ("UNITY handControllerPos: " + handControllerPos);

												/*Transform camera = GameObject.Find ("Main Camera").transform;
												Vector3 cameraPos = camera.position;
												Debug.Log ("UNITY cameraPos: " + cameraPos);*/

												//get center point of distal
												Vector middle = indexBoneTip.Center;
												Debug.Log ("middle: " + middle);

												//transform into unityWorld
												Vector3 unityMiddleScaled = middle.ToUnityScaled();
												Debug.Log ("UNITY UnityMiddleScaled: " + unityMiddleScaled);



												//rotate because of HMD
												Vector3 rotUnityMiddleScaled = Quaternion.Euler(270, 180, 0) * unityMiddleScaled;
												Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

												//get head rotation
												var rot = Cardboard.SDK.HeadRotation;
												//GameObject head = GameObject.Find("Head").transform.position;

												//Quaternion headRot = rot.HeadRotation;
												Vector3 final = rot * rotUnityMiddleScaled;

												Debug.DrawRay (handControllerPos, final*10, Color.cyan, 2.0f, true);

												Transform bulp = GameObject.Find("bulp").transform;
												Vector3 bulpPos = bulp.position; //.transform.position;
												Debug.Log ("bulp position: " + bulpPos);
												//Raycast
												//RaycastHit hit = new RaycastHit();
												//Debug.Log ("**************");
												//float distanceToObject = 1.0f;

												/*if (Physics.Raycast(handControllerPos, rotUnityMiddleScaled, 1)) {
													Debug.Log ("*********Lampe getroffen************");
												}*/

												//TODO: offset because of two raycast origins

												//Debug.Log ("LEAP Middle of DISTAL: " + middle);
												//leap to unity world
												//Vector3 unityMiddle = middle.ToUnity();
												//Debug.Log ("UNITY Middle.ToUnity() of DISTAL: " + unityMiddle);

												/*Debug.Log ("UNITY Middle.ToUnityScaled() of DISTAL: " + unityMiddleScaled);
												Debug.Log ("UNITY Middle.ToUnityScaled() of DISTAL X: " + unityMiddleScaled.x);
												Debug.Log ("UNITY Middle.ToUnityScaled() of DISTAL Y: " + unityMiddleScaled.y);
												Debug.Log ("UNITY Middle.ToUnityScaled() of DISTAL Z: " + unityMiddleScaled.z);*/


												/*Debug.Log ("UNITY rotVector X: " + rotUnityMiddleScaled.x);
												Debug.Log ("UNITY rotVector (old Z) new Y: " + rotUnityMiddleScaled.y);
												Debug.Log ("UNITY rotVector (old Y) new Z: " + rotUnityMiddleScaled.z);*/
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