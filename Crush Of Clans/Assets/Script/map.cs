using UnityEngine;
using System.Collections;

public class map : MonoBehaviour {
	public GUISkin guiSkin;
	private float starttime;
	private GameObject NewSource,PlayerGameObject;
	public GameObject joy;
	public Rigidbody BuildNow,AnotherPlayer,AnotherPlayerNow;
	public GameObject[] source={null,null};//wood,stone;
	public Rigidbody[] build;
//	public bool DataLoad = false;

	private Source thisSource;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		//PlayerGameObject = (GameObject)Instantiate(Player,new Vector3(500,2,500),Player.transform.rotation);
		//PlayerGameObject.name="Player";
		StartCoroutine("server_function");
		State.ProcessTime = (int)Time.time;
		State.Process = 0;
		State.DataLoad = false;

	}
	IEnumerator server_function(){
		while (true) {
			Server.Send("31@@@@@");
			Server.Send("32@@@@@");
			yield return new WaitForSeconds(3);	

			if(State.PosX!=-1&&State.PosZ!=-1&&State.mainPlayerStatus==false){
				PlayerGameObject = (GameObject)Instantiate(Player,new Vector3(State.PosX,1.5f,State.PosZ),Player.transform.rotation);
				PlayerGameObject.name=State.name;

				State.mainPlayerStatus=true;
				Instantiate(joy,joy.transform.position,joy.transform.rotation);
				
			}

			foreach (string HouseIDKey in State.HouseID.Keys) {
				if(!State.HouseStatus[HouseIDKey]){
					BuildNow = (Rigidbody)Instantiate( build[ State.HouseKind[HouseIDKey] ] ,new Vector3(State.HousePositionX[HouseIDKey],0,State.HousePositionZ[HouseIDKey]),build[State.HouseKind[HouseIDKey]].transform.rotation);
					build thisBuild =BuildNow.gameObject.GetComponent<build>();
					thisBuild.HouseID=State.HouseID[HouseIDKey];
					State.HouseStatus[HouseIDKey]=true;
				}			
				if(State.HouseUpdate[HouseIDKey]<0){
					State.HouseID.Remove(HouseIDKey);
				}
			}
			foreach (string PlayerIDKey in State.PlayerID.Keys) {
				if(!State.PlayerStatus[PlayerIDKey]){
					AnotherPlayerNow = (Rigidbody)Instantiate( AnotherPlayer ,new Vector3(State.PlayerPositionX[PlayerIDKey],0,State.PlayerPositionZ[PlayerIDKey]),AnotherPlayer.transform.rotation);
					AnotherPlayer thisPlayer =AnotherPlayerNow.gameObject.GetComponent<AnotherPlayer>();
					thisPlayer.PlayerID=State.PlayerID[PlayerIDKey];
					State.PlayerStatus[PlayerIDKey]=true;
				}		
				if(State.PlayerUpdate[PlayerIDKey]<0){
					State.PlayerID.Remove(PlayerIDKey);
				}
			}
			if(State.isConnect==false){
				Application.LoadLevel("Start");
				State.isConnect=true;
			}
		}

	
	}

	void build_function(){
		foreach (string HouseIDKey in State.HouseID.Keys) {
			if(!State.HouseStatus[HouseIDKey]){
				BuildNow = (Rigidbody)Instantiate( build[ State.HouseKind[HouseIDKey] ] ,new Vector3(State.HousePositionX[HouseIDKey],0,State.HousePositionZ[HouseIDKey]),build[State.HouseKind[HouseIDKey]].transform.rotation);
				build thisBuild =BuildNow.gameObject.GetComponent<build>();
				thisBuild.HouseID=State.HouseID[HouseIDKey];
				State.HouseStatus[HouseIDKey]=true;
			}			
		}
	}
	// Update is called once per frame
	void Update () {
		if (State.DataLoad == false) {
			State.Process=(int)Time.time-State.ProcessTime;
			if (State.Process >= 5) {
				State.DataLoad = true;	
			}
		}
	
	}
	void OnGUI(){
		if (State.DataLoad == false) {
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
			GUI.Label(new Rect (0-Screen.width+(Screen.width*State.Process/5), Screen.height*4/5, (Screen.width) , Screen.height/20),"    ",guiSkin.button);
		}
	}

}
