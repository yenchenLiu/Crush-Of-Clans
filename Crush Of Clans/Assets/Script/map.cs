using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

public class map : MonoBehaviour {
	public GUISkin guiSkin;
	private float starttime;
	private GameObject NewSource,PlayerGameObject;
	public GameObject joy;
	public Rigidbody BuildNow,AnotherPlayer,AnotherPlayerNow,SourceNow;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation,Treasure,BigBomb;
	
	public Rigidbody[] source={null,null};//wood,stone;
	public Rigidbody[] build;
//	public bool DataLoad = false;

	private Source thisSource;
	public GameObject Player;

	// Use this for initialization
	void destroyThisHouse(string HouseID){
		
		try{
			Destroy(State.HouseGameObject[HouseID]);
			
			if (State.HouseID.ContainsKey(HouseID)) {
				State.HouseID.Remove(HouseID);
				
			}
			if(State.HouseKind[HouseID]==2){
				//DestorySource
			}
			if(State.HouseKind[HouseID]==3){
				//DestoryTool
			}

			State.HouseKind.Remove(HouseID);
			//State.HouseID.Remove(HouseID);
			State.HousePlayerID.Remove(HouseID);
			
			State.HouseHP.Remove(HouseID);
			State.HousePositionX.Remove(HouseID);
			State.HousePositionZ.Remove(HouseID);
			State.HouseLevel.Remove(HouseID);
			State.HouseStatusNow.Remove(HouseID);
			State.HouseStatus.Remove(HouseID);
			State.HouseUpdate.Remove (HouseID);

			State.HouseGameObject.Remove(HouseID);
			
		//	print ("error12");
			
		}catch(Exception){
			print ("error3");
		}
		
	}

	void destroyThisPlayer(string PlayerID){
		Destroy(State.AnotherPlayerGameObject[PlayerID]);
		
		if (State.PlayerID.ContainsKey (PlayerID)) {
			State.PlayerID.Remove (PlayerID);
		}
		State.PlayerPositionX.Remove(PlayerID);
		State.PlayerPositionZ.Remove(PlayerID);
		State.PlayerStatus.Remove(PlayerID);
		State.PlayerUpdate.Remove (PlayerID);
		
		//Destroy(State.AnotherPlayerGameObject[PlayerID));
		State.AnotherPlayerGameObject.Remove (PlayerID);
		                                      
	}
	void destroyThisSource(string SourceID){
		Destroy(State.SourceGameObject[SourceID]);
		
		if (State.SourceID.ContainsKey (SourceID)) {
			State.SourceID.Remove (SourceID);
		}
		State.Sourcekind.Remove (SourceID);
		State.SourceQuatity.Remove (SourceID);
		State.SourcePositionX.Remove(SourceID);
		State.SourcePositionZ.Remove(SourceID);
		State.SourceStatus.Remove(SourceID);
		State.SourceUpdate.Remove (SourceID);
		
		//Destroy(State.AnotherPlayerGameObject[PlayerID));
		State.SourceGameObject.Remove (SourceID);
		
	}
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
				Server.Send("31");
				yield return new WaitForSeconds(0.2f);	
			
				Server.Send("32");
				yield return new WaitForSeconds(0.2f);	
			
				Server.Send("33");
				yield return new WaitForSeconds(0.2f);
			//	Server.Send ("23");
			//try{		
				if(State.PosX!=-1&&State.PosZ!=-1&&State.mainPlayerStatus==false){
					PlayerGameObject = (GameObject)Instantiate(Player,new Vector3(State.PosX,1.5f,State.PosZ),Player.transform.rotation);
					PlayerGameObject.name=State.PlayerName;
					State.PlayerGameObject=PlayerGameObject;
					State.mainPlayerStatus=true;
					Instantiate(joy,joy.transform.position,joy.transform.rotation);
					
				}
				if(State.mainPlayerStatus==true){
					Server.Send ("41"+State.PosX+","+State.PosZ);
				}
				
