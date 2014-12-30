using UnityEngine;
using System.Collections;

public class StockHouse : MonoBehaviour {
	//資料庫
	/*
	StockHouse的欄位：
	PlayerID 玩家帳號
	HouseID(看需不需要)
	x(x座標)
	z(z座標)
	StockHouseLevel=1;
	StockHouseHP=100;(暫定耐久)
	StockSouce[0]=0(wood)
	StockSouce[1]=0(stone)
	StockSouce[2]=0(metal)
	...
	*/
	public GUISkin guiSkin;
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	public int HouseLevel=1;
	private int[,] LevelUpSource = {{10,10,0},{20,20,0},{30,30,20},{100,100,40}};
	
	public int[] stockSource = {20,20,20,0,0,0,0,0};
	public int[] stocklimit = {100,10,2,0,0,0,0,0};
	public bool work,LevelUp,put,get;
	public int HP=100,Lelel=1;
	public string PlayerID,Quatity;
	
	private int vSliderValue,limit;
	private float hSliderValue;
	private GameObject player;
	private int selectSource,playerSource;
	private Player playerNow;
	// Use this for initialization
	void Start () {
		put = false;
		get=false;
		work = false;
		LevelUp = false;
		hSliderValue = Screen.width / 8;
		selectSource = 0;
		Quatity = "0";
	}
	
