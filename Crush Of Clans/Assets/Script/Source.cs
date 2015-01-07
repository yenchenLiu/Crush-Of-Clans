using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {
	//資料庫
		/*
		Source的欄位：
		SourceID(看需不需要)
		x(x座標)
		z(z座標)
		kind(種類)
		quatity(數量)

		*/
	public int kind ,x,z;
	public int[] quatity;
	public bool triggerStatus;
	public string SourceID;
	public static int output,outputQuataty;
//	public string Kind { get; set; }
	// Use this for initialization

	void Start () {
		//triggerStatus = false;

	}

	// Update is called once per frame
	void Update () {

	}
	void OnTriggerStay(Collider other){

		/*if (other.tag !="Player" ||other.tag !="Animation" ||other.tag !="bomb" ||other.tag !="Untagged") {
			Server.Send("3A"+SourceID+"@@@@@");
		}*/
	}
	void OnTriggerExit(Collider toher){

			triggerStatus=false;		
	}
}
