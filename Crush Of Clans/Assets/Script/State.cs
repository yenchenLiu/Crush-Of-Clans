using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

public class State : MonoBehaviour {
	//Player
	static public string PlayerName=null;
	static public string ID=null;
	static public string Password=null;
	static public float PosX=-1f,PosZ=-1f;
	static public int[] source = new int[8];
	static public int bombTotal=0;
	static public bool mainPlayerStatus = false;
	static public bool DataLoad=false;
	static public GameObject PlayerGameObject=null;

	
	//Player End
	
	//AnotherPlyer
	static public Dictionary<string,string> PlayerID=new Dictionary<string,string>();
	static public Dictionary<string,float> PlayerPositionX=new Dictionary<string,float>();
	static public Dictionary<string,float> PlayerPositionZ=new Dictionary<string,float>();
	static public Dictionary<string,int> PlayerUpdate=new Dictionary<string,int>();

	static public Dictionary<string,bool> PlayerStatus=new Dictionary<string,bool>();//is Instantiate or not
	static public Dictionary<string,GameObject> AnotherPlayerGameObject= new Dictionary<string,GameObject>();
	static public Dictionary<string,string> AnotherPlayerList= new Dictionary<string,string>();

	//AnotherPlyer End

	/// House
	static public Dictionary<string,string> HouseID=new Dictionary<string,string> ();
	static public Dictionary<string,int> HousePositionX=new  Dictionary<string,int> ();
	static public Dictionary<string,int> HousePositionZ=new Dictionary<string,int> (); 
	static public Dictionary<string,int> HouseHP=new Dictionary<string,int>();
	static public Dictionary<string,int> HouseKind=new Dictionary<string,int> ();
	static public Dictionary<string,int> HouseLevel=new Dictionary<string,int>();
	static public Dictionary<string,string> HousePlayerID=new Dictionary<string,string>();
	static public Dictionary<string,int> HouseUpdate=new Dictionary<string,int>();
	static public Dictionary<string,int> HouseStatusNow=new Dictionary<string,int>();

	static public Dictionary<string,GameObject> HouseGameObject=new Dictionary<string,GameObject>();
	static public Dictionary<string,string> HouseList=new Dictionary<string,string>();
	


	static public Dictionary<string,bool> HouseStatus=new Dictionary<string,bool>();//is Instantiate or not

	
	/// House End

	/// Source
	static public Dictionary<string,string> SourceID=new Dictionary<string,string> ();
	static public Dictionary<string,int> Sourcekind=new Dictionary<string,int> ();
	static public Dictionary<string,int> SourcePositionX=new  Dictionary<string,int> ();
	static public Dictionary<string,int> SourcePositionZ=new Dictionary<string,int> (); 
	static public Dictionary<string,int[]> SourceQuatity =new Dictionary<string,int[]> (); 
	static public Dictionary<string,bool> SourceStatus =new Dictionary<string,bool> (); 
	static public Dictionary<string,int> SourceUpdate=new Dictionary<string,int>();
	
	static public Dictionary<string,GameObject> SourceGameObject=new Dictionary<string,GameObject>();
	static public Dictionary<string,string> SourceList=new Dictionary<string,string>();
	
	/// Source End
	
	static public Socket clientSocket;
	static public bool chkThread = true;
	static public bool LoginSuecess = false;
	static public bool SignSuecess = false;
	static public int Process;
	static public int ProcessTime;
	static public bool isConnect=false;
}