	// Update is called once per frame
	void Update () {
		if (work == true) {



		//	playerNow.source [selectSource] = vSliderValue;
			//playerNow.weightNow+=(playerNow.source[selectSource]-playerSource)*playerNow.weight[selectSource];
			//stockSource[selectSource]=stockSource[selectSource]+(playerSource-playerNow.source[selectSource]);
			//資料庫
				/*
				UPDATE Player
				Key:PlayerID
				欄位：
				source[0]=playerNow.source[0](wood)
				source[1]=playerNow.source[1](stone)
				source[2]=playerNow.source[2](metal)

				UPDATE StockHouse
				Key:PlayerID,x,z(,HouseID)
				欄位：			
				StockSouce[0]=stockSource[0](wood)
				StockSouce[1]=stockSource[1](stone)
				StockSouce[2]=stockSource[2](metal)
				

			*/
		}
	}
	void OnGUI () {
		//style.fontSize = 30;
		if (work == true) {

			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5),"", guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10+Screen.width*1/20,Screen.height*9/20, Screen.width*1/10, Screen.width*1/5), "", guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*9/20+Screen.width*1/10, Screen.width*1/5, Screen.width*1/5),"", guiSkin.label);
			//GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
			if(put==true || get==true) GUI.enabled=false;
			else GUI.enabled=true;
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 1/ 13 , Screen.width/3, Screen.height/16 ),sourceName[0]+":"+ stockSource[0].ToString(),guiSkin.button)){
				selectSource=0;
				

			}
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 13 , Screen.width/3, Screen.height/16),sourceName[1]+":"+stockSource[1].ToString(),guiSkin.button)){
				selectSource=1;
				
			}
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 3/ 13 , Screen.width/3, Screen.height/16 ),sourceName[2]+":"+stockSource[2].ToString(),guiSkin.button) ){
				selectSource=2;
				
			}
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 13 , Screen.width/3, Screen.height/16 ),sourceName[3]+":"+stockSource[3].ToString(),guiSkin.button)){
				selectSource=3;
				
			}
			
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 5/ 13 , Screen.width/3, Screen.height/16 ),sourceName[4]+":"+stockSource[4].ToString(),guiSkin.button)){
				selectSource=4;
				
			}
			
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 13 , Screen.width/3, Screen.height/16),sourceName[5]+":"+stockSource[5].ToString(),guiSkin.button)){
				selectSource=5;
				
			}
			
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 7/ 13, Screen.width/3, Screen.height/16 ),sourceName[6]+":"+stockSource[6].ToString(),guiSkin.button)){
				selectSource=6;
				
			}
			
			if(GUI.Button (new Rect (Screen.width*3/8, Screen.height* 8/ 13 , Screen.width/3, Screen.height/16 ),sourceName[7]+":"+stockSource[7].ToString(),guiSkin.button)){
				selectSource=7;
				
			}
			
			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				playerNow.click=false;
			}
			GUI.EndGroup();
			//vSliderValue=(playerNow.source[selectSource])>0?playerNow.source[selectSource]:0;
			playerSource=playerNow.source [selectSource];
			limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
			
			if (GUI.Button (new Rect (Screen.width*3/20,Screen.height*4/10+Screen.width*1/5, Screen.width*1/8, Screen.width*1/15 ),"取出",guiSkin.button)){
				get=true;
			}
			if (GUI.Button (new Rect (Screen.width*6/20,Screen.height*4/10+Screen.width*1/5, Screen.width*1/8, Screen.width*1/15 ),"放入",guiSkin.button)){
				put=true;

			}
			GUI.enabled=true;
			if(get==true||put==true){
				GUI.Box (new Rect (Screen.width / 2 - (Screen.width / 6), Screen.height / 2 - (Screen.height / 6), Screen.width / 3, Screen.height / 3), "",guiSkin.box);
				Quatity=GUI.TextField( new Rect (Screen.width*9/20, Screen.height/2-Screen.height*1/15, Screen.width*1/10, Screen.height*1/15),Quatity, guiSkin.textField);
				if (GUI.Button (new Rect (Screen.width*7/20, Screen.height/2-Screen.height*1/15, Screen.width*1/20, Screen.height*1/15),"<<",guiSkin.button)){
					Quatity="0";
				}
				if (GUI.Button (new Rect (Screen.width*8/20, Screen.height/2-Screen.height*1/15, Screen.width*1/20, Screen.height*1/15),"<",guiSkin.button)){
					if(int.Parse(Quatity)>0){
						Quatity=(( int.Parse(Quatity))-1).ToString();
					}

				}
				if (GUI.Button (new Rect (Screen.width*12/20, Screen.height/2-Screen.height*1/15, Screen.width*1/20, Screen.height*1/15),">>",guiSkin.button)){
					if(put==true){
						
						Quatity=playerNow.source[selectSource].ToString();
					}
					if(get==true){
						Quatity=stockSource[selectSource].ToString();
						
					}
			
				}
				if (GUI.Button (new Rect (Screen.width*11/20, Screen.height/2-Screen.height*1/15, Screen.width*1/20, Screen.height*1/15),">",guiSkin.button)){
					if(put==true){

						limit=playerNow.source[selectSource];
					}
					if(get==true){
						limit=stockSource[selectSource];
						
					}
					if(int.Parse(Quatity)<limit){
					Quatity=(( int.Parse(Quatity))+1).ToString();
					}
				}
				if (GUI.Button (new Rect (Screen.width*8/20, Screen.height/2+Screen.height*1/15, Screen.width*1/10, Screen.height*1/15),"確定",guiSkin.button)){
					if(get==true){
						playerNow.source[selectSource]=playerNow.source[selectSource]+int.Parse(Quatity);
						stockSource[selectSource]=stockSource[selectSource]-int.Parse(Quatity);
					}
					if(put==true){
						playerNow.source[selectSource]=playerNow.source[selectSource]-int.Parse(Quatity);
						stockSource[selectSource]=stockSource[selectSource]+int.Parse(Quatity);
						
					}
					Quatity="0";
					get=false;
					put=false;
				}

				if (GUI.Button (new Rect (Screen.width*10/20, Screen.height/2+Screen.height*1/15, Screen.width*1/10, Screen.height*1/15),"取消",guiSkin.button)){
					Quatity="0";
					get=false;
					put=false;
				}
			}
			/*if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 2/ 8 , Screen.width/10, Screen.height/8 ),"LevelUp")){
				LevelUp=true;
				work=false;
			}*/
			

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
					playerNow.infomationText("Stock House Level Up!");
					
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
			//vSliderValue = playerNow.source[selectSource];
			//playerSource=playerNow.source [selectSource];
			
		}
		
	}
//	int caculateLimit(int select){
//		for(i)

//	}
}
