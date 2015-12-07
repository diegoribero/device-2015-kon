using UnityEngine;
using System.Collections;

public class Level1GameManager : MonoBehaviour {

	public GameObject dialogueSystem;
	private GameObject dialogueManager;

	// Use this for initialization
	void Start () {
		string[] stringParam = new string[1];
		stringParam[0] = "Assets/Resources/Level1Start.txt";
		dialogueSystem.GetComponent<DialogueManager>().Invoke("startLevel1", 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
