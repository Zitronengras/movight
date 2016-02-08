using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {

	public static GameObject progressbarObject;
	//static Vector3 progressbarStartPosition;
	static Vector3 currentPos;
	static float currentY;
	static float moveUp;
	static bool justStarted = false;
	static Vector3 progressbarStartPosition;
	static bool runThrough = false;

	// Use this for initialization
	void Start () {
		
		progressbarObject = GameObject.Find ("Progressbar");
		progressbarStartPosition = progressbarObject.transform.position;
		progressbarObject.SetActive (false);

	}

	// Update is called once per frame
	void Update () {


		Debug.Log ("runThrough" + runThrough.ToString ());
		/*if(progressbarObject.activeSelf == true && justStarted == true){

			progressbarStartPosition = progressbarObject.transform.position;
			justStarted = false;
		
		}*/
	}

	public static void fillProgressbar(float counter){

		progressbarObject.SetActive (true);


		if (counter == 0) {
			progressbarStartPosition = progressbarObject.transform.position;
			runThrough = true;
		}
		//progressbarObject.transform.position = (Gestures.headRotation * progressbarObject.transform.position);//transform.LookAt (Gestures.handControllerPos);

		progressbarObject.gameObject.transform.localScale += new Vector3(0, (0.042f / SelectLight.waitCountdown), 0); //TODO change 0.1 ; 0,0
		currentPos = progressbarObject.transform.position;
		currentY = currentPos.y;
		moveUp = ((0.042f / SelectLight.waitCountdown)/2);
		currentY = (currentY + moveUp); //TODO change 0.1
		currentPos.y = currentY;
		//Debug.Log ("current X position" + currentX.ToString ());
		progressbarObject.transform.position = currentPos;
		//Debug.Log ("position.x" + progressbarObject.transform.position.x.ToString ());

	}

	public static void resetProgressbar(){

		Debug.Log ("reset progressbar");

		progressbarObject.SetActive (false);

		//currentPos = progressbarObject.transform.position;
		//Debug.Log ("PROGRESSBAR ist false #######################################################");
		//currentY = currentPos.y;
		//currentY = currentY - (((0.042f / SelectLight.waitCountdown)/2) * SelectLight.waitCountdown);
		//currentPos.y = currentY;

		if (runThrough == true) {
			
			progressbarObject.transform.position = progressbarStartPosition;
			progressbarObject.gameObject.transform.localScale = new Vector3 (0.003f, 0f, 0.001f); // 0.003f, 0.001f);
			runThrough = false;

		} else if (runThrough == false){
			
			progressbarObject.gameObject.transform.localScale = new Vector3 (0.003f, 0f, 0.001f); // 0.003f, 0.001f);

		}
		//Debug.Log ("ausmaße progressbar lossyScale vorher: " + progressbarObject.gameObject.transform.lossyScale.ToString ());

		//justStarted = true;
		//Debug.Log ("ausmaße progressbar lossyScale nacher: " + progressbarObject.gameObject.transform.lossyScale.ToString ());

	}
}
