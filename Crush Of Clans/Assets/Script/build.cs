using UnityEngine;
using System.Collections;

public class build : MonoBehaviour {
	
	//共同變數
	public GUISkin guiSkin;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;
	private GameObject player;
	private Player playerNow;
	
	
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	private SpriteRenderer spriteRenderer;
	
	public int kind;//建築類型
	public int HouseLevel=1;
	public bool work,LevelUp,loading;
	public int HP=100;
	public string PlayerID;
	public string HouseID;
	public string HouseKind;
	
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	public string[] HouseName = {"工地","住宅","倉庫","工作屋","精煉屋","世界奇觀"};
	private int[] houseLevelLimit={1,3,5,5,5,1};
	
	
	
	//共同變數end
	///
	//相似變數
	private string[,] LevelInfo = {
		{"","","",""},//工地
		{"新增功能：建築方向指標","新增功能：瞬間移動","",""},//住宅
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
	private string[] HouseInfo={"","建築地標，提供外出時的方向指引，等級提升後會有瞬間移動的功能可使用。","儲存資源，依據倉庫等級、能存放的數量及種類會有所變化。","製作道具，依據工作屋等級、能製作的工具種類會有所變化。","合成資源，依據精煉屋等級、能合成的資源種類會有所變化。","世界奇觀，能在一定時間後，能將除了自己以外所有建築夷平。"};
	public int selectHouse;
	public Texture[] housePng; 
	
	///Construction End
	
	///Home
	
	///Home End
	////Stock
	private int[] stockNeedLevel = {1,1,3,4,3,5,2,2};
	public int[] stockSource = {20,20,20,0,0,0,0,0};
	public int[] stocklimit = {5000,10000,20000,30000,40000};
	public bool put,get;
	public string Quatity;
	private int selectSource,playerSource;
	private int limit;
	
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
	IEnumerator server_function(){
		while (true) {
			yield return new WaitForSeconds(5);	
			
			if (HouseID != "" && HouseID != null) {
				State.HouseUpdate[HouseID]--;
				if(!State.HouseID.ContainsKey(HouseID)){
					if(kind==2){
						//DestorySource
					}
					if(kind==3){
						//DestoryTool
					}
					Destroy (this.gameObject);
					break;
				}
				kind = State.HouseKind [HouseID];
				//HouseLevel = State.HouseLevel [HouseID];
				HP=State.HouseHP [HouseID];
				PlayerID = State.HousePlayerID [HouseID];
				this.name = HouseID;
				
				if (LevelSpritePng.Length != 0 ) {
					spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
					
					spriteRenderer.sprite = LevelSpritePng[HouseLevel-1];
					
				}
				HouseKind=HouseName[kind];
			}
			yield return new WaitForSeconds(5);
		}
		
		
	}
	void Start () {
		print ("OK!");
		////Server////
	
		//kind = State.HouseKind [HouseKind];

		//this.transform.position = new Vector3 (State.HousePositionX [HouseID], 0, State.HousePositionY [HouseID]);
		
		/// Server///
		///共同
		StartCoroutine("server_function");

		work = false;
		LevelUp = false;
		bombCount = 0;
		bombSet = null;
		///共同
		////
		//獨有
		
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
		
		//workHouse End
		///scienceHouse
		selectSource = 0;
		///scienceHouse End
		
		
		//獨有
		
	}///End of Start
	
	void Update () {

	}///End of Update
	
	
	
	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			player=other.gameObject;
			playerNow=player.GetComponent<Player>();
		}
		
	}//End of OnTriggerStay
	
	//炸彈碰撞///
	IEnumerator bomb_function(){
		
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
		
		
		this.HP -= demage;
		if (this.HP <= 0) {
			bombCount = 0;
			State.bombTotal--;
			if(State.bombTotal<0) State.bombTotal=0;
			
			print( (State.bombTotal).ToString());
			Destroy (this.gameObject);		
		} else {
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
			
			bombSet=other.gameObject;
			bomb=bombSet.GetComponent<Bomb>();	
			
			demage=bomb.power;
			StartCoroutine("bomb_function");
			
		}
	}
	//炸彈碰撞///End
	
	void OnGUI () {

	

		string text;
		text = "";
		//獨有//////
		///Construction
		if (work == true) {
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
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 12 / 20, Screen.width / 3, Screen.height / 8), HouseInfo [selectHouse], guiSkin.label);
				
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 2 / 20, Screen.width / 3, Screen.height / 10), "住宅", guiSkin.button)) {
					selectHouse = 1;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 20, Screen.width / 3, Screen.height / 10), "倉庫", guiSkin.button)) {
					selectHouse = 2;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 6 / 20, Screen.width / 3, Screen.height / 10), "工作屋", guiSkin.button)) {
					selectHouse = 3;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 8 / 20, Screen.width / 3, Screen.height / 10), "精煉屋", guiSkin.button)) {
					selectHouse = 4;
				}
				if (GUI.Button (new Rect (Screen.width * 3 / 8, Screen.height * 10 / 20, Screen.width / 3, Screen.height / 10), "世界奇觀", guiSkin.button)) {
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
				
				if (GUI.Button (new Rect (Screen.width * 2 / 10, Screen.height * 4 / 10 + Screen.width * 1 / 5, Screen.width * 1 / 5, Screen.width * 1 / 15), "建造", guiSkin.button)) {
					switch (selectHouse) {
					case 1:
						if (!playerNow.isHomeExsist) {
							buildNow = (Rigidbody)Instantiate (build_house, this.transform.position, build_house.transform.rotation);
							buildNow.gameObject.collider.enabled = true;
							build thisHomeBuild = buildNow.GetComponent<build> ();
							thisHomeBuild.PlayerID = playerNow.PlayerID;
							thisHomeBuild.HP = 100;
							for (int i =0; i<8; i++) {
								playerNow.source [i] = playerNow.source [i] - needHouseSource [3, i];
							}
							
							playerNow.infomationText ("Home is Build");
							playerNow.click = false;
							Destroy (this.gameObject);
							playerNow.isHomeExsist = true;
							
							work = false;
							
						}
						break;
					case 2:
						buildNow = (Rigidbody)Instantiate (build_stock, this.transform.position, build_stock.transform.rotation);
						buildNow.gameObject.collider.enabled = true;
						
						build thisStockBuild = buildNow.GetComponent<build> ();
						thisStockBuild.PlayerID = playerNow.PlayerID;
						thisStockBuild.HP = 100;
						for (int i =0; i<8; i++) {
							thisStockBuild.stockSource [i] = 0;
							
							playerNow.source [i] = playerNow.source [i] - needHouseSource [0, i];
						}
						playerNow.infomationText ("Stock House is Build");
						playerNow.click = false;
						Destroy (this.gameObject);
						work = false;
						break;
					case 3:
						buildNow = (Rigidbody)Instantiate (build_work, this.transform.position, build_work.transform.rotation);
						buildNow.gameObject.collider.enabled = true;
						
						build thisWorkBuild = buildNow.GetComponent<build> ();
						thisWorkBuild.PlayerID = playerNow.PlayerID;
						thisWorkBuild.HP = 100;
						for (int i =0; i<8; i++) {
							playerNow.source [i] = playerNow.source [i] - needHouseSource [1, i];
						}
						playerNow.infomationText ("Work House is Build");
						playerNow.click = false;
						Destroy (this.gameObject);
						work = false;
						break;
					case 4:
						buildNow = (Rigidbody)Instantiate (build_science, this.transform.position, build_science.transform.rotation);
						buildNow.gameObject.collider.enabled = true;
						build thisScienceBuild = buildNow.GetComponent<build> ();
						thisScienceBuild.PlayerID = playerNow.PlayerID;
						thisScienceBuild.HP = 100;
						for (int i =0; i<8; i++) {
							playerNow.source [i] = playerNow.source [i] - needHouseSource [2, i];
						}
						
						playerNow.infomationText ("Science House is Build");
						playerNow.click = false;
						Destroy (this.gameObject);
						work = false;
						break;
						
					case 5:
						if (!playerNow.isHomeExsist) {
							buildNow = (Rigidbody)Instantiate (build_house, this.transform.position, build_house.transform.rotation);
							buildNow.gameObject.collider.enabled = true;
							build thisHomeBuild = buildNow.GetComponent<build> ();
							thisHomeBuild.PlayerID = playerNow.PlayerID;
							thisHomeBuild.HP = 100;
							for (int i =0; i<8; i++) {
								playerNow.source [i] = playerNow.source [i] - needHouseSource [3, i];
							}
							
							playerNow.infomationText ("Home is Build");
							playerNow.click = false;
							Destroy (this.gameObject);
							playerNow.isHomeExsist = true;
							
							work = false;
							
						}
						break;
					}
					
					Vector3 Pos = new Vector3 (buildNow.gameObject.transform.position.x - 1, buildNow.gameObject.transform.position.y + 5, buildNow.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 3);
					Destroy (SomkeNow, 3);
					Destroy (SmokeAnimateNow, 3);
					
				}///End of Building
				GUI.enabled = true;
				
				
				
				
				
				
			}
			
			///End of Consturction
			///stockHouse 
			
			if (kind == 2) {
				
				
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), "", guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10 + Screen.width * 1 / 20, Screen.height * 9 / 20, Screen.width * 1 / 10, Screen.width * 1 / 5), "", guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20 + Screen.width * 1 / 10, Screen.width * 1 / 5, Screen.width * 1 / 5), "", guiSkin.label);
				//GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
				if (put == true || get == true)
					GUI.enabled = false;
				else
					GUI.enabled = true;
				for (int i=0; i<7; i+=2) {
					int j=i/2;
					GUI.Label (new Rect (Screen.width * 6 / 16, Screen.height * (3 + (4 * j) )/ 26, Screen.width / 8, Screen.height / 16), stockSource[i].ToString (), guiSkin.customStyles [4]);
					
					if (GUI.Button (new Rect (Screen.width * 5 / 16, Screen.height * (1 + (2 * j)) / 13, Screen.width / 12, Screen.height / 8), sourceName [i], guiSkin.customStyles [3])) {
						selectSource = i;
					}
					GUI.Label (new Rect (Screen.width * 9 / 16, Screen.height * (3 + (4 * j)) / 26, Screen.width / 8, Screen.height / 16), stockSource[i + 1].ToString (), guiSkin.customStyles [4]);
					
					if (GUI.Button (new Rect (Screen.width * 8 / 16, Screen.height * (1 + (2 * j)) / 13, Screen.width / 12, Screen.height / 8), sourceName [i + 1], guiSkin.customStyles [3])) {
						selectSource = i + 1;
						
					}
				}
				
				
				if (GUI.Button (new Rect (Screen.width * 11 / 15, 0, Screen.width / 15, Screen.width / 15), "X", guiSkin.button)) {
					work = false;		
					playerNow.click = false;
					LevelUp = false;
				}
				GUI.EndGroup ();
				
				playerSource = playerNow.source [selectSource];
				limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
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
					GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "", guiSkin.box);
					Quatity = GUI.TextField (new Rect (Screen.width * 9 / 20, Screen.height / 2 - Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 15), Quatity, guiSkin.textField);
					if (GUI.Button (new Rect (Screen.width * 7 / 20, Screen.height / 2 - Screen.height * 1 / 15, Screen.width * 1 / 20, Screen.height * 1 / 15), "<<", guiSkin.button)) {
						Quatity = "0";
					}
					if (GUI.Button (new Rect (Screen.width * 8 / 20, Screen.height / 2 - Screen.height * 1 / 15, Screen.width * 1 / 20, Screen.height * 1 / 15), "<", guiSkin.button)) {
						if (int.Parse (Quatity) > 0) {
							Quatity = ((int.Parse (Quatity)) - 1).ToString ();
						}
						
					}
					if (GUI.Button (new Rect (Screen.width * 12 / 20, Screen.height / 2 - Screen.height * 1 / 15, Screen.width * 1 / 20, Screen.height * 1 / 15), ">>", guiSkin.button)) {
						if (put == true) {
							
							Quatity = playerNow.source [selectSource].ToString ();
						}
						if (get == true) {
							Quatity = stockSource [selectSource].ToString ();
							
						}
						
					}
					if (GUI.Button (new Rect (Screen.width * 11 / 20, Screen.height / 2 - Screen.height * 1 / 15, Screen.width * 1 / 20, Screen.height * 1 / 15), ">", guiSkin.button)) {
						if (put == true) {
							
							limit = playerNow.source [selectSource];
						}
						if (get == true) {
							limit = stockSource [selectSource];
							
						}
						if (int.Parse (Quatity) < limit) {
							Quatity = ((int.Parse (Quatity)) + 1).ToString ();
						}
					}
					if (GUI.Button (new Rect (Screen.width * 8 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 15), "確定", guiSkin.button)) {
						if (get == true) {
							playerNow.source [selectSource] = playerNow.source [selectSource] + int.Parse (Quatity);
							stockSource [selectSource] = stockSource [selectSource] - int.Parse (Quatity);
						}
						if (put == true) {
							playerNow.source [selectSource] = playerNow.source [selectSource] - int.Parse (Quatity);
							stockSource [selectSource] = stockSource [selectSource] + int.Parse (Quatity);
							
						}
						Quatity = "0";
						get = false;
						put = false;
					}
					
					if (GUI.Button (new Rect (Screen.width * 10 / 20, Screen.height / 2 + Screen.height * 1 / 15, Screen.width * 1 / 10, Screen.height * 1 / 15), "取消", guiSkin.button)) {
						Quatity = "0";
						get = false;
						put = false;
					}
					
				}
				///end of stockHouse 
			}
			if (kind == 3) {//工作屋////
				
				
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
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), "", guiSkin.box);
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
						for (int i=0; i<8; i++) {
							playerNow.source [i] -= toolNeedSource [selectTool, selectToolKind, i];
							
						}
						
						
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
							playerNow.toolHp = toolHpNow [selectTool, selectToolKind];
							toolExist [selectTool, selectToolKind]--;
							
							/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
							usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
							usetool.name=toolName[selectTool];*/
							playerNow.infomationText ("已裝備" + toolName [selectTool] + "!");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							
						}
						if (selectTool == 2) {
							playerNow.toolBomb [selectToolKind]++;
							toolExist [selectTool, selectToolKind]--;
						}
						if (selectTool == 3) {
							if (playerNow.cart != 0) {
								toolExist [3, playerNow.cart - 1]++;
							}
							playerNow.cart = selectToolKind + 1;
							toolExist [selectTool, selectToolKind]--;
							playerNow.infomationText ("已裝備手推車");
							
							
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
							
							toolExist [playerNow.tool - 1, playerNow.toolKind]++;
							toolHpNow [playerNow.tool - 1, playerNow.toolKind] = playerNow.toolHp;
							
							playerNow.tool = 0;
							playerNow.toolKind = 0;
							playerNow.toolHp = 0;
							
							/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
							usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
							usetool.name=toolName[selectTool];*/
							playerNow.infomationText ("已卸載" + toolName [selectTool] + "!");
							work = false;		
							LevelUp = false;		
							playerNow.click = false;
							selectHouseKind = false;
							
						}
						if (selectTool == 2) {
							playerNow.toolBomb [selectToolKind]--;
							toolExist [selectTool, selectToolKind]++;
						}
						if (selectTool == 3) {
							
							playerNow.cart = 0;
							toolExist [selectTool, selectToolKind]++;
							playerNow.infomationText ("已卸載手推車");
							
							
						}
						
						
						
					}
					/// Take OFF //
					
					
					GUI.enabled = true;
					
					
				}
				
				GUI.enabled = true;
				
				
				
			}///end of workhouse
			if (kind == 4) {
				for (int i=0; i<8; i++) {
					if (needScienceSource [selectScienceSource, i] != 0) {
						text += sourceName [i] + ":" + playerNow.source [i] + "/" + needScienceSource [selectScienceSource, i] + "\r\n";
					}
				}
				text += "所需等級：" + HouseLevel + "/" + needScienceLevel [selectSource];
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 4 / 5));
				GUI.Box (new Rect (0, 0, Screen.width * 4 / 5, Screen.height * 4 / 5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 16, Screen.width * 1 / 5, Screen.width * 1 / 5), "", guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width / 10, Screen.height * 9 / 20, Screen.width * 1 / 5, Screen.width * 1 / 5), text, guiSkin.label);
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 16, Screen.width / 3, Screen.height / 8), "", guiSkin.label);
				
				
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
					if (selectScienceSource == 0) {
						playerNow.source [2]++;
					}
					if (selectScienceSource == 1) {
						playerNow.source [3]++;
					}
					if (selectScienceSource == 2) {
						playerNow.source [4]++;
					}
					if (selectScienceSource == 3) {
						playerNow.source [5]++;
					}				
					
					for (int i=0; i<8; i++) {
						playerNow.source [i] -= needScienceSource [selectScienceSource, i];
					}
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
					
					GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 9 / 16, Screen.width / 3, Screen.height / 8), LevelText, guiSkin.label);
				}
				GUI.Label (new Rect (Screen.width * 3 / 8, Screen.height * 4 / 16, Screen.width / 3, Screen.width * 1 / 5), LevelInfo [kind, HouseLevel - 1], guiSkin.label);
				
				
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
					for (int i =0; i<8; i++) {
						
						playerNow.source [i] = playerNow.source [i] - LevelUpSource [kind, HouseLevel - 1, i];
					}
					spriteRenderer.sprite = LevelSpritePng [HouseLevel];
					
					HouseLevel++;
					playerNow.click = false;
					playerNow.infomationText ("工作屋等級提升!");
					Vector3 Pos = new Vector3 (this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y + 5, this.gameObject.transform.position.z + 1);
					GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);
					
					GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
					Pos = new Vector3 (this.transform.position.x, this.transform.position.y + 4.1f, this.transform.position.z);
					
					GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
					
					Destroy (animateNow, 3);
					Destroy (SomkeNow, 3);
					Destroy (SmokeAnimateNow, 3);
					Destroy (animateNow, 3);
					
				}
				GUI.enabled = true;
				
			} else {
				playerNow.infomationText ("已達到最高等級!");	
				LevelUp = false;
				playerNow.click = false;
			}
			
			
			
		}//end of level
		
	}
	
	
	//共同fuction End
	
	
}