				foreach (string HouseIDKey in State.HouseList.Keys) {
					if(State.HouseID.ContainsKey(HouseIDKey)){

						if(!State.HouseStatus[HouseIDKey]){
							BuildNow = (Rigidbody)Instantiate( build[ State.HouseKind[HouseIDKey] ] ,new Vector3(State.HousePositionX[HouseIDKey],0,State.HousePositionZ[HouseIDKey]),build[State.HouseKind[HouseIDKey]].transform.rotation);
							build thisBuild =BuildNow.gameObject.GetComponent<build>();
							State.HouseGameObject.Add(HouseIDKey,BuildNow.gameObject);
							thisBuild.HouseID=State.HouseID[HouseIDKey];
							thisBuild.PlayerID=State.HousePlayerID[HouseIDKey];
							thisBuild.gameObject.name = State.HouseID[HouseIDKey];
							thisBuild.kind=State.HouseKind[HouseIDKey];
							thisBuild.HouseLevel=State.HouseLevel[HouseIDKey];
							thisBuild.HP=State.HouseHP[HouseIDKey];
							thisBuild.MaxHP=State.HouseMaxHP[HouseIDKey];
							thisBuild.status=State.HouseStatusNow[HouseIDKey];
							if( State.HousePlayerID [HouseIDKey]!=State.PlayerName){
								BuildNow.gameObject.tag="Enemy";
							}

							if(State.HouseKind[HouseIDKey]==0){
								Destroy (BuildNow.gameObject.transform.FindChild("HomePlane").gameObject);
								BuildNow.gameObject.collider.enabled=true;
							}

							State.HouseStatus[HouseIDKey]=true;
							continue;
						}			
						State.HouseUpdate[HouseIDKey]--;
						build buildNow=State.HouseGameObject[HouseIDKey].GetComponent<build>();
						if(buildNow.status==2||buildNow.status==3||buildNow.status==4){
							if(State.HouseStatusNow[HouseIDKey]==1){
								Vector3 Pos=new Vector3(State.HousePositionX[HouseIDKey],4f,State.HousePositionZ[HouseIDKey]);
								
							GameObject BombAnimateNow=(GameObject) Instantiate(BigBomb,Pos,BigBomb.transform.rotation);
								
								
								Destroy(BombAnimateNow,2);
							}
						}
						if(State.HouseStatusNow[HouseIDKey]==6){
							Vector3 Pos=new Vector3(State.HousePositionX[HouseIDKey],4f,State.HousePositionZ[HouseIDKey]);
							
							GameObject BombAnimateNow=(GameObject) Instantiate(BigBomb,Pos,Building.transform.rotation);

							Destroy(BombAnimateNow,2);
							Server.Send ("58"+HouseIDKey+","+(-1*buildNow.HP));
								
						}
					
						if(State.HouseHP[HouseIDKey]<=0){
							Server.Send ("57"+HouseIDKey);
							destroyThisHouse(HouseIDKey);
							break;
						}
						
						if(State.HouseUpdate[HouseIDKey]<=0){
							print ("error15");
							destroyThisHouse(HouseIDKey);
							continue;
						}

						if(buildNow.kind!=State.HouseKind[HouseIDKey]){
							print ("error10");
							Destroy(State.HouseGameObject[HouseIDKey]);
							if(State.HousePlayerID[HouseIDKey]!=State.PlayerName){
								Vector3 Pos=new Vector3(State.HousePositionX[HouseIDKey]-1,5f,State.HousePositionZ[HouseIDKey]+1);
								
								GameObject animateNow = (GameObject)Instantiate (Building, Pos, Building.transform.rotation);
								Pos=new Vector3(State.HousePositionX[HouseIDKey],4f,State.HousePositionZ[HouseIDKey]);
								
								GameObject SomkeNow = (GameObject)Instantiate (Smoke, Pos, Building.transform.rotation);
								Pos=new Vector3(State.HousePositionX[HouseIDKey],4.1f,State.HousePositionZ[HouseIDKey]);
								
								GameObject SmokeAnimateNow = (GameObject)Instantiate (SmokeAnimation, Pos, Building.transform.rotation);
								
								Destroy (animateNow, 5);
								Destroy (SomkeNow, 5);
								Destroy (SmokeAnimateNow, 5);
							}
							BuildNow = (Rigidbody)Instantiate( build[ State.HouseKind[HouseIDKey] ] ,new Vector3(State.HousePositionX[HouseIDKey],0,State.HousePositionZ[HouseIDKey]),build[State.HouseKind[HouseIDKey]].transform.rotation);
							build thisBuild =BuildNow.gameObject.GetComponent<build>();
							State.HouseGameObject[HouseIDKey]=BuildNow.gameObject;
							thisBuild.HouseID=State.HouseID[HouseIDKey];
							thisBuild.PlayerID=State.HousePlayerID[HouseIDKey];
							thisBuild.gameObject.name = State.HouseID[HouseIDKey];
							thisBuild.kind=State.HouseKind[HouseIDKey];
							thisBuild.HouseLevel=State.HouseLevel[HouseIDKey];
							thisBuild.HP=State.HouseHP[HouseIDKey];
							thisBuild.MaxHP=State.HouseMaxHP[HouseIDKey];
							
							if( State.HousePlayerID [HouseIDKey]!=State.PlayerName){
								BuildNow.gameObject.tag="Enemy";
							}

							//destroyThisHouse(HouseIDKey);
							print ("error11");
							continue;
						}

						if(buildNow.HP>State.HouseHP[HouseIDKey]){
							Vector3 Pos=new Vector3(State.HousePositionX[HouseIDKey],4f,State.HousePositionZ[HouseIDKey]);
							
							GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
							Pos=new Vector3(State.HousePositionX[HouseIDKey],4.1f,State.HousePositionZ[HouseIDKey]);
							
							GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
							Pos=new Vector3(State.HousePositionX[HouseIDKey],4.2f,State.HousePositionZ[HouseIDKey]);
							
							GameObject BombAnimateNow=(GameObject) Instantiate(BombAnimation,Pos,Building.transform.rotation);
							
							Destroy(SomkeNow,3);
							Destroy(SmokeAnimateNow,3);
							Destroy(BombAnimateNow,3);
						}

						buildNow.kind=State.HouseKind[HouseIDKey];
						buildNow.HouseLevel=State.HouseLevel[HouseIDKey];
						buildNow.HP=State.HouseHP[HouseIDKey];
						buildNow.MaxHP=State.HouseMaxHP[HouseIDKey];
						buildNow.status=State.HouseStatusNow[HouseIDKey];
					
					}//if containkey
				}//house Foreach


