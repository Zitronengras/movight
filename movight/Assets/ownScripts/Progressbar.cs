using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {

	public static GameObject progressbarObject;
	//static Image progressImg;
	float i;

	// Use this for initialization
	void Start () {
		
		progressbarObject = GameObject.Find ("Progressbar");
		Debug.Log ("progressbarObject found" + progressbarObject.ToString ());
		i = (0.1f / SelectLight.waitCountdown);

		progressbarObject.SetActive (false);
		//progressImg = progressbarObject.GetComponent<Image> ();

	}
	float newX = 0;
	int b =  SelectLight.waitCountdown;

	// Update is called once per frame
	void Update () {




		//progressbarObject.transform.localScale = new Vector3(1, i, 1);


	}

	public static void fillProgressbar(){

		progressbarObject.SetActive (true);

		progressbarObject.gameObject.transform.localScale += new Vector3((0.1f / SelectLight.waitCountdown), 0, 0);
		Vector3 currentPos = progressbarObject.transform.position;
		float currentX = currentPos.x;

		currentX = (currentX + ((0.1f / SelectLight.waitCountdown)/2));
		currentPos.x = currentX;
		progressbarObject.transform.position = currentPos;

		Debug.Log ("position.x" + progressbarObject.transform.position.x.ToString ());

	}
}
