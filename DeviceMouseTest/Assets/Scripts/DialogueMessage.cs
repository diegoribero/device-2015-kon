using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  
using System;

using SimpleJSON;

public class DialogueMessage : MonoBehaviour {

	bool activeMessage = false;
	string dialogue = "";
	int currentSentence = -1;
	public List<string> conversation = new List<string>();

	//Dialogue box vars
	int boxBorderColor = 0;
	int boxBackgroundColor = 0;
	int boxHorizontalPadding = 10;
	int boxVerticalPadding = 10;
	int boxWidth, boxHeight, boxX, boxY;
	Rect boxRectangle;
	bool boxTop = false;
	float boxHeightProportion = 0.33f;
	bool avatarBoxEnabled = true;
	bool nameBoxEnabled = true;

	//Avatar box vars
	int avatarBoxBorderColor = 0;
	int avatarBoxBackgroundColor = 0;
	int avatarBoxHorizontalPadding = 10;
	int avatarBoxVerticalPadding = 10;
	int avatarBoxWidth, avatarBoxHeight, avatarBoxX, avatarBoxY;
	Rect avatarBoxRectangle;
	bool avatarBoxRight = false;

	int nameBoxHorizontalPadding = 10;
	int nameBoxVerticalPadding = 10;
	int nameBoxWidth, nameBoxHeight, nameBoxX, nameBoxY;
	Rect nameBoxRectangle;

	int messageBoxHorizontalPadding = 10;
	int messageBoxVerticalPadding = 10;
	int messageBoxWidth, messageBoxHeight, messageBoxX, messageBoxY;
	Rect messageBoxRectangle;
	
	string currentMessage = "";
	string currentName = "";

	public Texture2D boxTexture;
	public Texture2D avatarBoxTexture;

	public List<JSONNode> jsonList = new List<JSONNode>();


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

	bool getNextMessage(){
		Debug.Log("JSON count = " + jsonList.Count);
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
					name = tempJSON["name"].Value;
				}
				if (tempJSON["message"] != null){
					message = tempJSON["message"].Value;
				}
				if (tempJSON["top"] != null){
					top = tempJSON["top"].AsBool;
				}
				if (tempJSON["left"] != null){
					left = tempJSON["left"].AsBool;
				}
				if (tempJSON["avatar"] != null){
					avatar = tempJSON["avatar"].AsInt;
				}

				Debug.Log("Left is " + (left? "true" : "false"));

