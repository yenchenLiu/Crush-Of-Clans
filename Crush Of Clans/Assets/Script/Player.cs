using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private int Status;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈"};
	private bool Build=false,click=false;
	private int stone,wood;
	public Rigidbody build_stock,build_work,build_science,BuildNow;
	// Use this for initialization
	void Start () {
		Status = 0;
		stone = 0;
		wood = 0;
		Build=false;
		click = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.W)) {
			this.transform.Translate(new Vector3(0,0,-1));
		}
		
		if (Input.GetKey (KeyCode.S)) {
			this.transform.Translate(new Vector3(0,0,1));
		}
		
		if (Input.GetKey (KeyCode.D)) {
			this.transform.Translate(new Vector3(-1,0,0));
		}
		
		if (Input.GetKey (KeyCode.A)) {
			this.transform.Translate(new Vector3(1,0,0));
		}


	}

	//Status = 0 虛擬搖桿 移動
	void OnGUI(){
		if ( GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),StatusName[Status])){

			switch (Status){
			case 0:
				Build = true;
				click=false;
				break;
			case 1:			
				//採集放這邊
						
				break;
			case 2:
				//倉庫放這邊
				
				break;
			case 3:
				//合成放這邊				
				break;
			case 4:
				//精煉放這邊	
				break;
			case 5:
				//裝炸彈放這邊
				break;
			}	
		
		}

		if (Build == true) {
			// 蓋房子放這邊

			if(click==false){
				if (GUI.Button (new Rect (0, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Stock House")){
					BuildNow = (Rigidbody)Instantiate(build_stock,this.transform.position,build_stock.transform.rotation);
					BuildNow.hingeJoint.connectedBody=this.rigidbody;
					click=true;
				}
				if (GUI.Button (new Rect (Screen.width* 1/3, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Work House")){
					BuildNow = (Rigidbody)Instantiate(build_work,this.transform.position,build_stock.transform.rotation);
					BuildNow.hingeJoint.connectedBody=this.rigidbody;
					click=true;
				}		
				if (GUI.Button (new Rect (Screen.width* 2/3, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Science House")){
					BuildNow = (Rigidbody)Instantiate(build_science,this.transform.position,build_stock.transform.rotation);
					BuildNow.hingeJoint.connectedBody=this.rigidbody;
					click=true;
				}	
			}else{
				if (GUI.Button (new Rect (Screen.width* 1/3, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"OK")){
					BuildNow.hingeJoint.connectedBody = null;
					
					Build=false;				
				}	
				if (GUI.Button (new Rect (0, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"Cancel")){
					Destroy(BuildNow.gameObject);
					Build=false;				
				}
			}


		}


	}
	void OnTriggerEnter(Collider other){

		switch (other.tag)
		{
		case "Source":

			Status = 1;
			break;
		case "Stock":
			Status = 2;
			
			break;
		case "Work":
			Status = 3;
			
			break;
		case "Science":
			Status = 4;
			
			break;
		case "Enemy":
			Status = 5;
		
			break;
		default:
			Status = 0;
			break;
		}

	}
	void OnTriggerExit(Collider other){
		Status = 0;
	}
}