				foreach (string PlayerIDKey in State.AnotherPlayerList.Keys) {
					if(State.PlayerID.ContainsKey(PlayerIDKey)){
						if(!State.PlayerStatus[PlayerIDKey]){
							AnotherPlayerNow = (Rigidbody)Instantiate( AnotherPlayer ,new Vector3(State.PlayerPositionX[PlayerIDKey],0,State.PlayerPositionZ[PlayerIDKey]),AnotherPlayer.transform.rotation);
							AnotherPlayer thisPlayer =AnotherPlayerNow.gameObject.GetComponent<AnotherPlayer>();
							State.AnotherPlayerGameObject.Add (PlayerIDKey,AnotherPlayerNow.gameObject);
							thisPlayer.PlayerID=State.PlayerID[PlayerIDKey];
							State.PlayerStatus[PlayerIDKey]=true;

						}		
						State.PlayerUpdate[PlayerIDKey]--;
						if(State.PlayerUpdate[PlayerIDKey]<=0){
							destroyThisPlayer(PlayerIDKey);
							continue;
							
						}
					}//if containkey
				}//Player foreach

				foreach (string SourceIDKey in State.SourceList.Keys) {
					if(State.SourceID.ContainsKey(SourceIDKey)){
						if(!State.SourceStatus[SourceIDKey]){
							print ("SourceError");
							SourceNow = (Rigidbody)Instantiate( source[State.Sourcekind[SourceIDKey]] ,new Vector3(State.SourcePositionX[SourceIDKey],0,State.SourcePositionZ[SourceIDKey]),source[State.Sourcekind[SourceIDKey]].transform.rotation);
							Source thisSource =SourceNow.gameObject.GetComponent<Source>();
							State.SourceGameObject.Add (SourceIDKey,SourceNow.gameObject);

							thisSource.SourceID=State.SourceID[SourceIDKey];
							thisSource.gameObject.name = State.SourceID[SourceIDKey];
						
							thisSource.kind=State.Sourcekind[SourceIDKey];
							thisSource.quatity=State.SourceQuatity[SourceIDKey];
							State.SourceStatus[SourceIDKey]=true;
						}		

						Source suildNow=State.SourceGameObject[SourceIDKey].GetComponent<Source>();
						
						
						suildNow.quatity=State.SourceQuatity[SourceIDKey];

						State.SourceUpdate[SourceIDKey]--;
						if(State.SourceUpdate[SourceIDKey]<=0){
							destroyThisSource(SourceIDKey);
							continue;
							
						}
					}
				}//Source foreach

				/*}catch(Exception ){
					print ("error6");
					//State.isConnect=false;
				
					//StartCoroutine("server_function");
					
					
				}*/
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
			if (State.Process >= 7) {
				State.DataLoad = true;	
			}
		}
		if(State.isConnect==false){
			Server.Send("99");
			State.LoginSuecess=false;
			State.isConnect=true;
			State.clientSocket.Close();//關閉通訊器
		//	State.th_Listen.Abort();//刪除執行緒
			
			Application.LoadLevel("Start");

		}
	}
	void OnGUI(){
		if (State.DataLoad == false) {
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", guiSkin.customStyles [0]);
			GUI.Label(new Rect (0-Screen.width+(Screen.width*State.Process/7), Screen.height*4/5, (Screen.width) , Screen.height/20),"    ",guiSkin.button);
		}
	}

}
