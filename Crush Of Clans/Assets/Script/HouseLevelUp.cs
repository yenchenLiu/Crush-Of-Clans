using UnityEngine;
using System.Collections;

public class HouseLevelUp : MonoBehaviour {
	//資料庫
	/*
	House的欄位：
	PlayerID 玩家帳號
	HouseID(看需不需要)
	x(x座標)
	z(z座標)
	*/
	//private int x, z;
	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	public GUISkin guiSkin;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;
	public Texture[] housePng; 
	private int[,] needSource = {{300,0,0,0,0,0,0,0},{500,100,0,0,0,0,0,0},{500,300,0,0,0,0,0,0},{200,50,0,0,0,0,0,0},{1000,500,200,0,0,0,0,0}};//st,w,sc,h,sky
	private string[] HouseName = {"倉庫","工作屋","精煉屋","住宅","世界奇觀"};
	public bool work;
	public string PlayerID;
	private GameObject player;
	private Player playerNow;
	public Rigidbody build_stock,build_work,build_science,build_house,build_101,buildNow;
	//public string[] sourceName={"Wood","Stone","Metal"};
	//private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭"};
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	private string[] HouseInfo={"儲存資源，依據倉庫等級、能存放的數量及種類會有所變化。","製作道具，依據工作屋等級、能製作的工具種類會有所變化。","合成資源，依據精煉屋等級、能合成的資源種類會有所變化。","建築地標，提供外出時的方向指引，等級提升後會有瞬間移動的功能可使用。","世界奇觀，能在一定時間後，能將除了自己以外所有建築夷平。"};
	public int selectHouse,HP;
	//public string PlayerID;
	
