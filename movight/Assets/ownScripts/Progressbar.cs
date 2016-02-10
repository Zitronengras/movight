using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {

	public static GameObject progressbarObject;
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

	public static void fillProgressbar(float counter){

		progressbarObject.SetActive (true);


		if (counter == 0) {
			progressbarStartPosition = progressbarObject.transform.position;
			runThrough = true;
		}

		progressbarObject.gameObject.transform.localScale += new Vector3(0, (0.075f / SelectLight.waitCountdown), 0);

	}

	public static void resetProgressbar(){

		progressbarObject.SetActive (false);
		progressbarObject.gameObject.transform.localScale = new Vector3 (0.003f, 0.03f, 0.001f);

	}
}
