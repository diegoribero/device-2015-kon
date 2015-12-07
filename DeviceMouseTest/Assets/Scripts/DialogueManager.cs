using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using SimpleJSON;
using System.IO;  
using System;


public class DialogueManager : MonoBehaviour {

	public List<JSONNode> jsonList = new List<JSONNode>();
	public GameObject dialogueSystem;
	public Text dialogueName;
	public Text dialogueMessage;
	public Image dialogueAvatar;
	public Image viktorAvatar;
	public Image oldManAvatar;
	public EndThisLevel scriptWithEndMethod;

	bool activeDialogue = false;
	int currentSentence = -1;
	float nextButtonPressed = 0;
	float previousNextButtonPressed = 0;

	void startLevel1(){
		//startDialogueFromFile("Assets/Resources/Level1Start.txt");
		startDialogueFromVar(1);
	}

	void endLevel1(){
		//startDialogueFromFile("Assets/Resources/Level1End.txt");
		startDialogueFromVar(2);
	}

	void invokeEndMethod(){
		if(scriptWithEndMethod != null){
			scriptWithEndMethod.Invoke("endMethod", 0f);
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		previousNextButtonPressed = nextButtonPressed;
		nextButtonPressed = Input.GetAxis("Next");

		if(previousNextButtonPressed > 0 && nextButtonPressed <= 0){
			onNextButton();
		}
	}


	public void startDialogueFromVar(int var){
		
		if(dialogueSystem != null){
			if(!activeDialogue){
				Debug.Log("Calling new dialog!");
				jsonList.Clear();
				if(var == 1){
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Este es nuestro sitio de entrenamiento, empezaremos con algo simple\"}");
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Primero deberas ubicar la excavadora en la zona verde\"}");
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Luego deberas tumbar el bloque que se encuentra en el tejado\"}");
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Vamos a ver que tan bien se te da esto de manejar maquinaria pesada...\"}");

				} else if (var == 2){
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Muy bien!\"}");
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Parece que tienes lo que se necesita\"}");
					processJSONLine("{\"avatar\":0, \"name\":\"Viejo Maestro\", \"message\":\"Ya veremos como te va en nuestro siguiente entrenamiento...\"}");
				}

				Debug.Log("Successfully loaded!");
				onNextButton();
				activeDialogue = true;
				dialogueSystem.SetActive(true);
			} else {
				Debug.Log("Another dialogue is already active!!!");
			}
		} else {
			Debug.Log("No dialogue system linked!!!");
		}
	}

	public void startDialogueFromFile(string filePath){

		if(dialogueSystem != null){
			if(!activeDialogue){
				Debug.Log("Calling new dialog!");
				if(loadConversationFromFile(filePath)){
					Debug.Log("Successfully loaded!");
					onNextButton();
					activeDialogue = true;
					dialogueSystem.SetActive(true);
				} else {
					Debug.Log("Unable to load dialogue...");
				}
			} else {
				Debug.Log("Another dialogue is already active!!!");
			}
		} else {
			Debug.Log("No dialogue system linked!!!");
		}
	}

	bool loadConversationFromFile(string filePath){
		jsonList.Clear();
		try {
			string line;
			StreamReader streamReader = new StreamReader(filePath, Encoding.Default);
			using (streamReader) {				
				do {
					line = streamReader.ReadLine();
					if (line != null) {
						processJSONLine(line);
					}
				}
				while (line != null);
				streamReader.Close();
			}
		} catch (Exception e) {
			Debug.Log(e.Message);
			return false;
		}
		
		return true;
	}
	
	void processJSONLine(string JSONLine){
		jsonList.Add(JSON.Parse(JSONLine));
	}

	public void onNextButton(){
		if(!getNextMessage()){
			conversationFinished();
		}
	}

	bool getNextMessage(){
		//Debug.Log("JSON count = " + jsonList.Count);
		currentSentence++;
		if(currentSentence+1 > jsonList.Count){ //Finished the message script
			jsonList.Clear();
			currentSentence = -1;
			return false;
		} else {
			if(jsonList.Count > 0){
				JSONNode tempJSON;
				string name = "";
				string message = "";
				bool left = true;
				bool top = false; 
				int avatar = -1;
				tempJSON = jsonList[currentSentence];

				if (tempJSON["name"] != null){
					//name = tempJSON["name"].Value;
					dialogueName.text = tempJSON["name"].Value;
				} else {
					dialogueName.text = "";
				}

				if (tempJSON["message"] != null){
					//message = tempJSON["message"].Value;
					dialogueMessage.text = tempJSON["message"].Value;
				}


				/*if (tempJSON["top"] != null){
					top = tempJSON["top"].AsBool;
				}
				if (tempJSON["left"] != null){
					left = tempJSON["left"].AsBool;
				}
				*/

				if (tempJSON["avatar"] != null){
					avatar = tempJSON["avatar"].AsInt;
				}
			}
			return true;
		}
	}

	public void showMessage(bool dialogueAtTop, bool avatarLeft, string message, Texture2D backgroundTexture, string name = ""){
		/*
		avatarBoxEnabled = false;
		avatarBoxRight = !avatarLeft;
		boxTop = dialogueAtTop;
		boxTexture = backgroundTexture;
		currentMessage = message;
		currentName = name;
		
		if(name == ""){
			nameBoxEnabled = false;
		} else {
			nameBoxEnabled = true;
		}
		defineNameBoxPosition();
		defineDialogueBoxPosition();
		defineMessageBoxPosition();
		*/
	}

	public void conversationFinished(){
		activeDialogue = false;
		dialogueSystem.SetActive(false);
		invokeEndMethod();
		//resetConversation();
	}
}
