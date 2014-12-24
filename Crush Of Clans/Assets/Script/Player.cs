using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//資料庫
	/*
	Player的欄位:
	PlayerID(帳號)
	x(x座標)
	z(z座標)
	cart(目前使用的交通工具)
	tool(目前使用的採集工具) 
	source[0](wood)
	source[1](stone)
	source[2](metal)
	...(看有沒有其他資源要加)
	toolLevel[0](手動採集等級)
	toolLevel[1](斧頭採集等級)
	toolLevel[2](十字鎬採集等級)
	...(看有沒有其他工具要加)
	cartLevel[0](手動搬運等級)
	cartLevel[1](手推車搬運等級)
	*/
	//status
	public string PlayerID="000";
	private float picktime,GameTime,BombTimeCount;
	public float infoTime;
	public int tool,cart,toolKind,cartKind,j,w,h;
	private int Status;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 6"UpGrade" 
	public  int[] source={0,0,0};//0 wood, 1 stone ,2 metal;
	public  string[] sourceName={"wood","stone","metal"};//0 wood, 1 stone ,2 metal;
	public int[] weight = {5,10,50};//0 wood, 1 stone ,2 metal;
	public int x,y,z;
	public bool Build=false,click=false,Bomb=false,BombGameStart=false;
	private Rigidbody BuildNow;
	private GameObject PickSource,TriggerHouse;
	public GameObject fire;
	public Rigidbody build_house;
	public bool info;
	public string infotext;

	//attribute
	private char[] BombGame={'1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','I','M','N','O','P'};
	private char[] GameQ = {'x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x','x'};
	private int[] BombGameTime = {3,5,8,10};
	private int[] pick = {5,10,20,50};//{{5,5,10,15},{10,20,30,40},{20,40,60,80},{30,60,90,120}};
	private int[] BombGameButton = {4,9,16,25};
	public int[] toolLevel = {1,0,0,0};
	public int[] cartLevel = {1,0};
	private string BombGameQ,BombGameInput;
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈","UpGrade"};
	private string[] toolName={"手","十字鎬","斧頭","炸彈"};
	private string[] cartName={"手","推車"};
	private float view;
	public int[] package = {1000,2000};
	private int[] CDtime = {5,4,3,2};
	public int weightNow;

	// Use this for initialization
	void Start () {

		////////資料庫
		/*SELECT Player
			key: PlayerID
			欄位：
			cart(目前使用背包推車)
			tool(目前使用工具)
			source[0](wood)
			source[1](stone)
			source[2](metal)
			x(x座標)
			z(z座標)
			toolLevel[0](手動採集等級)
			toolLevel[1](斧頭採集等級)
			toolLevel[2](十字鎬採集等級)
			...(看有沒有其他工具要加)
			cartLevel[0](手動搬運等級)
			cartLevel[1](手推車搬運等級)
			這些全部先印出來就好
		*/
		info = false;
		weightNow = 0;
		cart = 0;
		tool = 0;
		Status = 0;
		view = 50;
		Build=false;
		click = false;
		picktime=Time.time-5;
		BombTimeCount = Time.time - 6;
		BombGameStart = false;
	}
	
	// Update is called once per frame
	void Update () {
				weightNow = (source [0] * weight [0])+(source [1] * weight [1]) + (source [2] * weight [2]);
				x = (int)this.transform.position.x;
				y = (int)this.transform.position.y;
				z = (int)this.transform.position.z;
				//this.transform.FindChild ("Main Camera").camera.fieldOfView = view;
				////////資料庫
				/*UPDATE Player
					key: PlayerID
					欄位：
					x(x座標)
					z(z座標)
				*/
				if (Input.GetKey (KeyCode.W)) {
					this.transform.Translate (new Vector3 (0, 0, -1));

					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,180,0);
				}
		
				if (Input.GetKey (KeyCode.S)) {
					this.transform.Translate (new Vector3 (0, 0, 1));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,0,0);
			
				}
		
				if (Input.GetKey (KeyCode.D)) {
					this.transform.Translate (new Vector3 (-1, 0, 0));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,270,0);
			
				}
		
				if (Input.GetKey (KeyCode.A)) {
					this.transform.Translate (new Vector3 (1, 0, 0));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,90,0);
			
				}



	}

	//Status = 0 虛擬搖桿 移動
	public void infomationText(string text){
		infotext = text;
		info = true;
		infoTime = Time.time;
	}
	void OnGUI(){
		GUI.skin.button.fontSize = 30;
		GUI.skin.box.fontSize=30;
		GUI.skin.textArea.fontSize = 30;
		if (info == true) {
			GUI.Box (new Rect (Screen.width*1/3, Screen.height/5, Screen.width/3, Screen.height / 10),infotext);
			if((int)(Time.time-infoTime)==1){
				info=false;
			}
		}
		GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height / 10),"");

		GUI.Box (new Rect ( 0, 0, Screen.width/8, Screen.height / 10),"Wood");
		GUI.Box (new Rect ( Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[0].ToString());
		GUI.Box (new Rect ( 2*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Stone");
		GUI.Box (new Rect ( 3*Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[1].ToString());
		GUI.Box (new Rect ( 4*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Metal");
		GUI.Box (new Rect ( 5*Screen.width/8, 0, Screen.width/8, Screen.height / 10),source[2].ToString());

		GUI.Box (new Rect ( 6*Screen.width/8, 0, Screen.width/8, Screen.height / 10),"Weight");
		GUI.Box (new Rect ( 7*Screen.width/8, 0, Screen.width/8, Screen.height / 10),weightNow.ToString());
		GUI.Box (new Rect ( 0, Screen.height / 10, Screen.width/8, Screen.height / 10),toolName[tool]+pick[toolKind].ToString());
		GUI.Box (new Rect ( 0, Screen.height *2/ 10, Screen.width/8, Screen.height / 10),cartName[cart]+package[cart].ToString());
		string ButtonText;
		if (Time.time - picktime < CDtime[toolKind] && Status==1) {
			GUI.enabled=false;
			ButtonText =StatusName[Status]+"\r\n"+ ((int)(CDtime [tool] - (Time.time - picktime))).ToString ();
						//GUI.Box(new Rect (9 * Screen.width / 10, Screen.height * 1 / 8, Screen.width / 10, Screen.height / 8), ((int)( CDtime[tool]-(Time.time - picktime) )).ToString() );		
		} 
		else {
			ButtonText=StatusName[Status];
		}

		if (5 - (int)(Time.time - BombTimeCount) >= 1) {
			infomationText(((int)(5-(Time.time-BombTimeCount))).ToString());
					
		}
		if (BombGameStart == true) {

			if(Time.time-GameTime>=BombGameTime[toolKind]){
				infomationText("False....");
				BombGameStart=false;

			}	
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 1/ 10 , Screen.width/2, Screen.height/10 ),BombGameQ);
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 2/ 10, Screen.width/2, Screen.height/10 ),BombGameInput);
			GUI.TextArea(new Rect (Screen.width* 1/4, Screen.height* 3/ 10, Screen.width/2*((BombGameTime[toolKind]-Time.time+GameTime)/BombGameTime[toolKind]), Screen.height/20 ),"");

			switch(toolKind){
			case 0:
				w=1;
				h=1;
				for(int i=0;i<4;i++){
					//if(h>2) h=1;
					if(w>2){
						w=1;
						h++;
					}

					print (w+" "+h );
					if (GUI.Button (new Rect (Screen.width*w/4, Screen.height* h/ 3 , Screen.width/4, Screen.height/3 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
							if(j>=4){
								BombTimeCount=Time.time;

							}
						}
					}
					w++;

				}
			/*	if (GUI.Button (new Rect (Screen.width* 1/4, Screen.height* 1/ 3 , Screen.width/4, Screen.height/3 ),"1")){
					if(GameQ[j]!='1'){
						infomationText("False....");
						BombGameStart=false;
						break;
					}else{
						j++;
						BombGameInput+=1;
					}
				}
				if (GUI.Button (new Rect (Screen.width* 2/4, Screen.height* 1/ 3 , Screen.width/4, Screen.height/3 ),"2")){
					if(GameQ[j]!='2'){
						infomationText("False....");
						
						BombGameStart=false;
						break;
					}else{
						BombGameInput+=2;
						j++;
					}
				}
				if (GUI.Button (new Rect (Screen.width* 1/4, Screen.height* 2/ 3 , Screen.width/4, Screen.height/3 ),"3")){
					if(GameQ[j]!='3'){
						infomationText("False....");
						
						BombGameStart=false;
						break;
					}else{
						BombGameInput+=3;
						j++;
					}
				}
				if (GUI.Button (new Rect (Screen.width* 2/4, Screen.height* 2/ 3 , Screen.width/4, Screen.height/3 ),"4")){
					if(GameQ[j]!='4'){
						infomationText("False....");
						
						BombGameStart=false;
						break;
					}else{
						BombGameInput+=4;
						j++;
					}
				}*/
				if (j>=4){

					Destroy(TriggerHouse.gameObject,5);
					Bomb=false;
					BombGameStart=false;
					j=0;
				}
				break;
			case 1:
				w=1;
				h=1;
				for(int i=0;i<9;i++){
					
					if(w>3){
						w=1;
						h++;
					}
					if (GUI.Button (new Rect (Screen.width*w/5, Screen.height* 1/ 3 + (Screen.height* (h-1)*2/ 9), Screen.width/5, Screen.height*2/9 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=9){
					Destroy(TriggerHouse.gameObject);
					Bomb=false;
					BombGameStart=false;
					j=0;
				}
				break;
			case 2:
				w=1;
				h=1;
				for(int i=0;i<16;i++){
					
					if(w>4){
						w=1;
						h++;
					}
					if (GUI.Button (new Rect (Screen.width*1/4+(Screen.width*(w-1)/8), Screen.height* 1/ 3 + (Screen.height* (h-1)/ 6), Screen.width/8, Screen.height*1/6 ),BombGame[i].ToString())){
						if(GameQ[j]!=BombGame[i]){
							infomationText("False....");
							BombGameStart=false;
							break;
						}else{
							j++;
							BombGameInput+=BombGame[i];
						}
					}
					w++;
				}
				if (j>=16){
					Destroy(TriggerHouse.gameObject);
					Bomb=false;
					BombGameStart=false;
					j=0;
				}
				break;
			case 3:
				break;
				
			}	
		
		}

		if (tool==3 && click == false && Build == false && Bomb==true && GUI.Button (new Rect (Screen.width * 5 / 6, Screen.height * 2 / 4, Screen.width / 6, Screen.height / 4), "Destroy")) {
			j=0;
			w=1;
			h=1;
			BombGameQ="";
			BombGameInput="";
			print (toolKind);

			for(int i =0;i<BombGameButton[toolKind];i++){
				GameQ[i]=BombGame[(int)(UnityEngine.Random.Range(0, BombGameButton[toolKind]))];
				BombGameQ+=GameQ[i];
				BombGameQ+=" ";
			}	
			BombGameStart=true;
			GameTime=Time.time;
			//Destroy(TriggerHouse.gameObject);
			Bomb=false;
		}
		if ( click==false && Build==false && GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),ButtonText)){

			switch (Status){
			case 0://build a house

				Build = true;
				click=false;
				break;
			case 1:			
				//採集放這邊
				//source++;
				if(Time.time-picktime>CDtime[toolKind] && weightNow < package[cart]){
					Source pickup=PickSource.GetComponent<Source>();					
					int kind=pickup.kind;
					if(tool==0||kind+1==tool ){
						int quatity=pickup.quatity;
						int sourceWeight=weight[kind];
						int getQutity;
						print (quatity);
						if(quatity<=pick[toolKind]){
							if(package[cart]-weightNow-(quatity*sourceWeight)<0){
								getQutity = (package[cart]-weightNow)/sourceWeight;
								source[kind]+=getQutity;
								pickup.quatity-=getQutity;
								//weightNow+=getQutity*sourceWeight;
							}else{
								getQutity=quatity;
									
								source[kind]+=quatity;
								pickup.quatity-=quatity;
								//weightNow+=quatity*sourceWeight;
								//資料庫
								/*
								DELETE Source
								Key: x=pickup.x , z=pickup.z
								*/
								Destroy(PickSource.gameObject);
								
							}
							
						}else{
							if(package[cart]-weightNow-(pick[toolKind]*sourceWeight)<0){
								getQutity = (package[cart]-weightNow)/sourceWeight;
								source[kind]+=getQutity;
								pickup.quatity-=getQutity;
								//weightNow+=getQutity*sourceWeight;
								
							}else{
								getQutity=pick[toolKind];
								source[kind]+=pick[toolKind];
								pickup.quatity-=pick[toolKind];
							//	weightNow+=pick[tool]*sourceWeight;
								
							}
						}
						infomationText("Get "+getQutity.ToString()+" "+sourceName[kind]+"s !");
						//infotext=sourceName[kind]+":"+getQutity.ToString();
						//info=true;
						//infoTime=Time.time;
						print ("wood: "+source[0]+" Stone: "+source[1]+"Weight:"+weightNow);
						////////資料庫
						/*
						UPDATE Player
						key: PlayerID
						欄位：
						source[0]=source[0](wood)
						source[1]=source[1](stone)
						source[2]=source[2](metal)
						(可以再加其他你想到資源種類)

						UPDATE Source
						key: x,y
						quatity=pickup.quatity;

						*/
						picktime=Time.time;
					}
				}
				Status=0;
				break;
			case 2:
				//倉庫放這邊
				StockHouse StockNow = TriggerHouse.GetComponent<StockHouse>();
				StockNow.work=true;
				click=true;
				break;
			case 3:
				WorkHouse WorkNow = TriggerHouse.GetComponent<WorkHouse>();
				WorkNow.work=true;
				click=true;
				//合成放這邊				
				break;
			case 4:
				ScientificHouse ScienceNow = TriggerHouse.GetComponent<ScientificHouse>();
				ScienceNow.work=true;
				click=true;
				//精煉放這邊	
				break;
			case 5:
				//裝炸彈放這邊
				break;
			case 6:
				HouseLevelUp HouseNow = TriggerHouse.GetComponent<HouseLevelUp>();
				HouseNow.work=true;
				click=true;

				break;
			}	
		
		}
		//放大縮小
		//view = GUILayout.VerticalSlider ((float)view, (float)100.0, (float)30.0);

		if (Build == true) {
			// 蓋房子放這邊
			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			
				House BuildHouse=build_house.gameObject.transform.FindChild("HomePlane").gameObject.GetComponent<House>();
			
			if (GUI.Button (new Rect (Screen.width* 1/2, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"House")){
				if(source[0]>=BuildHouse.needSource[0] && source[1]>=BuildHouse.needSource[1] && source[2]>=BuildHouse.needSource[2]){
					BuildNow = (Rigidbody)Instantiate(build_house,new Vector3(x-5,y,z-5),build_house.transform.rotation);
					House thisBuild=BuildNow.gameObject.GetComponent<House>();
					//Player thisPlayer=this.gameObject.GetComponent<Player>();
					//thisBuild.player=thisPlayer;
					//thisBuild.PlayerID=PlayerID;
					Build=false;
					click=true;
				}

			}	
			string text="Wood:"+BuildHouse.needSource[0]+"\r\nStone:"+BuildHouse.needSource[1]+"\r\nMetal:"+BuildHouse.needSource[2];
			GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
			
			if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
				Build=false;	
				click=false;
			}



		}


	}


	void OnTriggerStay(Collider other){

		switch (other.tag)
		{
		case "Source":
			PickSource=other.gameObject;

			Status = 1;
			break;
		case "Stock":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			Status = 2;
			
			break;
		case "Work":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			//print (WorkHouse.)
			Status = 3;
			
			break;
		case "Science":
			TriggerHouse=other.gameObject;
			Bomb=true;		
			
			Status = 4;
			
			break;
		case "Enemy":
			Status = 5;
			
			break;
		case "House":
			Bomb=true;		
			Status = 6;
			TriggerHouse=other.gameObject;
			
			break;
		default:
			Status = 0;
			break;
		}
		
	}
	void OnTriggerExit(Collider other){
		Bomb=false;
		PickSource = null;
		Status = 0;
	}
}
