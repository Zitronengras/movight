using UnityEngine;
using System.Collections;

public class SelectLight : MonoBehaviour{
	
	HandFeedback labelScript;
	GameObject labelScriptObject;

	Vector3 controlPoint;

	//highlighter
	GameObject highlighter;
	MeshRenderer highlighterRenderer;
	Material highlighterMaterial;
	float ceilingY = 0.02799769f; //TODO get dynamicly

	//Raycast
	RaycastHit hitObject = new RaycastHit();
	LayerMask onlyLightLayer;
	public static GameObject light;
	public static Vector3 lightPosition;
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

		onlyLightLayer = 1 << LayerMask.NameToLayer ("light"); //only raycast layer 8 (light)

		GameObject controllerObject = GameObject.Find ("HeadMountedHandController");

		labelScriptObject = GameObject.Find("SelectionLabelObject");
		//Debug.Log (" 46 labelScriptObject: " + labelScriptObject.ToString ());
		labelScript = labelScriptObject.GetComponent<HandFeedback> ();
		//Debug.Log (" 50 labelScript: " + labelScript.ToString ());
		labelScriptObject.SetActive(false);

		//lightSelectedMaterial = new Material(Shader.Find("Standard"));

		highlighter = GameObject.Find("LightHighlight");
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
					//Debug.Log ("bufferCounter" + bufferCounter.ToString ());

					highlighter.SetActive (false);

					//if (bufferMax) {
					if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

						highlighter.SetActive (true);

						/*GameObject hitGameObject = hitObject.collider.gameObject;
						Renderer objectRenderer = hitGameObject.GetComponent<Renderer> (); //.GetComponent<Renderer> ();
						float hitObjectHeight = objectRenderer.bounds.extents.y; //* 2.0;
						highlighter.transform.localScale = new Vector3 (0.0016f, 0f, hitObjectHeight);

						float hitObjectWidthX = objectRenderer.bounds.extents.x; //* 2.0;
						float hitObjectWidthY = objectRenderer.bounds.extents.x; //* 2.0;



						Vector3 newPosition = hitObject.transform.position;
						newPosition.y = (ConstructionDistance.ceilingDistance - (hitObjectHeight / 2));
						highlighter.transform.position = newPosition;*/

						hitCounter += 1;
						Progressbar.fillProgressbar (hitCounter);

						Debug.Log ("hitCounter: " + hitCounter.ToString ());

						if (hitCounter == waitCountdown) {

							light = hitObject.collider.gameObject;
							lightPosition = light.transform.position;

							MeshRenderer highlighterRenderer = highlighter.GetComponent<MeshRenderer> (); //.material;
							highlighterMaterial = highlighterRenderer.material;
							highlighterMaterial.color = new Color32(255,255,255,1);
							highlighterRenderer.material = highlighterMaterial;

							Debug.Log ("Licht Objekt ausgewählt*********************************: " + light.ToString ());


							//stop select sequence
							hitCounter = 0;
							isLightSelected = true;
							firstPassThrough = false;						
							Progressbar.progressbarObject.SetActive (false);

						}
					} else {
						
						highlighter.SetActive (false);
						hitCounter = 0;
						firstPassThrough = true;
						Progressbar.progressbarObject.SetActive (false);

					}
				}
			} else {
				
				bufferCounter += 1;
				//Debug.Log ("bufferCounter: " + bufferCounter.ToString ());

				if (bufferCounter >= bufferMax) {

					if (SelectLight.isLightSelected == false) {

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							/*highlighter.SetActive (true);
							float height = highlighter.transform.h
							Vector3 newPosition = hitObject.transform.position;
							newPosition.y = newPosition.y - 0.1f;
							highlighter.transform.position = newPosition;*/

							hitCounter += 1;
							Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								light = hitObject.collider.gameObject;
								lightPosition = light.transform.position;

								Debug.Log ("Licht Objekt ausgewählt**************************************************: " + light.ToString ());

								//stop select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = true;
							}
						} else {
							highlighter.SetActive (false);
						}
					}
					//
					if (isLightSelected == true) { //deselect light

						if (Physics.Raycast (Gestures.handControllerPos, Gestures.controlPoint, out hitObject, ConstructionDistance.maxWallDistance, onlyLightLayer)) {

							hitCounter += 1;
							Debug.Log ("hitCounter: " + hitCounter.ToString ());

							if (hitCounter == waitCountdown) {

								Debug.Log ("Licht abgewählt ***************************************: " + light.ToString ());

								highlighter.SetActive (false);

								//stop select sequence
								hitCounter = 0;
								bufferCounter = 0;
								isLightSelected = false;
							}
						} else {

							hitCounter = 0;

						}
					}
				}			
			}
		} else {
			
			labelScriptObject.SetActive(false);

			//Debug.Log ("should set labelScriptObject false*********************");
				

		}
	}
}
