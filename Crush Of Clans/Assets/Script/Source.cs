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
	public int kind ,quatity,x,z;

	public bool triggerStatus;

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
		if(other.tag == "Player") {
			output=this.kind;
			outputQuataty=this.quatity;
			//print (output);
		}
		if (other.tag == "Source") {s
			//資料庫
			/*
			DELETE Source
			Key: x,z
			*/
			Destroy(this.gameObject);	
		}
	}
	void OnTriggerExit(Collider toher){

			triggerStatus=false;		
	}
}
