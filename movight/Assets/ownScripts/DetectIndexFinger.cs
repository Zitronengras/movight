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
	public static bool isPositionGesture = false;


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
						//Debug.Log ("Found right hand" + rightHand);

						checkForSelectGesture (rightHand);
						checkForPositionGesture (rightHand);





					}
					//TODO right position for else???			
				}/*else {
					isFingerDetected = false;
				}*/
			}
		}

	}

	void checkForSelectGesture(Hand rightHand){

		Debug.Log ("#########################");

		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	
			
			Finger indexFinger = extendedFingers [0]; //since there is only one per hand
			Finger middleFinger = extendedFingers [1]; //since there is only one per hand
			Finger ringFinger = extendedFingers [2]; //since there is only one per hand
			Finger pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if (indexFinger.IsValid && middleFinger.IsValid) {
				if (!ringFinger.IsValid && !pinkyFinger.IsValid) {
					
					Debug.Log ("now there should be the selectGesture############################");

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
					Vector3 pointBetweenIndexMiddle = rotHMDIndexFingerTip + (normalizedBetweenIndexMiddle * (indexToMiddle.magnitude / 2));

					//get head rotation
					var headRotation = Cardboard.SDK.HeadRotation;

					controlPoint = (headRotation * pointBetweenIndexMiddle);

					Debug.DrawRay (handControllerPos, controlPoint, Color.red, 2.0f, true);

				} else {

					isSelectGesture = false;

				}
			} else {
				
				isSelectGesture = false;

			}
		}
	}

	void checkForPositionGesture(Hand rightHand){
		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			Finger indexFinger = extendedFingers [0]; //since there is only one per hand
			Finger middleFinger = extendedFingers [1]; //since there is only one per hand
			Finger ringFinger = extendedFingers [2]; //since there is only one per hand
			Finger pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if(indexFinger.IsValid){
				if (!middleFinger.IsValid && !ringFinger.IsValid && !pinkyFinger.IsValid) {

					Debug.Log("now there should be the positionGesture############################");

					isPositionGesture = true;

					leapIndexTipPosition = indexFinger.TipPosition;

					// work with tipPositon
					Vector3 unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();

					Vector3 rotHMDIndexFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;

					//get head rotation
					var headRotation = Cardboard.SDK.HeadRotation;

					controlPoint = (headRotation * rotHMDIndexFingerTip);

					Debug.DrawRay (handControllerPos, controlPoint, Color.cyan, 2.0f, true);

				} else {

					isPositionGesture = false;

				}
			} else {

				isPositionGesture = false;

			}
		}
	}


}