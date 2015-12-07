using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level1GameManager : MonoBehaviour {

	public GameObject dialogueSystem;
	private GameObject dialogueManager;
	public GameObject leftHydra;
	public GameObject rightHydra;
	public Camera mainCam; 
	public float timer = 0f;
	public Text timerText;

	// Use this for initialization
	void Start () {
		string[] stringParam = new string[1];
		stringParam[0] = "Assets/Resources/Level1Start.txt";
		dialogueSystem.GetComponent<DialogueManager>().Invoke("startLevel1", 0f);
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if(Input.GetKey("escape")){
			Application.Quit();
		}
	}

	void OnGUI() {
		int minutes = Mathf.FloorToInt(timer / 60F);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		timerText.text = niceTime;
		//GUI.Label(new Rect(10,10,250,100), niceTime);
	}


	public void setHydraControl(bool value){
		Debug.Log("Test value = " + value);
		if(value){
			leftHydra.GetComponent<ExcavatorTracker>().enabled = true;
			rightHydra.GetComponent<ExcavatorTracker>().enabled = true;
			mainCam.GetComponent<ExcavatorScript>().enabled = false;
		} else {
			leftHydra.GetComponent<ExcavatorTracker>().enabled = false;
			rightHydra.GetComponent<ExcavatorTracker>().enabled = false;
			mainCam.GetComponent<ExcavatorScript>().enabled = true;
		}
	}
}
