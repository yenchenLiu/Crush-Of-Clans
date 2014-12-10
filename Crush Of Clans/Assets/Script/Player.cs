using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int pick;
	private int Status;//0無狀態可蓋房子、 1採資源、2倉庫、3工作屋、4精煉屋、5裝炸彈 
	private string[] StatusName={"蓋房子","採資源","倉庫","合成","精煉","裝炸彈"};
	private bool Build=false,click=false;
	public static int[] source={0,0};//0 wood, 1 stone;
	private GameObject PickSource;
	public Rigidbody build_stock,build_work,build_science;
	//private Rigidbody[] buildclick = {build_stock,build_work,build_science};
	private Rigidbody BuildNow;
	private int x,y,z;
	private float view;

	// Use this for initialization
	void Start () {
		pick = 10;
		Status = 0;
		view = 50;
		Build=false;
		click = false;

	}
	
	// Update is called once per frame
	void Update () {
				x = (int)this.transform.position.x;
				y = (int)this.transform.position.y;
				z = (int)this.transform.position.z;
				this.transform.FindChild ("Main Camera").camera.fieldOfView = view;

				if (Input.GetKey (KeyCode.W)) {
					this.transform.Translate (new Vector3 (0, 0, -1));

					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,180,0);
				}
		
				if (Input.GetKey (KeyCode.S)) {
					this.transform.Translate (new Vector3 (0, 0, 1));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,0,0);
			
				}
		
				if (Input.GetKey (KeyCode.D)) {
					this.transform.Translate (new Vector3 (-1, 0, 0));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,270,0);
			
				}
		
				if (Input.GetKey (KeyCode.A)) {
					this.transform.Translate (new Vector3 (1, 0, 0));
					this.transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0,90,0);
			
				}



	}

	//Status = 0 虛擬搖桿 移動
	void OnGUI(){
		GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height / 10),"");
		GUI.Box (new Rect ( 0, 0, Screen.width/4, Screen.height / 10),"Wood");
		GUI.Box (new Rect ( Screen.width/4, 0, Screen.width/4, Screen.height / 10),source[0].ToString());
		GUI.Box (new Rect ( 2*Screen.width/4, 0, Screen.width/4, Screen.height / 10),"Stone");
		GUI.Box (new Rect ( 3*Screen.width/4, 0, Screen.width/4, Screen.height / 10),source[1].ToString());

		if ( GUI.Button (new Rect (Screen.width* 5 / 6, Screen.height* 3 / 4 , Screen.width/6, Screen.height/4 ),StatusName[Status])){

			switch (Status){
			case 0:
				Build = true;
				click=false;
				break;
			case 1:			
				//採集放這邊
				//source++;
					
				int kind=Source.getKind();
				int quatity=Source.getQuatity();
				Source pickup=PickSource.GetComponent<Source>();
				print (quatity);
				if(quatity<=pick){
					source[kind]+=quatity;
					pickup.quatity-=quatity;
					Destroy(PickSource.gameObject);
				}else{
					source[kind]+=pick;		
					pickup.quatity-=pick;
					
				}

				print ("wood: "+source[0]+" Stone: "+source[1]);


				Status=0;
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
		view = GUILayout.VerticalSlider ((float)view, (float)100.0, (float)30.0);

		if (Build == true) {
			// 蓋房子放這邊

			if(click==false){
				if (GUI.Button (new Rect (0, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Stock House")){
					//GameObject BuildUp = (Rigidbody)Instantiate(build_stock,this.transform.position,build_stock.transform.rotation);
					BuildNow = (Rigidbody)Instantiate(build_stock,new Vector3(x-10,y,z),build_stock.transform.rotation);
					//BuildNow.gameObject.transform.parent=this.transform;
				//	BuildNow.GetComponent<FixedJoint>().connectedBody=this.rigidbody;
					click=true;
				}
				if (GUI.Button (new Rect (Screen.width* 1/3, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Work House")){
					BuildNow = (Rigidbody)Instantiate(build_work,new Vector3(x-10,y,z),build_stock.transform.rotation);
					//BuildNow.hingeJoint.connectedBody=this.rigidbody;
					click=true;
				}		
				if (GUI.Button (new Rect (Screen.width* 2/3, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),"Science House")){

					BuildNow = (Rigidbody)Instantiate(build_science,new Vector3(x-10,y,z),build_stock.transform.rotation);
					//BuildNow.hingeJoint.connectedBody=this.rigidbody;
					click=true;
				}	
				if (GUI.Button (new Rect (0, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"Cancel")){
					Build=false;				
				}
			}else{
				/*if (GUI.Button (new Rect (Screen.width* 1/3, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"OK")){
					BuildNow.hingeJoint.connectedBody = null;
					
					Build=false;				
				}*/	
				/*if (GUI.Button (new Rect (0, Screen.height* 3/ 4 , Screen.width/3, Screen.height/4 ),"Cancel")){
					Destroy(BuildNow.gameObject);
					Build=false;		
					Status=0;
				}*/
			}


		}


	}
	void OnTriggerStay(Collider other){

		switch (other.tag)
		{
		case "Source":
			PickSource=other.gameObject;
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
		PickSource = null;
		Status = 0;
	}
}
