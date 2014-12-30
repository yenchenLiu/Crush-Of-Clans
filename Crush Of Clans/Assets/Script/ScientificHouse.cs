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
		public GUISkin guiSkin;
	
		private int[,] LevelUpSource = {{10,10,0},{10,20,0},{30,30,20},{100,100,40}};
		public int HouseLevel=1;
		//private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭"};
		private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
		
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


			work = false;
			LevelUp = false;
			selectSource = 0;
			vSliderValue =Screen.height * 1 / 4;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		void OnGUI () {
			string text="";
			if (work == true) {
			for(int i=0;i<8;i++){
				if(needSource[selectSource,i]!=0){
					text+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+needSource[selectSource,i];
				}
			}
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
			if(playerNow.source[0]>=needSource[selectSource,0] && playerNow.source[1]>=needSource[selectSource,1] && playerNow.source[2]>=needSource[selectSource,2]&& playerNow.source[3]>=needSource[selectSource,3]&& playerNow.source[4]>=needSource[selectSource,4]&&  playerNow.source[5]>=needSource[selectSource,5]&& playerNow.source[6]>=needSource[selectSource,6]&& playerNow.source[7]>=needSource[selectSource,7]){
			
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
			GUI.Box (new Rect ( Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2),"LevelUp");
			string LevelUpText="Wood:"+LevelUpSource[HouseLevel-1,0]+"\r\nStone:"+LevelUpSource[HouseLevel-1,1]+"\r\nMetal:"+LevelUpSource[HouseLevel-1,2];
			GUI.TextArea(new Rect (Screen.width*2/6, Screen.height*2/ 6 , Screen.width/3, Screen.height/3 ),LevelUpText);
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 5/ 6 , Screen.width/4, Screen.height/8 ),"LevelUp")){
				if(playerNow.source[0]>=LevelUpSource[HouseLevel-1,0]&&playerNow.source[1]>=LevelUpSource[HouseLevel-1,1]&&playerNow.source[2]>=LevelUpSource[HouseLevel-1,2]){
					HouseLevel++;
					work=false;
					LevelUp=false;
					playerNow.click=false;
					playerNow.infomationText("Science House Level Up!");
					
				}
			}
			if (GUI.Button (new Rect (Screen.width*3/4-Screen.width/10, Screen.height* 1/ 4 , Screen.width/10, Screen.height/10 ),"X")){
				LevelUp=false;
				work=true;
			}
			
		}
		}
		void OnTriggerStay(Collider other){
			if (other.tag == "Player") {
				player=other.gameObject;
				playerNow=player.GetComponent<Player>();
			}
			
		}
	}
