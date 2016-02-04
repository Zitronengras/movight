using UnityEngine;
using System.Collections;
using Leap;

//namespace Leap{

public class DetectIndexFinger : MonoBehaviour{

	/*public DetectIndexFinger(){

	}*/

	//GetHandControllerPos();

	int i = 0;

	Frame frame;
	Controller controller = new Controller();
	Transform handController;
	public static Vector3 handControllerPos; //fingerScript
	Hand rightHand = new Hand();
	Hand currentHand = new Hand();
	Finger indexFinger = new Finger();
	Finger middleFinger = new Finger();

	Bone distalBone = new Bone ();		



	Leap.Vector leapIndexTipPosition;
	Leap.Vector leapMiddleTipPosition;

	public static bool isFingerDetected = false;

	bool isIndexFingerDetected = false;
	bool isMiddleFingerDetected = false;
	bool isRingFingerDetected = false;
	bool isPinkyFingerDetected = false;

	//checkForSelectGesture
	public static bool isSelectGesture = false;
	int checkForSelectGestureCounter = 0;

	public static Vector3 controlPoint;
	//TODO get them down
	Vector3 indexControlPoint;
	Vector3 middleControlPoint;

	//checkForPositionGesture
	int checkForPositionGestureCounter = 0;
	bool isPositionSelected = false;


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

