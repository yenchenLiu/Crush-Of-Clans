using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Text;
using System.Threading;
public class build : MonoBehaviour {
	
	//共同變數
	public GUISkin guiSkin;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation,Treasure;
	private GameObject player;
	private Player playerNow;
	public Texture[] tool;
	public Texture[] source; 
	private int[] damageSource=new int[8];
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	private SpriteRenderer spriteRenderer;
	
	public int kind,status;//建築類型
	public int HouseLevel;
	public bool work,LevelUp,loading,fixedHouse,update;
	public int HP=100,MaxHP;
	public string PlayerID;
	public string HouseID;
	public string HouseKind;
	private string sendString;
	public AudioClip sound;
	public AudioClip LevelUpSound;
	protected AudioSource m_sound;
	
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	public string[] HouseName = {"工地","住宅","倉庫","工作屋","精煉屋","世界奇觀"};
	private int[] houseLevelLimit={1,3,5,5,5,1};
	private int[] HpMax = {100,300,700,1000,1500};
	
	
	//共同變數end
	///
	//相似變數
	private string[,] LevelInfo = {
		{"","","",""},//工地
		{"新增功能：建築方向指標<即將開放>","新增功能：瞬間移動<即將開放>","",""},//住宅
		{"倉庫存放量：10000,可存放資源：鐵礦、硫磺礦","倉庫存放量：20000,可存放資源：鐵片、木炭","倉庫存放量：30000,可存放資源：硫磺","倉庫存放量：40000,可存放資源：火藥"},//倉庫
		{"可建造工具：鐵製斧頭、鐵製十字鎬","可建造工具：木製手推車、普通炸彈","可建造工具：中級炸彈","可建造工具：高級炸彈、鐵製手推車"},//工作屋
		{"可合成資源：木炭","可合成資源：硫磺","","可合成資源：火藥”"},//精煉屋
		{"","","",""}//世界奇觀
	};
	private int[,,] LevelUpSource = {
		{{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0}},//工地
		{{100,100,10,0,0,0,0,0},{400,300,10,0,0,0,0,0},{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0}},//住宅
		{{50,20,5,0,0,0,0,0},{100,50,10,0,0,0,0,0},{200,100,50,0,0,0,0,0},{400,200,200,0,0,0,0,0}},//倉庫
		{{100,100,10,0,0,0,0,0},{200,200,50,0,0,0,0,0},{300,300,100,0,0,0,0,0},{500,400,200,0,0,0,0,0}},//工作屋
		{{100,100,10,0,0,0,0,0},{200,200,50,0,0,0,0,0},{300,300,100,0,0,0,0,0},{500,400,200,0,0,0,0,0}},//精煉屋
		{{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0},{0,0,0,0,0,0,0,0}}//世界奇觀
	};
	public Texture[] LevelPng ;
	public Sprite[] LevelSpritePng;
	
	//相似變數end
	///
	//獨有變數
	
	///Construction
	
	private int[,] needHouseSource = {{0,0,0,0,0,0,0,0},{200,50,0,0,0,0,0,0},{300,0,0,0,0,0,0,0},{500,100,0,0,0,0,0,0},{500,300,0,0,0,0,0,0},{1000,500,200,0,0,0,0,0}};//st,w,sc,h,sky
	public Rigidbody build_stock,build_work,build_science,build_house,build_101,buildNow;
	private string[] HouseInfo={"","建築地標，提供外出時的方向指引，等級提升後會有瞬間移動的功能可使用。<即將開放>\r\n陷阱：可陷害來裝炸彈的玩家失去資源並飛到遠處、依建築等級資源失去量及飛行距離皆會提升。","儲存資源，依據倉庫等級、能存放的數量及種類會有所變化。\r\n陷阱：可陷害來裝炸彈的玩家失去資源並飛到遠處、依建築等級資源失去量及飛行距離皆會提升。","製作道具，依據工作屋等級、能製作的工具種類會有所變化。\r\n陷阱：可陷害來裝炸彈的玩家失去資源並飛到遠處、依建築等級資源失去量及飛行距離皆會提升。","合成資源，依據精煉屋等級、能合成的資源種類會有所變化。\r\n陷阱：可陷害來裝炸彈的玩家失去資源並飛到遠處、依建築等級資源失去量及飛行距離皆會提升。","世界奇觀，能在一定時間後，能將除了自己以外所有建築夷平。<即將開放>\r\n陷阱：可陷害來裝炸彈的玩家失去資源並飛到遠處、依建築等級資源失去量及飛行距離皆會提升。"};
	public int selectHouse;
	public Texture[] housePng; 
	
	///Construction End
	
	///Home
	
	///Home End
	////Stock
	private int[] stockNeedLevel = {1,1,3,4,3,5,2,2};
	public int[] stockSource = {20,20,20,0,0,0,0,0};
	public int[] stocklimit = {5000,10000,20000,30000,40000};
	public int weightNow;
	public bool put,get;
	public string Quatity;
	private int selectSource,playerSource;
	private int limit;
	private int[] weight={5,10,20,10,10,10,30,30};// = {5,10,50};//0 wood, 1 stone ,2 metal;
	
