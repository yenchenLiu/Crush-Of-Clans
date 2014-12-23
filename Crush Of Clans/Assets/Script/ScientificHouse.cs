﻿using UnityEngine;
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
		private int[,] LevelUpSource = {{10,10,0},{20,20,0},{30,30,20},{100,100,40}};
		public int HouseLevel=1;
	
		public int[,] needSource = {{2,1}};
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
			string text;
			if (work == true) {
				GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
				vSliderValue = GUI.VerticalSlider(new Rect(7*Screen.width/8, Screen.height/3, Screen.width/16, Screen.height*2/5), vSliderValue, Screen.height+Screen.height/4, Screen.height * 1 / 4);
				if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(0*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Metal")){
					selectSource=0;
					
				}
				/*if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(1*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool2")){
					selectSource=1;
					
				}
				if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(2*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool3")){
					selectSource=2;
					
				}*/
				text="Wood:"+needSource[selectSource,0]+"\r\nStone:"+needSource[selectSource,1];
				if(playerNow.source[0]>=needSource[selectSource,0]&&playerNow.source[1]>=needSource[selectSource,1]){
					if(GUI.Button(new Rect(Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),"OK")){
						playerNow.source[selectSource+2]++;					

						for(int i=0;i<=1;i++){
							playerNow.source[i]-=needSource[selectSource,i];
						}
						
					//playerNow.weightNow = (playerNow.source [0] * playerNow.weight [0])+(playerNow.source [1] * playerNow.weight [1]) + (playerNow.source [2] * playerNow.weight [2]);
					//資料庫
					/*
						UPDATE Player
						Key:PlayerID
						欄位：
						source[0]=playerNow.source[0](wood)
						source[1]=playerNow.source[1](stone)
						source[2]=playerNow.source[2](metal)
						
					*/
					}
				}
				
				GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
				if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
					work=false;		
					playerNow.click=false;
				}
				if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 2/ 8 , Screen.width/10, Screen.height/8 ),"LevelUp")){
					LevelUp=true;
					work=false;
				}
				

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