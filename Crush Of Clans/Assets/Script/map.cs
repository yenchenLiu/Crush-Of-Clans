using UnityEngine;
using System.Collections;

public class map : MonoBehaviour {
	private float starttime;
	private GameObject NewSource,PlayerGameObject;
	private Rigidbody BuildNow;
	public GameObject[] source={null,null};//wood,stone;
	public Rigidbody build_stock,build_work,build_science,buildNow;
	public Rigidbody build_house;
	private Source thisSource;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		//PlayerGameObject = (GameObject)Instantiate(Player,new Vector3(500,2,500),Player.transform.rotation);
		//PlayerGameObject.name="Player";

		//starttime = Time.time;
		//資料庫
		/*
		SELECT House
		Key PlayerID
		欄位：
		PlayerID
		x(x座標)
		z(z座標)
		HouseID(看需不需要)

		SELECT StockHouse
		欄位：
		PlayerID
		x(x座標)
		z(z座標)
		HouseID(看需不需要)
		StockHouseLevel
		StockHouseHP
		StockSouce[0]
		StockSouce[1]
		StockSouce[2]

		SELECT WorkHouse
		欄位：
		PlayerID
		x(x座標)
		z(z座標)
		HouseID(看需不需要)
		StockHouseLevel
		StockHouseHP

		SELECT ScienceHouse
		欄位：
		PlayerID
		x(x座標)
		z(z座標)
		HouseID(看需不需要)
		StockHouseLevel
		StockHouseHP

		SELECT Source
		欄位：
		x(x座標)
		z(z座標)
		kind(種類)
		quatity(數量)

		這些全部先印出來就好
		*/
	}
	
	// Update is called once per frame
	void Update () {
		//print (Time.time - starttime);
		/*
		if ( (int)(Time.time - starttime )% 10 == 0 ) {
			GameObject[] temp = GameObject.FindGameObjectsWithTag("Source");

			if(temp.Length<=5000){
				int x, z, kind;
				//for(int i=0;i<5;i++){
					x= 10*(UnityEngine.Random.Range(0, 2000)/10);//r.Next(2000);
					z= 10*(UnityEngine.Random.Range(0, 2000)/10);//r.Next(2000);
					kind= UnityEngine.Random.Range(0, 2);//r.Next(2000);
					
					NewSource = (GameObject)Instantiate(source[kind],new Vector3(x,0,z),source[kind].transform.rotation);
					thisSource =NewSource.GetComponent<Source>();
					thisSource.quatity=UnityEngine.Random.Range(50, 100);
				

			//	}

			}	
		}*/
	}
}