	////Stock End
	//workwHouse////
	private string[,] TooLInfo = {{"採集量：10 最高耐久：100","採集量：30 最高耐久：200",""},{"採集量：10 最高耐久：100","採集量：30 最高耐久：200",""},{"威力：50 範圍：單一","威力：100 範圍：九宮格","威力：200 範圍：連鎖"},{"負重量：10000","負重量：20000",""}};
	private int[,] toolNeedLevel = {{1,2,10},{1,2,10},{2,4,5},{3,5,10}};
	private int[,,] toolNeedSource = {{ {100,50,0,0,0,0,0,0},{100,20,50,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {100,100,0,0,0,0,0,0},{300,0,150,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {0,0,0,0,10,5,0,0},{0,0,0,0,100,50,0,0},{0,0,0,0,500,300,0,0} },{ {500,0,0,0,0,0,0,0},{100,0,300,0,0,0,0,0},{0,0,0,0,0,0,0,0} }};
	private int[,] toolExist={{0,0,0},{0,0,0},{0,0,0},{0,0,0}}; 
	private int[,] toolHp={{100,200},{100,200}};
	private int[,] toolHpNow={{0,0},{0,0}};
	private bool selectHouseKind;//false工作坊 ture 工具庫
	private string[] toolName={"斧頭","十字鎬","炸彈","推車"};
	private int selectTool,selectToolKind;
	//public GameObject[] tool={null,null,null,null};
	
	//workwHouse End////
	///scienceHouse
	private int[] needScienceLevel = {1,2,4,5};
	private int[,] needScienceSource = {{2,0,0,0,0,0,1,0},{4,0,0,0,0,0,0,1},{1,0,0,0,0,0,0,0},{0,0,0,5,10,0,0,0}};
	private int selectScienceSource;
	
	///scienceHouse End
	
	
	//獨有變數end
	

	//共同fuction	
	void destroyThisHouse(){
		//			print ("error2");
		
		try{
		if (State.HouseID.ContainsKey (HouseID)) {
			State.HouseID.Remove(HouseID);
				
		}
		
			State.HouseKind.Remove(HouseID);
			State.HousePlayerID.Remove(HouseID);
			
			State.HouseHP.Remove(HouseID);
			State.HousePositionX.Remove(HouseID);
			State.HousePositionZ.Remove(HouseID);
			State.HouseLevel.Remove(HouseID);
			State.HouseStatusNow.Remove(HouseID);
			State.HouseStatus.Remove(HouseID);
			State.HouseUpdate.Remove (HouseID);

			Destroy(this.gameObject);
		}catch(Exception){
			print ("error3");
		}
	
	}

	IEnumerator source_function(){
			
		
		while (work) {
			
			Server.Send ("61");
			yield return new WaitForSeconds(1);
			if(State.HouseStockSource.ContainsKey(HouseID)){
				stockSource=State.HouseStockSource[HouseID];
			}
			yield return new WaitForSeconds(2);
			
		}
		
		
	}

	IEnumerator tool_function(){
		
		
		while (work) {
			
			Server.Send ("63");
			yield return new WaitForSeconds(1);
			if(State.HouseTool.ContainsKey(HouseID)){
			
				toolExist=State.HouseTool[HouseID];
			}
			yield return new WaitForSeconds(2);
			
		}
		
		
	}

	void sendSourceModify(int[] quatity){
		print ("HAHAHA!!");
		sendString = "24";
		for (int i=0; i<quatity.Length; i++) {
			if(quatity[i]!=0){
				sendString = "24";
				sendString+=(i+1).ToString()+","+(quatity[i]).ToString();
				Server.Send (sendString);
				print (sendString);
			}
		}

	}
	void Start () {
		print ("OK!");
		////Server////
		spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
	
		
		/// Server///
		///共同
		//StartCoroutine("server_function");
		sendString = "";
		fixedHouse = false;
		work = false;
		LevelUp = false;
		bombCount = 0;
		bombSet = null;
		update = false;
		///共同
		////
		//獨有
		weightNow = 0;
		///Construction
		selectHouse = 0;
		///Construstion End
		//stockHouse
		put = false;
		get=false;
		selectSource = 0;
		Quatity = "0";
		//stockHouse End
		//workhouse
		selectTool = 0;
		selectToolKind = 0;
		selectHouseKind = false;
		selectHouse = 1;
		//workHouse End
		///scienceHouse
		selectSource = 0;
		///scienceHouse End
		m_sound = this.audio;
		
		//獨有
		
	}///End of Start
	
	void Update () {
		if (State.HouseKind.ContainsKey (HouseID)) {
		 	//spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = LevelSpritePng[HouseLevel-1];
			
			HouseKind = HouseName [State.HouseKind [HouseID]];
		
		}
	}///End of Update
	
	
	
	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			player=other.gameObject;
			playerNow=player.GetComponent<Player>();
		}
		
	}//End of OnTriggerStay
	
	//炸彈碰撞///
	IEnumerator bomb_function(){
		print ("Bomb2");
		yield return new WaitForSeconds(1);
		if(bomb.kind==2&&bombCount<2&& State.bombTotal<=5){
			State.bombTotal++;
			Instantiate(BombObject,this.transform.position+new Vector3(0,3,0),BombObject.transform.rotation);
			bombCount++;
		}
		yield return new WaitForSeconds(2);
		
		
		print( (State.bombTotal).ToString());

		Vector3 Pos=new Vector3(this.transform.position.x,this.transform.position.y+4,this.transform.position.z);
		
		GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
		Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.1f,this.transform.position.z);
		
		GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
		Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.2f,this.transform.position.z);
		
		GameObject BombAnimateNow=(GameObject) Instantiate(BombAnimation,Pos,Building.transform.rotation);
		
		Destroy(SomkeNow,3);
		Destroy(SmokeAnimateNow,3);
		Destroy(BombAnimateNow,3);
		

		if (this.HP-demage <= 0) {
			State.HouseHP[HouseID] -= demage;
			HP-=demage;
			Server.Send ("58"+HouseID+","+(demage*-1));
			
			print("BOMB!!!!");
			GameObject thisTreasure=(GameObject)Instantiate(Treasure,this.transform.position,Treasure.transform.rotation);
			Treasure treasureNow =thisTreasure.GetComponent<Treasure>();
				System.Random r=new System.Random();
			
			for(int i=0;i<8;i++){
				treasureNow.source[i]=r.Next(100);
			}
			bombCount = 0;
			State.bombTotal--;
			if(State.bombTotal<0) State.bombTotal=0;
			
			print( (State.bombTotal).ToString());
				
		} else {
			HP-=demage;
			State.HouseHP[HouseID]-=demage;
			
			Server.Send ("58"+HouseID+","+(demage*-1));
			yield return new WaitForSeconds (3);
			
			bombCount = 0;
			State.bombTotal--;
			
			if(State.bombTotal<0) {
				State.bombTotal=0;
			}
			
			print( (State.bombTotal).ToString());
			
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (bombSet==null && other.tag == "bomb") {
			print ("Bomb!");
			bombSet=other.gameObject;
			bomb=bombSet.GetComponent<Bomb>();	
			
			demage=bomb.power;
			print (demage.ToString());
			StartCoroutine("bomb_function");
			
		}
	}
	//炸彈碰撞///End
	
	void OnGUI () {



				string text;
				text = "";
				//獨有//////

		///Construction
		if(work == true &&status==5){
			playerNow.infomationText ("這是陷阱喔～！");
			work=false;
			playerNow.click=false;
		}else if(work == true &&(status==2 ||status==3||status==4)){
			
		///
			
		}else if (work == true && status==1) {
			if(kind==1 || kind==5){
				playerNow.infomationText("<即將開放>");
				work=false;
				playerNow.click=false;
			}
			if (kind == 0) {
				
				
				for (int i=0; i<8; i++) {
					if (needHouseSource [selectHouse, i] != 0) {
						text += "\r\n" + sourceName [i] + ":" + playerNow.source [i] + "/" + needHouseSource [selectHouse, i];
					}
				}
				
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), housePng[selectHouse], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), text, guiSkin.label);
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 11 / 20, Screen.width / 3, Screen.height / 6), HouseInfo [selectHouse], guiSkin.label);
				
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 1 / 20, Screen.width / 3, Screen.height / 10), "住宅", guiSkin.button)) {
					selectHouse = 1;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 3 / 20, Screen.width / 3, Screen.height / 10), "倉庫", guiSkin.button)) {
					selectHouse = 2;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 5 / 20, Screen.width / 3, Screen.height / 10), "工作屋", guiSkin.button)) {
					selectHouse = 3;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 7 / 20, Screen.width / 3, Screen.height / 10), "精煉屋", guiSkin.button)) {
					selectHouse = 4;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 20, Screen.width / 3, Screen.height / 10), "世界奇觀", guiSkin.button)) {
					selectHouse = 5;
				}
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					work = false;		
					playerNow.click = false;
				}
				GUI.EndGroup ();
				
				GUI.enabled = true;
				for (int i=0; i<8; i++) {
					if (playerNow.source [i] < needHouseSource [selectHouse, i]) {
						GUI.enabled = false;
						break;
					}
				}
				
				if (GUI.Button (new Rect (Screen.width * 3 / 20, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 8, Screen.width * 1 / 15), "建造建築", guiSkin.button)) {
					int[] modifySource=new int[8];
					
					switch (selectHouse) {
					case 1:
						if (!playerNow.isHomeExsist) {
						
							Server.Send("56"+HouseID+",2,1");
							State.HouseKind[HouseID]=1;

							sendString="";
							for (int i =0; i<8; i++) {
								modifySource[i]=-1*needHouseSource [1, i];
								playerNow.source [i] = playerNow.source [i] - needHouseSource [1, i];
							}
							sendSourceModify(modifySource);
							
							playerNow.infomationText ("房屋建造完成！");
							playerNow.click = false;
							playerNow.isHomeExsist = true;
							
							work = false;

						}
						break;
					case 2:
						Server.Send("56"+HouseID+",3,1");
						State.HouseKind[HouseID]=2;

						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [2, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [2, i];
						}
						sendSourceModify(modifySource);
						playerNow.infomationText ("倉庫建造完成！");
						playerNow.click = false;

						work = false;
						break;
					case 3:
						Server.Send("56"+HouseID+",4,1");
							State.HouseKind[HouseID]=3;

						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [3, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [3, i];
						}
						sendSourceModify(modifySource);
						playerNow.infomationText ("工具屋建造完成！");
						playerNow.click = false;
						work = false;
						
					
						break;
					case 4:
							Server.Send("56"+HouseID+",5,1");
							State.HouseKind[HouseID]=4;

						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [4, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [4, i];
						}
						sendSourceModify(modifySource);
						
						playerNow.infomationText ("精煉屋建造完成！");
						playerNow.click = false;
						work = false;
					
						break;
						
					case 5:
						Server.Send("56"+HouseID+",6,1");
						State.HouseKind[HouseID]=5;
						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [5, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [5, i];
						}
						sendSourceModify(modifySource);
						
						playerNow.infomationText ("世界奇觀建造完成！");
						playerNow.click = false;
						work = false;

						break;
					}
					
					Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 5);
					Destroy (SomkeNow, 5);
					Destroy (SmokeAnimateNow, 5);
					
				}///End of Building
				if (GUI.Button (new Rect (Screen.width * 6 / 20, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 8, Screen.width * 1 / 15), "建造陷阱", guiSkin.button)) {
					int[] modifySource=new int[8];
					
					switch (selectHouse) {
					case 1:
						if (!playerNow.isHomeExsist) {
							
							Server.Send("56"+HouseID+",2,1");
							Server.Send("65"+HouseID+",5");
							
							State.HouseKind[HouseID]=1;
							
							sendString="";
							for (int i =0; i<8; i++) {
								modifySource[i]=-1*needHouseSource [1, i];
								playerNow.source [i] = playerNow.source [i] - needHouseSource [1, i];
							}
							sendSourceModify(modifySource);
							
							playerNow.infomationText ("陷阱建築建造完成！");
							playerNow.click = false;
							playerNow.isHomeExsist = true;
							
							work = false;
							
						}
						break;
					case 2:
						Server.Send("56"+HouseID+",3,1");
						Server.Send("65"+HouseID+",5");
						
						State.HouseKind[HouseID]=2;
						
						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [2, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [2, i];
						}
						sendSourceModify(modifySource);
						playerNow.infomationText ("陷阱建築建造完成！");
						playerNow.click = false;
						
						work = false;
						break;
					case 3:
						Server.Send("56"+HouseID+",4,1");
						Server.Send("65"+HouseID+",5");
						
						State.HouseKind[HouseID]=3;
						
						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [3, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [3, i];
						}
						sendSourceModify(modifySource);
						playerNow.infomationText ("陷阱建築建造完成！");
						playerNow.click = false;
						work = false;
						
						
						break;
					case 4:
						Server.Send("56"+HouseID+",5,1");
						Server.Send("65"+HouseID+",5");
						
						State.HouseKind[HouseID]=4;
						
						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [4, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [4, i];
						}
						sendSourceModify(modifySource);
						
						playerNow.infomationText ("陷阱建築建造完成！");
						playerNow.click = false;
						work = false;
						
						break;
						
					case 5:
						Server.Send("56"+HouseID+",6,1");
						Server.Send("65"+HouseID+",5");
						
						State.HouseKind[HouseID]=5;
						for (int i =0; i<8; i++) {
							modifySource[i]=-1*needHouseSource [5, i];
							playerNow.source [i] = playerNow.source [i] - needHouseSource [5, i];
						}
						sendSourceModify(modifySource);
						
						playerNow.infomationText ("陷阱建築建造完成！");
						playerNow.click = false;
						work = false;
						
						break;
					}
					
					Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 5);
					Destroy (SomkeNow, 5);
					Destroy (SmokeAnimateNow, 5);
					
				}///End of Building
				GUI.enabled = true;
				
				
				
				
				
				
			}
			
			///End of Consturction
			///stockHouse 
			
			if (kind == 2) {
				if(update==false){
				//	StartCoroutine("source_function");
					update=true;
				}
				weightNow=0;
				for(int i=0;i<8;i++){
					weightNow+=stockSource[i]*weight[i];
				}
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), source[selectSource], guiSkin.box);
				GUI.Label (new Rect (Screen.width/10,Screen.height*6/10, Screen.width*1/5, Screen.width*1/25),"儲存上限："+weightNow+"/"+stocklimit[HouseLevel], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10 + Screen.width * 1 / 20, Screen.height * 10 / 20, Screen.width * 1 / 10, Screen.width * 1 / 5), sourceName[selectSource]+"\r\n持有量："+playerNow.source[selectSource], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20 + Screen.width * 1 / 10, Screen.width * 1 / 5, Screen.width * 1 / 5), "", guiSkin.label);
				//GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
				if (put == true || get == true)
					GUI.enabled = false;
				else
					GUI.enabled = true;
				for (int i=0; i<7; i+=2) {
					int j=i/2;
					GUI.Label (new Rect (Screen.width * 6 / 16, Screen.height * (3 + (4 * j) )/ 26, Screen.width / 8, Screen.height / 16), stockSource[i].ToString (), guiSkin.customStyles [4]);
					
					if (GUI.Button (new Rect (Screen.width * 5 / 16, Screen.height * (1 + (2 * j)) / 13, Screen.width / 12, Screen.height / 8), source [i], guiSkin.customStyles [6])) {
						selectSource = i;
					}
					GUI.Label (new Rect (Screen.width * 9 / 16, Screen.height * (3 + (4 * j)) / 26, Screen.width / 8, Screen.height / 16), stockSource[i + 1].ToString (), guiSkin.customStyles [4]);
					
					if (GUI.Button (new Rect (Screen.width * 8 / 16, Screen.height * (1 + (2 * j)) / 13, Screen.width / 12, Screen.height / 8), source [i + 1], guiSkin.customStyles [6])) {
						selectSource = i + 1;
						
					}
				}
				
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					work = false;		
					playerNow.click = false;
					LevelUp = false;
					update=false;
				}
				GUI.EndGroup ();
				
				playerSource = playerNow.source [selectSource];
				limit=playerNow.package[playerNow.cart]-playerNow.source[1]*weight[1]-playerNow.source[2]*weight[2];
				if (HouseLevel >= stockNeedLevel [selectSource]) {
					GUI.enabled = true;
				} else {
					GUI.enabled = false;
					
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 20, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 8, Screen.width * 1 / 15), "取出", guiSkin.button)) {
					get = true;
				}
				if (GUI.Button (new Rect (Screen.width * 6 / 20, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 8, Screen.width * 1 / 15), "放入", guiSkin.button)) {
					put = true;
					
				}
				GUI.enabled = true;
				if (get == true || put == true) {
					GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 4), Screen.height / 2 - (Screen.height / 4), Screen.width / 2, Screen.height / 2), "", guiSkin.box);
					Quatity = GUI.TextField (new Rect (Screen.width*7/16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 8, Screen.height * 1 / 12), Quatity, guiSkin.textField);
					if (GUI.Button (new Rect (Screen.width * 5 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), "<<", guiSkin.button)) {
						if(get==true){
							Quatity = "0";
							
						}
						if(put==true){
							Quatity = "0";
							//Quatity=((int)(stocklimit[HouseLevel]-weightNow)/weight[selectSource]).ToString();
						}
					}
					if (GUI.Button (new Rect (Screen.width * 6 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), "<", guiSkin.button)) {
						if (int.Parse (Quatity) > 0) {

								Quatity = ((int.Parse (Quatity)) - 1).ToString ();
								

						}
						
						
					}
					if (GUI.Button (new Rect (Screen.width * 10 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), ">>", guiSkin.button)) {
						if (put == true) {
							if(playerNow.source [selectSource]*weight[selectSource] <= stocklimit[HouseLevel]-weightNow ){
								Quatity = playerNow.source [selectSource].ToString ();
							}else{
								Quatity=((int)(stocklimit[HouseLevel]-weightNow)/weight[selectSource]).ToString();

							}
							if(int.Parse(Quatity)<0 ) Quatity="0";
							
						}
						if (get == true) {
							if(stockSource [selectSource]*weight[selectSource]<=  playerNow.package[playerNow.cart]){
								Quatity = stockSource [selectSource].ToString ();
							}else{
								Quatity=((int)(playerNow.package[playerNow.cart]-playerNow.weightNow)/weight[selectSource]).ToString();

							}
							if(int.Parse(Quatity)<0 ) Quatity="0";
							

							
						}
						
					}
					if (GUI.Button (new Rect (Screen.width * 9 / 16, Screen.height / 2 - Screen.height * 1 / 24, Screen.width * 1 / 16, Screen.height * 1 / 12), ">", guiSkin.button)) {
						if (put == true) {
							
							limit = playerNow.source [selectSource];
							if(limit*weight[selectSource]>stocklimit[HouseLevel]-weightNow){
								limit=(int)((stocklimit[HouseLevel]-weightNow)/weight[selectSource]);
								if(limit<0) limit=0;
							}
						}
						if (get == true) {
							limit = stockSource [selectSource];
							if(limit*weight[selectSource]>playerNow.package[playerNow.cart]-playerNow.weightNow){
								limit=(int)((playerNow.package[playerNow.cart]-playerNow.weightNow)/weight[selectSource]);
							}
								if(limit<0) limit=0;
							
						}

						if (int.Parse (Quatity) < limit) {
							Quatity = ((int.Parse (Quatity)) + 1).ToString ();
						}
						if(int.Parse(Quatity)<0 ) Quatity="0";
						
					}

					if (GUI.Button (new Rect (Screen.width * 8 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 10), "確定", guiSkin.button)) {
						if (get == true) {
							int[] quatity=new int[8]{0,0,0,0,0,0,0,0};
							quatity[selectSource]= int.Parse (Quatity);
							playerNow.source [selectSource] = playerNow.source [selectSource] + int.Parse (Quatity);

							stockSource [selectSource] = stockSource [selectSource] - int.Parse (Quatity);
							sendSourceModify(quatity);
							
						//	Server.Send("62"+HouseID+","+(selectSource+1).ToString()+","+(int.Parse (Quatity)*-1).ToString());
						}
						if (put == true) {
							int[] quatity=new int[8]{0,0,0,0,0,0,0,0};
							quatity[selectSource]= -1*int.Parse (Quatity);
							
							playerNow.source [selectSource] = playerNow.source [selectSource] - int.Parse (Quatity);
							stockSource [selectSource] = stockSource [selectSource] + int.Parse (Quatity);
							sendSourceModify(quatity);
							
						//	Server.Send("62"+HouseID+","+(selectSource+1).ToString()+","+(int.Parse (Quatity)).ToString());
							
						}

						Quatity = "0";
						get = false;
						put = false;
					}
					
					if (GUI.Button (new Rect (Screen.width * 10 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 10), "取消", guiSkin.button)) {
						Quatity = "0";
						get = false;
						put = false;
					}
					
				}
				///end of stockHouse 
			}
			if (kind == 3) {//工作屋////
				if(update==false){
					//StartCoroutine("tool_function");
					update=true;
				}
				
				for (int i=0; i<8; i++) {
					if (toolNeedSource [selectTool, selectToolKind, i] != 0) {
						text += sourceName [i] + ":" + playerNow.source [i] + "/" + toolNeedSource [selectTool, selectToolKind, i] + "\r\n";
					}
				}
				text += "所需等級：" + HouseLevel + "/" + toolNeedLevel [selectTool, selectToolKind];
				
				
				GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "");
				if (selectHouseKind == false) {
					GUI.enabled = false;
				} else {
					GUI.enabled = true;
				}
				if (GUI.Button (new Rect (Screen.width * 2 / 40, Screen.height / 10, Screen.width * 1 / 20, Screen.height / 4), "工\r\n作\r\n坊", guiSkin.button)) {
					selectHouseKind = false;
					selectTool = 0;	
					selectToolKind = 0;
				}
				if (selectHouseKind == false) {
					GUI.enabled = true;
				} else {
					GUI.enabled = false;
				}
				if (GUI.Button (new Rect (Screen.width * 2 / 40, Screen.height / 10 + Screen.height / 4, Screen.width * 1 / 20, Screen.height / 4), "工\r\n具\r\n庫", guiSkin.button)) {
					selectHouseKind = true;
					selectTool = 0;	
					selectToolKind = 0;
					
				}
				for (int i =0; i<4; i++) {
					if (selectTool == i) {
						GUI.enabled = false;
					} else {
						GUI.enabled = true;
					}
					if (GUI.Button (new Rect (Screen.width * (1 + (i * 2)) / 10, 0, Screen.width * 1 / 5, Screen.height / 8), toolName [i], guiSkin.button)) {
						selectTool = i;	
						selectToolKind = 0;
						
					}
				}	
				
				GUI.enabled = true;
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				//GUI.Label (new Rect (0,10, Screen.width*4/5, Screen.height*1/10),toolName[selectTool], guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), tool[selectToolKind+(selectTool*3)], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				if (selectHouseKind == false && selectTool != 2) {
					GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), text, guiSkin.label);
				}
				if (selectHouseKind == false && (selectTool == 2)) {
					GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), text + "\r\n庫存量:" + toolExist [selectTool, selectToolKind], guiSkin.label);
					
				}
				if (selectHouseKind == true && (selectTool == 0 || selectTool == 1)) {
					GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), "耐久:" + toolHpNow [selectTool, selectToolKind], guiSkin.label);
					
				}
				if (selectHouseKind == true && (selectTool == 2)) {
					GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), "庫存量:" + toolExist [selectTool, selectToolKind] + "\r\n攜帶量:" + playerNow.toolBomb [selectToolKind], guiSkin.label);
					
				}
				
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 16, Screen.width / 3, Screen.height / 8), TooLInfo [selectTool, selectToolKind], guiSkin.label);
				switch (selectTool) {
				case 0:
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 16, Screen.width / 3, Screen.height / 8), "石製斧頭", guiSkin.button)) {
						selectToolKind = 0;
					}
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.height / 8), "鐵製斧頭", guiSkin.button)) {
						selectToolKind = 1;
					}
					
					break;
				case 1:
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 16, Screen.width / 3, Screen.height / 8), "石製十字鎬", guiSkin.button)) {
						selectToolKind = 0;
					}
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.height / 8), "鐵製十字鎬", guiSkin.button)) {
						selectToolKind = 1;
					}
					break;
				case 2:
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 16, Screen.width / 3, Screen.height / 8), "普通炸彈", guiSkin.button)) {
						selectToolKind = 0;
					}
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.height / 8), "中級炸彈", guiSkin.button)) {
						selectToolKind = 1;
					}
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 6 / 16, Screen.width / 3, Screen.height / 8), "高級炸彈", guiSkin.button)) {
						selectToolKind = 2;
					}
					break;
					
				case 3:
					
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 16, Screen.width / 3, Screen.height / 8), "木製手推車", guiSkin.button)) {
						selectToolKind = 0;
					}
					if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.height / 8), "鐵製手推車", guiSkin.button)) {
						selectToolKind = 1;
					}
					break;
				}
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					work = false;		
					LevelUp = false;		
					playerNow.click = false;
					selectHouseKind = false;
					update=false;
					
				}
				GUI.EndGroup ();
				
				////make Tool////
				if (selectHouseKind == false) {
					
					
					if (playerNow.source [0] >= toolNeedSource [selectTool, selectToolKind, 0] && playerNow.source [1] >= toolNeedSource [selectTool, selectToolKind, 1] && playerNow.source [2] >= toolNeedSource [selectTool, selectToolKind, 2] && playerNow.source [3] >= toolNeedSource [selectTool, selectToolKind, 3] && playerNow.source [4] >= toolNeedSource [selectTool, selectToolKind, 4] && playerNow.source [1] >= toolNeedSource [selectTool, selectToolKind, 1] && playerNow.source [5] >= toolNeedSource [selectTool, selectToolKind, 5] && playerNow.source [6] >= toolNeedSource [selectTool, selectToolKind, 6] && playerNow.source [7] >= toolNeedSource [selectTool, selectToolKind, 7] && HouseLevel >= toolNeedLevel [selectTool, selectToolKind]) {
						GUI.enabled = true;
						
						if (selectTool == 0 || selectTool == 1 || selectTool == 3) {
							if (playerNow.tool == selectTool + 1 && playerNow.toolKind == selectToolKind) {
								GUI.enabled = false;
							}
							if (toolExist [selectTool, selectToolKind] != 0) {
								GUI.enabled = false;
								
							}
						}
					} else {
						GUI.enabled = false;
						
					}
					
					if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 5, Screen.width * 1 / 15), "製作", guiSkin.button)) {
						toolExist [selectTool, selectToolKind]++;	
						if (selectTool == 0 || selectTool == 1) {
							toolHpNow [selectTool, selectToolKind] = toolHp [selectTool, selectToolKind];
						}
						playerNow.infomationText (toolName [selectTool] + "製作成功!");
						int[] quatity=new int[8];
						for (int i=0; i<8; i++) {
							quatity[0]=-1*toolNeedSource [selectTool, selectToolKind, i];
							playerNow.source [i] -= toolNeedSource [selectTool, selectToolKind, i];
							
						}
						int toolKind=0;
						if(selectTool == 0&& selectToolKind==0){
							toolKind=1;
						}
						if(selectTool == 0&& selectToolKind==1){
							toolKind=2;
						}
						if(selectTool == 1&& selectToolKind==0){
							toolKind=3;
						}
						if(selectTool == 1&& selectToolKind==1){
							toolKind=4;
						}
						if(selectTool == 2&& selectToolKind==0){
							toolKind=5;
						}
						if(selectTool == 2&& selectToolKind==1){
							toolKind=6;
						}
						if(selectTool == 2&& selectToolKind==2){
							toolKind=7;
						}
						if(selectTool == 3&& selectToolKind==0){
							toolKind=8;
						}
						if(selectTool == 3&& selectToolKind==1){
							toolKind=9;
						}
						//Server.Send ("64"+HouseID+","+toolKind+",1");
						print ("64"+HouseID+","+toolKind+",1");
						sendSourceModify(quatity);
						
						
					}
					GUI.enabled = true;
					
					////make Tool///
					///
					/// Equipe TOOL//
				} else if (selectHouseKind == true) {
					/// Equipe //
					if (toolExist [selectTool, selectToolKind] != 0) {
						
						GUI.enabled = true;
					} else {
						
						GUI.enabled = false;
					}
					
					if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 10, Screen.width * 1 / 15), "裝備", guiSkin.button)) {
						
						if (selectTool == 0 || selectTool == 1) {
							if (playerNow.tool != 0) {
								toolExist [playerNow.tool - 1, playerNow.toolKind]++;
								toolHpNow [playerNow.tool - 1, playerNow.toolKind] = playerNow.toolHp;
							}
							playerNow.tool = selectTool + 1;
							playerNow.toolKind = selectToolKind;
							int toolkind=0;
							if(selectTool==0 && selectToolKind==0){
								toolkind=1;
							}
							if(selectTool==0 && selectToolKind==1){
								toolkind=2;
							}
							if(selectTool==1 && selectToolKind==0){
								toolkind=3;
							}
							if(selectTool==1 && selectToolKind==1){
								toolkind=4;
							}
							Server.Send ("26"+toolkind.ToString()+","+playerNow.cart);
							playerNow.toolHp = toolHpNow [selectTool, selectToolKind];
							toolExist [selectTool, selectToolKind]--;

							int toolKind=0;
							if(selectTool == 0&& selectToolKind==0){
								toolKind=1;
							}
							if(selectTool == 0&& selectToolKind==1){
								toolKind=2;
							}
							if(selectTool == 1&& selectToolKind==0){
								toolKind=3;
							}
							if(selectTool == 1&& selectToolKind==1){
								toolKind=4;
							}

						//	Server.Send ("64"+HouseID+","+toolKind+",-1");
							print ("64"+HouseID+","+toolKind+",-1");
							/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
							usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
							usetool.name=toolName[selectTool];*/
							playerNow.infomationText ("已裝備" + toolName [selectTool] + "!");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							update=false;
							m_sound.PlayOneShot(sound);
							
						}
						if (selectTool == 2) {
							playerNow.toolBomb [selectToolKind]++;
							Server.Send ("28"+(selectToolKind+1).ToString()+",1");
							toolExist [selectTool, selectToolKind]--;
							int toolKind=0;

							if(selectTool == 2&& selectToolKind==0){
								toolKind=5;
							}
							if(selectTool == 2&& selectToolKind==1){
								toolKind=6;
							}
							if(selectTool == 2&& selectToolKind==2){
								toolKind=7;
							}

							//Server.Send ("64"+HouseID+","+toolKind+",-1");
							print ("64"+HouseID+","+toolKind+",-1");
							m_sound.PlayOneShot(sound);
							
						}
						if (selectTool == 3) {
							if (playerNow.cart != 0) {
								toolExist [3, playerNow.cart - 1]++;
							}
							int toolkind=0;
							if(playerNow.tool==1 && playerNow.toolKind==0){
								toolkind=1;
							}
							if(playerNow.tool==1 && playerNow.toolKind==1){
								toolkind=2;
							}
							if(playerNow.tool==2 && playerNow.toolKind==0){
								toolkind=3;
							}
							if(playerNow.tool==2 && playerNow.toolKind==1){
								toolkind=4;
							}


							Server.Send ("26"+toolkind.ToString()+","+(selectToolKind + 1).ToString());


							int toolKind=0;

							if(selectTool == 3&& selectToolKind==0){
								toolKind=8;
							}
							if(selectTool == 3&& selectToolKind==1){
								toolKind=9;
							}
							//Server.Send ("64"+HouseID+","+toolKind+",-1");
							print ("64"+HouseID+","+toolKind+",-1");

							playerNow.cart = selectToolKind + 1;
							toolExist [selectTool, selectToolKind]--;
							playerNow.infomationText ("已裝備手推車");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							update=false;
							m_sound.PlayOneShot(sound);
							
						}
						
						
					}
					/// Equipe //
					/// 
					/// Take OFF //
					if (toolExist [selectTool, selectToolKind] == 0) {
						
						GUI.enabled = true;
						if ((selectTool == 0 || selectTool == 1) && (playerNow.tool != selectTool + 1 || playerNow.toolKind != selectToolKind)) {
							GUI.enabled = false;
							
						}
						if (selectTool == 2 && playerNow.toolBomb [selectToolKind] == 0) {
							GUI.enabled = false;
							
						}
						if (selectTool == 3 && playerNow.cart != selectToolKind + 1) {
							GUI.enabled = false;
							
						}
						
					} else {
						GUI.enabled = false;
						
						if (selectTool == 2 && toolExist [selectTool, selectToolKind] > 0 && playerNow.toolBomb [selectToolKind] != 0) {
							GUI.enabled = true;
							
						}
					}
					
					if (GUI.Button (new Rect (Screen.width * 3 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 10, Screen.width * 1 / 15), "卸載", guiSkin.button)) {
						
						if (selectTool == 0 || selectTool == 1) {
							if(playerNow.tool==1){
								//Server.Send ("64"+HouseID+","+(playerNow.tool+playerNow.toolKind).ToString()+",1");
							}else if(playerNow.tool==2){
								//Server.Send ("64"+HouseID+","+(playerNow.tool+playerNow.toolKind+1).ToString()+",1");
								
							}
							toolExist [playerNow.tool - 1, playerNow.toolKind]++;
							toolHpNow [playerNow.tool - 1, playerNow.toolKind] = playerNow.toolHp;
							
							playerNow.tool = 0;
							playerNow.toolKind = 0;
							playerNow.toolHp = 0;
							
							/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
							usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
							usetool.name=toolName[selectTool];*/
							Server.Send ("260,"+playerNow.cart);
							
							playerNow.infomationText ("已卸載" + toolName [selectTool] + "!");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							update=false;
							m_sound.PlayOneShot(sound);
							
							
						}
						if (selectTool == 2) {
							playerNow.toolBomb [selectToolKind]--;
							Server.Send ("28"+(selectToolKind+1).ToString()+",-1");
							//Server.Send ("64"+HouseID+","+(selectToolKind+5).ToString()+",1");
							m_sound.PlayOneShot(sound);
							
							toolExist [selectTool, selectToolKind]++;
						}
						if (selectTool == 3) {
							
							playerNow.cart = 0;
							int toolkind=0;
							if(playerNow.tool==1 && playerNow.toolKind==0){
								toolkind=1;
							}
							if(playerNow.tool==1 && playerNow.toolKind==1){
								toolkind=2;
							}
							if(playerNow.tool==2 && playerNow.toolKind==0){
								toolkind=3;
							}
							if(playerNow.tool==2 && playerNow.toolKind==1){
								toolkind=4;
							}
							
							
							Server.Send ("26"+toolkind.ToString()+",0");

							toolExist [selectTool, selectToolKind]++;
							playerNow.infomationText ("已卸載手推車");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							update=false;
							m_sound.PlayOneShot(sound);
							
						}
						
						
						
					}
					/// Take OFF //
					
					
					GUI.enabled = true;
					
					
				}
				
				GUI.enabled = true;
				
				
				
			}///end of workhouse
			if (kind == 4) {
				text+=sourceName[selectScienceSource+2]+"\r\n";
				for (int i=0; i<8; i++) {
					if (needScienceSource [selectScienceSource, i] != 0) {
						text += sourceName [i] + ":" + playerNow.source [i] + "/" + needScienceSource [selectScienceSource, i] + "\r\n";
					}
				}
				text += "所需等級：" + HouseLevel + "/" + needScienceLevel [selectScienceSource];
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), source[selectScienceSource], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), text+"\r\n攜帶量:"+playerNow.source[selectScienceSource+2], guiSkin.label);
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 10 / 16, Screen.width / 3, Screen.height / 8), "", guiSkin.label);
				
				
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 16, Screen.width / 3, Screen.height / 8), "鐵片", guiSkin.button)) {
					selectScienceSource = 0;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.height / 8), "硫磺", guiSkin.button)) {
					selectScienceSource = 1;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 6 / 16, Screen.width / 3, Screen.height / 8), "木炭", guiSkin.button)) {
					selectScienceSource = 2;
				}		
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 8 / 16, Screen.width / 3, Screen.height / 8), "火藥", guiSkin.button)) {
					selectScienceSource = 3;
				}
				
				
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					work = false;		
					LevelUp = false;		
					playerNow.click = false;
				}
				GUI.EndGroup ();
				if (playerNow.source [0] >= needScienceSource [selectScienceSource, 0] && playerNow.source [1] >= needScienceSource [selectScienceSource, 1] && playerNow.source [2] >= needScienceSource [selectScienceSource, 2] && playerNow.source [3] >= needScienceSource [selectScienceSource, 3] && playerNow.source [4] >= needScienceSource [selectScienceSource, 4] && playerNow.source [5] >= needScienceSource [selectScienceSource, 5] && playerNow.source [6] >= needScienceSource [selectScienceSource, 6] && playerNow.source [7] >= needScienceSource [selectScienceSource, 7] && HouseLevel >= needScienceLevel [selectScienceSource]) {
					
					GUI.enabled = true;
				} else {
					GUI.enabled = false;
					
				}
				if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 5, Screen.width * 1 / 15), "建造", guiSkin.button)) {
					int[] quatity=new int[8]{0,0,0,0,0,0,0,0};
					if (selectScienceSource == 0) {
						quatity[2]++;
						playerNow.source [2]++;
					}
					if (selectScienceSource == 1) {
						quatity[3]++;
						
						playerNow.source [3]++;
					}
					if (selectScienceSource == 2) {
						quatity[4]++;
						
						playerNow.source [4]++;
					}
					if (selectScienceSource == 3) {
						quatity[5]++;
						
						playerNow.source [5]++;
					}				

					for (int i=0; i<8; i++) {
						quatity[i]-=needScienceSource [selectScienceSource, i];
						playerNow.source [i] -= needScienceSource [selectScienceSource, i];
					}
					sendSourceModify(quatity);
					m_sound.PlayOneShot(sound);
					
				}
				GUI.enabled = true;
				
				
				
			}///End of ScienceHouse
			
		}
		if (LevelUp == true) {
			if (HouseLevel < houseLevelLimit [kind]) {
				string LevelText = "";
				for (int i=0; i<8; i++) {
					if (LevelUpSource [kind, HouseLevel - 1, i] != 0) {
						LevelText += "\r\n" + sourceName [i] + ":" + playerNow.source [i] + "/" + LevelUpSource [kind, HouseLevel - 1, i];
					}
				}
				
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 15, Screen.height / 12, Screen.width * 2 / 7, Screen.width * 2 / 7), LevelPng [HouseLevel], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				if (selectHouseKind == false) {
					
					GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 16, Screen.width / 3, Screen.height / 6), LevelText, guiSkin.label);
				}
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.width * 1 / 4), LevelInfo [kind, HouseLevel - 1], guiSkin.label);
				
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					LevelUp = false;
					playerNow.click = false;
				}
				
				GUI.EndGroup ();
				
				if (playerNow.source [0] >= LevelUpSource [kind, HouseLevel - 1, 0] && playerNow.source [1] >= LevelUpSource [kind, HouseLevel - 1, 1] && playerNow.source [2] >= LevelUpSource [kind, HouseLevel - 1, 2] && playerNow.source [3] >= LevelUpSource [kind, HouseLevel - 1, 3] && playerNow.source [4] >= LevelUpSource [kind, HouseLevel - 1, 4] && playerNow.source [5] >= LevelUpSource [kind, HouseLevel - 1, 5] && playerNow.source [6] >= LevelUpSource [kind, HouseLevel - 1, 6] && playerNow.source [7] >= LevelUpSource [kind, HouseLevel - 1, 7]) {
					GUI.enabled = true;
				} else {
					GUI.enabled = false;
					
				}
				
				if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 5, Screen.width * 1 / 15), "建造", guiSkin.button)) {
					LevelUp = false;
					int[] quatity=new int[8];
					for (int i =0; i<8; i++) {
						quatity[i]=-1* LevelUpSource [kind, HouseLevel - 1, i];
						playerNow.source [i] = playerNow.source [i] - LevelUpSource [kind, HouseLevel - 1, i];
					}

					spriteRenderer.sprite = LevelSpritePng [HouseLevel];
					
					playerNow.click = false;
					playerNow.infomationText ("等級提升!");
					Server.Send("56"+HouseID+","+(kind+1)+","+(HouseLevel+1));
					HouseLevel++;
						print("error20");
					//this.HP=HpMax[HouseLevel];
					m_sound.PlayOneShot(LevelUpSound);
					
					Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 5);
					Destroy (SomkeNow, 5);
					Destroy (SmokeAnimateNow, 5);
					Destroy (animateNow, 5);
					
				}
				GUI.enabled = true;
				
			} else {
				playerNow.infomationText ("已達到最高等級!");	
				LevelUp = false;
				playerNow.click = false;
			}
			
			
			
		}//end of level
		if (fixedHouse == true) {
				
			if ( HP < MaxHP) {
				string LevelText = "";
				int woodSource,stoneSource;
				woodSource=((int)((MaxHP-HP)/5));
				stoneSource=((int)((MaxHP-HP)/10));
				LevelText = "\r\n" + sourceName [0] + ":" + playerNow.source [0] + "/" + woodSource;
				LevelText += "\r\n" + sourceName [1] + ":" + playerNow.source [1] + "/" + stoneSource ;


				
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 15, Screen.height / 12, Screen.width * 2 / 7, Screen.width * 2 / 7), LevelPng [HouseLevel-1], guiSkin.box);
				if (selectHouseKind == false) {
					
					GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 16, Screen.width / 3, Screen.height / 6), LevelText, guiSkin.label);
				}
				//GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.width * 1 / 5), LevelInfo [kind, HouseLevel - 1], guiSkin.label);
				
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					fixedHouse = false;
					playerNow.click = false;
				}
				
				GUI.EndGroup ();

				if(playerNow.source[0]>=woodSource && playerNow.source[1]>=stoneSource){
					GUI.enabled = true;
				} else {
					GUI.enabled = false;
					
				}
				
				if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 5, Screen.width * 1 / 15), "修理", guiSkin.button)) {
					LevelUp = false;
					int[] quatity=new int[8]{0,0,0,0,0,0,0,0};
					playerNow.source[0]-=woodSource;
					quatity[0]-=woodSource;
					playerNow.source[1]-=stoneSource;
					quatity[1]-=stoneSource;
					sendSourceModify(quatity);
					Server.Send ("58"+HouseID+","+(MaxHP-HP));
					
					HP=MaxHP;
					
					playerNow.click = false;
					playerNow.infomationText ("修理完成!");
					Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 5);
					Destroy (SomkeNow, 5);
					Destroy (SmokeAnimateNow, 5);
					Destroy (animateNow, 5);
					
				}
				GUI.enabled = true;
				
			} else {
				playerNow.infomationText ("房屋已經修好了!");	
				fixedHouse = false;
				playerNow.click = false;
			}
		}//fixedEnd


	}
	
	
	//共同fuction End
	
	
}