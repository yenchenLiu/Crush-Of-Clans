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
	public GUISkin guiSkin;
	public Texture[] housePng; 
	public int[,] needSource = {{20,10,0,0,0,0,0,0},{200,10,5,0,0,0,0,0},{50,20,10,0,0,0,0,0}};
	private string[] HouseName = {"倉庫","工作屋","精煉屋"};
	public bool work;
	public string PlayerID;
	private GameObject player;
	private Player playerNow;
	public Rigidbody build_stock,build_work,build_science,buildNow;
	//public string[] sourceName={"Wood","Stone","Metal"};
	//private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭"};
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	public string[] HouseInfo={"儲存資源，依據倉庫等級、能存放的數量及種類會有所變化。","製作道具，依據工作屋等級、能製作的工具種類會有所變化。","合成資源，依據精煉屋等級、能合成的資源種類會有所變化。"};
	public int selectHouse;
	//public string PlayerID;
	
	// Use this for initialization
	void Start () {
		selectHouse = 0;
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
			GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), HouseInfo[selectHouse], guiSkin.label);

			
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 2/ 16 , Screen.width/3, Screen.height/8 ),"倉庫",guiSkin.button)){
				selectHouse=0;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 4/ 16 , Screen.width/3, Screen.height/8 ),"工作屋",guiSkin.button)){
				selectHouse=1;
			}
			if (GUI.Button (new Rect (Screen.width*3/8, Screen.height* 6/ 16 , Screen.width/3, Screen.height/8 ),"精煉屋",guiSkin.button)){
				selectHouse=2;
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
					buildNow=(Rigidbody)Instantiate(build_work,this.transform.position,build_stock.transform.rotation);
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
					buildNow=(Rigidbody)Instantiate(build_science,this.transform.position,build_stock.transform.rotation);
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
				}

			}
				GUI.enabled=true;
			
			//GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
			
		/*	if (GUI.Button (new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),text)){

				if(playerNow.source[0]>=needSource[0,0] && playerNow.source[1]>=needSource[0,1] && playerNow.source[2]>=needSource[0,2]){
				
					buildNow=(Rigidbody)Instantiate(build_stock,this.transform.position,build_stock.transform.rotation);
					buildNow.gameObject.collider.enabled=true;

					StockHouse thisBuild=buildNow.GetComponent<StockHouse>();
					thisBuild.PlayerID=playerNow.PlayerID;
					thisBuild.HP=100;
					for(int i =0;i<=2;i++){
						thisBuild.stockSource[i]=0;
						
						playerNow.source[i]=playerNow.source[i]-needSource[0,i];
					}
					playerNow.infomationText("Stock House is Build");
				
				//資料庫

				INSERT StockHouse
				Key:PlayerID
				欄位：
				PlayerID
				StockHouseLevel=1;
				StockHouseHP=100;(暫定耐久)
				StockSouce[0]=0(wood)
				StockSouce[1]=0(stone)
				StockSouce[2]=0(metal)
				x(x座標)
				z(z座標)

				DELETE House
				Key:PlayerID,x,z(或是要弄個房屋的ID)
				
				
				UPDATE Player
				source[0]=playerNow.source[0](wood)
				source[1]=playerNow.source[1](stone)
				source[2]=playerNow.source[2](metal)

				playerNow.click=false;
				Destroy (this.gameObject);
				work=false;
				
				}
			}
			text="Work House"+"\r\nwood:"+needSource[1,0]+"\r\nstone:"+needSource[1,1]+"\r\nmetal:"+needSource[1,2];
			
			if (GUI.Button (new Rect (Screen.width* 3/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),text)){
				if(playerNow.source[0]>=needSource[1,0] && playerNow.source[1]>=needSource[1,1] && playerNow.source[2]>=needSource[1,2]){
				
				buildNow=(Rigidbody)Instantiate(build_work,this.transform.position,build_stock.transform.rotation);
				buildNow.gameObject.collider.enabled=true;

				WorkHouse thisBuild=buildNow.GetComponent<WorkHouse>();
				thisBuild.PlayerID=playerNow.PlayerID;
				thisBuild.HP=100;
					for(int i =0;i<=2;i++){
						playerNow.source[i]=playerNow.source[i]-needSource[1,i];
					}
					playerNow.infomationText("Work House is Build");
					
				//Destroy(buildNow.transform.FindChild("WorkPlane").gameObject);	
				//資料庫
				/*
				INSERT WorkHouse
				Key:PlayerID
				欄位：
				PlayerID
				WorkHouseHouseLevel=1;
				WorkHouseHouseHP=100;(暫定耐久)
				x(x座標)
				z(z座標)

				DELETE House
				Key:PlayerID,x,z(或是要弄個房屋的ID)
				

				UPDATE Player
				source[0]=playerNow.source[0](wood)
				source[1]=playerNow.source[1](stone)
				source[2]=playerNow.source[2](metal)

				playerNow.click=false;
				Destroy (this.gameObject);
				work=false;
				}
			}		
			text="Science House"+"\r\nwood:"+needSource[2,0]+"\r\nstone:"+needSource[2,1]+"\r\nmetal:"+needSource[2,2];
			
			if (GUI.Button (new Rect (Screen.width* 5/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),text)){
				if(playerNow.source[0]>=needSource[2,0] && playerNow.source[1]>=needSource[2,1] && playerNow.source[2]>=needSource[2,2]){
				
				buildNow=(Rigidbody)Instantiate(build_science,this.transform.position,build_stock.transform.rotation);
				buildNow.gameObject.collider.enabled=true;
				ScientificHouse thisBuild=buildNow.GetComponent<ScientificHouse>();
				thisBuild.PlayerID=playerNow.PlayerID;
				thisBuild.HP=100;
				for(int i =0;i<=2;i++){
					playerNow.source[i]=playerNow.source[i]-needSource[2,i];
				}

					playerNow.infomationText("Science House is Build");
					
				//資料庫
				/*
				INSERT ScienceHouse
				Key:PlayerID
				欄位：
				PlayerID
				Science HouseLevel=1;
				Science HouseHP=100;(暫定耐久)
				x(x座標)
				z(z座標)

				DELETE House
				Key:PlayerID,x,z(或是要弄個房屋的ID)
				

				UPDATE Player
				source[0]=playerNow.source[0](wood)
				source[1]=playerNow.source[1](stone)
				source[2]=playerNow.source[2](metal)

					playerNow.click=false;
					Destroy (this.gameObject);
					work=false;
				}
			}*/

		}

	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			player=other.gameObject;
			playerNow=player.GetComponent<Player>();
		}
		
	}
}
