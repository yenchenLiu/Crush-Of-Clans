using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	public GUISkin guiSkin;
	public Texture startButton;
	private bool login,wrong;
	private string id,password;
	// Use this for initialization
	void Start () {

		wrong = false;
		login = false;
		id = "ID";
		password = "Password";

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
		
		if (login == false && wrong==false) {
			if (GUI.Button (new Rect (Screen.width * 1 / 2 - Screen.width * 1 / 8, Screen.height * 4 / 5 - Screen.height * 1 / 10, Screen.width / 4, Screen.height / 5), startButton, guiSkin.customStyles [1])) {
				login = true;
			}
		}
		if (login == true) {

			//GUI.BeginGroup(new Rect ( Screen.width/6, Screen.height/6, Screen.width*2/3, Screen.height*2/3));
			GUI.Box (new Rect (Screen.width/6, Screen.height/6, Screen.width*2/3, Screen.height*2/3), "", guiSkin.box);
			GUI.Label (new Rect (Screen.width*1/4, Screen.height*2/3-Screen.height*4/10, Screen.width*1/2, Screen.height*1/10), "登入遊戲", guiSkin.customStyles[2]);
			id=GUI.TextField( new Rect (Screen.width*1/4, Screen.height*2/3-Screen.height*3/10, Screen.width*1/2, Screen.height*1/10),id, guiSkin.textField);
			password=GUI.TextField(new Rect (Screen.width*1/4, Screen.height*2/3-Screen.height*3/20, Screen.width*1/2, Screen.height*1/10),password, guiSkin.textField);

			if (GUI.Button (new Rect (Screen.width/6+Screen.width*2/3-Screen.width*1/10, Screen.height/6, Screen.width*1/10, Screen.height*1/15), "X" ,guiSkin.button)) {
				login = false;
			}
			if (GUI.Button (new Rect (Screen.width*1/2-Screen.width*1/8, Screen.height*2/3-Screen.height*1/30, Screen.width*1/4, Screen.height*1/10),"確認",guiSkin.button)) {
				if(id=="1234" && password=="5678"){
					print ("Connect Sucesses!");
					PlayerPrefs.SetString ("id",id);
					//CONNECT SERVER
					//PlayerPrefs.SetString ("password",password);
					Application.LoadLevel("MainScene");
				}else{
					wrong=true;
					login=false;
					print ("Connect False!");
					GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "帳號或密碼錯誤",guiSkin.box);
					
					
				}

			}

			//GUI.EndGroup();
		}
		if (wrong == true) {
			GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "帳號或密碼錯誤",guiSkin.box);
			if (GUI.Button (new Rect (Screen.width*1/2-Screen.width*1/8, Screen.height*2/3-Screen.height*1/30, Screen.width*1/4, Screen.height*1/10),"確認",guiSkin.button)) {

				wrong = false;
			}		
		}

	}

}