				if(avatar == 0){
					showMessage(top, left, message, boxTexture, avatarBoxTexture, 20, name);
				} else {
					showMessage(top, left, message, boxTexture, name);
				}
				/*for(int i=0 ; i < jsonList.Count-1 ; i++){
					tempJSON = jsonList[i];
					if (tempJSON["name"] != null){
						name = tempJSON["name"].Value;
					}
					if (tempJSON["message"] != null){
						message = tempJSON["message"].Value;
					}
					if (tempJSON["top"] != null){
						top = tempJSON["top"].AsBool;
					}
					if (tempJSON["left"] != null){
						left = tempJSON["left"].AsBool;
					}
					if (tempJSON["avatar"] != null){
						avatar = tempJSON["avatar"].AsInt;
					}
					
					if(avatar == 0){
						showMessage(top, left, message, boxTexture, avatarBoxTexture, 20, name);
					} else {
						showMessage(top, left, message, boxTexture, name);
					}
					
					Debug.Log("Message: " + message);
					avatar = -1;
					top = false;
					left = false;
					name = "";
					message = "";
				}*/
				
			}

			return true;
		}


	}

	public void defineDialogueBoxPosition(){
		boxWidth = Screen.width - (boxHorizontalPadding*2);
		boxHeight = (int)(Screen.height*boxHeightProportion) - (boxVerticalPadding*2);
		boxX = 0 + boxHorizontalPadding;
		boxY = boxTop ? 0 + boxVerticalPadding : Screen.height - boxVerticalPadding - boxHeight;
		boxRectangle = new Rect(boxX, boxY, boxWidth, boxHeight);
	}

	public void defineAvatarBoxPosition(){
		avatarBoxHeight = boxHeight - (avatarBoxVerticalPadding*2);
		avatarBoxWidth = avatarBoxHeight;
		avatarBoxX = avatarBoxRight ? boxX + boxWidth - (avatarBoxHorizontalPadding*2) - avatarBoxWidth : boxX + avatarBoxHorizontalPadding;
		avatarBoxY = boxY + boxVerticalPadding;
		avatarBoxRectangle = new Rect(avatarBoxX, avatarBoxY, avatarBoxWidth, avatarBoxHeight);
	}

	public void defineNameBoxPosition(){

		if(avatarBoxEnabled){
			if(avatarBoxRight){
				nameBoxX = boxX + nameBoxHorizontalPadding;
			} else {
				nameBoxX = avatarBoxX + avatarBoxWidth + avatarBoxHorizontalPadding + nameBoxHorizontalPadding;
			}
			nameBoxWidth = boxWidth - (avatarBoxHorizontalPadding*2) - avatarBoxWidth - (nameBoxHorizontalPadding*2);
		} else {
			nameBoxWidth = boxWidth - (nameBoxHorizontalPadding*2);
			nameBoxX = boxX + nameBoxHorizontalPadding;
		}

		nameBoxHeight = 20; //Font size?
		nameBoxY = boxY + nameBoxVerticalPadding;
		nameBoxRectangle = new Rect(nameBoxX, nameBoxY, nameBoxWidth, nameBoxHeight);
	}

	public void defineMessageBoxPosition(){
		
		if(avatarBoxEnabled){
			if(avatarBoxRight){
				messageBoxX = boxX + messageBoxHorizontalPadding;
			} else {
				messageBoxX = avatarBoxX + avatarBoxWidth + avatarBoxHorizontalPadding + messageBoxHorizontalPadding;
			}
			messageBoxWidth = boxWidth - (avatarBoxHorizontalPadding*2) - avatarBoxWidth - (messageBoxHorizontalPadding*2);
		} else {
			messageBoxWidth = boxWidth - (messageBoxHorizontalPadding*2);
			messageBoxX = boxX + messageBoxHorizontalPadding;
		}

		if(nameBoxEnabled){
			messageBoxY = boxY + nameBoxVerticalPadding + nameBoxHeight + messageBoxVerticalPadding;
			messageBoxHeight = boxHeight - nameBoxHeight - nameBoxVerticalPadding - (messageBoxVerticalPadding*2);
		} else {
			messageBoxY = boxY + messageBoxVerticalPadding;
			messageBoxHeight = boxHeight - (messageBoxVerticalPadding*2);
		}

		messageBoxRectangle = new Rect(messageBoxX, messageBoxY, messageBoxWidth, messageBoxHeight);
	}


	void Start () {

		/*Debug.Log("X:" + boxX + " Y: " + boxY + " Width: " + boxWidth + " Height: " + boxHeight);
		Debug.Log("AX:" + avatarBoxX + " AY: " + avatarBoxY + " AWidth: " + avatarBoxWidth + " AHeight: " + avatarBoxHeight);
		Debug.Log("ABHP: " + avatarBoxHorizontalPadding + " ABVP: " + avatarBoxVerticalPadding);
		Debug.Log("SW: " + Screen.width + " SH: " + Screen.height);
		Debug.Log("(" + Screen.height + "*" + boxHeightProportion + ") - (" + boxVerticalPadding*2+ ");");*/



		//conversation.Add("Who are you?");	
		//conversation.Add("Where did you find it?");
		//conversation.Add("I must have it!");
		//conversation.Add("How much!?");

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){ //Next message || end conversation
			if(!activeMessage){
				if(loadConversationFromFile("Assets/Resources/SampleScript.txt")){
					onNextButton();
					activeMessage = true;
				}
			} else {
				onNextButton();
			}
		} else if (Input.GetMouseButtonDown(1)){ //Skip to last message || end conversation
			onSkipButton();
		}
	}

	void OnGUI() {
		if(activeMessage){
			drawBox(boxRectangle, new Color(20, 20, 120, 120), boxTexture);
			if(avatarBoxEnabled) drawBox(avatarBoxRectangle, new Color(20, 120, 20, 120), avatarBoxTexture);
			if(nameBoxEnabled) drawLabel(nameBoxRectangle, new Color(120, 120, 20), currentName);
			drawLabel(messageBoxRectangle, new Color(20, 120, 120), currentMessage);
		}
	}


	public void drawLabel(Rect position, Color color, string content){
		Color oldColor = GUI.color;
		GUI.color = color;
		GUI.Label(position, content);
		GUI.color = oldColor;
	}

	public void drawBox(Rect position, Color color) {
		GUIStyle boxStyle = new GUIStyle();
		boxStyle.fixedWidth = position.width;
		boxStyle.fixedHeight = position.height;
		boxStyle.stretchWidth = true;
		boxStyle.stretchHeight = true;
		boxStyle.wordWrap = true;

		Color oldColor = GUI.color;
		Color oldContentColor = GUI.contentColor;
		Color oldBackgroundColor = GUI.backgroundColor;
		GUI.color = color;
		GUI.contentColor = color;
		GUI.backgroundColor = color;
		GUI.Box(position, "");
		
		GUI.color = oldColor;
		GUI.contentColor = oldContentColor;
		GUI.backgroundColor = oldBackgroundColor;
	}

	public void drawBox(Rect position, Color color, Texture2D texture) {
		GUIStyle boxStyle = new GUIStyle();
		boxStyle.fixedWidth = position.width;
		boxStyle.fixedHeight = position.height;
		boxStyle.stretchWidth = true;
		boxStyle.stretchHeight = true;

		Color oldColor = GUI.color;
		Color oldContentColor = GUI.contentColor;
		Color oldBackgroundColor = GUI.backgroundColor;
		GUIStyle oldStyle = GUI.skin.box;
		Texture2D oldTexture = GUI.skin.box.normal.background;

		GUI.skin.box = boxStyle;
		GUI.skin.box.normal.background = boxTexture;

		GUI.color = color;
		GUI.contentColor = color;
		GUI.backgroundColor = color;
		GUI.Box(position, texture, boxStyle);
		GUI.color = oldColor;
		GUI.contentColor = oldContentColor;
		GUI.backgroundColor = oldBackgroundColor;
		GUI.skin.box = oldStyle;
		GUI.skin.box.normal.background = oldTexture;
		
	}

	public void showMessage(bool dialogueAtTop, bool avatarLeft, string message, Texture2D backgroundTexture, string name = ""){

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
	}
	
	public void showMessage(bool dialogueAtTop, bool avatarLeft, string message, Texture2D backgroundTexture, Texture2D avatarTexture, int avatarPadding = 20, string name = ""){
		if(name == ""){
			nameBoxEnabled = false;
		} else {
			nameBoxEnabled = true;
		}
		avatarBoxEnabled = true;
		avatarBoxVerticalPadding = avatarPadding;
		avatarBoxHorizontalPadding = avatarPadding;
		avatarBoxRight = !avatarLeft;
		boxTop = dialogueAtTop;
		boxTexture = backgroundTexture;
		avatarBoxTexture = avatarTexture;
		currentMessage = message;
		currentName = name;

		defineDialogueBoxPosition();
		defineAvatarBoxPosition();
		defineNameBoxPosition();
		defineMessageBoxPosition();
	}

	public void onNextButton(){
		if(!getNextMessage()){
			conversationFinished();
		}
	}

	public void onSkipButton(){
		skipConversation();
		if(!getNextMessage()){
			conversationFinished();
		}
	}

	public void resetConversation(){
		currentSentence = -1;
	}

	public void skipConversation(){
		currentSentence = conversation.Count-1;
	}

	public void conversationFinished(){
		activeMessage = false;
		resetConversation();
	}
}
