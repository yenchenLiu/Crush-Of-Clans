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

	*/

	//status
	public Texture[] FunctionButton;
	public Texture Bag,Fixed,BombPng,UpGrade,Black,Construction,tool1_0,tool1_1,tool2_0,tool2_1,wood,stone,metal,pulfur,cone,firePng,metalmine,pulfurmine;
	public Texture[] BombPngLarge;
	public GUISkin guiSkin;
	public string PlayerID="000";//
	private float picktime,GameTime,BombTimeCount;
	public float infoTime;
	public int tool,cart,toolKind,toolHp,bombKind,selectPutSource;//
	public int[] toolBomb ={0,0,0};//
	public int j,w,h;
	private int Status,Process;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 6"UpGrade" 
	public  int[] source;//
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	private string[] BombInfo = {"威力：400 範圍：單一","威力：800 範圍：九宮格","威力：1000 範圍：連鎖"};
	
	//public  string[] sourceName={"wood","stone","metal","XX","YY"};//0 wood, 1 stone ,2 metal;
	private int[] weight={3,5,10,5,5,5,10,10};// = {5,10,50};//0 wood, 1 stone ,2 metal;
	public int[] bombWeight;
	public int[,] toolWeight;
	public int x,y,z;//
	public bool Build=false,click=false,Bomb=false,BombGameStart=false,buttonEnable=false,bag=false,GetPosition=false,isHomeExsist=false,BombSelect=false,put=false;
	private Rigidbody BuildNow;
	private GameObject PickSource,TriggerHouse,DestroyHouse;
	public GameObject fire,Building,GetStone,GetWood,Smoke,SmokeAnimation,BombAnimation,BombObjectS ,BombObjectM ,BombObjectL,BigBomb;
	public Rigidbody build_house;
	public bool info,BombSelected;
	public string infotext;
	private int[] needSource = {200,0,0,0,0,0,0,0};
	
	//attribute
	private char[] BombGame={'1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','I','M','N','O','P'};
	private char[] GameQ = {'x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x'};
	private int[] BombGameTime = {4,8,20,10};
	private int[,] pick = {{5,10},{20,40},{20,40}};//{{5,5,10,15},{10,20,30,40},{20,40,60,80},{30,60,90,120}};
	private int[] BombGameButton = {4,9,16,25};

	private string BombGameQ,BombGameInput;
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈","UpGrade"};
	private string[] toolName={"手","十字鎬","斧頭","炸彈"};
	private string[] cartName={"手","推車"};
	public int[] package;// = {1000,2000};
	private int[,] CDtime = {{3,3},{2,1},{2,1}};
	public int weightNow;
	private int limit=0;
	private string PutQuatity="0";
	public AudioClip sound;
	public AudioClip isBomb;
	public AudioClip laught;
	protected AudioSource m_sound;

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


	
	void bigBomb(int status,string HouseID){
		int[] damageSource = new int[8]{0,0,0,0,0,0,0,0};
		if(status==2){
			
			infomationText("敵人在你的建築上設了陷阱！你失去了身上的30％資源！！");
			for (int i =0; i<8; i++) {
				damageSource[i]=(int)-1*source[i]*30/100;
				print (damageSource[i].ToString());
			}
			sendSourceModify(damageSource);
			Server.Send("65"+HouseID+",1");
		}
		if(status==3){
			
			infomationText("敵人在你的建築上設了陷阱！你失去了身上的60％資源！！");
			for (int i =0; i<8; i++) {
				damageSource[i]=(int)-1*source[i]*60/100;
			}
			sendSourceModify(damageSource);
			Server.Send("65"+HouseID+",1");
		}
		if(status==4){
			
			infomationText("敵人在你的建築上設了陷阱！你失去了身上的100％資源！！");
			for (int i =0; i<8; i++) {
				damageSource[i]=(int)-1*source[i];
			}
			sendSourceModify(damageSource);
			Server.Send("65"+HouseID+",1");
		}
		m_sound.PlayOneShot(isBomb);
		
		Vector3 Pos=new Vector3(State.HousePositionX[HouseID],4f,State.HousePositionZ[HouseID]);
		
		GameObject BombAnimateNow=(GameObject) Instantiate(BigBomb,Pos,BigBomb.transform.rotation);
		
		
		Destroy(BombAnimateNow,2);
	}

	void sendSourceModify(int[] quatity){
		
		string sendString = "24";
		for (int i=0; i<quatity.Length; i++) {
			if(quatity[i]!=0){
				sendString = "24";
				sendString+=(i+1).ToString()+","+(quatity[i]).ToString();
				Server.Send (sendString);
				print (sendString);
			}
		}
		
	}
	public void Send(string strSend)
	{
		byte[] byteSend = new byte[1024];
		
		try
		{
			byteSend = Encoding.UTF8.GetBytes(strSend);
			State.clientSocket.Send (byteSend);  
		}
		catch (Exception ex)
		{
			
			print(" Connection Break!");
		}
	}
	
	//監聽 Server 訊息 (Listening to the Server)

	
	
	
	/// ///////////////////
	
	// Use this for initialization
	/*IEnumerator server_function(){
		while (true) {
			yield return new WaitForSeconds (1);
			Server.Send ("41"+x+","+z+"@@@@@");
		}
	}*/
	void Start () {
		m_sound = this.audio;
		BombSelect = false;
		GetPosition = false;
		selectPutSource = 0;
		Process = 10;
		
		put = false;
		bag=false;
		info = false;
		weightNow = 0;
		cart = 0;
		tool = 0;
		toolKind = 0;
		Status = 0;
		Build=false;
		click = false;
		limit = 0;
		PutQuatity="0";
		picktime=Time.time-5;
		BombTimeCount = Time.time - 6;
		BombGameStart = false;
		BombSelected = false;

		bombKind = 0;
		 
		for (int i=0; i<8; i++) {
			source[i]=State.source[i];		
		}
		PlayerID = State.PlayerName;
		//StartCoroutine("server_function");
		tool=State.tool;
		toolKind=State.toolKind;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<8; i++) {
			source[i]=State.source[i];		
		}
		if (Trigger_ob == null) {
			Status=0;
			move_flag=false;
		}
	
		weightNow = (source [0] * weight [0])+(source [1] * weight [1]) + (source [2] * weight [2])+ (source [3] * weight [3])+ (source [4] * weight [4])+ (source [5] * weight [5])+ (source [6] * weight [6])+ (source [7] * weight [7]);

		//this.transform.FindChild ("Main Camera").camera.fieldOfView = view;
		////////資料庫
		/*UPDATE Player
					key: PlayerID
					欄位：
					x(x座標)
					z(z座標)
				*/

		x = (int)this.transform.position.x;
		y = (int)this.transform.position.y;
		z = (int)this.transform.position.z;
		State.PosX = x;
		State.PosZ = z;
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

		//座標/////
	//	GUI.BeginGroup(new Rect (Screen.width*7/16, 0, Screen.width/8, Screen.height / 20));
		GUI.Box (new Rect (Screen.width*5/12, 0, Screen.width/6, Screen.height / 20),"",guiSkin.box);
		GUI.Box (new Rect (Screen.width*14/32, 0, Screen.width/16, Screen.height / 20),((int)(this.transform.position.x)).ToString(),guiSkin.textField);
		GUI.Box (new Rect (Screen.width*16/32, 0, Screen.width/16, Screen.height / 20),((int)(this.transform.position.z)).ToString(),guiSkin.textField);
		//座標/////End


		//INFOMATION///////////
		if (info == true) {
			GUI.Box (new Rect (0, Screen.height/2-Screen.height / 20, Screen.width, Screen.height / 10),infotext,guiSkin.box);
			if((int)(Time.time-infoTime)==1){
				info=false;

			}
		}
		//INFOMATION///////////End

		///Load///

		if (State.DataLoad == false) {

 			
			//GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
			//GUI.Label(new Rect (0-Screen.width+(Screen.width*Process/5), Screen.height*4/5, (Screen.width) , Screen.height/20),"    ",guiSkin.button);

		} else{ //DataLoad
			
		

			/*//插在OnGUI內，但這中間不要插入其他GUI繪製，不然會亂轉唷
			Matrix4x4 old_matrix = GUI.matrix;
			//取得角色位置
			Vector3 this_position = this.transform.position;
			//取得目的地位置，你在自己換成看到指向哪裡 << 這裡要改
			Vector3 destination = GameObject.Find("AnotherPlayer").gameObject.transform.position;
			float angle = Mathf.Atan2 (this_position.z - destination.z, this_position.x - destination.x) * 180 / Mathf.PI;
			//Vector2裡面的兩個數值，你要改成，下面GUI旋轉的   left + width / 2 ,  top + height / 2  << 這裡要改
			GUIUtility.RotateAroundPivot (360 - angle + 45,new Vector2(100+50,100+50));
			//圖片跟skin你在換掉 << 這裡也要改
			GUI.Box (new Rect (100, 100, 100, 100), UpGrade, guiSkin.box);
			GUI.matrix = old_matrix;*/

		/*if (GUILayout.Button ("Connect", GUILayout.Height(50))) {
			Server.ConnectToServer();
			Send("10singo,singo");
		}*/
/*if (GUILayout.Button ("Send", GUILayout.Height(50))) {
			
			Server.Send("264,0");
			//Send("32");

		}*/
			/*
		if(GUILayout.Button("Close")){
			State.clientSocket.Close();//關閉通訊器
				infomationText("伺服器斷線了！");
			print("伺服器斷線了！");//顯示斷線
			
			State.chkThread=false;
		}*/

		
		if(click==false ||BombSelected==true){
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
				GUI.Label(new Rect(screenPosition.x-50,screenPosition.y,100,100),PlayerID,guiSkin.label);
			
				//if (((Status >= 1 && Status <= 4) || Status == 6 ||Status==7 )&& Trigger_ob!=null) {
				if (Status!=0 && Trigger_ob!=null) {
				
					worldPosition = new Vector3 (Trigger_ob.transform.position.x, Trigger_ob.transform.position.y + Trigger_ob.collider.bounds.size.y, Trigger_ob.transform.position.z);
					screenPosition = transform.FindChild ("Main Camera").camera.WorldToScreenPoint (worldPosition);
					screenPosition.y = Screen.height - screenPosition.y;
					if (Status == 1) {
						Source data = Trigger_ob.GetComponent<Source> ();
							if(data.kind==0){
								GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), sourceName [data.kind] + "\n" + data.quatity[0],guiSkin.customStyles[5]);
							}else{
								GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), sourceName [1] + ":" + data.quatity[0]+ "\n" +sourceName [6] + ":" + data.quatity[1]+ "\n"+sourceName [7] + ":" + data.quatity[2] ,guiSkin.customStyles[5]);
								
							}
					} else if (Status == 2) {
						build data = Trigger_ob.GetComponent<build> ();
						GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
					} else if (Status == 3) {
						build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
					} else if (Status == 4) {
						build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
					} else if (Status == 5) {
						build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[7]);
						//	GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.customStyles[5]);
						} else if (Status == 6) {
							build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
							//	GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.customStyles[5]);

						}else if (Status == 7) {
							build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
							//GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.customStyles[5]);
						}else if (Status == 8) {
							build data = Trigger_ob.GetComponent<build>();
							GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.customStyles[5]);
							//GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.customStyles[5]);
						}

				}
				}

			}
		///Bag/////
		if (bag == true) {
			tool=State.tool;
			toolKind=State.toolKind;
			cart=State.cart;

			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
				if(tool==1 && toolKind==0){
					GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),tool1_0, guiSkin.box);
					
				}else if(tool==1 && toolKind==1){
					GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),tool1_1, guiSkin.box);
					
				}else if(tool==2 && toolKind==0){
					GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),tool2_0, guiSkin.box);
					
				}else if(tool==2 && toolKind==1){
					GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),tool2_1, guiSkin.box);
					
				}else{
					GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),"無裝備工具", guiSkin.box);
					
				}
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width/10+Screen.width*1/20,Screen.height*9/20, Screen.width*1/10, Screen.width*1/5), "CD："+CDtime[tool,toolKind], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*7/20+Screen.width*1/10, Screen.width*1/5, Screen.width*1/4),"重量:"+weightNow+"/"+package[cart], guiSkin.label);
			//GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
				
				if(selectPutSource==0 ||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), source[0].ToString(),guiSkin.customStyles[4]);
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),wood,guiSkin.customStyles[6])){
					selectPutSource=0;
					
				}
				if(selectPutSource==1||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), source[1].ToString(),guiSkin.customStyles[4]);
				
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),stone,guiSkin.customStyles[6])){

					selectPutSource=1;
					
				}
				if(selectPutSource==2||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), source[2].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),metal,guiSkin.customStyles[6])){
				

					selectPutSource=2;
					
				}
				if(selectPutSource==3||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), source[3].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),pulfur,guiSkin.customStyles[6])){

					selectPutSource=3;
					
				}
				if(selectPutSource==4||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*6/16, Screen.height*11/ 26 , Screen.width/8, Screen.height/16 ), source[4].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),cone,guiSkin.customStyles[6])){

					selectPutSource=4;
					
				}
				if(selectPutSource==5||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 11/ 26 , Screen.width/8, Screen.height/16 ), source[5].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),firePng,guiSkin.customStyles[6])){

					selectPutSource=5;
				}
				if(selectPutSource==6||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 15/ 26 , Screen.width/8, Screen.height/16 ), source[6].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),metalmine,guiSkin.customStyles[6])){
					selectPutSource=6;
					
				}
				if(selectPutSource==7||put==true)GUI.enabled=false;else GUI.enabled=true;
				GUI.Label (new Rect (Screen.width*9/16, Screen.height*15/ 26 , Screen.width/8, Screen.height/16 ), source[7].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),pulfurmine,guiSkin.customStyles[6])){

					selectPutSource=7;
					
				}
				GUI.enabled=true;
			
				if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
					bag = false;
					click=false;
						put=false;
				}
				GUI.EndGroup();
			

				
				if (GUI.Button (new Rect (Screen.width * 5 / 20, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 8, Screen.width * 1 / 15), "丟棄", guiSkin.button)) {
					put = true;
					limit=0;
					PutQuatity="0";
				}
				GUI.enabled = true;
				if (put == true) {
					GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 4), Screen.height / 2 - (Screen.height / 4), Screen.width / 2, Screen.height / 2), "", guiSkin.box);
					PutQuatity = GUI.TextField (new Rect (Screen.width*7/16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 8, Screen.height * 1 / 12), PutQuatity, guiSkin.textField);
					if (GUI.Button (new Rect (Screen.width * 5 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), "<<", guiSkin.button)) {


						PutQuatity = "0";
						 
					}
					if (GUI.Button (new Rect (Screen.width * 6 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), "<", guiSkin.button)) {
						if (int.Parse (PutQuatity) > 0) {
						

							PutQuatity = ((int.Parse (PutQuatity)) - 1).ToString ();
							
						}
						
					}
					if (GUI.Button (new Rect (Screen.width * 10 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), ">>", guiSkin.button)) {
						if (put == true) {
							PutQuatity=source[selectPutSource].ToString();
						}

						
					}
					if (GUI.Button (new Rect (Screen.width * 9 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), ">", guiSkin.button)) {
						if (put == true) {
							
							limit = source [selectPutSource];

						}

						if (int.Parse (PutQuatity) < limit) {
							PutQuatity = ((int.Parse (PutQuatity)) + 1).ToString ();
						}
					}
					int[] quatity=new int[8]{0,0,0,0,0,0,0,0};
					if (GUI.Button (new Rect (Screen.width * 8 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 10), "確定", guiSkin.button)) {

						if (put == true) {
							quatity[selectPutSource]= -1*int.Parse (PutQuatity);
							
							source [selectPutSource] = source [selectPutSource] - int.Parse (PutQuatity);
							
						}
						sendSourceModify(quatity);
						PutQuatity = "0";

						put = false;
					}
					
					if (GUI.Button (new Rect (Screen.width * 10 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 10), "取消", guiSkin.button)) {
						PutQuatity = "0";
						limit=0;
						put = false;
					}
				}
		}
		
		///Bag/////end
		

		
		
		
		
	
		
		///BOMB//////////
			/// 
			/// 
		if(BombSelected==true){ 
				GUI.Box (new Rect (0, Screen.height / 5, Screen.width, Screen.height / 10),"選擇目標",guiSkin.box);
				
				if(Status==5){
					GUI.enabled=true;
				}else{
					GUI.enabled=false;
					
				}

			if (GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),BombPng,guiSkin.customStyles[3])){
					build triggerHouesNow=TriggerHouse.GetComponent<build>();
					if(triggerHouesNow.status!=5){


						
							j=0;
							w=1;
							h=1;
							BombGameQ="";
							BombGameInput="";
							
							for(int i =0;i<BombGameButton[bombKind%3];i++){
								GameQ[i]=BombGame[(int)(UnityEngine.Random.Range(0, BombGameButton[bombKind%3]))];
								BombGameQ+=GameQ[i];
								BombGameQ+=" ";
							}	
							BombGameStart=true;
							GameTime=Time.time;

							//toolBomb[bombKind]--;
							Server.Send ("28"+((bombKind%3)+1).ToString()+",-1");
							print ("28"+((bombKind%3)+1).ToString()+",-1");
							BombSelected=false;


					}else{
						int persent=0;
						switch(triggerHouesNow.HouseLevel){
						case 1:
							persent=20;
							break;
						case 2:
							persent=40;
							break;
						case 3:
							persent=60;
							break;
						case 4:
							persent=80;
							break;
						case 5:
							persent=100;
							break;
						}
						m_sound.PlayOneShot(isBomb);
						infomationText("這是敵人設計的陷阱！你失去了身上的"+persent.ToString()+"％資源！！");
						int[] quantity=new int[8]{0,0,0,0,0,0,0,0};
						for(int i=0;i<8;i++){
							quantity[i]=(int)-1*source[i]*persent/100;
						}
						sendSourceModify (quantity);

						Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
						GameObject animateNow = (GameObject)Instantiate (BombAnimation, Pos, Building.transform.rotation);
						Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
						
						GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
						Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
						
						GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
						Server.Send("65"+triggerHouesNow.HouseID+",6");
						Destroy (animateNow, 1);
						Destroy (SomkeNow, 1);
						Destroy (SmokeAnimateNow, 1);

						BombSelected=false;
						
						click=false;

					}
				}

				GUI.enabled=true;
				
			if (GUI.Button (new Rect (Screen.width*14/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				BombSelected=false;

				click=false;
			}
		}

	/*	if (5 - (int)(Time.time - BombTimeCount) >= 1) {
			infomationText(((int)(5-(Time.time-BombTimeCount))).ToString());
			
		}*/
		if (BombGameStart == true) {

			if(Time.time-GameTime>=BombGameTime[bombKind%3]){
				infomationText("False....");
				BombGameStart=false;
				click=false;
				
			}

				GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 1/ 10 , Screen.width/2, Screen.height/10 ),BombGameQ,guiSkin.textField);
				GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 2/ 10, Screen.width/2, Screen.height/10 ),BombGameInput,guiSkin.textField);
				GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 3/ 10, Screen.width/2*((BombGameTime[bombKind%3]-Time.time+GameTime)/BombGameTime[bombKind%3]), Screen.height/20 ),"",guiSkin.textField);
			
			switch(bombKind%3){
			case 0:
				w=1;
				h=1;
				for(int i=0;i<4;i++){
					//if(h>2) h=1;
					if(w>2){
						w=1;
						h++;
					}
					
					//print (w+" "+h );
					if (GUI.Button (new Rect (Screen.width*w/4, Screen.height* h/ 3 , Screen.width/4, Screen.height/3 ),BombGame[i].ToString(),guiSkin.button)){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							m_sound.PlayOneShot(isBomb);
								
							BombGameStart=false;
								click=false;
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
						if(bombKind==0){
							DestroyHouse=TriggerHouse;
							Instantiate(BombObjectS,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectS.transform.rotation);
							
							click=false;
							Bomb=false;
							BombGameStart=false;
							move_flag = false;
							j=0;
						}else{
							build triggerHouesNow=TriggerHouse.GetComponent<build>();
							
							//triggerHouesNow.status=bombKind-1;
							Server.Send("65"+triggerHouesNow.HouseID+","+(bombKind-1).ToString());
							print ("65"+triggerHouesNow.HouseID+","+(bombKind-1).ToString());
							m_sound.PlayOneShot(laught);
							infomationText("陷阱設置成功！");
							click=false;
							Bomb=false;
							BombGameStart=false;
							move_flag = false;
							j=0;
							
						}
					
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
						if (GUI.Button (new Rect (Screen.width*w/5, Screen.height* 1/ 3 + (Screen.height* (h-1)*2/ 9), Screen.width/5, Screen.height*2/9 ),BombGame[i].ToString(),guiSkin.button)){
						if(GameQ[j]!=BombGame[i]){
							m_sound.PlayOneShot(isBomb);
								
							infomationText("False....");
							BombGameStart=false;
								click=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=9){
						if(bombKind==1){
							DestroyHouse=TriggerHouse;
								Instantiate(BombObjectM,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectM.transform.rotation);
								
							click=false;
							Bomb=false;
							BombGameStart=false;
							j=0;
							move_flag = false;
						}else{
							build triggerHouesNow=TriggerHouse.GetComponent<build>();
							
							//triggerHouesNow.status=bombKind-1;
							Server.Send("65"+triggerHouesNow.HouseID+","+(bombKind-1).ToString ());
							infomationText("陷阱設置成功！");
							m_sound.PlayOneShot(laught);
							
							click=false;
							Bomb=false;
							BombGameStart=false;
							j=0;
							move_flag = false;
						}
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
						if (GUI.Button (new Rect (Screen.width*1/4+(Screen.width*(w-1)/8), Screen.height* 1/ 3 + (Screen.height* (h-1)/ 6), Screen.width/8, Screen.height*1/6 ),BombGame[i].ToString(),guiSkin.button)){
						if(GameQ[j]!=BombGame[i]){
							m_sound.PlayOneShot(isBomb);
								
							infomationText("False....");
							BombGameStart=false;
						click=false;
								
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=16){
						if(bombKind==2){
							DestroyHouse=TriggerHouse;
							Instantiate(BombObjectL,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectL.transform.rotation);
								
							click=false;
							Bomb=false;
							BombGameStart=false;
							j=0;
							move_flag = false;
						}else{
							build triggerHouesNow=TriggerHouse.GetComponent<build>();
							
							//triggerHouesNow.status=bombKind-1;
							Server.Send("65"+triggerHouesNow.HouseID+","+(bombKind-1).ToString ());
							infomationText("陷阱設置成功！");
							m_sound.PlayOneShot(laught);
							
							j=0;
							move_flag = false;
							click=false;
							Bomb=false;
							BombGameStart=false;
						}
				}
				break;
			case 3:
				break;
				
			}	
			
		}
		

		///BOMB//////////
		
		
		///FuctionButton/////
		if (click == false) {//hide FuctionButton
			if (Build==false && GUI.Button (new Rect (Screen.width-Screen.width/8, Screen.height* 3 / 4-Screen.height/5 , Screen.width/10, Screen.height/6 ),Bag,guiSkin.customStyles[3])){
				Server.Send ("25");
				bag=true;
				click=true;
			}
			if (Build==false && GUI.Button (new Rect (Screen.width-Screen.width/8, Screen.height* 3 / 4-Screen.height*2/5, Screen.width/10, Screen.height/6 ),BombPng,guiSkin.customStyles[3])){
				Server.Send ("27");
				
				BombSelect = true;

				click=true;
			}
				if(Status==1 &&Time.time-picktime<CDtime[tool,toolKind]){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;
					
				}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),FunctionButton[Status],guiSkin.customStyles[3])){
					if(Status!=0 && Status!=1 &&Status!=5){
						m_sound.PlayOneShot(sound);
						
					}
					float randomKind=0f;
					//randomKind=Random.value;
				switch (Status){
				case 0://build a house
					
					Build = true;
					click=false;
					break;
				case 1:			
					//採集放這邊
					//source++;
					if(Time.time-picktime>CDtime[tool,toolKind] && weightNow < package[cart]){
						
						
							
						Source pickup=PickSource.GetComponent<Source>();					
						int kind=pickup.kind;
							/*if(kind==1){
								System.Random r = new System.Random();
								randomKind=r.Next(100);
								print (randomKind.ToString());
								if(randomKind<=20) kind=6;
								else if(randomKind>20&&randomKind<=30) kind=7;
								else kind=1;
							}*/
							print (kind.ToString());
						if(tool==0||kind+1==tool ){
							int[] quatity=pickup.quatity;
							int sourceWeight=weight[kind];
							int[] getQutity = new int[3];
							//print (quatity);
								if(kind==0){
									if(quatity[0]<=pick[tool,toolKind]){
										if(package[cart]-weightNow-(quatity[0]*weight[kind])<0){
											
											getQutity[0] = (package[cart]-weightNow)/weight[kind];
											source[kind]+=getQutity[0];
											//pickup.quatity[0]-=getQutity[0];
											//weightNow+=getQutity*sourceWeight;
										}else{
											getQutity[0]=quatity[0];
											
											source[kind]+=quatity[0];
											//pickup.quatity[0]-=quatity[0];

										}
										
									}else{
										if(package[cart]-weightNow-(pick[tool,toolKind]*weight[kind])<0){
											getQutity[0] = (package[cart]-weightNow)/weight[kind];
											source[kind]+=getQutity[0];
										//	pickup.quatity[0]-=getQutity[0];
											//weightNow+=getQutity*sourceWeight;
											
										}else{
											getQutity[0]=pick[tool,toolKind];
											source[kind]+=pick[tool,toolKind];
										//	pickup.quatity[0]-=pick[tool,toolKind];
											//	weightNow+=pick[tool]*sourceWeight;
											
										}
									}
									Server.Send ("46"+pickup.SourceID);

									infomationText("取得 "+getQutity[0].ToString()+" "+sourceName[kind]+" !");

								}else{
									if(quatity[0]+quatity[1]+quatity[2]<=pick[tool,toolKind]){
										if(package[cart]-weightNow-(quatity[0]*weight[1]+quatity[1]*weight[6]+quatity[2]*weight[7])<0){
											
											getQutity[0] = (package[cart]-weightNow)/weight[kind];
											getQutity[1] = (package[cart]-weightNow)/weight[kind];
											getQutity[2] = (package[cart]-weightNow)/weight[kind];
											getQutity[0]=(int)(getQutity[0]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[1]=(int)(getQutity[1]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[2]=(int)(getQutity[2]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));

											source[1]+=getQutity[0];
											source[6]+=getQutity[1];
											source[7]+=getQutity[2];

											//pickup.quatity[0]-=(int)(getQutity[0]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[1]-=(int)(getQutity[1]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[2]-=(int)(getQutity[2]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));
											//weightNow+=getQutity*sourceWeight;
										}else{
											getQutity[0]=quatity[0];
											getQutity[1]=quatity[1];
											getQutity[2]=quatity[2];
											source[1]+=(int)quatity[0];
											source[6]+=(int)quatity[1];
											source[7]+=(int)quatity[2];
											//pickup.quatity[0]-=(int)quatity[0];
											//pickup.quatity[1]-=(int)quatity[1];
											//pickup.quatity[2]-=(int)quatity[2];

											
										}
										
									}else{
										if(package[cart]-weightNow-(pick[tool,toolKind]*weight[kind])<0){
											getQutity[0] = (package[cart]-weightNow)/weight[kind];
											getQutity[1] = (package[cart]-weightNow)/weight[kind];
											getQutity[2] = (package[cart]-weightNow)/weight[kind];
											getQutity[0]=(int)(getQutity[0]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[1]=(int)(getQutity[1]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[2]=(int)(getQutity[2]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));
											
											source[1]+=getQutity[0];
											source[6]+=getQutity[1];
											source[7]+=getQutity[2];
											
											//pickup.quatity[0]-=(int)(getQutity[0]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[1]-=(int)(getQutity[1]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[2]-=(int)(getQutity[2]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));
											//weightNow+=getQutity*sourceWeight;
											
										}else{


											getQutity[0]=(int)(pick[tool,toolKind]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[1]=(int)(pick[tool,toolKind]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											getQutity[2]=(int)(pick[tool,toolKind]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));
											
											source[1]+=getQutity[0];
											source[6]+=getQutity[1];
											source[7]+=getQutity[2];
											
											//pickup.quatity[0]-=(int)(pick[tool,toolKind]*quatity[0]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[1]-=(int)(pick[tool,toolKind]*quatity[1]/(quatity[0]+quatity[1]+quatity[2]));
											//pickup.quatity[2]-=(int)(pick[tool,toolKind]*quatity[2]/(quatity[0]+quatity[1]+quatity[2]));

										
											//	weightNow+=pick[tool]*sourceWeight;
											
										}
									}
									Server.Send ("46"+pickup.SourceID);
									getQutity[0]=(pickup.quatity[0]>=3)?3:pickup.quatity[0];
									getQutity[1]=(pickup.quatity[1]>=2)?2:pickup.quatity[1];
									getQutity[2]=(pickup.quatity[1]>=1)?1:pickup.quatity[2];
								

									infomationText("取得 "+(getQutity[0]).ToString()+sourceName[1]+" "+(getQutity[1]).ToString()+sourceName[6]+" "+(getQutity[2]).ToString()+" "+sourceName[7]+" !");
									
								}

							

								if(kind==0){
									Vector3 Pos=new Vector3(PickSource.transform.position.x-1,PickSource.transform.position.y+5,PickSource.transform.position.z+1);
									GameObject animateNow=(GameObject) Instantiate(GetWood,Pos,GetWood.transform.rotation);
									Destroy(animateNow,2);
								}else{
									Vector3 Pos=new Vector3(PickSource.transform.position.x-1,PickSource.transform.position.y+5,PickSource.transform.position.z+1);
									
									GameObject animateNow=(GameObject) Instantiate(GetStone,Pos,GetWood.transform.rotation);
									Destroy(animateNow,2);
									
								}
							
							////////資料庫
							/*
						UPDATE Player
						key: PlayerID
						欄位：
						source[0]=source[0](wood
						source[1]=source[1](stone)
						source[2]=source[2](metal)
						(可以再加其他你想到資源種類)

						UPDATE Source
						key: x,y
						quatity=pickup.quatity;

						*/
							picktime=Time.time;
							}
						}else{
							infomationText("超過負載!");
							
						}
					Status=0;
					break;
				case 2:
					//倉庫放這邊

					build StockNow = TriggerHouse.GetComponent<build>();
						if(StockNow.status!=1 && StockNow.status!=5){
							bigBomb(StockNow.status,StockNow.HouseID);
							StockNow.status=1;
						}else{
							StockNow.work=true;
							click=true;
						}
					
					break;
				case 3:
					build WorkNow = TriggerHouse.GetComponent<build>();
						if(WorkNow.status!=1 && WorkNow.status!=5){
							bigBomb(WorkNow.status,WorkNow.HouseID);
							WorkNow.status=1;
							
						}else{
							WorkNow.work=true;
							click=true;
						}
					//合成放這邊				
					break;
				case 4:
					build ScienceNow = TriggerHouse.GetComponent<build>();
						if(ScienceNow.status!=1 && ScienceNow.status!=5){
							bigBomb(ScienceNow.status,ScienceNow.HouseID);
							ScienceNow.status=1;
							
						}else{
							ScienceNow.work=true;
							click=true;
						}
					//精煉放這邊	
					break;
				case 5:
					//裝炸彈放這邊
					break;
				case 6:
					build HouseNow = TriggerHouse.GetComponent<build>();
						if(HouseNow.status!=1 && HouseNow.status!=5){
							bigBomb(HouseNow.status,HouseNow.HouseID);
							HouseNow.status=1;
							
						}else{
							HouseNow.work=true;
							click=true;
						}
					break;
				case 7:
					build B101Now = TriggerHouse.GetComponent<build>();
						if(B101Now.status!=1 && B101Now.status!=5){
							bigBomb(B101Now.status,B101Now.HouseID);
							B101Now.status=1;
							
						}else{
							B101Now.work=true;
							click=true;
						}
					
				break;
				}	
				
			}

			if(Status==0||Status==1||Status==7||Status==5){
				GUI.enabled=false;//buttonEnable=false;
			}else{
				GUI.enabled=true;
			}

			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6-Screen.width/10, Screen.height-Screen.height/5 , Screen.width/10, Screen.height/6 ),UpGrade,guiSkin.customStyles[3])){
					switch(Status){
					case 2:
						//倉庫放這邊
						build StockNow = TriggerHouse.GetComponent<build>();
						StockNow.LevelUp=true;
						click=true;
						break;
					case 3:
						build WorkNow = TriggerHouse.GetComponent<build>();
						WorkNow.LevelUp=true;
						click=true;
						//合成放這邊				
						break;
					case 4:
						build ScienceNow = TriggerHouse.GetComponent<build>();
						ScienceNow.LevelUp=true;
						click=true;
						//精煉放這邊	
						break;

					case 6:
						build HomeNow = TriggerHouse.GetComponent<build>();
						HomeNow.LevelUp=true;
						click=true;
						break;
					}
			}
			if (Build==false && GUI.Button (new Rect (Screen.width* 5 / 6-Screen.width*2/10, Screen.height-Screen.height/5, Screen.width/10, Screen.height/6 ),Fixed,guiSkin.customStyles[3])){
					build HomeNow = TriggerHouse.GetComponent<build>();
					HomeNow.fixedHouse=true;
					click=true;
			}
				GUI.enabled=true;

				///Bomb Button
			
				///Bomb Button
		}//end of click
		///FuctionButton/////
		
		
		///Build/////
		GUI.enabled = true;

			if(BombSelect==true){
				toolBomb=State.toolBomb;
				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);

				////
				GUI.BeginGroup(new Rect (Screen.width*3/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), BombPngLarge[0], guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[0]+ "\r\n攜帶量:"+toolBomb[0], guiSkin.label);
				if(toolBomb[0]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*10/20, Screen.width*1/6, Screen.width*1/32 ),"設置炸彈",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=0;
					
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*23/40, Screen.width*1/6, Screen.width*1/32 ),"設置地雷",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=3;
					
				}
				GUI.enabled=true;
				
				GUI.EndGroup();
				///
				GUI.BeginGroup(new Rect (Screen.width*17/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), BombPngLarge[1], guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[1]+ "\r\n攜帶量:"+toolBomb[1], guiSkin.label);
				if(toolBomb[1]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*10/20, Screen.width*1/6, Screen.width*1/32 ),"設置炸彈",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=1;
				
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*23/40, Screen.width*1/6, Screen.width*1/32 ),"設置地雷",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=4;
					
				}
				GUI.enabled=true;
				
				GUI.EndGroup();
				////
				/// 
				GUI.BeginGroup(new Rect (Screen.width*31/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), BombPngLarge[2], guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[2]+ "\r\n攜帶量:"+toolBomb[2], guiSkin.label);
				if(toolBomb[2]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*10/20, Screen.width*1/6, Screen.width*1/32 ),"設置炸彈",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=2;
				}
				if (GUI.Button (new Rect (Screen.width*1/30,Screen.height*23/40, Screen.width*1/6, Screen.width*1/32 ),"設置地雷",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=5;
				}
				GUI.enabled=true;
				
				GUI.EndGroup();
				////////

			
				
				if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
					BombSelect=false;	
					click=false;
				}
				GUI.EndGroup();
			}
		if (Build == true) {
			// 蓋房子放這邊
				string text="";
				for(int i=0;i<8;i++){
					if(needSource[i]!=0){
						text+="\r\n"+sourceName[i]+":"+source[i]+"/"+needSource[i];
					}
				}
				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width/15,Screen.height/12, Screen.width*2/7, Screen.width*2/7), Construction, guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), text, guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8,Screen.height* 4/ 16, Screen.width/3, Screen.width*1/5),"蓋房子前的土地整理及地基建設，為建造房子前的必須工程。", guiSkin.label);

				
				if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
					Build=false;	
					click=false;
				}

				GUI.EndGroup();

				if(source[0]>=needSource[0] && source[1]>=needSource[1] && source[2]>=needSource[2]&& source[3]>=needSource[3]&& source[4]>=needSource[4]&& source[5]>=needSource[5]&&source[6]>=needSource[6]&& source[7]>=needSource[7]){
					GUI.enabled=true;
				}else{
					GUI.enabled=false;
					
				}

				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"建造",guiSkin.button)){
				//	House BuildHouse=build_house.gameObject.transform.FindChild("HomePlane").gameObject.GetComponent<House>();

					

					BuildNow = (Rigidbody)Instantiate(build_house,new Vector3(x-5,y,z-5),build_house.transform.rotation);
					build thisBuild =BuildNow.gameObject.GetComponent<build>();
					//House thisBuild=BuildNow.gameObject.GetComponent<House>();

					Build=false;
					click=true;

				}
				GUI.enabled=true;


			
			
			
		}
		
		///Build/////
		/// 
			/// 
		}//DataLoad
	}
	
	
	void OnApplicationQuit ()
	{
		if (State.clientSocket != null) {
						State.clientSocket.Close ();//關閉通訊器
		
						print ("伺服器斷線了！");//顯示斷線

						State.chkThread = false;
				
		}
	}
	
	
	
	void OnTriggerStay(Collider other){
		point [0] = other.transform.position.x;
		point [1] = other.transform.position.z;
						move_flag = true;
		Trigger_ob = other.gameObject;
		
		switch (other.tag)
		{
		case "Source":
			PickSource=other.gameObject;
			
			Status = 1;
			break;
		case "Stock":
			TriggerHouse=other.gameObject;
			Status = 2;
			
			break;
		case "Work":
			TriggerHouse=other.gameObject;
			//print (WorkHouse.)
			Status = 3;
			
			break;
		case "Science":
			TriggerHouse=other.gameObject;
			
			Status = 4;
			
			break;
		case "Enemy":
			Status = 5;
			TriggerHouse=other.gameObject;
			
			break;
		case "House":
			Status = 6;
			TriggerHouse=other.gameObject;
			
			break;
		case "Struction":
			Status = 7;
			TriggerHouse=other.gameObject;
						//	print ("7");
			
			break;
		case "B101":
			Status = 8;
			TriggerHouse=other.gameObject;
			//	print ("7");
			
			break;
		default:

			Status = 0;
			break;
		}
		
	}
	void OnTriggerExit(Collider other){
		PickSource = null;
		Status = 0;
		move_flag = false;
	}
}
