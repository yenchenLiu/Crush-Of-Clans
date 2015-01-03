using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

	public Texture FunctionButton;
	public GUISkin guiSkin;
	private bool build;
	private int x, z;
//	private bool player;
	public Player thisPlayer;
	public int[] needSource = {200,0,0};
	public string PlayerID;
	private GameObject player;
	
	// Use this for initialization
	void Start () {


		this.build = true;
		this.renderer.material.color =Color.green;
		player = GameObject.Find (State.name).gameObject;
		thisPlayer = player.GetComponent<Player> ();
		PlayerID = thisPlayer.PlayerID;

	}
	
	// Update is called once per frame
	void Update () {
		//if (this.build == true) {
			x=5*((int)player.transform.position.x/5) -5;
			//y=(int)GameObject.FindGameObjectWithTag ("Player").transform.position.y;
			z=5*((int)player.transform.position.z/5)-5;
			this.transform.parent.transform.position = new Vector3 (x , 0, z);

		
		//}

	}
	void OnGUI(){
		if (this.build == true) {
				
			if (GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),FunctionButton,guiSkin.customStyles[3])){

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
				thisPlayer.infomationText("The House is Build");
			//	thisPlayer.infotext="The House is Build";
			//	thisPlayer.info=true;
			//	thisPlayer.infoTime=Time.time;
				HouseLevelUp thisParent= this.transform.parent.GetComponent<HouseLevelUp>();
				thisParent.PlayerID=PlayerID;

				thisPlayer.click=false;
				build = false;
				Vector3 Pos=new Vector3(this.transform.position.x-1,this.transform.position.y+5,this.transform.position.z+1);
				GameObject animateNow=(GameObject) Instantiate(thisParent.Building,Pos,thisParent.Building.transform.rotation);
				Pos=new Vector3(this.transform.position.x-1,this.transform.position.y+4,this.transform.position.z+1);
				
				GameObject SomkeNow=(GameObject) Instantiate(thisParent.Smoke,Pos,thisParent.Building.transform.rotation);
				Pos=new Vector3(this.transform.position.x-1,this.transform.position.y+4.1f,this.transform.position.z+1);
				
				GameObject SmokeAnimateNow=(GameObject) Instantiate(thisParent.SmokeAnimation,Pos,thisParent.Building.transform.rotation);
				
				Destroy(animateNow,3);
				Destroy(SomkeNow,3);
				Destroy(SmokeAnimateNow,3);
				Destroy(this.gameObject);
				

			}			
		}
		if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X",guiSkin.button)){
			thisPlayer.click=false;
			
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
