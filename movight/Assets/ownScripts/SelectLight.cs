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
	float ceilingY = 0.02799769f; //TODO get dynamicly

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
	public static int deselectCountdown = 20;
	public static int waitCountdown = 30; //15;

	int bufferCounter = 0;
	int bufferMax = 50;

	//public static int PositionCountdown = 15;

	//TODO no other seletion possible when light selected


	//GameObject selectedLight;


	// Use this for initialization
	void Start () {

		Debug.Log("----------------------------------------------------------------");


		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		GameObject controllerObject = GameObject.Find ("HeadMountedHandController");

		labelScriptObject = GameObject.Find("SelectionLabelObject");
		//Debug.Log (" ###### labelScriptObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		//Debug.Log (" 50 labelScript: " + labelScript.ToString ());
		labelScriptObject.SetActive(false);

		//lightSelectedMaterial = new Material(Shader.Find("Standard"));

		highlighter = GameObject.Find("LightHighlight");
		//Debug.Log("highlighter: " + highlighter.ToString());
		highlighter.SetActive(false);



		//gestureScript = controllerObject.GetComponent<Gestures> ();

		//castDistance = ConstructionDistance.maxWallDistance;

	}

	// Update is called once per frame
	void Update () {

		if (Gestures.isSelectGesture == true) {

			controlPoint = Gestures.controlPoint;

			labelScript.displayLabel (Gestures.palmCenter, labelScriptObject);


			if (firstPassThrough == true) { //select light

				if (isLightSelected == false) {

					if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {			

						hitCounter += 1;
						Progressbar.fillProgressbar ();

						//Debug.Log ("hitCounter: " + hitCounter.ToString ());

						if (hitCounter == waitCountdown) {

							lightCollider = hitObject.collider;
							light = hitObject.collider.gameObject;
							lightPosition = light.transform.position;

							/*MeshRenderer highlighterRenderer = highlighter.GetComponent<MeshRenderer> (); //.material;
							highlighterMaterial = highlighterRenderer.material;
							highlighterMaterial.color = new Color32(255,255,255,1);
							highlighterRenderer.material = highlighterMaterial;*/

							highlighter.SetActive (true);
							setHighlighterPosition (light);
							Progressbar.resetProgressbar ();
							Debug.Log ("Progressbar reset '''''''''''''''''''''''''''''''''''''''''''''''''''");

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
							
							hitCounter += 1;
							Progressbar.fillProgressbar ();

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

							hitCounter += 1;
							Progressbar.fillProgressbar ();

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

		//GameObject hitGameObject = hitObject.collider.gameObject;
		//Renderer lightRenderer = light.GetComponent<Renderer> (); //.GetComponent<Renderer> ();
		//float hitObjectHeight = lightRenderer.bounds.extents.y; //* 2.0;

		float hitObjectHeight = lightCollider.bounds.extents.y; //* 2.0;


		//highlighter.transform.localScale = new Vector3 (0.0016f, 0f, hitObjectHeight);

		/*float hitObjectWidthX = objectRenderer.bounds.extents.x; //* 2.0;
		float hitObjectWidthY = objectRenderer.bounds.extents.x; //* 2.0;*/

		Vector3 highlighterPosition = light.transform.position;
		Debug.Log ("hitObjectHeight: " + hitObjectHeight.ToString ());
		Debug.Log ("ConstructionDistance.ceilingDistance: " + ConstructionDistance.ceilingDistance.ToString ());

		highlighterPosition.y = ((ConstructionDistance.ceilingDistance - (2 * hitObjectHeight)) - 0.07f);
		highlighter.transform.position = highlighterPosition;

	}
}

