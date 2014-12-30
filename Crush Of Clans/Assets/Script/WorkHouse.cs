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
	public GUISkin guiSkin;

	private int[,,] needSource = {{ {100,50,0,0,0,0,0,0},{100,20,50,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {100,100,0,0,0,0,0,0},{300,0,150,0,0,0,0,0},{0,0,0,0,0,0,0,0} },{ {0,0,0,0,10,5,0,0},{0,0,0,0,100,50,0,0},{0,0,0,0,500,300,0,0} },{ {500,0,0,0,0,0,0,0},{100,0,300,0,0,0,0,0},{0,0,0,0,0,0,0,0} }};//{{10,5,0},{10,20,5},{20,40,20},{100,100,100}};
	private int[,] LevelUpSource = {{10,10,0},{20,20,0},{30,30,20},{100,100,40}};
	private int[,] toolExist={{0,0,0},{0,0,0},{0,0,0},{0,0,0}}; 
	public int HouseLevel=1;
	public bool work,LevelUp;
	public int HP=100,Lelel=1;
	public string PlayerID;
	private float vSliderValue;
	//private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭"};
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	private GameObject player;
	private string[] toolName={"斧頭","十字鎬","炸彈","推車"};
	public GameObject[] tool={null,null,null,null};
	//private GameObject[] tool = {null,null,null};
	private int selectTool,selectToolKind;
	private Player playerNow;
	// Use this for initialization
	void Start () {

		selectTool = 0;
		selectToolKind = 0;
		work = false;
		LevelUp = false;
		vSliderValue =Screen.height * 1 / 4;
	}
	
	// Update is called once per frame
	void Update () {
		//vSliderValue = (Screen.height * selectTool / 2) +Screen.height * 1 / 4;

	}
	void OnGUI () {
		string text="";

		//text="Wood:"+needSource[selectTool,0]+"\r\nStone:"+needSource[selectTool,1]+"\r\nMetal:"+needSource[selectTool,2];
		if (work == true) {

			for(int i=0;i<8;i++){
				if(needSource[selectTool,selectToolKind,i]!=0){
					text+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+needSource[selectTool,selectToolKind,i];
				}
			}

			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			if (GUI.Button (new Rect (Screen.width* 1/10, 0 , Screen.width*1/5, Screen.height/8 ),"斧頭",guiSkin.button)){
				selectTool=0;	
				selectToolKind = 0;
				
			}
			if (GUI.Button (new Rect (Screen.width* 3/10, 0 , Screen.width*1/5, Screen.height/8 ),"十字鎬",guiSkin.button)){
				selectTool=1;	
				selectToolKind = 0;
				
			}
			if (GUI.Button (new Rect (Screen.width* 5/10, 0 , Screen.width*1/5, Screen.height/8 ),"炸彈",guiSkin.button)){
				selectTool=2;	
				selectToolKind = 0;
					
			}
			if (GUI.Button (new Rect (Screen.width* 7/10, 0, Screen.width*1/5, Screen.height/8 ),"推車",guiSkin.button)){
				selectTool=3;	
				selectToolKind = 0;
				
			}

			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			//GUI.Label (new Rect (0,10, Screen.width*4/5, Screen.height*1/10),toolName[selectTool], guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5), "", guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), text, guiSkin.label);
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), "", guiSkin.label);
			
			switch(selectTool){
				case 0:
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"石製斧頭",guiSkin.button)){
						selectToolKind=0;
					}
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製斧頭",guiSkin.button)){
						selectToolKind=1;
					}

				break;
				case 1:
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"石製十字鎬",guiSkin.button)){
							selectToolKind=0;
						}
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製十字鎬",guiSkin.button)){
							selectToolKind=1;
						}
					break;
				case 2:
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"普通炸彈",guiSkin.button)){
						selectToolKind=0;
					}
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"中級炸彈",guiSkin.button)){
						selectToolKind=1;
					}
					if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 16 , Screen.width/3, Screen.height/8 ),"高級炸彈",guiSkin.button)){
						selectToolKind=2;
					}
				break;

				case 3:
				if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"木製手推車",guiSkin.button)){
					selectToolKind=0;
				}
				if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"鐵製手推車",guiSkin.button)){
					selectToolKind=1;
				}
				break;
			}







			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				LevelUp=false;		
				playerNow.click=false;
			}
			GUI.EndGroup();

			if(toolExist[selectTool,selectToolKind]==0){
				if(playerNow.source[0]>=needSource[selectTool,selectToolKind,0] && playerNow.source[1]>=needSource[selectTool,selectToolKind,1] && playerNow.source[2]>=needSource[selectTool,selectToolKind,2]&& playerNow.source[3]>=needSource[selectTool,selectToolKind,3]&& playerNow.source[4]>=needSource[selectTool,selectToolKind,4]&& playerNow.source[1]>=needSource[selectTool,selectToolKind,1] && playerNow.source[5]>=needSource[selectTool,selectToolKind,5]&& playerNow.source[6]>=needSource[selectTool,selectToolKind,6]&& playerNow.source[7]>=needSource[selectTool,selectToolKind,7]){
					GUI.enabled=true;
				}else{
					GUI.enabled=false;
					
				}
				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"製作",guiSkin.button)){
					toolExist[selectTool,selectToolKind]++;	
					playerNow.infomationText(toolName[selectTool]+"the Tool Is Made!");
					for(int i=0;i<8;i++){
						playerNow.source[i]-=needSource[selectTool,selectToolKind,i];
					}
				}
				GUI.enabled=true;
			}else{

				if(playerNow.tool!=selectTool+1 && selectTool!=3 &&  toolExist[selectTool,selectToolKind]!=0){
					if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"裝備",guiSkin.button)){
					
						if(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.childCount!=0){
							Destroy(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.FindChild(toolName[playerNow.tool-1]).gameObject);
						}
						playerNow.tool=selectTool+1;
						playerNow.toolKind=selectToolKind;
						GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
						usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
						usetool.name=toolName[selectTool];
						playerNow.infomationText(toolName[selectTool]+" is Equiped");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
					}
					
				}
				if(selectTool==3 && playerNow.cartLevel[1]!=0){
					if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"裝備",guiSkin.button)){

						playerNow.cart=1;
						playerNow.cartKind=selectToolKind;
						
						playerNow.infomationText("Cart is Equiped");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
					}
					
				}
				if(playerNow.tool==selectTool+1){
					if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"卸載",guiSkin.button)){
					
						if(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.childCount!=0){
							Destroy(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.FindChild(toolName[playerNow.tool-1]).gameObject);
						}
						playerNow.tool=0;
						playerNow.toolKind=0;
						playerNow.infomationText(toolName[selectTool]+" is took off");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
					}
				}
				if(selectTool==3 && playerNow.cart==1){
						if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"卸載",guiSkin.button)){
						
						playerNow.tool=0;
						playerNow.cartKind=0;
						
						playerNow.infomationText("Cart is took off");
						work=false;		
						LevelUp=false;		
						playerNow.click=false;
					}
				}
			}

			GUI.enabled=true;

		




		
		
		/*	if(playerNow.source[0]>=needSource[selectToolKind,0]&&playerNow.source[1]>=needSource[selectToolKind,1]&&playerNow.source[2]>=needSource[selectToolKind,2]){
				if(GUI.Button(new Rect(Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),"合成")){

						toolExist[selectTool,selectToolKind]++;	
						playerNow.infomationText(toolName[selectTool]+"the Tool Is Made!");

				


					for(int i=0;i<=2;i++){
						playerNow.source[i]-=needSource[selectToolKind,i];
					}*/
					//資料庫
					/*
						UPDATE Player
						Key:PlayerID
						欄位：
						source[0]=playerNow.source[0](wood)
						source[1]=playerNow.source[1](stone)
						source[2]=playerNow.source[2](metal)
						toolLevel[1]=playerNow.toolLevel[1](斧頭採集等級)
						toolLevel[2]=playerNow.toolLevel[2](十字鎬採集等級)
						cartLevel[1]playerNow.toolcartLevelLevel[1](手推車搬運等級)
						

				}

		
			}*/
		/*	if(playerNow.tool!=selectTool+1 && selectTool!=3 &&  toolExist[selectTool,selectToolKind]!=0){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 2 / 6 , Screen.width/10, Screen.height/6 ),"裝備")){
					if(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.childCount!=0){
						Destroy(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.FindChild(toolName[playerNow.tool-1]).gameObject);
					}
					playerNow.tool=selectTool+1;
					playerNow.toolKind=selectToolKind;
					GameObject usetool=(GameObject) Instantiate(tool[selectTool],playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.position,tool[selectTool].transform.rotation);
					usetool.transform.parent=playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform;
					usetool.name=tool[selectTool].name;
					playerNow.infomationText(toolName[selectTool]+" is Equiped");
					work=false;		
					LevelUp=false;		
					playerNow.click=false;
				}
				
			}
			if(selectTool==3 && playerNow.cartLevel[1]!=0){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 2 / 6 , Screen.width/10, Screen.height/6 ),"裝備")){
					playerNow.cart=1;
					playerNow.cartKind=selectToolKind;
					
					playerNow.infomationText("Cart is Equiped");
					work=false;		
					LevelUp=false;		
					playerNow.click=false;
				}
				
			}
			if(playerNow.tool==selectTool+1){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 3 / 6 , Screen.width/10, Screen.height/6 ),"卸載")){
					if(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.childCount!=0){
						Destroy(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.FindChild(toolName[playerNow.tool-1]).gameObject);
					}
					playerNow.tool=0;
					playerNow.toolKind=0;
					playerNow.infomationText(toolName[selectTool]+" is took off");
					work=false;		
					LevelUp=false;		
					playerNow.click=false;
				}
			}
			if(selectTool==3 && playerNow.cart==1){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 3 / 6 , Screen.width/10, Screen.height/6 ),"卸載")){
					playerNow.tool=0;
					playerNow.cartKind=0;
					
					playerNow.infomationText("Cart is took off");
					work=false;		
					LevelUp=false;		
					playerNow.click=false;
				}
			}

		//	GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
			if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
				work=false;		
				LevelUp=false;		
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
					playerNow.infomationText("Work House Level Up!");
					
					work=false;
					LevelUp=false;
					playerNow.click=false;
					
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