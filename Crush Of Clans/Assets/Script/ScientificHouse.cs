using UnityEngine;
using System.Collections;

public class ScientificHouse : MonoBehaviour {
		//資料庫
		/*
		ScienceHouse的欄位：
		PlayerID 玩家帳號
		HouseID(看需不需要)
		x(x座標)
		z(z座標)
		ScienceHouseLevel=1;
		ScienceHouseHP=100;(暫定耐久)

		*/
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
		public GUISkin guiSkin;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;
		
		public Texture[] LevelPng ;
		public Sprite[] LevelSpritePng;
		private SpriteRenderer spriteRenderer;// 
		private string[] LevelInfo = {"可合成資源：木炭","","可合成資源：硫磺","可合成資源：火藥"};
	
		private int[,] LevelUpSource = {{100,100,10,0,0,0,0,0},{200,200,50,0,0,0,0,0},{300,300,100,0,0,0,0,0},{500,400,200,0,0,0,0,0}};
		public int HouseLevel=1;
		//private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭"};
		private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
		private int[] needLevel = {1,2,4,5};
		private int[,] needSource = {{2,0,0,0,0,0,1,0},{4,0,0,0,0,0,0,1},{1,0,0,0,0,0,0,0},{0,0,0,5,10,0,0,0}};
		public bool work,LevelUp;
		public int HP=100,Lelel=1;
		public string PlayerID;
	
		private float vSliderValue;
		private GameObject player;
		private int selectSource;
		private Player playerNow;


		// Use this for initialization
		void Start () {
		 	spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
			work = false;
			LevelUp = false;
			selectSource = 0;
			vSliderValue =Screen.height * 1 / 4;
			bombCount = 0;
			bombSet = null;
		}
		
