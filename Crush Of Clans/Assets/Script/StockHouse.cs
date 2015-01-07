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
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	
	public GUISkin guiSkin;
		public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;
	
	public Texture[] LevelPng;
	public Sprite[] LevelSprite;
	private SpriteRenderer spriteRenderer;// 
	private int[,] LevelUpSource = {{50,20,5,0,0,0,0,0},{100,50,10,0,0,0,0,0},{200,100,50,0,0,0,0,0},{400,200,200,0,0,0,0,0}};
		private int[] needLevel = {1,1,3,4,3,5,2,2};
	
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	private string[] LevelInfo = {
				"倉庫存放量：10000,可存放資源：鐵礦、硫磺礦",
				"倉庫存放量：20000,可存放資源：鐵片、木炭",
				"倉庫存放量：30000,可存放資源：硫磺",
				"倉庫存放量：40000,可存放資源：火藥"
		};
	public int HouseLevel=1;
	
	//private int[,] LevelUpSource = {{10,10,0},{20,20,0},{30,30,20},{100,100,40}};
	
	public int[] stockSource = {20,20,20,0,0,0,0,0};
	public int[] stocklimit = {5000,10000,20000,30000,40000};
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
		 spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
		put = false;
		get=false;
		work = false;
		LevelUp = false;
		hSliderValue = Screen.width / 8;
		selectSource = 0;
		Quatity = "0";
		bombCount = 0;
		bombSet = null;
		

	}
	
	// Update is called once per frame
	void Update () {
		if (LevelSprite.Length != 0) {
			spriteRenderer.sprite=LevelSprite[HouseLevel-1];
			
		}
		/*	for (int i =0; i<10; i++) {
			this.transform.FindChild("House").transform.localScale+=new Vector3(0.01F, 0, 0.01F);
			//this.transform.+=0.1f;		
			//this.transform.localScale.y+=0.1f;		
			//this.transform.localScale.z+=0.1f;		
		}*/
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

			GUI.Label (new Rect (Screen.width*6/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), stockSource[0].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),sourceName[0],guiSkin.customStyles[3])){
				selectSource=0;
				
				
			}
			GUI.Label (new Rect (Screen.width*9/16, Screen.height* 3/ 26 , Screen.width/8, Screen.height/16 ), stockSource[1].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 1/ 13 , Screen.width/12, Screen.height/8 ),sourceName[1],guiSkin.customStyles[3])){
				selectSource=1;
				
				
			}
			GUI.Label (new Rect (Screen.width*6/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), stockSource[2].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),sourceName[2],guiSkin.customStyles[3])){
				selectSource=2;
				
				
			}
			GUI.Label (new Rect (Screen.width*9/16, Screen.height* 7/ 26 , Screen.width/8, Screen.height/16 ), stockSource[3].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 3/ 13 , Screen.width/12, Screen.height/8 ),sourceName[3],guiSkin.customStyles[3])){
				selectSource=3;
				
				
			}
			GUI.Label (new Rect (Screen.width*6/16, Screen.height*11/ 26 , Screen.width/8, Screen.height/16 ), stockSource[4].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),sourceName[4],guiSkin.customStyles[3])){
				selectSource=4;
				
				
			}
			GUI.Label (new Rect (Screen.width*9/16, Screen.height* 11/ 26 , Screen.width/8, Screen.height/16 ), stockSource[5].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 5/ 13 , Screen.width/12, Screen.height/8 ),sourceName[5],guiSkin.customStyles[3])){
				selectSource=5;
				
				
			}
			GUI.Label (new Rect (Screen.width*6/16, Screen.height* 15/ 26 , Screen.width/8, Screen.height/16 ), stockSource[6].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*5/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),sourceName[6],guiSkin.customStyles[3])){
				selectSource=6;
				
				
			}
			GUI.Label (new Rect (Screen.width*9/16, Screen.height*15/ 26 , Screen.width/8, Screen.height/16 ), stockSource[7].ToString(),guiSkin.customStyles[4]);
			
			if(GUI.Button (new Rect (Screen.width*8/16, Screen.height* 7/ 13 , Screen.width/12, Screen.height/8 ),sourceName[7],guiSkin.customStyles[3])){
				selectSource=7;
				
				
			}
			
			
			
			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				playerNow.click=false;
			}
			GUI.EndGroup();
			//vSliderValue=(playerNow.source[selectSource])>0?playerNow.source[selectSource]:0;
			playerSource=playerNow.source [selectSource];
//			limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
			if(HouseLevel>=needLevel[selectSource]){
				GUI.enabled=true;
			}else{
				GUI.enabled=false;
				
			}
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
			if(HouseLevel<5){
				string text="";
				for(int i=0;i<8;i++){
					if(LevelUpSource[HouseLevel-1,i]!=0){
						text+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+LevelUpSource[HouseLevel-1,i];
					}
				}

				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width/15,Screen.height/12, Screen.width*2/7, Screen.width*2/7), LevelPng[HouseLevel], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), text, guiSkin.label);
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
					LevelUp=false;
					for(int i =0;i<8;i++){
						
						playerNow.source[i]=playerNow.source[i]-LevelUpSource[HouseLevel-1,i];
					}
					spriteRenderer.sprite = LevelSprite[HouseLevel];
					
					HouseLevel++;
					playerNow.click=false;
					playerNow.infomationText("倉庫等級提升!");

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
			//vSliderValue = playerNow.source[selectSource];
			//playerSource=playerNow.source [selectSource];
			
		}
		
	}
	IEnumerator bomb_function(){

		yield return new WaitForSeconds(1);
		if(bomb.kind==2&&bombCount<2 && State.bombTotal<=5){
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
