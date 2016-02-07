using UnityEngine;
using System.Collections;
using Leap;

public class Gestures : MonoBehaviour {


	public static Quaternion headRotation;

	int i = 0;

	Frame frame;
	Controller controller = new Controller();
	Transform handController;
	public static Vector3 handControllerPos;
	Hand rightHand = new Hand();
	Hand currentHand = new Hand();
	Finger indexFinger = new Finger();
	Finger middleFinger = new Finger();
	Finger ringFinger = new Finger();
	Finger pinkyFinger = new Finger();
	HandList allHandsInFrame = new HandList ();
	FingerList extendedFingers = new FingerList ();
	Vector3 unityPalmCenter;
	Vector3 rotHMDPalmCenter;
	Vector3 unityIndexFingerTip;
	Vector3 rotHMDIndexFingerTip;
	Vector3 tmpControlPoint;

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

	//checkForScreenTapGesture
	bool firstRun = true;
	ScreenTapGesture tapGesture; //= new ScreenTapGesture();
	bool detectTapGesture;


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
		allHandsInFrame = frame.Hands;

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

								controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP, false);
								checkForIntensityGesture (rightHand);

							} else if (Position.lightShouldMove == true) { //if light is moving
								
								controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP, false);
								checkForPositionGesture (rightHand);

							} else if (ColorTemperature.temperatureShouldChange == true) { //if color is changing

								controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP, true);
								checkForTemperatureGesture (rightHand);
								checkForScreenTapGesture ();

							} else {
								controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP, false);

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

	void checkForScreenTapGesture(){

		if (firstRun) {
			controller.Config.SetFloat ("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
			controller.Config.SetFloat ("Gesture.ScreenTap.HistorySeconds", .5f);
			controller.Config.SetFloat ("Gestures.ScreenTap.MinDistance", 1.0f);
			controller.Config.Save ();
		}

		if (!frame.Gestures().IsEmpty) {

			GestureList gesturesInFrame = frame.Gestures ();

			for (int i = 0; i < gesturesInFrame.Count; i++) {
				
				if (gesturesInFrame [i].Type == Gesture.GestureType.TYPE_SCREEN_TAP) {
					
					tapGesture = new ScreenTapGesture (gesturesInFrame [i]);
					detectTapGesture = true;

				} else {
					
					detectTapGesture = false;

				}
			}

			if (detectTapGesture) {
				
				Pointable pointable = frame.Pointables.Frontmost;
				Pointable.Zone zone = pointable.TouchZone;
				float distance = pointable.TouchDistance;
				Vector stabilizedPosition = pointable.StabilizedTipPosition;

				InteractionBox iBox = controller.Frame ().InteractionBox;
				Vector normalizedPosition = iBox.NormalizePoint (stabilizedPosition);
				//float x = normalizedPosition.x * windowWidth;
				//float y = windowHeight - normalizedPosition.y * windowHeight;


			}
				
		}



		firstRun = false;
	}

	//TODO correct controlPointValue

	void checkForSelectGesture(Hand rightHand){

		//check for extended fingers on right hand
		extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			indexFinger = extendedFingers [0]; //since there is only one per hand
			middleFinger = extendedFingers [1]; //since there is only one per hand
			ringFinger = extendedFingers [2]; //since there is only one per hand
			pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if (indexFinger.IsValid && middleFinger.IsValid) {
				if (!ringFinger.IsValid && !pinkyFinger.IsValid) {

					Debug.Log ("now there should be the selectGesture##################");

					isSelectGesture = true;

					//palm
					leapPalmCenter = rightHand.PalmPosition;
					unityPalmCenter = leapPalmCenter.ToUnityScaled ();
					rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;
					palmCenter = (headRotation * rotHMDPalmCenter);

					leapIndexTipPosition = indexFinger.TipPosition;
					unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();
					rotHMDIndexFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;
					headRotation = Cardboard.SDK.HeadRotation;
					tmpControlPoint = (headRotation * rotHMDIndexFingerTip);

					controlPoint = Quaternion.Euler (0, 4, 0) * tmpControlPoint;


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
		extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			indexFinger = extendedFingers [0]; //since there is only one per hand
			middleFinger = extendedFingers [1]; //since there is only one per hand
			ringFinger = extendedFingers [2]; //since there is only one per hand
			pinkyFinger = extendedFingers [3]; //since there is only one per hand

			if(indexFinger.IsValid){
				if (!middleFinger.IsValid && !ringFinger.IsValid && !pinkyFinger.IsValid) {

					Debug.Log("############### positionGesture ######################");

					isPositionGesture = true;

					//palm
					leapPalmCenter = rightHand.PalmPosition;
					unityPalmCenter = leapPalmCenter.ToUnityScaled ();
					rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;
					palmCenter = (headRotation * rotHMDPalmCenter);

					//indexfinger
					leapIndexTipPosition = indexFinger.TipPosition;
					// work with tipPositon
					unityIndexFingerTip = leapIndexTipPosition.ToUnityScaled ();
					rotHMDIndexFingerTip = Quaternion.Euler (270, 180, 0) * unityIndexFingerTip;
					//get head rotation
					headRotation = Cardboard.SDK.HeadRotation;


					tmpControlPoint = (headRotation * rotHMDIndexFingerTip);
					controlPoint = Quaternion.Euler (0, 4, 0) * tmpControlPoint;

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
		extendedFingers = rightHand.Fingers.Extended();

		if (extendedFingers.IsEmpty) {	

			Debug.Log ("******FAUST**");

			isIntensityGesture = true;

			leapPalmCenter = rightHand.PalmPosition;

			// work with tipPositon
			unityPalmCenter = leapPalmCenter.ToUnityScaled ();

			rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;

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
		extendedFingers = rightHand.Fingers.Extended();

		if (!extendedFingers.IsEmpty) {	

			indexFinger = extendedFingers [0];
			middleFinger = extendedFingers [1];
			ringFinger = extendedFingers [2];
			pinkyFinger = extendedFingers [3];

			if (indexFinger.IsValid && middleFinger.IsValid && ringFinger.IsValid && pinkyFinger.IsValid) {

				Debug.Log ("now there should be the TemperatureGesture##################");

				isTemperatureGesture = true;

				//palm
				leapPalmCenter = rightHand.PalmPosition;
				unityPalmCenter = leapPalmCenter.ToUnityScaled ();
				rotHMDPalmCenter = Quaternion.Euler (270, 180, 0) * unityPalmCenter;
				//get head rotation
				headRotation = Cardboard.SDK.HeadRotation;
				controlPoint = (headRotation * rotHMDPalmCenter);

				Debug.DrawRay (handControllerPos, controlPoint, Color.yellow, 2.0f, true);
				
			} else {

				isTemperatureGesture = false;

			}
		} else {

			isTemperatureGesture = false;

		}
	}


}