		// Update is called once per frame
		void Update () {
			if (LevelSpritePng.Length != 0) {
			spriteRenderer.sprite = LevelSpritePng[HouseLevel-1];
					
			}
		}
		void OnGUI () {
			string text="";
			if (work == true) {
			for(int i=0;i<8;i++){
				if(needSource[selectSource,i]!=0){
					text+=sourceName[i]+":"+playerNow.source[i]+"/"+needSource[selectSource,i]+"\r\n";
				}
			}
			text+="所需等級："+HouseLevel+"/"+needLevel[selectSource];
			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5), "", guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), text, guiSkin.label);
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
			

			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"鐵片",guiSkin.button)){
					selectSource=0;
				}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"硫磺",guiSkin.button)){
					selectSource=1;
				}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 16 , Screen.width/3, Screen.height/8 ),"木炭",guiSkin.button)){
					selectSource=2;
				}		
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 8/ 16 , Screen.width/3, Screen.height/8 ),"火藥",guiSkin.button)){
					selectSource=3;
				}
		
			
			
			
			
			
			
			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				LevelUp=false;		
				playerNow.click=false;
			}
			GUI.EndGroup();
			if(playerNow.source[0]>=needSource[selectSource,0] && playerNow.source[1]>=needSource[selectSource,1] && playerNow.source[2]>=needSource[selectSource,2]&& playerNow.source[3]>=needSource[selectSource,3]&& playerNow.source[4]>=needSource[selectSource,4]&&  playerNow.source[5]>=needSource[selectSource,5]&& playerNow.source[6]>=needSource[selectSource,6]&& playerNow.source[7]>=needSource[selectSource,7] && HouseLevel>=needLevel[selectSource]){
			
				GUI.enabled=true;
			}else{
				GUI.enabled=false;
				
			}
			if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"建造",guiSkin.button)){
				if (selectSource==0){
					playerNow.source[2]++;
				}
				if (selectSource==1){
					playerNow.source[3]++;
				}
				if (selectSource==2){
					playerNow.source[4]++;
				}
				if (selectSource==3){
					playerNow.source[5]++;
				}				
				
				for(int i=0;i<8;i++){
					playerNow.source[i]-=needSource[selectSource,i];
				}
			}
			GUI.enabled=true;
				/*GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
				vSliderValue = GUI.VerticalSlider(new Rect(7*Screen.width/8, Screen.height/3, Screen.width/16, Screen.height*2/5), vSliderValue, Screen.height+Screen.height/4, Screen.height * 1 / 4);
				if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(0*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Metal")){
					selectSource=0;
					
				}*/
				/*if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(1*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool2")){
					selectSource=1;
					
				}
				if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(2*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool3")){
					selectSource=2;
					
				}*/

			/*	if(playerNow.source[0]>=needSource[selectSource,0]&&playerNow.source[1]>=needSource[selectSource,1]){
					if(GUI.Button(new Rect(Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),"OK")){
						playerNow.source[selectSource+2]++;					

						for(int i=0;i<=1;i++){
							playerNow.source[i]-=needSource[selectSource,i];
						}
						
					//playerNow.weightNow = (playerNow.source [0] * playerNow.weight [0])+(playerNow.source [1] * playerNow.weight [1]) + (playerNow.source [2] * playerNow.weight [2]);
					//資料庫

						UPDATE Player
						Key:PlayerID
						欄位：
						source[0]=playerNow.source[0](wood)
						source[1]=playerNow.source[1](stone)
						source[2]=playerNow.source[2](metal)
						

					}
				}*/
				
				/*GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
				if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
					work=false;		
					playerNow.click=false;
				}
				if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 2/ 8 , Screen.width/10, Screen.height/8 ),"LevelUp")){
					LevelUp=true;
					work=false;
				}
				*/

		}
			
		if(LevelUp==true){
			if(HouseLevel<5){
				string LevelText="";
				for(int i=0;i<8;i++){
					if(LevelUpSource[HouseLevel-1,i]!=0){
						LevelText+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+LevelUpSource[HouseLevel-1,i];
					}
				}
				
				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width/15,Screen.height/12, Screen.width*2/7, Screen.width*2/7), LevelPng[HouseLevel], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), LevelText, guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8,Screen.height* 4/ 16, Screen.width/3, Screen.width*1/5),LevelInfo[HouseLevel-1], guiSkin.label);
				
				
				if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
					LevelUp=false;
					playerNow.click=false;
				}
				
				GUI.EndGroup();
				
				if(playerNow.source[0]>=LevelUpSource[HouseLevel-1,0] && playerNow.source[1]>=LevelUpSource[HouseLevel-1,1] && playerNow.source[2]>=LevelUpSource[HouseLevel-1,2]&& playerNow.source[3]>=LevelUpSource[HouseLevel-1,3]&& playerNow.source[4]>=LevelUpSource[HouseLevel-1,4]&& playerNow.source[5]>=LevelUpSource[HouseLevel-1,5]&&playerNow.source[6]>=LevelUpSource[HouseLevel-1,6]&& playerNow.source[7]>=LevelUpSource[HouseLevel-1,7]){
					GUI.enabled=true;
				}else{
					GUI.enabled=false;
					
				}
				
				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"建造",guiSkin.button)){
					//SpriteRenderer spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
					//spriteRenderer.sprite=LevelSprite[HouseLevel];
					for(int i =0;i<8;i++){
						
						playerNow.source[i]=playerNow.source[i]-LevelUpSource[HouseLevel-1,i];
					}
					spriteRenderer.sprite = LevelSpritePng[HouseLevel];
					
					LevelUp=false;
					HouseLevel++;
					playerNow.click=false;
					playerNow.infomationText("精煉屋等級提升!");

					Vector3 Pos=new Vector3(this.gameObject.transform.position.x-1,this.gameObject.transform.position.y+5,this.gameObject.transform.position.z+1);
					GameObject animateNow=(GameObject) Instantiate(Building,Pos,Building.transform.rotation);
					Pos=new Vector3(this.transform.position.x,this.transform.position.y+4,this.transform.position.z);
					
					GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
					Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.1f,this.transform.position.z);
					
					GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
					
					Destroy(animateNow,3);
					Destroy(SomkeNow,3);
					Destroy(SmokeAnimateNow,3);
					Destroy(animateNow,3);

				}
				GUI.enabled=true;
				
			}else{
				playerNow.infomationText("以達到最高等級!");
				
				
			}
			
			
			
		}
	}
		void OnTriggerStay(Collider other){
			if (other.tag == "Player") {
				player=other.gameObject;
				playerNow=player.GetComponent<Player>();
			}
			
		}
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
			if(State.bombTotal<0) State.bombTotal=0;
			
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
}
