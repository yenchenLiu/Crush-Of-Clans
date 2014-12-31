using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

public class State : MonoBehaviour {
	static public string name=null;
	static public float PosX=0f,PosZ=0f;
	static public Dictionary<string,int[]> PlayerPosition=null;
	static public Socket clientSocket;
	static public int[] source = new int[8];
	static public bool chkThread = true;
	static public bool LoginSuecess = false;

}
