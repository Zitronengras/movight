using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {

	public static GameObject progressbarObject;
	static Vector3 progressbarStartPosition;
	static Vector3 currentPos;
	static float currentX;

	// Use this for initialization
	void Start () {
		
		progressbarObject = GameObject.Find ("Progressbar");
		progressbarStartPosition = progressbarObject.transform.position;
		progressbarObject.SetActive (false);

	}

	// Update is called once per frame
	void Update () {
		

	}

	public static void fillProgressbar(){

		progressbarObject.SetActive (true);

		progressbarObject.gameObject.transform.localScale += new Vector3((0.1f / SelectLight.waitCountdown), 0, 0);
		currentPos = progressbarObject.transform.position;
		currentX = currentPos.x;
		currentX = (currentX + ((0.1f / SelectLight.waitCountdown)/2));
		currentPos.x = currentX;
		progressbarObject.transform.position = currentPos;
		//Debug.Log ("position.x" + progressbarObject.transform.position.x.ToString ());

	}

	public static void resetProgressbar(){

		progressbarObject.SetActive (false);

		//Debug.Log ("PROGRESSBAR ist false #######################################################");
		progressbarObject.transform.position = progressbarStartPosition;
		//Debug.Log ("ausmaße progressbar lossyScale vorher: " + progressbarObject.gameObject.transform.lossyScale.ToString ());

		progressbarObject.gameObject.transform.localScale = new Vector3 (0f, 0.003f, 0.001f); // 0.003f, 0.001f);
		//Debug.Log ("ausmaße progressbar lossyScale nacher: " + progressbarObject.gameObject.transform.lossyScale.ToString ());

	}
}
