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
	public int[,] needSource = {{10,5,0},{10,5,0},{20,20,20}};
	public bool work;
	public int HP=100,Lelel=1;
	public string PlayerID;
	private float vSliderValue;
	private GameObject player;
	//private GameObject[] tool = {null,null,null};
	private int selectTool;
	private Player playerNow;
	// Use this for initialization
	void Start () {
		selectTool = 0;
		work = false;
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
			vSliderValue = GUI.VerticalSlider(new Rect(7*Screen.width/8, Screen.height/3, Screen.width/16, Screen.height/2), vSliderValue, Screen.height+Screen.height/4, Screen.height * 1 / 4);
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(0*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool1")){
				selectTool=0;
				
			}
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(1*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool2")){
				selectTool=1;
				
			}
			if (GUI.Button (new Rect (Screen.width* 1/2, vSliderValue-(2*Screen.height/2) , Screen.width/3, Screen.height/2 ),"Tool3")){
				selectTool=2;
				
			}
			text="Wood:"+needSource[selectTool,0]+"\r\nStone:"+needSource[selectTool,1]+"\r\nMetal:"+needSource[selectTool,2];
			if(playerNow.source[0]>=needSource[selectTool,0]&&playerNow.source[1]>=needSource[selectTool,1]&&playerNow.source[2]>=needSource[selectTool,2]){
				if(GUI.Button(new Rect(Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),"OK")){
					if(selectTool==0 ||selectTool==1){
						playerNow.toolLevel[selectTool+1]++;					
					}else{
						playerNow.cartLevel[1]++;
					}
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

			GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
			if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
				work=false;		
				playerNow.click=false;
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