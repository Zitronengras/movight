using UnityEngine;
using System.Collections;

public class SelectLight : MonoBehaviour{
	
	HandFeedback labelScript;
	GameObject labelScriptObject;

	Vector3 controlPoint;

	//highlighter
	static GameObject highlighter;
	MeshRenderer highlighterRenderer;
	Material highlighterMaterial;
	float ceilingY = ConstructionDistance.ceilingDistance; //TODO get dynamicly
	static float hitObjectHeight;
	static Vector3 highlighterPosition;

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	public static GameObject light;
	public static Vector3 lightPosition;
	static Collider lightCollider;
	//public static int castDistance;

	//countdown
	bool firstPassThrough = true;
	public static bool isLightHit = false;
	public static bool isLightSelected = false;
	int hitCounter = 0;
	public static int deselectCountdown = 40;
	public static int waitCountdown = 40; //15;

	int bufferCounter = 0;
	int bufferMax = 50;

	// Use this for initialization
	void Start () {

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		labelScriptObject = GameObject.Find("SelectionLabelObject");
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		labelScriptObject.SetActive(false);

		highlighter = GameObject.Find("LightHighlight");
		highlighter.SetActive(false);

	}

	// Update is called once per frame
	void Update () {

		if (Gestures.isSelectGesture == true) {

			controlPoint = Gestures.controlPoint;

			labelScript.displayLabel (Gestures.palmCenter, labelScriptObject);

			if (firstPassThrough == true) { //select light

				if (isLightSelected == false) {

					if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {			

						Progressbar.fillProgressbar (hitCounter);
						hitCounter += 1;

						//Debug.Log ("hitCounter: " + hitCounter.ToString ());

						if (hitCounter == waitCountdown) {

							lightCollider = hitObject.collider;
							light = hitObject.collider.gameObject;
							lightPosition = light.transform.position;

							highlighter.SetActive (true);
							setHighlighterPosition (light);
							Progressbar.resetProgressbar ();

							//Debug.Log ("Licht Objekt ausgewählt*********************************: " + light.ToString ());

							//stop tmp elements of select sequence
							hitCounter = 0;
							isLightSelected = true;
							firstPassThrough = false;						

						}
					} else {
						
						highlighter.SetActive (false);
						hitCounter = 0;
						firstPassThrough = true;
						Progressbar.resetProgressbar ();

					}
				}

			} else {
				
				bufferCounter += 1;
				//Debug.Log ("bufferCounter: " + bufferCounter.ToString ());

				if (bufferCounter >= bufferMax) {

					if (SelectLight.isLightSelected == false) {

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {
							
							Progressbar.fillProgressbar (hitCounter);
							hitCounter += 1;

							//Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								lightCollider = hitObject.collider;
								light = hitObject.collider.gameObject;
								lightPosition = light.transform.position;

								//Debug.Log ("Licht Objekt ausgewählt**************************************************: " + light.ToString ());

								highlighter.SetActive (true);
								setHighlighterPosition (light);
								Progressbar.resetProgressbar ();

								//stop tmp elements in select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = true;
							}
						} else {
							
							highlighter.SetActive (false);
							Progressbar.resetProgressbar ();

						}
					}
					//
					if (isLightSelected == true) { //deselect light

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							Progressbar.fillProgressbar (hitCounter);
							hitCounter += 1;

							//Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								//Debug.Log ("Licht abgewählt ***************************************: " + light.ToString ());

								highlighter.SetActive (false);
								Progressbar.resetProgressbar ();

								//stop select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = false;
							}
						} else {
							
							Progressbar.resetProgressbar ();
							hitCounter = 0;

						}
					} else {
						
						highlighter.SetActive (false);

					}
				}			
			}
		} else {
			
			labelScriptObject.SetActive(false);				

		}
	}		

	public static void setHighlighterPosition(GameObject light){

		hitObjectHeight = lightCollider.bounds.extents.y;
		highlighterPosition = light.transform.position;
		//Debug.Log ("hitObjectHeight: " + hitObjectHeight.ToString ());
		//Debug.Log ("ConstructionDistance.ceilingDistance: " + ConstructionDistance.ceilingDistance.ToString ());
		highlighterPosition.y = ((ConstructionDistance.ceilingDistance - (2 * hitObjectHeight)) - 0.07f);
		highlighter.transform.position = highlighterPosition;

	}
}

