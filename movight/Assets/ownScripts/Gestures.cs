using UnityEngine;
using System.Collections;
using Leap;

public class Gestures : MonoBehaviour {


	public static Quaternion headRotation;

	public Quaternion offSet;

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

	//checkForIntensityGesture
	public static bool isIntensityGesture = false;
	Leap.Vector leapPalmCenter;
	public static Vector3 palmCenter;



	public static Vector3 controlPoint;

	//TODO get them down
	Vector3 indexControlPoint;
	Vector3 middleControlPoint;

	//checkForPositionGesture
	int checkForPositionGestureCounter = 0;
	public static bool isPositionGesture = false;

	//checkForTemperatureGesture
	public static bool isTemperatureGesture = false;

	// Use this for initialization
	void Start () {

		handController = GameObject.Find ("HeadMountedHandController").transform;
		handControllerPos = handController.position;	

	}

	// Update is called once per frame
	void Update () {

		//TEST


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

						if (SelectLight.isLightSelected == false) { //when no light is selected

							checkForSelectGesture (rightHand);

						} else if (SelectLight.isLightSelected == true) { //when light is selected
							
							if (LightIntensity.intensityShouldChange == true) {//if intensity is changing

								checkForIntensityGesture (rightHand);

							} else if (Position.lightShouldMove == true) {//if light is moving

								checkForPositionGesture (rightHand);

							} else {

								checkForSelectGesture (rightHand);
								checkForPositionGesture (rightHand);
								checkForIntensityGesture (rightHand);
								checkForTemperatureGesture (rightHand);

							}
						}
					}
				}
			}
		}
	}

	//TODO correct controlPointValue

	void checkForSelectGesture(Hand rightHand){

		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			Finger indexFinger = extendedFingers [0]; //since there is only one per hand
			Finger middleFinger = extendedFingers [1]; //since there is only one per hand
			Finger ringFinger = extendedFingers [2]; //since there is only one per hand
			Finger pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if (indexFinger.IsValid && middleFinger.IsValid) {
				if (!ringFinger.IsValid && !pinkyFinger.IsValid) {

					Debug.Log ("now there should be the selectGesture##################");

					isSelectGesture = true;

					//palm
					leapPalmCenter = rightHand.PalmPosition;
					Vector3 unityPalmCenter = leapPalmCenter.ToUnityScaled ();
					Vector3 rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;
					palmCenter = (headRotation * rotHMDPalmCenter);

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
					headRotation = Cardboard.SDK.HeadRotation;
					Vector3 tmpControlPoint = (headRotation * pointBetweenIndexMiddle);

					//offset
					Quaternion offSet;

					controlPoint = Quaternion.Euler (0, 2, 0) * tmpControlPoint; //transform.up;

					//controlPoint = (headRotation * 0.2) * controlPoint;

					Debug.DrawRay (handControllerPos, controlPoint, Color.red, 2.0f, true);

				} else {

					isSelectGesture = false;

				}
			} else {

				isSelectGesture = false;

			}
		} else {

			isSelectGesture = false;

		}
	}

	//TODO is in position even when the gesture is not there

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

					Debug.Log("############### positionGesture ######################");

					isPositionGesture = true;

					//palm
					leapPalmCenter = rightHand.PalmPosition;
					Vector3 unityPalmCenter = leapPalmCenter.ToUnityScaled ();
					Vector3 rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;
					palmCenter = (headRotation * rotHMDPalmCenter);

					//indexfinger
					leapIndexTipPosition = indexFinger.TipPosition;
					// work with tipPositon
					Vector3 unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();
					Vector3 rotHMDIndexFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;

					controlPoint = (headRotation * rotHMDIndexFingerTip);

					Debug.DrawRay (handControllerPos, controlPoint, Color.cyan, 2.0f, true);

				} else {

					isPositionGesture = false;

				}
			} else {

				isPositionGesture = false;

			}
		} else {

			isPositionGesture = false;

		}
	}

	void checkForIntensityGesture(Hand rightHand){
		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (extendedFingers.IsEmpty) {	

			Debug.Log ("******FAUST**");

			isIntensityGesture = true;

			leapPalmCenter = rightHand.PalmPosition;

			// work with tipPositon
			Vector3 unityPalmCenter = leapPalmCenter.ToUnityScaled ();

			Vector3 rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;

			//get head rotation
			headRotation = Cardboard.SDK.HeadRotation;

			controlPoint = (headRotation * rotHMDPalmCenter);

			//Debug.Log ("FistPosition.y: " + controlPoint.y.ToString ());

			Debug.DrawRay (handControllerPos, controlPoint, Color.green, 2.0f, true);

		} else {

			isIntensityGesture = false;

		}
	}

	void checkForTemperatureGesture(Hand rightHand){

		//check for extended fingers on right hand
		FingerList extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			Finger indexFinger = extendedFingers [0];
			Finger middleFinger = extendedFingers [1];
			Finger ringFinger = extendedFingers [2];
			Finger pinkyFinger = extendedFingers [3];

			if (indexFinger.IsValid && middleFinger.IsValid && ringFinger.IsValid && pinkyFinger.IsValid) {

				Debug.Log ("now there should be the TemperatureGesture##################");

				isTemperatureGesture = true;

				//palm
				leapPalmCenter = rightHand.PalmPosition;
				Vector3 unityPalmCenter = leapPalmCenter.ToUnityScaled ();
				Vector3 rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
				//get head rotation
				headRotation = Cardboard.SDK.HeadRotation;
				controlPoint = (headRotation * rotHMDPalmCenter);

				/*leapIndexTipPosition = indexFinger.TipPosition;
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
				headRotation = Cardboard.SDK.HeadRotation;
				controlPoint = (headRotation * pointBetweenIndexMiddle);*/

				Debug.DrawRay (handControllerPos, controlPoint, Color.yellow, 2.0f, true);
				
			} else {

				isTemperatureGesture = false;

			}
		} else {

			isTemperatureGesture = false;

		}
	}


}
