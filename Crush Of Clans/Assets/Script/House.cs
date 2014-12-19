using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

	private bool build;
	private int x, z;
//	private bool player;
	public Player thisPlayer;
	public int[] needSource = {10,5,0};
	public string PlayerID;
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		this.build = true;
		this.renderer.material.color =Color.green;
		player = GameObject.Find ("Player").gameObject;
		thisPlayer = player.GetComponent<Player> ();
		PlayerID = thisPlayer.PlayerID;

	}
	
	// Update is called once per frame
	void Update () {
		//if (this.build == true) {
			x=5*((int)player.transform.position.x/5) -5;
			//y=(int)GameObject.FindGameObjectWithTag ("Player").transform.position.y;
			z=5*((int)player.transform.position.z/5)-5;
			this.transform.parent.transform.position = new Vector3 (x -10, 0, z);

		
		//}

	}
	void OnGUI(){
		if (this.build == true) {
				
			if (GUI.Button (new Rect (Screen.width* 3/4, Screen.height* 4/ 5 , Screen.width/4, Screen.height/5 ),"Build")){

				this.transform.parent.collider.enabled=true;
				//Player thisPlayer = GameObject.Find("Player").gameObject.GetComponent<Player>();
				for(int i =0;i<=2;i++){
					thisPlayer.source[i]=thisPlayer.source[i]-needSource[i];
				}
				//資料庫
				/*
				INSERT House
				Key:PlayerID
				欄位：
				PlayerID
				x,z(屋子的座標)

				UPDATE Player
				欄位：
				source[0]=player.source[0](wood)
				source[1]=player.source[1](stone)
				source[2]=player.source[2](metal)
				*/
				HouseLevelUp thisParent= this.transform.parent.GetComponent<HouseLevelUp>();
				thisParent.PlayerID=PlayerID;
				build = false;

				Destroy(this.gameObject);
			}			
		}
		if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
			Destroy (this.transform.parent.gameObject);
			//build=false;				
		}

	}
	void OnTriggerStay(Collider other){
		if (other.tag != "Player") {
			this.renderer.material.color =Color.red;
			this.build = false;	
		//	print (other.tag);
		}

		
	}
	void OnTriggerExit(Collider other){
		this.renderer.material.color =Color.green;
		this.build = true;
	}
}
