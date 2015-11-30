using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  

public class DialogueMessage : MonoBehaviour {

	string dialogue = "";
	int currentSentence = 0;
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
	

	public Texture2D boxTexture;
	public Texture2D avatarBoxTexture;

	//conversation.Add(new string("Who are you?"));

	// Use this for initialization
	
	void loadFromFileToConversation(string filePath){
		try {
			string line;
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			using (theReader) {				
				do {
					line = theReader.ReadLine();
					if (line != null) {
						string[] entries = line.Split(',');
						if (entries.Length > 0)
							DoStuff(entries);
					}
				}
				while (line != null);    
				theReader.Close();
				return true;
			}
		} catch (Exception e) {
			Console.WriteLine("{0}\n", e.Message);
			return false;
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
			nameBoxWidth = boxWidth - (avatarBoxHorizontalPadding*2) - avatarBoxWidth - (nameBoxHorizontalPadding*2);
			nameBoxX = avatarBoxX + avatarBoxWidth + avatarBoxHorizontalPadding + nameBoxHorizontalPadding;
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
			messageBoxWidth = boxWidth - (avatarBoxHorizontalPadding*2) - avatarBoxWidth - (messageBoxHorizontalPadding*2);
			messageBoxX = avatarBoxX + avatarBoxWidth + avatarBoxHorizontalPadding + messageBoxHorizontalPadding;
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

		defineDialogueBoxPosition();
		if(avatarBoxEnabled) defineAvatarBoxPosition();
		if(nameBoxEnabled) defineNameBoxPosition();
		defineMessageBoxPosition();

		Debug.Log("X:" + boxX + " Y: " + boxY + " Width: " + boxWidth + " Height: " + boxHeight);
		Debug.Log("AX:" + avatarBoxX + " AY: " + avatarBoxY + " AWidth: " + avatarBoxWidth + " AHeight: " + avatarBoxHeight);
		Debug.Log("ABHP: " + avatarBoxHorizontalPadding + " ABVP: " + avatarBoxVerticalPadding);
		Debug.Log("SW: " + Screen.width + " SH: " + Screen.height);
		Debug.Log("(" + Screen.height + "*" + boxHeightProportion + ") - (" + boxVerticalPadding*2+ ");");



		conversation.Add("Who are you?");
		conversation.Add("Where did you find it?");
		conversation.Add("I must have it!");
		conversation.Add("How much!?");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("currentSentence:" + currentSentence + "/" + (conversation.Count-1));
		dialogue = conversation[currentSentence];
		if(Input.GetMouseButtonDown(0)){ //Next message || end conversation
			onNextButton();
		} else if (Input.GetMouseButtonDown(1)){ //Skip to last message || end conversation
			onSkipButton();
		}
	}

	void OnGUI() {
		drawBox(boxRectangle, new Color(20, 20, 120, 120), boxTexture);
		if(avatarBoxEnabled) drawBox(avatarBoxRectangle, new Color(20, 120, 20, 120), avatarBoxTexture);
		if(nameBoxEnabled) drawLabel(nameBoxRectangle, new Color(120, 120, 20), "Diana");
		drawLabel(messageBoxRectangle, new Color(20, 120, 120), "Test Text In a really extended version to try and wrap around the container, this will go around and try to look like a sample of the conversation dialogue used in an RPG game");

		GUI.Button( new Rect(20, 20, 400, 20), dialogue);
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

	public void drawLabel(Rect position, Color color, string content){
		//GUIStyle labelStyle = new GUIStyle();
		//labelStyle.fixedWidth = position.width;
		//labelStyle.fixedHeight = position.height;
		//labelStyle.stretchWidth = true;
		//labelStyle.stretchHeight = true;
		
		Color oldColor = GUI.color;
		GUI.color = color;
		GUI.Label(position, content);
		GUI.color = oldColor;
	}

	public void drawBox(Rect position, Color color, Texture2D texture) {
		GUIStyle boxStyle = new GUIStyle();
		boxStyle.fixedWidth = position.width;
		boxStyle.fixedHeight = position.height;
		boxStyle.stretchWidth = true;
		boxStyle.stretchHeight = true;
		//boxStyle.border( new Box(0,0,0,0));

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

	public void showMessage(bool dialogueAtTop, bool avatarLeft, string name = "", string message, Texture2D backgroundTexture){
		if(name == ""){
			nameBoxEnabled = false;
		} else {
			nameBoxEnabled = true;
		}
		avatarBoxEnabled = false;
		avatarBoxRight = !avatarLeft;
		boxTop = dialogueAtTop;
		boxTexture = backgroundTexture;
	}
	
	public void showMessage(bool dialogueAtTop, bool avatarLeft, string name = "", string message, Texture2D backgroundTexture, Texture2D avatarTexture, int avatarPadding = 20){
		if(name == ""){
			nameBoxEnabled = false;
		}
		avatarBoxEnabled = true;
		avatarBoxVerticalPadding = avatarPadding;
		avatarBoxHorizontalPadding = avatarPadding;
		avatarBoxRight = !avatarLeft;
		boxTop = dialogueAtTop;
		boxTexture = backgroundTexture;
		avatarBoxTexture = avatarTexture;
	}

	public void showDemoDialogue(){
		//GUI.BeginGroup(new Rect());
		//GUI.EndGroup();
	}

	public void onNextButton(){
		if(currentSentence < conversation.Count-1){
			currentSentence++;
		} else {
			conversationFinished();
		}
	}

	public void onSkipButton(){
		if(currentSentence < conversation.Count-1){
			skipConversation();
		} else {
			conversationFinished();
		}
	}

	public void resetConversation(){
		currentSentence = 0;
	}

	public void skipConversation(){
		currentSentence = conversation.Count-1;
	}

	public void conversationFinished(){
		resetConversation();
	}
}
