﻿using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
public class Player : MonoBehaviour {
	//資料庫
	/*
	Player的欄位:
	PlayerID(帳號)
	x(x座標)
	z(z座標)
	cart(目前使用的交通工具)
	tool(目前使用的採集工具) 
	source[0](wood)
	source[1](stone)
	source[2](metal)
	...(看有沒有其他資源要加)
	toolLevel[0](手動採集等級)
	toolLevel[1](斧頭採集等級)
	toolLevel[2](十字鎬採集等級)
	...(看有沒有其他工具要加)
	cartLevel[0](手動搬運等級)
	cartLevel[1](手推車搬運等級)
	*/
	//status
	public Texture[] FunctionButton;
	public Texture Bag,Fixed,BombPng,UpGrade,Black;
	public GUISkin guiSkin;
	public string PlayerID="000";
	public float PosX ,PosZ;
	private float picktime,GameTime,BombTimeCount;
	public float infoTime;
	public int tool,cart,toolKind,cartKind,j,w,h;
	private int Status,Process;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 6"UpGrade" 
	public  int[] source;//={0,0,0,0,0,0,0,0};//0 wood, 1 stone ,2 metal;
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	//public  string[] sourceName={"wood","stone","metal","XX","YY"};//0 wood, 1 stone ,2 metal;
	public int[] weight = {5,10,50};//0 wood, 1 stone ,2 metal;
	public int x,y,z;
	public bool Build=false,click=false,Bomb=false,BombGameStart=false,buttonEnable=false,bag=false,DataLoad=false;
	private Rigidbody BuildNow;
	private GameObject PickSource,TriggerHouse,DestroyHouse;
	public GameObject fire,Building,GetStone,GetWood;
	public Rigidbody build_house;
	public bool info;
	public string infotext;
	
	//attribute
	private char[] BombGame={'1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','I','M','N','O','P'};
	private char[] GameQ = {'x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x'};
	private int[] BombGameTime = {3,5,8,10};
	private int[] pick = {5,10,20,50};//{{5,5,10,15},{10,20,30,40},{20,40,60,80},{30,60,90,120}};
	private int[] BombGameButton = {4,9,16,25};
	public int[] toolLevel = {1,0,0,0};
	public int[] cartLevel = {1,0};
	private string BombGameQ,BombGameInput;
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈","UpGrade"};
	private string[] toolName={"手","十字鎬","斧頭","炸彈"};
	private string[] cartName={"手","推車"};
	private float view;
	public int[] package;// = {1000,2000};
	private int[] CDtime = {5,4,3,2};
	public int weightNow;
	public string serverIP="107.167.178.99";//"107.167.178.99";
	public string serverPORT="25565";
	
	//控制移動
	public bool move_flag = false;
	public float[] point = new float[2];
	public float move_speed = 0.2f;
	public Vector3 player_temp;
	
	//顯示資料
	GameObject Trigger_ob;
	/// ///////////////////
	
