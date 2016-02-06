using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {

	public static GameObject progressbarObject;
	static Image progressImg;

	// Use this for initialization
	void Start () {
		
		progressbarObject = GameObject.Find ("ProgressImage");
		progressbarObject.SetActive (false);
		progressImg = progressbarObject.GetComponent<Image> ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	public static void fillProgressbar(float step){
		
		progressbarObject.SetActive (true);
		float newStep = ((1.0f / SelectLight.waitCountdown) * step);
		progressImg.fillAmount = newStep;

	}
}
