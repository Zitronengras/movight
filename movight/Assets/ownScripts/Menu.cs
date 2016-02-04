using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	Position positionScript;

	GameObject light;
	Vector3 fingerPos;
	Vector3 lightPos;
	float selectedLightDistanceToController;


	//Raycaster
	LayerMask onlyMenuLayer;
	RaycastHit hitObject = new RaycastHit();
	GameObject hitTile;

	bool isIntensityHit = false;
	bool isPositionHit = false;
	bool isTemperatureHit = false;

	//selectMenuTile
	Color highlightColor;
	Color inactiveColor;
	int hitIntensityCounter = 0;
	int hitPositionCounter = 0;
	int hitTemperatureCounter = 0;

	int menuCountdown = 15;

	GameObject intensityTile;
	GameObject positionTile;
	GameObject colorTile;

	bool isMenuActiv = false;


	public static bool isIntensityActive = false;
	public static bool isPositionActive = false;
	public static bool isTemperatureActive = false;


	// Use this for initialization
	void Start () {

		GameObject positionScriptObject = GameObject.Find ("PositionLight");
		positionScript = positionScriptObject.GetComponent<Position> ();
		//Debug.Log(

		onlyMenuLayer = 1 << LayerMask.NameToLayer ("menu");

		intensityTile = GameObject.Find ("LightIntensity");
		positionTile  = GameObject.Find ("Position");
		colorTile  = GameObject.Find ("ColorTemperature");
		inactiveColor = colorTile.GetComponent<Renderer> ().material.color;
		highlightColor = new Color(0.17f, 0.70f, 1.0f, 0.6f);

		intensityTile.SetActive(false);
		positionTile.SetActive(false);
		colorTile.SetActive(false);


	}
	
	// Update is called once per frame
	void Update () {

		if (DetectIndexFinger.isFingerDetected == true) {				

			if (SelectLight.isLightSelected == true) { 

				light = SelectLight.light; //selected light
				lightPos = light.transform.position;
				fingerPos = DetectIndexFinger.fingerPos; //unity absolute fingerPosition

				//if (isMenuActiv == false) {
				if (isPositionActive == false && isIntensityActive == false && isTemperatureActive == false) {

					activeMenu ();
					selectMenuTile ();

				}
				if (isPositionActive == true) {

					//if (hitCounter == menuCountdown) {

						deactiveMenu ();
						SelectLight.waitCountdown = 1;
						positionScript.moveLight (light, lightPos, fingerPos);
						Debug.Log ("moveLight()");

					/*} else(hitCounter == menuCountdown){

						activeMenu ();
						selectMenuTile ();

					}*/





				}


					

					
				//}
			}
					


				


		}
	}
	


	void selectMenuTile(){

		//activeMenu ();

		if (Physics.Raycast (DetectIndexFinger.handControllerPos, DetectIndexFinger.fingerPos, out hitObject, ConstructionDistance.maxWallDistance, onlyMenuLayer)) {

			hitTile =  hitObject.collider.gameObject;
			Debug.Log ("hitTile: " + hitTile.ToString ());

			//hitTile.GetComponent<Renderer> ().material.color = highlightColor;

			Debug.Log ("hitTile.name: " + hitTile.name.ToString ());

			//change color when hit
			if (hitTile.name.Equals("LightIntensity")) {

				hitPositionCounter = 0;
				hitTemperatureCounter = 0;

				isIntensityHit = true;
				isPositionHit = false;
				isTemperatureHit = false;

				hitTile.GetComponent<Renderer> ().material.color = highlightColor;

				hitIntensityCounter += 1;
				Debug.LogFormat ("hitCounter: " + hitIntensityCounter.ToString ());
				if (hitIntensityCounter == menuCountdown) {
					
					isIntensityActive = true;
					hitIntensityCounter = 0;

				}

				//positionTile.GetComponent<Renderer> ().material.color = inactiveColor;
				//colorTile.GetComponent<Renderer> ().material.color = inactiveColor;

				Debug.Log ("LightIntensity ausgewählt");

			}

			if (hitTile.name.Equals("Position")) {

				hitIntensityCounter = 0;
				hitTemperatureCounter = 0;


				isIntensityHit = false;
				isPositionHit = true;
				isTemperatureHit = false;

				hitTile.GetComponent<Renderer> ().material.color = highlightColor;

				hitPositionCounter += 1;
				Debug.Log ("hitCounter: " + hitPositionCounter.ToString());

				if (hitPositionCounter == menuCountdown) {

					Debug.Log ("Position selected");

					//deactiveMenu ();

					isPositionActive = true;
					hitPositionCounter = 0;


				}

				Debug.Log ("position ausgewählt");

			}
			if (hitTile.name.Equals("ColorTemperature")) {

				hitIntensityCounter = 0;
				hitPositionCounter = 0;

				isIntensityHit = false;
				isPositionHit = false;
				isTemperatureHit = true;

				hitTile.GetComponent<Renderer> ().material.color = highlightColor;
				//positionTile.GetComponent<Renderer> ().material.color = inactiveColor;
				//intensityTile.GetComponent<Renderer> ().material.color = inactiveColor;

				hitTemperatureCounter += 1;

				if (hitTemperatureCounter == menuCountdown) {

					Debug.Log ("Position selected");

					//deactiveMenu ();

					isPositionActive = true;

					hitTemperatureCounter = 0;


				}

				Debug.Log ("ColorTemperature ausgewählt");

			}

			//change color back when not hit anymore
			if (isIntensityHit == false) {

				intensityTile.GetComponent<Renderer> ().material.color = inactiveColor;

			}
			if (isPositionHit == false) {

				positionTile.GetComponent<Renderer> ().material.color = inactiveColor;

			}
			if (isTemperatureHit == false) {

				colorTile.GetComponent<Renderer> ().material.color = inactiveColor;

			}
		}
		
	}


	void activeMenu(){
		
		GameObject menu = GameObject.Find ("Menu");

		Vector3 lightPosition = SelectLight.light.transform.position;
		Vector3 normalizedLightPosition = lightPosition.normalized;
		Vector3 menuPosition = normalizedLightPosition * 0.6f;
		menuPosition.y = lightPosition.y - 0.6f;

		menu.transform.position = menuPosition;

		intensityTile.SetActive(true);
		positionTile.SetActive(true);
		colorTile.SetActive(true);

	}

	void deactiveMenu(){
		
		intensityTile.SetActive(false);
		positionTile.SetActive(false);
		colorTile.SetActive(false);

	}
}