	//網路用變數
	//Socket: 網路溝通, Thread: 網路監聽
	public Socket clientSocket;
	Thread th_Listen;
	public bool chkThread=true;//Thread Received
	string findThisString = "@@@@@";//chkstring
	int chkCommand = 0;//chk value != -1 is OK
	
	
	private bool ConnectToServer() {
		IPEndPoint ServerEP = new IPEndPoint(IPAddress.Parse(serverIP), int.Parse(serverPORT));
		clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			clientSocket.Connect(ServerEP);
			th_Listen = new Thread(new ThreadStart(Listen));
			th_Listen.Start();
			Thread.Sleep(200);
			infomationText("Already connect to the Server!");
			print("Already connect to the Server!");
			return true;
			
		}
		catch (Exception ex) {
			infomationText("Can't connect to the Server!");
			
			print("Can't connect to the Server!");
			return false;
		}
	}
	
	public void Send(string strSend)
	{
		byte[] byteSend = new byte[1024];
		
		try
		{
			byteSend = Encoding.UTF8.GetBytes(strSend);
			clientSocket.Send (byteSend);  
		}
		catch (Exception ex)
		{
			infomationText("Connection Break!");
			
			print(" Connection Break!");
		}
	}
	
	//監聽 Server 訊息 (Listening to the Server)
	private void Listen() {
		EndPoint ClientEP = clientSocket.RemoteEndPoint;
		//接收要用的 Byte Array
		Byte[] byteLoad = new byte[1024];
		int loadLen;
		
		String strAll;    //接收到的完整訊息strAll=strCase+strInfo
		String strCase;  //命令碼: 00 ~ 99 (前兩碼)
		String strInfo;     //真正傳達的訊息
		while(chkThread)
		{
			try
			{
				
				loadLen = clientSocket.ReceiveFrom(byteLoad, 0, byteLoad.Length, SocketFlags.None, ref ClientEP);

				if (loadLen != 0) {
					strAll = Encoding.UTF8.GetString (byteLoad, 0, loadLen);
					print (strAll);
					strCase = strAll.Substring(0, 2);
					switch (strCase) {
					case "21"://Server Send 01PlayID
						strInfo = strAll.Substring(2);
						string[] com21= strInfo.Split(new string[] { "@@@@@" }, StringSplitOptions.RemoveEmptyEntries);
						infomationText(com21[com21.Length-1]);
						print (com21[com21.Length-1]);
						PlayerID=com21[com21.Length-1];
						break;
						
					case "10"://If Client Login Success 
						strInfo = strAll.Substring(2);
						chkCommand = strInfo.IndexOf(findThisString);
						if(chkCommand!=-1){
							infomationText("Login Success!!");
							
							print("Login Success!!");
							print(PlayerID);
						}
						break;
					case "20":
						strInfo = strAll.Substring(2);
						string[] com20= strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
						PlayerID=com20[1];
						break;
					case "11"://If Client Login Failed
						infomationText("Login Error!!");
						
						print("Login Error!!");
						break;
					case "22":
						strInfo=strAll.Substring(2);
						string[] strs= strInfo.Split(new string[] { "@@@@@" }, StringSplitOptions.RemoveEmptyEntries);
						string[] strxy=strs[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
						print (strxy[0]);
						print (strxy[1]);
						x=int.Parse(strxy[0]);
						z=int.Parse(strxy[1]);
						PosX=int.Parse(strxy[0]);
						PosZ=int.Parse(strxy[1]);
						break;
					case "23":
						strInfo=strAll.Substring(2);
						string[] strsource= strInfo.Split(new string[] { "@@@@@" }, StringSplitOptions.RemoveEmptyEntries);
						string[] strsources=strsource[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
						infomationText(strsources[1]);
						
						print (strsources[1]);
						source[0]=int.Parse (strsources[1]);//0 wood, 1 stone ,2 metal;
						break;
					case "er"://If Client Send Message length < 2 , Server can send "er"
						print ("command Error!!");
						break;
						
					default:
						break;
					}
				}
				
			}
			catch (Exception ex)//產生錯誤時
			{
				print (ex);
				clientSocket.Close();//關閉通訊器
				
				print("伺服器斷線了！");//顯示斷線
				
				th_Listen.Abort();//刪除執行緒
			}
		} 
	}
	
	
	
	/// ///////////////////
	
	// Use this for initialization
	void Start () {
		Process = 10;
		////////資料庫
		/*SELECT Player
			key: PlayerID
			欄位：
			cart(目前使用背包推車)
			tool(目前使用工具)
			source[0](wood)
			source[1](stone)
			source[2](metal)
			x(x座標)
			z(z座標)
			toolLevel[0](手動採集等級)
			toolLevel[1](斧頭採集等級)
			toolLevel[2](十字鎬採集等級)
			...(看有沒有其他工具要加)
			cartLevel[0](手動搬運等級)
			cartLevel[1](手推車搬運等級)
			這些全部先印出來就好
		*/
		DataLoad = true;
		bag=false;
		info = false;
		weightNow = 0;
		cart = 0;
		tool = 0;
		Status = 0;
		view = 50;
		Build=false;
		click = false;
		picktime=Time.time-5;
		BombTimeCount = Time.time - 6;
		BombGameStart = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (DataLoad == false) {
						Process = (int)(Time.time - picktime) - 5;
						print (Process);
						if (Process >= 10) {
								DataLoad = true;	
						}
				}
		weightNow = (source [0] * weight [0])+(source [1] * weight [1]) + (source [2] * weight [2]);
		x = (int)this.transform.position.x;
		y = (int)this.transform.position.y;
		z = (int)this.transform.position.z;
		//this.transform.FindChild ("Main Camera").camera.fieldOfView = view;
		////////資料庫
		/*UPDATE Player
					key: PlayerID
					欄位：
					x(x座標)
					z(z座標)
				*/
		player_temp = this.transform.position;	
		if (Input.GetKey (KeyCode.W)) {
			this.transform.Translate (new Vector3 (0, 0, -1));
			this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,180,0);
			
			if(move_flag){
				float start = Mathf.Pow(Mathf.Pow (player_temp.x - point[0], 2) + Mathf.Pow (player_temp.z - point[1], 2), 0.5f);
				float end = Mathf.Pow(Mathf.Pow ((player_temp.x - move_speed) - point[0], 2) + Mathf.Pow ((player_temp.z - move_speed) - point[1], 2), 0.5f);
				if(end <= start){
					player_temp.x += move_speed ;
					player_temp.z += move_speed ;
				}
			}
			player_temp.x -= move_speed ;
			player_temp.z -= move_speed ;
		}
		
		if (Input.GetKey (KeyCode.S)) {
			this.transform.Translate (new Vector3 (0, 0, 1));
			this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,0,0);
			if(move_flag){
				float start = Mathf.Pow(Mathf.Pow (player_temp.x - point[0], 2) + Mathf.Pow (player_temp.z - point[1], 2), 0.5f);
				float end = Mathf.Pow(Mathf.Pow ((player_temp.x + move_speed) - point[0], 2) + Mathf.Pow ((player_temp.z + move_speed) - point[1], 2), 0.5f);
				if(end <= start){
					player_temp.x -= move_speed ;
					player_temp.z -= move_speed ;
				}
			}
			player_temp.x += move_speed ;
			player_temp.z += move_speed ;
		}
		
		if (Input.GetKey (KeyCode.D)) {
			this.transform.Translate (new Vector3 (-1, 0, 0));
			this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,270,0);
			if(move_flag){
				float start = Mathf.Pow(Mathf.Pow (player_temp.x - point[0], 2) + Mathf.Pow (player_temp.z - point[1], 2), 0.5f);
				float end = Mathf.Pow(Mathf.Pow ((player_temp.x - move_speed) - point[0], 2) + Mathf.Pow ((player_temp.z + move_speed) - point[1], 2), 0.5f);
				if(end <= start){
					player_temp.x += move_speed ;
					player_temp.z -= move_speed ;
				}
			}
			player_temp.x -= move_speed ;
			player_temp.z += move_speed ;
		}
		
		if (Input.GetKey (KeyCode.A)) {
			this.transform.Translate (new Vector3 (1, 0, 0));
			this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,90,0);
			if(move_flag){
				float start = Mathf.Pow(Mathf.Pow (player_temp.x - point[0], 2) + Mathf.Pow (player_temp.z - point[1], 2), 0.5f);
				float end = Mathf.Pow(Mathf.Pow ((player_temp.x + move_speed) - point[0], 2) + Mathf.Pow ((player_temp.z - move_speed) - point[1], 2), 0.5f);
				if(end <= start){
					player_temp.x -= move_speed ;
					player_temp.z += move_speed ;
				}
			}
			player_temp.x += move_speed ;
			player_temp.z -= move_speed ;
		}
		this.transform.position = player_temp;
		
		
		
	}
	
	//Status = 0 虛擬搖桿 移動
	public void infomationText(string text){
		infotext = text;
		info = true;
		infoTime = Time.time;
	}
	void OnGUI(){
		//INFOMATION///////////
		if (info == true) {
			GUI.Box (new Rect (0, Screen.height/2-Screen.height / 20, Screen.width, Screen.height / 10),infotext,guiSkin.box);
			if((int)(Time.time-infoTime)==1){
				info=false;
			}
		}
		//INFOMATION///////////

		if (DataLoad == false) {

			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
			GUI.Label(new Rect (0-Screen.width+(Screen.width*Process/10), Screen.height*4/5, (Screen.width) , Screen.height/20),"    ",guiSkin.button);
			
		} else{
			
		
		if (GUILayout.Button ("Connect", GUILayout.Height(50))) {
			ConnectToServer();
			Send("10singo,singo@@@@@");
		}
		if (GUILayout.Button ("Send", GUILayout.Height(50))) {
			
			Send("21@@@@@");
			//Send("22@@@@@");
			Send("23@@@@@");
		}
		if(GUILayout.Button("Close")){
			clientSocket.Close();//關閉通訊器
				infomationText("伺服器斷線了！");
			print("伺服器斷線了！");//顯示斷線
			
			th_Listen.Abort();//刪除執行緒
			chkThread=false;
		}
		
		
		//我這裡先不考慮其他人物存在的情況，因為如果多個角色存在，還會有攝影機的問題
		//應該要有一個變數來記錄說，如果是其他人物，要把攝影機關掉
		//依照物件的collider的高度做參考計算位置
		//所以建議人物、建築、素材的collider先調整好，在依照顯示的位置進行X、Y的微調加減
		Vector3 worldPosition = new Vector3 (transform.position.x, transform.position.y + collider.bounds.size.y, transform.position.z);
		Vector2 screenPosition = transform.FindChild ("Main Camera").camera.WorldToScreenPoint (worldPosition);
		screenPosition.y = Screen.height - screenPosition.y;
		//字形、顏色、大小，懶的用程式設定，就用一個skin吧
		//GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),"Player ID", style);
			if(info==false ){
				GUI.Label(new Rect(screenPosition.x-50,screenPosition.y,100,100),"Player ID",guiSkin.label);
			
			if (((Status >= 1 && Status <= 4) || Status == 6)&& Trigger_ob!=null) {
			worldPosition = new Vector3 (Trigger_ob.transform.position.x, Trigger_ob.transform.position.y + Trigger_ob.collider.bounds.size.y, Trigger_ob.transform.position.z);
			screenPosition = transform.FindChild ("Main Camera").camera.WorldToScreenPoint (worldPosition);
			screenPosition.y = Screen.height - screenPosition.y;
			if (Status == 1) {
				Source data = Trigger_ob.GetComponent<Source> ();
				GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), Trigger_ob.name + "\n" + data.quatity,guiSkin.label);
			} else if (Status == 2) {
				StockHouse data = Trigger_ob.GetComponent<StockHouse> ();
				GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), Trigger_ob.tag + "  " + data.HouseLevel + "\n" + data.PlayerID + "\n" + data.HP,guiSkin.label);
			} else if (Status == 3) {
				WorkHouse data = Trigger_ob.GetComponent<WorkHouse>();
				GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "  " + data.HouseLevel + "\n" + data.PlayerID + "\n" + data.HP,guiSkin.label);
			} else if (Status == 4) {
				ScientificHouse data = Trigger_ob.GetComponent<ScientificHouse>();
				GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "  " + data.HouseLevel + "\n" + data.PlayerID + "\n" + data.HP,guiSkin.label);
			} else if (Status == 6) {
				HouseLevelUp data = Trigger_ob.GetComponent<HouseLevelUp>();
				GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.label);
			}

		}
			}
		///Bag/////
		if (bag == true) {
			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),Bag, guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10+Screen.width*1/20,Screen.height*9/20, Screen.width*1/10, Screen.width*1/5), "採集量："+pick[toolKind], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*9/20+Screen.width*1/10, Screen.width*1/5, Screen.width*1/5),"重量:"+weightNow+"/"+package[cart], guiSkin.label);
			//GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 1/ 13 , Screen.width/3, Screen.height/16 ),sourceName[0]+":"+ source[0].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 2/ 13 , Screen.width/3, Screen.height/16),sourceName[1]+":"+source[1].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 3/ 13 , Screen.width/3, Screen.height/16 ),sourceName[2]+":"+source[2].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 4/ 13 , Screen.width/3, Screen.height/16 ),sourceName[3]+":"+source[3].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 5/ 13 , Screen.width/3, Screen.height/16 ),sourceName[4]+":"+source[4].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 6/ 13 , Screen.width/3, Screen.height/16),sourceName[5]+":"+source[5].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 7/ 13, Screen.width/3, Screen.height/16 ),sourceName[6]+":"+source[6].ToString(),guiSkin.customStyles[4]);
			
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 8/ 13 , Screen.width/3, Screen.height/16 ),sourceName[7]+":"+source[7].ToString(),guiSkin.customStyles[4]);
			
			
			
			
			
			/*GUI.Box (new Rect (Screen.width*3/8, Screen.height* 1/ 16 , Screen.height/16 , Screen.height/16 ),Bag,guiSkin.customStyles[2]);

			GUI.Box (new Rect (Screen.width*3/8, Screen.height* 3/ 16 , Screen.height/16 , Screen.height/16 ),Bag,guiSkin.customStyles[2]);

			GUI.Box (new Rect (Screen.width*3/8, Screen.height* 5/ 16 , Screen.height/16 , Screen.height/16),Bag,guiSkin.customStyles[2]);

			GUI.Box (new Rect (Screen.width*3/8, Screen.height* 7/ 16 ,Screen.height/16 , Screen.height/16 ),Bag,guiSkin.customStyles[2]);

			GUI.Box (new Rect (Screen.width*3/8, Screen.height* 9/ 16 ,Screen.height/16 , Screen.height/16 ),Bag,guiSkin.customStyles[2]);*/
			
			
			
			
			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				bag = false;
				click=false;
			}
			GUI.EndGroup();
			
			
		}
		
		///Bag/////
		

		
		
		
		
		/*GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height / 10),"");

		GUI.Box (new Rect ( 0, 0, Screen.width/8, Screen.height / 10),"Wood");
		GUI.Box (new Rect ( Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[0].ToString());
		GUI.Box (new Rect ( 2*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Stone");
		GUI.Box (new Rect ( 3*Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[1].ToString());
		GUI.Box (new Rect ( 4*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Metal");
		GUI.Box (new Rect ( 5*Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[2].ToString());

		GUI.Box (new Rect ( 6*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Weight");
		GUI.Box (new Rect ( 7*Screen.width/8, 0, Screen.width/8, Screen.height / 10),weightNow.ToString());
		GUI.Box (new Rect ( 0, Screen.height / 10, Screen.width/8, Screen.height / 10),toolName[tool]+pick[toolKind].ToString());
		GUI.Box (new Rect ( 0, Screen.height *2/ 10, Screen.width/8, Screen.height / 10),cartName[cart]+package[cart].ToString());
		*/
		
		/*string ButtonText;


		if (Time.time - picktime < CDtime[toolKind] && Status==1) {
			GUI.enabled=false;
			ButtonText =StatusName[Status]+"\r\n"+ ((int)(CDtime [tool] - (Time.time - picktime))).ToString ();
						//GUI.Box(new Rect (9 * Screen.width / 10, Screen.height * 1 / 8, Screen.width / 10, Screen.height / 8), ((int)( CDtime[tool]-(Time.time - picktime) )).ToString() );		
		} 
		else {
			ButtonText=StatusName[Status];
		}*/
		
		///BOMB//////////
		if (5 - (int)(Time.time - BombTimeCount) >= 1) {
			infomationText(((int)(5-(Time.time-BombTimeCount))).ToString());
			
		}
		if (BombGameStart == true) {

			if(Time.time-GameTime>=BombGameTime[toolKind]){
				infomationText("False....");
				BombGameStart=false;
				
			}	
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 1/ 10 , Screen.width/2, Screen.height/10 ),BombGameQ);
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 2/ 10, Screen.width/2, Screen.height/10 ),BombGameInput);
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 3/ 10, Screen.width/2*((BombGameTime[toolKind]-Time.time+GameTime)/BombGameTime[toolKind]), Screen.height/20 ),"");
			
			switch(toolKind){
			case 0:
				w=1;
				h=1;
				for(int i=0;i<4;i++){
					//if(h>2) h=1;
					if(w>2){
						w=1;
						h++;
					}
					
					print (w+" "+h );
					if (GUI.Button (new Rect (Screen.width*w/4, Screen.height* h/ 3 , Screen.width/4, Screen.height/3 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
							if(j>=4){
								BombTimeCount=Time.time;
								
							}
						}
					}
					w++;
					
				}
				
				if (j>=4){
						DestroyHouse=TriggerHouse;
						DestroyHouse.gameObject.transform.Translate(new Vector3(0,20,0));
					Destroy(TriggerHouse.gameObject,5);
					Bomb=false;
					BombGameStart=false;
					move_flag = false;
					j=0;
				}
				break;
			case 1:
				w=1;
				h=1;
				for(int i=0;i<9;i++){
					
					if(w>3){
						w=1;
						h++;
					}
					if (GUI.Button (new Rect (Screen.width*w/5, Screen.height* 1/ 3 + (Screen.height* (h-1)*2/ 9), Screen.width/5, Screen.height*2/9 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=9){
					Destroy(TriggerHouse.gameObject);
					Bomb=false;
					BombGameStart=false;
					j=0;
					move_flag = false;
				}
				break;
			case 2:
				w=1;
				h=1;
				for(int i=0;i<16;i++){
					
					if(w>4){
						w=1;
						h++;
					}
					if (GUI.Button (new Rect (Screen.width*1/4+(Screen.width*(w-1)/8), Screen.height* 1/ 3 + (Screen.height* (h-1)/ 6), Screen.width/8, Screen.height*1/6 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=16){
					Destroy(TriggerHouse.gameObject);
					Bomb=false;
					BombGameStart=false;
					j=0;
					move_flag = false;
				}
				break;
			case 3:
				break;
				
			}	
			
		}
		
		if (tool==3 && click == false && Build == false && Bomb==true && GUI.Button (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 4, Screen.width / 6, Screen.height / 4), "Destroy")) {
			j=0;
			w=1;
			h=1;
			BombGameQ="";
			BombGameInput="";
			print (toolKind);
			
			for(int i =0;i<BombGameButton[toolKind];i++){
				GameQ[i]=BombGame[(int)(UnityEngine.Random.Range(0, BombGameButton[toolKind]))];
				BombGameQ+=GameQ[i];
				BombGameQ+=" ";
			}	
			BombGameStart=true;
			GameTime=Time.time;
			//Destroy(TriggerHouse.gameObject);
			Bomb=false;
		}
		///BOMB//////////
		
		
		///FuctionButton/////
		if (click == false) {//hide FuctionButton
			if (Build==false && GUI.Button (new Rect (Screen.width-Screen.width/8, Screen.height* 3 / 4-Screen.height/5 , Screen.width/10, Screen.height/6 ),Bag,guiSkin.customStyles[3])){
				bag=true;
				click=true;
			}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),FunctionButton[Status],guiSkin.customStyles[3])){
				
				switch (Status){
				case 0://build a house
					
					Build = true;
					click=false;
					break;
				case 1:			
					//採集放這邊
					//source++;
					if(Time.time-picktime>CDtime[toolKind] && weightNow < package[cart]){
						Source pickup=PickSource.GetComponent<Source>();					
						int kind=pickup.kind;
						if(tool==0||kind+1==tool ){
							int quatity=pickup.quatity;
							int sourceWeight=weight[kind];
							int getQutity;
							print (quatity);
							if(quatity<=pick[toolKind]){
								if(package[cart]-weightNow-(quatity*sourceWeight)<0){
									getQutity = (package[cart]-weightNow)/sourceWeight;
									source[kind]+=getQutity;
									pickup.quatity-=getQutity;
									//weightNow+=getQutity*sourceWeight;
								}else{
									getQutity=quatity;
									
									source[kind]+=quatity;
									pickup.quatity-=quatity;
									//weightNow+=quatity*sourceWeight;
									//資料庫
									/*
								DELETE Source
								Key: x=pickup.x , z=pickup.z
								*/
									Destroy(PickSource.gameObject);
									
								}
								
							}else{
								if(package[cart]-weightNow-(pick[toolKind]*sourceWeight)<0){
									getQutity = (package[cart]-weightNow)/sourceWeight;
									source[kind]+=getQutity;
									pickup.quatity-=getQutity;
									//weightNow+=getQutity*sourceWeight;
									
								}else{
									getQutity=pick[toolKind];
									source[kind]+=pick[toolKind];
									pickup.quatity-=pick[toolKind];
									//	weightNow+=pick[tool]*sourceWeight;
									
								}
							}
								if(kind==0){
									Vector3 Pos=new Vector3(PickSource.transform.position.x-1,PickSource.transform.position.y+5,PickSource.transform.position.z+1);
									GameObject animateNow=(GameObject) Instantiate(GetWood,Pos,GetWood.transform.rotation);
									Destroy(animateNow,3);
								}else{
									Vector3 Pos=new Vector3(PickSource.transform.position.x-1,PickSource.transform.position.y+5,PickSource.transform.position.z+1);
									
									GameObject animateNow=(GameObject) Instantiate(GetStone,Pos,GetWood.transform.rotation);
									Destroy(animateNow,3);
									
								}
							infomationText("Get "+getQutity.ToString()+" "+sourceName[kind]+" !");
							
							////////資料庫
							/*
						UPDATE Player
						key: PlayerID
						欄位：
						source[0]=source[0](wood)
						source[1]=source[1](stone)
						source[2]=source[2](metal)
						(可以再加其他你想到資源種類)

						UPDATE Source
						key: x,y
						quatity=pickup.quatity;

						*/
							picktime=Time.time;
						}
					}
					Status=0;
					break;
				case 2:
					//倉庫放這邊
					StockHouse StockNow = TriggerHouse.GetComponent<StockHouse>();
					StockNow.work=true;
					click=true;
					break;
				case 3:
					WorkHouse WorkNow = TriggerHouse.GetComponent<WorkHouse>();
					WorkNow.work=true;
					click=true;
					//合成放這邊				
					break;
				case 4:
					ScientificHouse ScienceNow = TriggerHouse.GetComponent<ScientificHouse>();
					ScienceNow.work=true;
					click=true;
					//精煉放這邊	
					break;
				case 5:
					//裝炸彈放這邊
					break;
				case 6:
					HouseLevelUp HouseNow = TriggerHouse.GetComponent<HouseLevelUp>();
					HouseNow.work=true;
					click=true;
					
					break;
				}	
				
			}
			if(Status==0||Status==1){
				GUI.enabled=false;//buttonEnable=false;
			}else{
				GUI.enabled=true;
			}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6-Screen.width/10, Screen.height-Screen.height/5 , Screen.width/10, Screen.height/6 ),UpGrade,guiSkin.customStyles[3])){
				
			}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6-Screen.width*2/10, Screen.height-Screen.height/5, Screen.width/10, Screen.height/6 ),Fixed,guiSkin.customStyles[3])){
				
			}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6-Screen.width*3/10, Screen.height-Screen.height/5, Screen.width/10, Screen.height/6 ),BombPng,guiSkin.customStyles[3])){
				
			}
		}//end of click
		///FuctionButton/////
		
		
		///Build/////
		GUI.enabled = true;
		if (Build == true) {
			// 蓋房子放這邊
			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			
			House BuildHouse=build_house.gameObject.transform.FindChild("HomePlane").gameObject.GetComponent<House>();
			
			if (GUI.Button (new Rect (Screen.width* 1/2, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"House")){
				if(source[0]>=BuildHouse.needSource[0] && source[1]>=BuildHouse.needSource[1] && source[2]>=BuildHouse.needSource[2]){
					BuildNow = (Rigidbody)Instantiate(build_house,new Vector3(x-5,y,z-5),build_house.transform.rotation);
					House thisBuild=BuildNow.gameObject.GetComponent<House>();

					//Player thisPlayer=this.gameObject.GetComponent<Player>();
					//thisBuild.player=thisPlayer;
					//thisBuild.PlayerID=PlayerID;
					Build=false;
					click=true;
				}
				
			}	
			string text="Wood:"+BuildHouse.needSource[0]+"\r\nStone:"+BuildHouse.needSource[1]+"\r\nMetal:"+BuildHouse.needSource[2];
			GUI.Box(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
			
			if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
				Build=false;	
				click=false;
			}
			
			
			
		}
		
		///Build/////
		/// 
			/// 
		}//DataLoad
	}
	
	
	void OnApplicationQuit ()
	{
		clientSocket.Close();//關閉通訊器
		
		print("伺服器斷線了！");//顯示斷線
		
		th_Listen.Abort();//刪除執行緒
		chkThread=false;
	}
	
	
	
	void OnTriggerStay(Collider other){
		point [0] = other.transform.position.x;
		point [1] = other.transform.position.z;
		if (other.tag == "Work" || other.tag == "House" || other.tag == "Stock" || other.tag == "Science" || other.tag != "Source") {
						move_flag = true;
				}
		Trigger_ob = other.gameObject;
		
		switch (other.tag)
		{
		case "Source":
			PickSource=other.gameObject;
			
			Status = 1;
			break;
		case "Stock":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			Status = 2;
			
			break;
		case "Work":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			//print (WorkHouse.)
			Status = 3;
			
			break;
		case "Science":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			
			Status = 4;
			
			break;
		case "Enemy":
			Status = 5;
			
			break;
		case "House":
			Bomb=true;		
			Status = 6;
			TriggerHouse=other.gameObject;
			
			break;
		default:
			Status = 0;
			break;
		}
		
	}
	void OnTriggerExit(Collider other){
		Bomb=false;
		PickSource = null;
		Status = 0;
		move_flag = false;
	}
}
