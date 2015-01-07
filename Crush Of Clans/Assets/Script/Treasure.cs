using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {
	public int[] source;
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {
			Player PlayerNow =other.gameObject.GetComponent<Player>();
			string text="取得";
			
			for(int i=0;i<8;i++){
				PlayerNow.source[i]+=source[i];
				if(source[i]!=0){
					text+=sourceName[i]+":"+source[i]+",";
				}
			}

			PlayerNow.infomationText(text);
			Destroy(this.gameObject);
		}
	}

}
