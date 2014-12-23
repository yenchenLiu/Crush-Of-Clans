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
	public int[,] needSource = {{10,5,0},{10,20,5},{20,40,20},{100,100,100}};
	private int[,] LevelUpSource = {{10,10,0},{20,20,0},{30,30,20},{100,100,40}};
	private int[,] toolExist={{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0}}; 
	public int HouseLevel=1;
	public bool work,LevelUp;
	public int HP=100,Lelel=1;
	public string PlayerID;
	private float vSliderValue;
	private GameObject player;
	private string[] toolName={"cross","knife","bomb","cart"};
	public GameObject[] tool={null,null,null,null};
	//private GameObject[] tool = {null,null,null};
	private int selectTool,selectToolKind;
	private Player playerNow;
	// Use this for initialization
	void Start () {
		selectTool = 0;
		work = false;
		LevelUp = false;
		vSliderValue =Screen.height * 1 / 4;
	}
	
	// Update is called once per frame
	void Update () {
		//vSliderValue = (Screen.height * selectTool / 2) +Screen.height * 1 / 4;

	}
	void OnGUI () {
		string text;
		if (work == true) {

			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			if (GUI.Button (new Rect (Screen.width* 1/6, Screen.height/8 , Screen.width/6, Screen.height/8 ),"十字鎬")){
				selectTool=0;	
			}
			if (GUI.Button (new Rect (Screen.width* 2/6, Screen.height/8 , Screen.width/6, Screen.height/8 ),"斧頭")){
				selectTool=1;	
			}
			if (GUI.Button (new Rect (Screen.width* 3/6, Screen.height/8 , Screen.width/6, Screen.height/8 ),"炸彈")){
				selectTool=2;	
			}
			if (GUI.Button (new Rect (Screen.width* 4/6, Screen.height/8 , Screen.width/6, Screen.height/8 ),"推車")){
				selectTool=3;	
			}

			GUI.Box (new Rect ( Screen.width/6, Screen.height*2/8, Screen.width*4/6, Screen.height*6/8),toolName[selectTool]);

			if (GUI.Button (new Rect (Screen.width* 2/10, Screen.height*3/8 , Screen.width*6/10, Screen.height/8 ),"Wood   "+"Wood:"+needSource[0,0]+"  Stone:"+needSource[0,1]+"  Metal:"+needSource[0,2])){
				selectToolKind=0;	
			}
			if (GUI.Button (new Rect (Screen.width* 2/10, Screen.height*4/8 , Screen.width*6/10, Screen.height/8 ),"Stone   "+"Wood:"+needSource[1,0]+"  Stone:"+needSource[1,1]+"  Metal:"+needSource[1,2])){
				selectToolKind=1;	
			}
			if (GUI.Button (new Rect (Screen.width* 2/10, Screen.height*5/8 , Screen.width*6/10, Screen.height/8 ),"Metal   "+"Wood:"+needSource[2,0]+"  Stone:"+needSource[2,1]+"  Metal:"+needSource[2,2])){
				selectToolKind=2;	
			}
			if (GUI.Button (new Rect (Screen.width* 2/10, Screen.height*6/8 , Screen.width*6/10, Screen.height/8 ),"Goden   "+"Wood:"+needSource[3,0]+"  Stone:"+needSource[3,1]+"  Metal:"+needSource[3,2])){
				selectToolKind=3;	
			}

			//vSliderValue = GUI.VerticalSlider(new Rect(7*Screen.width/8, Screen.height/3, Screen.width/16, Screen.height * 2/5), vSliderValue, Screen.height+Screen.height*3/4, Screen.height * 1 / 4);


			/*if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(0*Screen.height/2) , Screen.width/3, Screen.height/2 ),"十字鎬\r\nLevel:"+playerNow.toolLevel[1])){
				selectTool=0;
				
			}
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(1*Screen.height/2) , Screen.width/3, Screen.height/2 ),"斧頭\r\nLevel:"+playerNow.toolLevel[2])){
				selectTool=1;
				
			}
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(2*Screen.height/2) , Screen.width/3, Screen.height/2 ),"炸彈\r\nLevel:"+playerNow.toolLevel[3])){
				selectTool=2;
				
			}
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(3*Screen.height/2) , Screen.width/3, Screen.height/2 ),"推車\r\nLevel:"+playerNow.cartLevel[1])){
				selectTool=3;
				
			}*/

		
			text="Wood:"+needSource[selectTool,0]+"\r\nStone:"+needSource[selectTool,1]+"\r\nMetal:"+needSource[selectTool,2];
			if(playerNow.source[0]>=needSource[selectToolKind,0]&&playerNow.source[1]>=needSource[selectToolKind,1]&&playerNow.source[2]>=needSource[selectToolKind,2]){
				if(GUI.Button(new Rect(Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),"合成")){

						toolExist[selectTool,selectToolKind]++;	
						playerNow.infomationText(toolName[selectTool]+"the Tool Is Made!");

				


					for(int i=0;i<=2;i++){
						playerNow.source[i]-=needSource[selectTool,i];
					}
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
						
					*/
				}

		
			}
			if(playerNow.tool!=selectTool+1 && selectTool!=3 &&  toolExist[selectTool,selectToolKind]!=0){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 2 / 6 , Screen.width/10, Screen.height/6 ),"裝備")){
					if(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.childCount!=0){
						Destroy(playerNow.transform.FindChild("People").gameObject.transform.FindChild("Hand").gameObject.transform.FindChild(toolName[playerNow.tool-1]).gameObject);
					}
					playerNow.tool=selectTool+1;
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
					playerNow.infomationText(toolName[selectTool]+" is took off");
					work=false;		
					LevelUp=false;		
					playerNow.click=false;
				}
			}
			if(selectTool==3 && playerNow.cart==1){
				if(GUI.Button(new Rect(Screen.width* 9 / 10, Screen.height* 3 / 6 , Screen.width/10, Screen.height/6 ),"卸載")){
					playerNow.tool=0;
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