	// Use this for initialization
	void Start () {
		HP = 100;
		selectHouse = 0;
		bombCount = 0;
		bombSet = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(work==true){
			string text="";
			for(int i=0;i<8;i++){
				if(needSource[selectHouse,i]!=0){
					text+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+needSource[selectHouse,i];
				}
			}
		//	text+=+"\r\nwood:"+playerNow.source[0]+"/"+needSource[selectHouse,0]+"\r\nstone:"+playerNow.source[1]+"/"+needSource[selectHouse,1]+"\r\nmetal:"+playerNow.source[2]+"/"+needSource[selectHouse,2];
			
			GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
			GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
			GUI.Box (new Rect (Screen.width/10,Screen.height/16, Screen.width*1/5, Screen.width*1/5), housePng[selectHouse], guiSkin.box);
			//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
			GUI.Label (new Rect (Screen.width/10,Screen.height*9/20, Screen.width*1/5, Screen.width*1/5), text, guiSkin.label);
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 12/ 20 , Screen.width/3, Screen.height/8 ), HouseInfo[selectHouse], guiSkin.label);

			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 20 , Screen.width/3, Screen.height/10 ),"住宅",guiSkin.button)){
				selectHouse=3;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 20 , Screen.width/3, Screen.height/10 ),"倉庫",guiSkin.button)){
				selectHouse=0;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 20 , Screen.width/3, Screen.height/10 ),"工作屋",guiSkin.button)){
				selectHouse=1;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 8/ 20, Screen.width/3, Screen.height/10 ),"精煉屋",guiSkin.button)){
				selectHouse=2;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height*10/ 20 , Screen.width/3, Screen.height/10 ),"世界奇觀",guiSkin.button)){
				selectHouse=4;
			}

			if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
				work=false;		
				playerNow.click=false;
			}
			GUI.EndGroup();
			if(playerNow.source[0]>=needSource[selectHouse,0] && playerNow.source[1]>=needSource[selectHouse,1] && playerNow.source[2]>=needSource[selectHouse,2]&& playerNow.source[3]>=needSource[selectHouse,3]&& playerNow.source[4]>=needSource[selectHouse,4]&& playerNow.source[5]>=needSource[selectHouse,5]&& playerNow.source[6]>=needSource[selectHouse,6]&& playerNow.source[7]>=needSource[selectHouse,7]){
				GUI.enabled=true;
			}else{
				GUI.enabled=false;
				
			}
			if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"建造",guiSkin.button)){
				switch(selectHouse){
				case 0:
					buildNow=(Rigidbody)Instantiate(build_stock,this.transform.position,build_stock.transform.rotation);
					buildNow.gameObject.collider.enabled=true;
					
					StockHouse thisStockBuild = buildNow.GetComponent<StockHouse>();
					thisStockBuild.PlayerID=playerNow.PlayerID;
					thisStockBuild.HP=100;
					for(int i =0;i<8;i++){
						thisStockBuild.stockSource[i]=0;
						
						playerNow.source[i]=playerNow.source[i]-needSource[0,i];
					}
					playerNow.infomationText("Stock House is Build");
					playerNow.click=false;
					Destroy (this.gameObject);
					work=false;
					break;
				case 1:
					buildNow=(Rigidbody)Instantiate(build_work,this.transform.position,build_work.transform.rotation);
					buildNow.gameObject.collider.enabled=true;
					
					WorkHouse thisWorkBuild=buildNow.GetComponent<WorkHouse>();
					thisWorkBuild.PlayerID=playerNow.PlayerID;
					thisWorkBuild.HP=100;
					for(int i =0;i<8;i++){
						playerNow.source[i]=playerNow.source[i]-needSource[1,i];
					}
					playerNow.infomationText("Work House is Build");
					playerNow.click=false;
					Destroy (this.gameObject);
					work=false;
					break;
				case 2:
					buildNow=(Rigidbody)Instantiate(build_science,this.transform.position,build_science.transform.rotation);
					buildNow.gameObject.collider.enabled=true;
					ScientificHouse thisScienceBuild=buildNow.GetComponent<ScientificHouse>();
					thisScienceBuild.PlayerID=playerNow.PlayerID;
					thisScienceBuild.HP=100;
					for(int i =0;i<8;i++){
						playerNow.source[i]=playerNow.source[i]-needSource[2,i];
					}
					
					playerNow.infomationText("Science House is Build");
					playerNow.click=false;
					Destroy (this.gameObject);
					work=false;
					break;
				case 3:
					if(!playerNow.isHomeExsist){
						buildNow=(Rigidbody)Instantiate(build_house,this.transform.position,build_house.transform.rotation);
						buildNow.gameObject.collider.enabled=true;
						Home thisHomeBuild=buildNow.GetComponent<Home>();
						thisHomeBuild.PlayerID=playerNow.PlayerID;
						thisHomeBuild.HP=100;
						for(int i =0;i<8;i++){
							playerNow.source[i]=playerNow.source[i]-needSource[3,i];
						}
						
						playerNow.infomationText("Home is Build");
						playerNow.click=false;
						Destroy (this.gameObject);
						playerNow.isHomeExsist=true;
						
						work=false;
					
					}
					break;
				case 4:
					if(!playerNow.isHomeExsist){
						buildNow=(Rigidbody)Instantiate(build_house,this.transform.position,build_house.transform.rotation);
						buildNow.gameObject.collider.enabled=true;
						Home thisHomeBuild=buildNow.GetComponent<Home>();
						thisHomeBuild.PlayerID=playerNow.PlayerID;
						thisHomeBuild.HP=100;
						for(int i =0;i<8;i++){
							playerNow.source[i]=playerNow.source[i]-needSource[3,i];
						}
						
						playerNow.infomationText("Home is Build");
						playerNow.click=false;
						Destroy (this.gameObject);
						playerNow.isHomeExsist=true;
						
						work=false;
						
					}
					break;
				}

				Vector3 Pos=new Vector3(buildNow.gameObject.transform.position.x-1,buildNow.gameObject.transform.position.y+5,buildNow.gameObject.transform.position.z+1);
				GameObject animateNow=(GameObject) Instantiate(Building,Pos,Building.transform.rotation);
				Pos=new Vector3(this.transform.position.x,this.transform.position.y+4,this.transform.position.z);
				
				GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
				Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.1f,this.transform.position.z);
				
				GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
				
				Destroy(animateNow,3);
				Destroy(SomkeNow,3);
				Destroy(SmokeAnimateNow,3);

			}
				GUI.enabled=true;
			
		
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
		print( (State.bombTotal).ToString());
		yield return new WaitForSeconds(2);
		
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
