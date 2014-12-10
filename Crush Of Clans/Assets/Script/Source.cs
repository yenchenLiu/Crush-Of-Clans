using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {
	public int kind ,quatity;

	public static int output,outputQuataty;
//	public string Kind { get; set; }
	// Use this for initialization

	public static int getKind(){
		return output;
	}	
	public static int getQuatity(){
		return outputQuataty;
	}
	void Start () {
	
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
	}
}
