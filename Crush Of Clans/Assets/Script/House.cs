using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {
	private bool build;
	private int x, y, z;
	private bool player;
	// Use this for initialization
	void Start () {
		this.build = true;
		this.renderer.material.color =Color.green;

	}
	
	// Update is called once per frame
	void Update () {
		//if (this.build == true) {
			x=5*((int)GameObject.FindGameObjectWithTag ("Player").transform.position.x/5) +5;
			y=(int)GameObject.FindGameObjectWithTag ("Player").transform.position.y;
			z=5*((int)GameObject.FindGameObjectWithTag ("Player").transform.position.z/5);
			this.transform.parent.transform.position = new Vector3 (x -10, y, z);

		
		//}

	}
	void OnGUI(){
		if (this.build == true) {
				
			if (GUI.Button (new Rect (Screen.width* 1/3, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"OK")){

				this.transform.parent.collider.enabled=true;
			//	this.transform.parent.hingeJoint.connectedBody = null;
				//this.transform.parent.rigidbody.GetComponent<FixedJoint>().connectedBody = null;

				//this.transform.parent.transform.parent=null;
				//this.renderer.enabled=false;
				build = false;
				Destroy(this.gameObject);
			}			
		}
		if (GUI.Button (new Rect (0, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"Cancel")){
			Destroy(this.transform.parent.gameObject);
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
