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
			if (GUI.Button (new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),"Stock House")){
				buildNow=(Rigidbody)Instantiate(build_stock,this.transform.position,build_stock.transform.rotation);
				buildNow.gameObject.collider.enabled=true;
				StockHouse thisBuild=buildNow.GetComponent<StockHouse>();
				thisBuild.PlayerID=playerNow.PlayerID;
				thisBuild.HP=100;
				Destroy(buildNow.transform.FindChild("StockPlane").gameObject);
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
			if (GUI.Button (new Rect (Screen.width* 3/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),"Work House")){
				buildNow=(Rigidbody)Instantiate(build_work,this.transform.position,build_stock.transform.rotation);
				buildNow.gameObject.collider.enabled=true;

				StockHouse thisBuild=buildNow.GetComponent<StockHouse>();
				thisBuild.PlayerID=playerNow.PlayerID;
				thisBuild.HP=100;
				Destroy(buildNow.transform.FindChild("WorkPlane").gameObject);	
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

			if (GUI.Button (new Rect (Screen.width* 5/8, Screen.height* 1/ 4 , Screen.width/4, Screen.height/2 ),"Science House")){
				buildNow=(Rigidbody)Instantiate(build_science,this.transform.position,build_stock.transform.rotation);
				buildNow.gameObject.collider.enabled=true;
				StockHouse thisBuild=buildNow.GetComponent<StockHouse>();
				thisBuild.PlayerID=playerNow.PlayerID;
				thisBuild.HP=100;
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
				Destroy(buildNow.transform.FindChild("SciencePlane").gameObject);
				playerNow.click=false;
				Destroy (this.gameObject);
				work=false;
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