		checkForGesture ();


	}

	void checkForGesture(){

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
						Debug.Log ("Found right hand" + rightHand);

						checkForSelectGesture (rightHand);
						//checkForPositionGesture (rightHand);





					}
					//TODO right position for else???			
				}/*else {
					isFingerDetected = false;
				}*/
			}
		}

	}

	void checkForSelectGesture(Hand rightHand){

		//TODO find better control point

		Debug.Log ("#########################");

		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	
			
			Finger indexFinger = extendedFingers [0]; //since there is only one per hand
			//Debug.Log("indexFinger" + indexFinger.Type.ToString());

			Finger middleFinger = extendedFingers [1]; //since there is only one per hand
			Finger ringFinger = extendedFingers [2]; //since there is only one per hand
			Finger pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if(indexFinger.IsValid && middleFinger.IsValid){
				if (!ringFinger.IsValid && !pinkyFinger.IsValid) {
					
					Debug.Log("now there should be the selectGesture############################");

					isSelectGesture = true;


					leapIndexTipPosition = indexFinger.TipPosition;
					leapMiddleTipPosition = middleFinger.TipPosition;	

					// work with tipPositon
					Vector3 unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();
					Vector3 unityMiddleFingerTip = leapMiddleTipPosition.ToUnityScaled ();

					Vector3 rotHMDIndexFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;
					Vector3 rotHMDMiddleFingerTip = Quaternion.Euler (270, 180, 0) * unityMiddleFingerTip;

					Vector3 indexToMiddle = -rotHMDIndexFingerTip + rotHMDMiddleFingerTip;
					Vector3 normalizedBetweenIndexMiddle = indexToMiddle.normalized;
					Vector3 pointBetweenIndexMiddle = rotHMDIndexFingerTip + (normalizedBetweenIndexMiddle * (indexToMiddle.magnitude /2));

					//TO change in HeadRotation
					//get head rotation
					var headRotation = Cardboard.SDK.HeadRotation;

					//rotate rotUnityDistalBoneCenter with headMovement
					//indexControlPoint = (headRotation * rotHMDIndexFingerTip); // * 2.5f;
					//middleControlPoint = (headRotation * rotHMDMiddleFingerTip); // * 2.5f;

					controlPoint = (headRotation * pointBetweenIndexMiddle);


					//Debug.Log ("Unity finger pos: " + fingerPos); 
					//Debug.Log ("Unity finger pos x : " + fingerPos.x); 

					Debug.DrawRay (handControllerPos, controlPoint, Color.red, 2.0f, true);

					//Debug.DrawRay (handControllerPos, middleControlPoint, Color.cyan, 2.0f, true);

				}
			}

		}


		/*if (!extendedFingers.IsEmpty) {	
			Debug.Log("extended fingers found");
			foreach (Finger currentExtFinger in extendedFingers) {

				i += 1;

				if (currentExtFinger.IsValid) {

					Finger.FingerType fingerType = currentExtFinger.Type;
					//Debug.Log("fingerType");

					//check for type of extended fingers
					if (fingerType == Finger.FingerType.TYPE_INDEX) {

						isIndexFingerDetected = true;
						//Debug.Log( i.ToString() + "isIndexFingerDetected = true");

						indexFinger = currentExtFinger;

						//continue;

						/*leapTipPosition = indexFinger.TipPosition;

						//isFingerDetected = true;
						//Debug.Log ("isFingerDetected = true");

						//Debug.Log ("found extended indexFinger");

						// work with tipPositon
						Vector3 unityFingerTip = leapTipPosition.ToUnityScaled ();

						//Debug.Log ("UNITY unityDistalBoneCenter: " + unityDistalBoneCenter);

						//rotate because of HMD
						Vector3 rotHMDUnityFingerTip = Quaternion.Euler (270, 180, 0) * unityFingerTip;
						//Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

						//TO change in HeadRotation
						//get head rotation
						var headRotation = Cardboard.SDK.HeadRotation;
						//rotate rotUnityDistalBoneCenter with headMovement
						fingerPos = (headRotation * rotHMDUnityFingerTip); // * 2.5f;
						//Debug.Log ("Unity finger pos: " + fingerPos); 
						//Debug.Log ("Unity finger pos x : " + fingerPos.x); 

						Debug.DrawRay (handControllerPos, fingerPos, Color.cyan, 2.0f, true);

					} else{
						//isIndexFingerDetected = false;
						//Debug.Log("isIndexFingerDetected = false");

					}
					//check for index and middle to select/deselect light
					if (fingerType == Finger.FingerType.TYPE_MIDDLE) {

						isMiddleFingerDetected = true;

						middleFinger = currentExtFinger;

						//Debug.Log( i.ToString() + "isMiddleFingerDetected = true");

						//continue;



					} else{
						//isMiddleFingerDetected = false;
						//Debug.Log("isMiddleFingerDetected = false");

					}
					if (fingerType == Finger.FingerType.TYPE_RING) {

						//isRingFingerDetected = true;

						//Debug.Log( i.ToString() + "isRingFingerDetected = true");


					} else{
						//isRingFingerDetected = false;
						//Debug.Log("isRingFingerDetected = false");

					}
					if (fingerType == Finger.FingerType.TYPE_PINKY) {

						isPinkyFingerDetected = true;


					} else{
						//isPinkyFingerDetected = false;
						//Debug.Log("isPinkyFingerDetected = false");

					}
					Debug.Log( i.ToString() + "isIndexFingerDetected" + isIndexFingerDetected.ToString());
					Debug.Log( i.ToString() + "isMiddleFingerDetected" + isMiddleFingerDetected.ToString());
					Debug.Log( i.ToString() + "isRingFingerDetected" + isRingFingerDetected.ToString());
					Debug.Log( i.ToString() + "isPinkyFingerDetected" + isPinkyFingerDetected.ToString());
				}
			}

			if(isIndexFingerDetected == true && isMiddleFingerDetected == true && isRingFingerDetected == false && isPinkyFingerDetected == false){

				//Debug.Log ( i.ToString() + "+++++++++++++++++++++++++++++++++++++++++++++++++++selectGesture starts to count");


				checkForSelectGestureCounter += 1;

				Debug.Log ( checkForSelectGestureCounter.ToString() + "checkForSelectGestureCounter");


				if (checkForSelectGestureCounter < 5) {

					isSelectGesture = false;

				}
				if (checkForSelectGestureCounter == 5) {

					isSelectGesture = true;

					Debug.Log ("selectGesture == true ***********************************************************************");

					leapIndexTipPosition = indexFinger.TipPosition;
					leapMiddleTipPosition = middleFinger.TipPosition;	

					// work with tipPositon
					Vector3 unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();
					//Vector3 unityMiddleFingerTip = leapMiddleTipPosition.ToUnityScaled ();

					//get angle between fingerTips
					//float angle = Vector3.Angle(unityIndexFingerTip, unityMiddleFingerTip);


					/*float newAngle = angle / 2;

							float lengthIndex = unityIndexFingerTip.magnitude;
							float lengthMiddle = unityMiddleFingerTip.magnitude;

							if (lengthIndex > lengthMiddle) {

								float angleLength = lengthIndex - lengthMiddle;

							} else {
								
								float angleLength = lengthMiddle - lengthIndex;

							}

							Vector3 controlPoint = (unityIndexFingerTip.normalized * angleLength);
							controlPoint = Vector3.RotateTowards(controlPoint, unityMiddleFingerTip, 

					//Debug.Log ("UNITY unityDistalBoneCenter: " + unityDistalBoneCenter);

					//rotate because of HMD
					Vector3 rotHMDUnityFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;

					//float angle = Vector3.Angle(rotHMDUnityFingerTip, unityMiddleFingerTip);


					//Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

					//TO change in HeadRotation
					//get head rotation
					var headRotation = Cardboard.SDK.HeadRotation;
					//rotate rotUnityDistalBoneCenter with headMovement
					controlPoint = (headRotation * rotHMDUnityFingerTip); // * 2.5f;
					//Debug.Log ("Unity finger pos: " + fingerPos); 
					//Debug.Log ("Unity finger pos x : " + fingerPos.x); 

					Debug.DrawRay (handControllerPos, controlPoint, Color.cyan, 2.0f, true);


				}
			}

		}
		Debug.Log ("#########################");*/

	}

	void checkForPositionGesture(Hand rightHand){
		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	
			//Debug.Log("extended fingers found");
			foreach (Finger currentExtFinger in extendedFingers) {

				if (currentExtFinger.IsValid) {

					Finger.FingerType fingerType = currentExtFinger.Type;

					//check for type of extended fingers
					if (fingerType == Finger.FingerType.TYPE_INDEX) {

						isIndexFingerDetected = true;

						indexFinger = currentExtFinger;

					} else{
						isIndexFingerDetected = false;
					}
					//check for index and middle to select/deselect light
					if (fingerType == Finger.FingerType.TYPE_MIDDLE) {

						isMiddleFingerDetected = true;

						//middleFinger = currentExtFinger;


					} else{
						isMiddleFingerDetected = false;
					}
					if (fingerType == Finger.FingerType.TYPE_RING) {

						isRingFingerDetected = true;

					} else{
						isRingFingerDetected = false;
					}
					if (fingerType == Finger.FingerType.TYPE_PINKY) {

						isPinkyFingerDetected = true;

					} else{
						isPinkyFingerDetected = false;
					}

					if (isIndexFingerDetected == true && isMiddleFingerDetected == false
					   && isRingFingerDetected == false && isPinkyFingerDetected == false) {

						checkForPositionGestureCounter += 1;
						if (checkForPositionGestureCounter < 15) {

							isPositionSelected = false;

						}
						if (checkForPositionGestureCounter == 15) {

							/*checkForPositionGestureCounter = 0;

							isPositionSelected = true;

							leapTipPosition = indexFinger.TipPosition;

							//isFingerDetected = true;
							//Debug.Log ("isFingerDetected = true");

							//Debug.Log ("found extended indexFinger");

							// work with tipPositon
							Vector3 unityFingerTip = leapTipPosition.ToUnityScaled ();

							//Debug.Log ("UNITY unityDistalBoneCenter: " + unityDistalBoneCenter);

							//rotate because of HMD
							Vector3 rotHMDUnityFingerTip = Quaternion.Euler (270, 180, 0) * unityFingerTip;
							//Debug.Log ("UNITY rotUnityMiddleScaled: " + rotUnityMiddleScaled);

							//TO change in HeadRotation
							//get head rotation
							var headRotation = Cardboard.SDK.HeadRotation;
							//rotate rotUnityDistalBoneCenter with headMovement
							fingerPos = (headRotation * rotHMDUnityFingerTip); // * 2.5f;
							//Debug.Log ("Unity finger pos: " + fingerPos); 
							//Debug.Log ("Unity finger pos x : " + fingerPos.x); 

							Debug.DrawRay (handControllerPos, fingerPos, Color.cyan, 2.0f, true);
*/
						}



					} else {
						
						checkForPositionGestureCounter = 0;
						isPositionSelected = false; // necessary?

					}

				}
			}
		}
	}


}