using UnityEngine;
using System.Collections;

public class WorkHouse : MonoBehaviour {
	//資料庫
	/*
	WorkHouse的欄位：
	PlayerID 玩家帳號
	HouseID(看需不需要)
	x(x座標)
	z(z座標)
	WorkHouseLevel=1;
	WorkHouseHP=100;(暫定耐久)

	*/
	//共同變數
	public GUISkin guiSkin;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;

	public int kind;//建築類型
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	private SpriteRenderer spriteRenderer;// 
	private int HouseLevel=1;
	public bool work,LevelUp;
	public int HP=100;
	public string PlayerID;
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	private GameObject player;
	private Player playerNow;


	//共同變數end
	///
	//相似變數
	private string[] LevelInfo = {"可建造工具：鐵製斧頭、鐵製十字鎬","可建造工具：木製手推車、普通炸彈","可建造工具：中級炸彈","可建造工具：高級炸彈、鐵製手推車"};
	private string[,] TooLInfo = {{"採集量：10 最高耐久：100","採集量：30 最高耐久：200",""},{"採集量：10 最高耐久：100","採集量：30 最高耐久：200",""},{"威力：50 範圍：單一","威力：100 範圍：九宮格","威力：200 範圍：連鎖"},{"負重量：10000","負重量：20000",""}};
	private int[,] LevelUpSource = {{100,100,10,0,0,0,0,0},{200,200,50,0,0,0,0,0},{300,300,100,0,0,0,0,0},{500,400,200,0,0,0,0,0}};
	private int[,] needLevel = {{1,2,10},{1,2,10},{2,4,5},{3,5,10}};
	private int[,,] needSource = {{ {100,50,0,0,0,0,0,0},{100,20,50,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {100,100,0,0,0,0,0,0},{300,0,150,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {0,0,0,0,10,5,0,0},{0,0,0,0,100,50,0,0},{0,0,0,0,500,300,0,0} },{ {500,0,0,0,0,0,0,0},{100,0,300,0,0,0,0,0},{0,0,0,0,0,0,0,0} }};//{{10,5,0},{10,20,5},{20,40,20},{100,100,100}};
	public Texture[] LevelPng ;
	public Sprite[] LevelSpritePng;

	//相似變數end
	///
	//獨有變數

	//workwouse////
	private int[,] toolExist={{0,0,0},{0,0,0},{0,0,0},{0,0,0}}; 
	private int[,] toolHp={{100,200},{100,200}};
	private int[,] toolHpNow={{0,0},{0,0}};
	private bool selectHouseKind;
	private string[] toolName={"斧頭","十字鎬","炸彈","推車"};
	private int selectTool,selectToolKind;
	//public GameObject[] tool={null,null,null,null};
	
	//workwouse End////

	//獨有變數end


	
	// Use this for initialization
	void Start () {
		 spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
		
		selectTool = 0;
		selectToolKind = 0;
		work = false;
		LevelUp = false;
		selectHouseKind = false;
		bombCount = 0;
		bombSet = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (LevelSpritePng.Length != 0) {
			spriteRenderer.sprite = LevelSpritePng[HouseLevel-1];
			
		}
		//vSliderValue = (Screen.height * selectTool / 2) +Screen.height * 1 / 4;

	}
	bool isEnable(int tool,int kind){
		if (selectHouseKind == false)
						return true;
		if (toolExist [tool, kind] == 0) {
			return false;	
			}
		else{
			return true;	
			
		}

	}

	void OnGUI () {
		string text="";

		//text="Wood:"+needSource[selectTool,0]+"\r\nStone:"+needSource[selectTool,1]+"\r\nMetal:"+needSource[selectTool,2];
		if (work == true) {

			for(int i=0;i<8;i++){
				if(needSource[selectTool,selectToolKind,i]!=0){
					text+=sourceName[i]+":"+playerNow.source[i]+"/"+needSource[selectTool,selectToolKind,i]+"\r\n";
				}
			}
			text+="所需等級："+HouseLevel+"/"+needLevel[selectTool,selectToolKind];
			
			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			if(selectHouseKind==false){
				GUI.enabled=false;
			}else{
				GUI.enabled=true;
			}
			if (GUI.Button (new Rect (Screen.width*2/40, Screen.height/10 , Screen.width*1/20, Screen.height/4 ),"工\r\n作\r\n坊",guiSkin.button)){
				selectHouseKind=false;
				selectTool=0;	
				selectToolKind = 0;
			}
			if(selectHouseKind==false){
				GUI.enabled=true;
			}else{
				GUI.enabled=false;
			}
			if (GUI.Button (new Rect (Screen.width*2/40, Screen.height/10+Screen.height/4 , Screen.width*1/20, Screen.height/4 ),"工\r\n具\r\n庫",guiSkin.button)){
				selectHouseKind=true;
				selectTool=0;	
				selectToolKind = 0;
				
			}
				
			if(selectTool==0){GUI.enabled=false;}else{GUI.enabled=true;}
			if (GUI.Button (new Rect (Screen.width* 1/10, 0 , Screen.width*1/5, Screen.height/8 ),"斧頭",guiSkin.button)){
				selectTool=0;	
				selectToolKind = 0;
				
			}
			if(selectTool==1){GUI.enabled=false;}else{GUI.enabled=true;}

			if (GUI.Button (new Rect (Screen.width* 3/10, 0 , Screen.width*1/5, Screen.height/8 ),"十字鎬",guiSkin.button)){
				selectTool=1;	
				selectToolKind = 0;
				
			}

			if(selectTool==2){GUI.enabled=false;}else{GUI.enabled=true;}
			if (GUI.Button (new Rect (Screen.width* 5/10, 0 , Screen.width*1/5, Screen.height/8 ),"炸彈",guiSkin.button)){
				selectTool=2;	
				selectToolKind = 0;
					
			}

			if(selectTool==3){GUI.enabled=false;}else{GUI.enabled=true;}
			if (GUI.Button (new Rect (Screen.width* 7/10, 0, Screen.width*1/5, Screen.height/8 ),"推車",guiSkin.button)){
				selectTool=3;	
				selectToolKind = 0;
				
			}
			GUI.enabled=true;
			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			//GUI.Label (new Rect (0,10, Screen.width*4/5, Screen.height*1/10),toolName[selectTool], guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5), "", guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			if(selectHouseKind==false && selectTool!=2){
				GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), text, guiSkin.label);
			}
			if(selectHouseKind==false &&(selectTool==2)){
				GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), text+"\r\n庫存量:"+toolExist[selectTool,selectToolKind], guiSkin.label);
				
			}
			if(selectHouseKind==true &&(selectTool==0||selectTool==1)){
				GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), "耐久:"+toolHpNow[selectTool,selectToolKind], guiSkin.label);
				
			}
			if(selectHouseKind==true &&(selectTool==2)){
				GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), "庫存量:"+toolExist[selectTool,selectToolKind]+"\r\n攜帶量:"+playerNow.toolBomb[selectToolKind], guiSkin.label);
				
			}
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), TooLInfo[selectTool,selectToolKind], guiSkin.label);
			switch(selectTool){
				case 0:
			//	GUI.enabled=isEnable(0,0);
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"石製斧頭",guiSkin.button)){
						selectToolKind=0;
					}
			//	GUI.enabled=isEnable(0,1);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製斧頭",guiSkin.button)){
						selectToolKind=1;
					}

				break;
				case 1:
			//	GUI.enabled=isEnable(1,0);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"石製十字鎬",guiSkin.button)){
							selectToolKind=0;
						}
			//	GUI.enabled=isEnable(1,1);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製十字鎬",guiSkin.button)){
							selectToolKind=1;
						}
					break;
				case 2:
			//	GUI.enabled=isEnable(2,0);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"普通炸彈",guiSkin.button)){
						selectToolKind=0;
					}
			//	GUI.enabled=isEnable(2,1);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"中級炸彈",guiSkin.button)){
						selectToolKind=1;
					}
			//	GUI.enabled=isEnable(2,2);
				
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 16 , Screen.width/3, Screen.height/8 ),"高級炸彈",guiSkin.button)){
						selectToolKind=2;
					}
				break;

				case 3:
			//	GUI.enabled=isEnable(3,0);
				
				if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"木製手推車",guiSkin.button)){
					selectToolKind=0;
				}
			//	GUI.enabled=isEnable(3,1);
				if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製手推車",guiSkin.button)){
					selectToolKind=1;
				}
				break;
			}







			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				LevelUp=false;		
				playerNow.click=false;
				selectHouseKind = false;
				
			}
			GUI.EndGroup();

			////make Tool////
			if( selectHouseKind==false){


				if(playerNow.source[0]>=needSource[selectTool,selectToolKind,0] && playerNow.source[1]>=needSource[selectTool,selectToolKind,1] && playerNow.source[2]>=needSource[selectTool,selectToolKind,2]&& playerNow.source[3]>=needSource[selectTool,selectToolKind,3]&& playerNow.source[4]>=needSource[selectTool,selectToolKind,4]&& playerNow.source[1]>=needSource[selectTool,selectToolKind,1] && playerNow.source[5]>=needSource[selectTool,selectToolKind,5]&& playerNow.source[6]>=needSource[selectTool,selectToolKind,6]&& playerNow.source[7]>=needSource[selectTool,selectToolKind,7]&& HouseLevel>=needLevel[selectTool,selectToolKind]){
					GUI.enabled=true;
					
					if(selectTool==0||selectTool==1||selectTool==3){
						if(playerNow.tool==selectTool+1 && playerNow.toolKind==selectToolKind){
							GUI.enabled=false;
						}
						if(toolExist[selectTool,selectToolKind]!=0){
							GUI.enabled=false;
							
						}
					}
				}else{
					GUI.enabled=false;
					
				}

				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"製作",guiSkin.button)){
					toolExist[selectTool,selectToolKind]++;	
					if(selectTool==0||selectTool==1){
						toolHpNow[selectTool,selectToolKind]=toolHp[selectTool,selectToolKind];
					}
					playerNow.infomationText(toolName[selectTool]+"製作成功!");
					for(int i=0;i<8;i++){
						playerNow.source[i]-=needSource[selectTool,selectToolKind,i];
					
					}


				}
				GUI.enabled=true;

			////make Tool///




			/// Equipe TOOL//
			}else if (selectHouseKind==true){
				/// Equipe //
				if(toolExist[selectTool,selectToolKind]!=0){
					
					GUI.enabled=true;
				}else {

					GUI.enabled=false;
				}

				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/10, Screen.width*1/15 ),"裝備",guiSkin.button)){
				
					if(selectTool==0|| selectTool==1){
						if(playerNow.tool!=0){
							toolExist[playerNow.tool-1,playerNow.toolKind]++;
							toolHpNow[playerNow.tool-1,playerNow.toolKind]=playerNow.toolHp;
						}
						playerNow.tool=selectTool+1;
						playerNow.toolKind=selectToolKind;
						playerNow.toolHp=toolHpNow[selectTool,selectToolKind];
						toolExist[selectTool,selectToolKind]--;
						
						/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
						usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
						usetool.name=toolName[selectTool];*/
						playerNow.infomationText("已裝備"+toolName[selectTool]+"!");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
						selectHouseKind = false;

					}
					if(selectTool==2){
						playerNow.toolBomb[selectToolKind]++;
						toolExist[selectTool,selectToolKind]--;
					}
					if(selectTool==3){
						if(playerNow.cart!=0){
							toolExist[3,playerNow.cart-1]++;
						}
						playerNow.cart=selectToolKind+1;
						toolExist[selectTool,selectToolKind]--;
						playerNow.infomationText("已裝備手推車");
						
						
					}


				}
				/// Equipe //
				/// 
				/// Take OFF //
				if( toolExist[selectTool,selectToolKind]==0){
					
					GUI.enabled=true;
					if((selectTool==0 || selectTool==1) && (playerNow.tool!=selectTool+1 || playerNow.toolKind!=selectToolKind)){
						GUI.enabled=false;
						
					}
					if(selectTool==2 && playerNow.toolBomb[selectToolKind]==0){
						GUI.enabled=false;
						
					}
					if(selectTool==3 && playerNow.cart!=selectToolKind+1){
						GUI.enabled=false;
						
					}

				}else {
					GUI.enabled=false;
					
					if(selectTool==2 && toolExist[selectTool,selectToolKind]>0 && playerNow.toolBomb[selectToolKind]!=0){
						GUI.enabled=true;
						
					}
				}

				if (GUI.Button (new Rect (Screen.width*3/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/10, Screen.width*1/15 ),"卸載",guiSkin.button)){

					if(selectTool==0|| selectTool==1){

						toolExist[playerNow.tool-1,playerNow.toolKind]++;
						toolHpNow[playerNow.tool-1,playerNow.toolKind]=playerNow.toolHp;

						playerNow.tool=0;
						playerNow.toolKind=0;
						playerNow.toolHp=0;
						
						/*GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
						usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
						usetool.name=toolName[selectTool];*/
						playerNow.infomationText("已卸載"+toolName[selectTool]+"!");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
						selectHouseKind = false;
						
					}
					if(selectTool==2){
						playerNow.toolBomb[selectToolKind]--;
						toolExist[selectTool,selectToolKind]++;
					}
					if(selectTool==3){

						playerNow.cart=0;
						toolExist[selectTool,selectToolKind]++;
						playerNow.infomationText("已卸載手推車");
						
						
					}



				}
				/// Take OFF //
					

				GUI.enabled=true;
				

			}

			GUI.enabled=true;

		
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
				if(selectHouseKind==false){

					GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), LevelText, guiSkin.label);
				}
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
					spriteRenderer.sprite = LevelSpritePng[HouseLevel];
					
					HouseLevel++;
					playerNow.click=false;
					playerNow.infomationText("工作屋等級提升!");
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