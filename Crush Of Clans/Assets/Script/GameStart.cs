using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	public GUISkin guiSkin;
	public Texture startButton;
	private bool login,wrong,SignWrong;
	private string id,password;
	// Use this for initialization
	void Start () {

		SignWrong = false;
		wrong = false;
		login = false;
		id = "Singo";
		password = "Singo";

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator login_function(){
		//yield return new WaitForSeconds(1);
		
		Server.ConnectToServer();
		Server.Send("10"+id+","+password);
		//Server.Send("31@@@@@");
		//Server.Send("32@@@@@");

		yield return new WaitForSeconds(1);


		
		if(State.LoginSuecess){
			Server.Send("21");
			Server.Send("22");//Position
			Server.Send("23");//source
			PlayerPrefs.SetString ("id",id);
			//CONNECT SERVER
			//PlayerPrefs.SetString ("password",password);
			State.ID=this.id;
			State.Password=this.password;
			print ("Connect Sucesses!");
			Application.LoadLevel("MainScene");
			//return true;
		}else{
			wrong=true;
			login=false;

			//GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "帳號或密碼錯誤",guiSkin.box);
			//return false;
			
		}
	}
	IEnumerator sign_function(){
		
		Server.ConnectToServer();
		
		yield return new WaitForSeconds(1);
		
		
		
		if(State.SignSuecess){

			PlayerPrefs.SetString ("id",id);
			//CONNECT SERVER
			//PlayerPrefs.SetString ("password",password);
			State.ID=this.id;
			State.Password=this.password;
			print ("Connect Sucesses!");
			Application.LoadLevel("MainScene");
			//return true;
		}else{
			SignWrong=true;
			login=false;
			
			//GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "帳號或密碼錯誤",guiSkin.box);
			//return false;
			
		}
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

				StartCoroutine("login_function");



			}
			if (GUI.Button (new Rect (Screen.width*1/2+Screen.width*1/8, Screen.height*2/3-Screen.height*1/30, Screen.width*1/6, Screen.height*1/10),"註冊",guiSkin.button)) {
				
				StartCoroutine("sign_function");
				

				
			}

			//GUI.EndGroup();
		}
		if (wrong == true) {
			GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "帳號或密碼錯誤",guiSkin.box);
			if (GUI.Button (new Rect (Screen.width*1/2-Screen.width*1/8, Screen.height*2/3-Screen.height*1/30, Screen.width*1/4, Screen.height*1/10),"確認",guiSkin.button)) {

				wrong = false;
			}		
		}
		if (SignWrong == true) {
			GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "此帳號已被註冊或格式不符",guiSkin.box);
			if (GUI.Button (new Rect (Screen.width*1/2-Screen.width*1/8, Screen.height*2/3-Screen.height*1/30, Screen.width*1/4, Screen.height*1/10),"確認",guiSkin.button)) {
				
				SignWrong = false;
			}		
		}

	}

}
