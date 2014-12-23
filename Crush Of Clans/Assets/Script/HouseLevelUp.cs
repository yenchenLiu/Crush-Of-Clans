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
	public int[,] needSource = {{20,10,0},{200,10,5},{50,20,10}};
	public bool work;
	public string PlayerID;
	private GameObject player;
	private Player playerNow;
	public Rigidbody build_stock,build_work,build_science,buildNow;
	//public string PlayerID;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(work==true){
			string text="Stock House"+"\r\nwood:"+needSource[0,0]+"\r\nstone:"+needSource[0,1]+"\r\nmetal:"+needSource[0,2];
			if (GUI.Button (new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),text)){

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
				/*
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
				*/
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
				*/
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
				*/
					playerNow.click=false;
					Destroy (this.gameObject);
					work=false;
				}
			}
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
