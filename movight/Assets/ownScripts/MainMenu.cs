using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Button groupA;
	public Button groupB;

	// Use this for initialization
	void Start () {
		
		groupA = groupA.GetComponent<Button>();
		groupB = groupB.GetComponent<Button>();
	
	}

	public void LoadSceneA(){
		SceneManager.LoadScene("InteractionGroupA");
	}
	public void LoadSceneB(){
		SceneManager.LoadScene("InteractionGroupB");
	}

}
