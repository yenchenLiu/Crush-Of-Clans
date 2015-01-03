using UnityEngine;
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
	public Texture Bag,Fixed,BombPng,UpGrade,Black,Construction;
	public GUISkin guiSkin;
	public string PlayerID="000";//
	private float picktime,GameTime,BombTimeCount;
	public float infoTime;
	public int tool,cart,toolKind,toolHp,bombKind;//
	public int[] toolBomb ={0,0,0};//
	public int j,w,h;
	private int Status,Process;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 6"UpGrade" 
	public  int[] source;//
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	private string[] BombInfo = {"威力：50 範圍：單一","威力：100 範圍：九宮格","威力：200 範圍：連鎖"};
	
	//public  string[] sourceName={"wood","stone","metal","XX","YY"};//0 wood, 1 stone ,2 metal;
	public int[] weight;// = {5,10,50};//0 wood, 1 stone ,2 metal;
	public int[] bombWeight;
	public int[,] toolWeight;
	public int x,y,z;//
	public bool Build=false,click=false,Bomb=false,BombGameStart=false,buttonEnable=false,bag=false,GetPosition=false,isHomeExsist=false,BombSelect=false;
	private Rigidbody BuildNow;
	private GameObject PickSource,TriggerHouse,DestroyHouse;
	public GameObject fire,Building,GetStone,GetWood,Smoke,SmokeAnimation,BombAnimation,BombObjectS ,BombObjectM ,BombObjectL;
	public Rigidbody build_house;
	public bool info,BombSelected;
	public string infotext;
	private int[] needSource = {200,0,0,0,0,0,0,0};
	
	//attribute
	private char[] BombGame={'1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','I','M','N','O','P'};
	private char[] GameQ = {'x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x'};
	private int[] BombGameTime = {5,20,50,10};
	private int[] pick = {10,30};//{{5,5,10,15},{10,20,30,40},{20,40,60,80},{30,60,90,120}};
	private int[] BombGameButton = {4,9,16,25};

	private string BombGameQ,BombGameInput;
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈","UpGrade"};
	private string[] toolName={"手","十字鎬","斧頭","炸彈"};
	private string[] cartName={"手","推車"};
	public int[] package;// = {1000,2000};
	private int[] CDtime = {2,2,2,2};
	public int weightNow;

	
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
	IEnumerator server_function(){
		while (true) {
			yield return new WaitForSeconds (3);
			Server.Send ("41"+x+","+z+"@@@@@");
		}
	}
	void Start () {

		BombSelect = false;
		GetPosition = false;

		Process = 10;
		

		bag=false;
		info = false;
		weightNow = 0;
		cart = 0;
		tool = 0;
		toolKind = 0;
		Status = 0;
		Build=false;
		click = false;

		picktime=Time.time-5;
		BombTimeCount = Time.time - 6;
		BombGameStart = false;
		BombSelected = false;

		bombKind = 0;
		 
		for (int i=0; i<8; i++) {
			source[i]=State.source[i];		
		}
		PlayerID = State.name;
		StartCoroutine("server_function");
		
	}
	
	// Update is called once per frame
	void Update () {

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

		if (GUILayout.Button ("Connect", GUILayout.Height(50))) {
			Server.ConnectToServer();
			Send("10singo,singo@@@@@");
		}
		if (GUILayout.Button ("Send", GUILayout.Height(50))) {
			
			Send("31@@@@@");
			Send("32@@@@@");

		}
		if(GUILayout.Button("Close")){
			State.clientSocket.Close();//關閉通訊器
				infomationText("伺服器斷線了！");
			print("伺服器斷線了！");//顯示斷線
			
			State.chkThread=false;
		}

		
		if(click==false){
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
			
				if (((Status >= 1 && Status <= 4) || Status == 6)&& Trigger_ob!=null) {
					worldPosition = new Vector3 (Trigger_ob.transform.position.x, Trigger_ob.transform.position.y + Trigger_ob.collider.bounds.size.y, Trigger_ob.transform.position.z);
					screenPosition = transform.FindChild ("Main Camera").camera.WorldToScreenPoint (worldPosition);
					screenPosition.y = Screen.height - screenPosition.y;
					if (Status == 1) {
						Source data = Trigger_ob.GetComponent<Source> ();
						GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), Trigger_ob.name + "\n" + data.quatity,guiSkin.label);
					} else if (Status == 2) {
						build data = Trigger_ob.GetComponent<build> ();
						GUI.Label (new Rect (screenPosition.x, screenPosition.y, 100, 100), data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.label);
					} else if (Status == 3) {
						build data = Trigger_ob.GetComponent<build>();
								GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.label);
					} else if (Status == 4) {
						build data = Trigger_ob.GetComponent<build>();
								GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),data.HouseName[data.kind] + " Lv" + data.HouseLevel + "\n" + data.PlayerID + "\nHp" + data.HP,guiSkin.label);
					} else if (Status == 6) {
						build data = Trigger_ob.GetComponent<build>();
						GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.label);
					}else if (Status == 7) {
								build data = Trigger_ob.GetComponent<build>();
								GUI.Label(new Rect(screenPosition.x,screenPosition.y,100,100),Trigger_ob.tag + "\n" + data.PlayerID,guiSkin.label);
					}

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
			
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), source[0].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),sourceName[0],guiSkin.customStyles[3])){
					
					
				}
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), source[1].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),sourceName[1],guiSkin.customStyles[3])){

					
					
				}
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), source[2].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),sourceName[2],guiSkin.customStyles[3])){

					
					
				}
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), source[3].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),sourceName[3],guiSkin.customStyles[3])){

					
					
				}
				GUI.Label (new Rect (Screen.width*6/16, Screen.height*11/ 26 , Screen.width/8, Screen.height/16 ), source[4].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),sourceName[4],guiSkin.customStyles[3])){

					
					
				}
				GUI.Label (new Rect (Screen.width*9/16, Screen.height* 11/ 26 , Screen.width/8, Screen.height/16 ), source[5].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),sourceName[5],guiSkin.customStyles[3])){

					
				}
				GUI.Label (new Rect (Screen.width*6/16, Screen.height* 15/ 26 , Screen.width/8, Screen.height/16 ), source[6].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),sourceName[6],guiSkin.customStyles[3])){

					
				}
				GUI.Label (new Rect (Screen.width*9/16, Screen.height*15/ 26 , Screen.width/8, Screen.height/16 ), source[7].ToString(),guiSkin.customStyles[4]);
				
				if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),sourceName[7],guiSkin.customStyles[3])){

					
					
				}

			
			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				bag = false;
				click=false;
			}
			GUI.EndGroup();
			
			
		}
		
		///Bag/////end
		

		
		
		
		
	
		
		///BOMB//////////
			/// 
			/// 
		if(BombSelected==true){ 
				GUI.Box (new Rect (0, Screen.height / 5, Screen.width, Screen.height / 10),"選擇目標",guiSkin.box);
				
				if(Status!=2 && Status!=3&&Status!=4&&Status!=6&&Status!=7){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;
					
				}

			if (GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),BombPng,guiSkin.customStyles[3])){
					j=0;
					w=1;
					h=1;
					BombGameQ="";
					BombGameInput="";
					
					for(int i =0;i<BombGameButton[bombKind];i++){
						GameQ[i]=BombGame[(int)(UnityEngine.Random.Range(0, BombGameButton[bombKind]))];
						BombGameQ+=GameQ[i];
						BombGameQ+=" ";
					}	
					BombGameStart=true;
					GameTime=Time.time;

					toolBomb[bombKind]--;
					BombSelected=false;
					
				}

				GUI.enabled=true;
				
			if (GUI.Button (new Rect (Screen.width*14/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				BombSelected=false;

				click=false;
			}
		}

		if (5 - (int)(Time.time - BombTimeCount) >= 1) {
			infomationText(((int)(5-(Time.time-BombTimeCount))).ToString());
			
		}
		if (BombGameStart == true) {

			if(Time.time-GameTime>=BombGameTime[bombKind]){
				infomationText("False....");
				BombGameStart=false;
				click=false;
				
			}

			GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 1/ 10 , Screen.width/2, Screen.height/10 ),BombGameQ,guiSkin.textArea);
				GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 2/ 10, Screen.width/2, Screen.height/10 ),BombGameInput,guiSkin.textArea);
				GUI.Box(new Rect (Screen.width* 1/4, Screen.height* 3/ 10, Screen.width/2*((BombGameTime[bombKind]-Time.time+GameTime)/BombGameTime[bombKind]), Screen.height/20 ),"",guiSkin.textArea);
			
			switch(bombKind){
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
					DestroyHouse=TriggerHouse;
						Instantiate(BombObjectS,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectS.transform.rotation);
					
					click=false;
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
						if (GUI.Button (new Rect (Screen.width*w/5, Screen.height* 1/ 3 + (Screen.height* (h-1)*2/ 9), Screen.width/5, Screen.height*2/9 ),BombGame[i].ToString(),guiSkin.button)){
						if(GameQ[j]!=BombGame[i]){
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
					DestroyHouse=TriggerHouse;
						Instantiate(BombObjectM,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectS.transform.rotation);
						
					click=false;
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
						if (GUI.Button (new Rect (Screen.width*1/4+(Screen.width*(w-1)/8), Screen.height* 1/ 3 + (Screen.height* (h-1)/ 6), Screen.width/8, Screen.height*1/6 ),BombGame[i].ToString(),guiSkin.button)){
						if(GameQ[j]!=BombGame[i]){
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
						DestroyHouse=TriggerHouse;
						Instantiate(BombObjectL,DestroyHouse.transform.position+new Vector3(0,3,0),BombObjectS.transform.rotation);
						
						click=false;
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
		

		///BOMB//////////
		
		
		///FuctionButton/////
		if (click == false) {//hide FuctionButton
			if (Build==false && GUI.Button (new Rect (Screen.width-Screen.width/8, Screen.height* 3 / 4-Screen.height/5 , Screen.width/10, Screen.height/6 ),Bag,guiSkin.customStyles[3])){
				bag=true;
				click=true;
			}
				if (Build==false && GUI.Button (new Rect (Screen.width-Screen.width/8, Screen.height* 3 / 4-Screen.height*2/5, Screen.width/10, Screen.height/6 ),BombPng,guiSkin.customStyles[3])){
				BombSelect = true;
				click=true;
			}
				if(Status==1 &&Time.time-picktime<CDtime[toolKind]){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;
					
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
								infomationText("取得 "+getQutity.ToString()+" "+sourceName[kind]+" !");
							
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
						}else{
							infomationText("超過負載!");
							
						}
					Status=0;
					break;
				case 2:
					//倉庫放這邊
					build StockNow = TriggerHouse.GetComponent<build>();
					StockNow.work=true;
					click=true;
					break;
				case 3:
					build WorkNow = TriggerHouse.GetComponent<build>();
					WorkNow.work=true;
					click=true;
					//合成放這邊				
					break;
				case 4:
					build ScienceNow = TriggerHouse.GetComponent<build>();
					ScienceNow.work=true;
					click=true;
					//精煉放這邊	
					break;
				case 5:
					//裝炸彈放這邊
					break;
				case 6:

						break;
				case 7:
				build HouseNow = TriggerHouse.GetComponent<build>();
				HouseNow.work=true;
				click=true;
					
				break;
				}	
				
			}

			if(Status==0||Status==1||Status==7){
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
				
			}
				GUI.enabled=true;

				///Bomb Button
			
				///Bomb Button
		}//end of click
		///FuctionButton/////
		
		
		///Build/////
		GUI.enabled = true;

			if(BombSelect==true){

				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);

				////
				GUI.BeginGroup(new Rect (Screen.width*3/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), Construction, guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[0]+ "\r\n攜帶量:"+toolBomb[0], guiSkin.label);
				if(toolBomb[0]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/16,Screen.height*10/20, Screen.width*1/10, Screen.width*1/20 ),"設置",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=0;
					
				}
				GUI.enabled=true;
				
				GUI.EndGroup();
				///
				GUI.BeginGroup(new Rect (Screen.width*17/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), Construction, guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[1]+ "\r\n攜帶量:"+toolBomb[1], guiSkin.label);
				if(toolBomb[1]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/16,Screen.height*10/20, Screen.width*1/10, Screen.width*1/20 ),"設置",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=1;
				
				}
				GUI.enabled=true;
				
				GUI.EndGroup();
				////
				/// 
				GUI.BeginGroup(new Rect (Screen.width*31/60,Screen.height/20, Screen.width*7/30, Screen.height*7/10));
				
				GUI.Box (new Rect (0, 0,Screen.width*7/30, Screen.height*7/10), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width*1/22,Screen.width*1/50, Screen.width*2/14, Screen.width*2/14), Construction, guiSkin.box);
				GUI.Label (new Rect (Screen.width*1/22,Screen.height*7/20, Screen.width*2/14, Screen.width*2/14), BombInfo[2]+ "\r\n攜帶量:"+toolBomb[2], guiSkin.label);
				if(toolBomb[2]==0){
					GUI.enabled=false;
				}else{
					GUI.enabled=true;	
				}
				if (GUI.Button (new Rect (Screen.width*1/16,Screen.height*10/20, Screen.width*1/10, Screen.width*1/20 ),"設置",guiSkin.button)){
					BombSelected=true;
					BombSelect=false;
					bombKind=2;
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
			
			break;
		case "House":
			Status = 6;
			TriggerHouse=other.gameObject;
			
			break;
		case "Struction":
			Status = 7;
			TriggerHouse=other.gameObject;
			
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
