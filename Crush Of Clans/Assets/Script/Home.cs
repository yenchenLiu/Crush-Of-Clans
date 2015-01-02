using UnityEngine;
using System.Collections;

public class Home : MonoBehaviour {

	private int bombCount, demage;
	private GameObject bombSet;
	private Bomb bomb;	
	public GUISkin guiSkin;
	public bool work,LevelUp;
	public GameObject Building,Smoke,SmokeAnimation,BombObject,BombAnimation;
	
	public int HP=100,Lelel=1;
	public string PlayerID;
	public int HouseLevel=1;
	private string[] sourceName={"木頭","石頭","鐵片","硫磺","木炭","火藥","鐵礦","硫磺礦"};
	
	public Texture[] LevelPng ;
	public Sprite[] LevelSpritePng;
	private SpriteRenderer spriteRenderer;// 
	private string[] LevelInfo = {"新增功能：建築方向指標","新增功能：瞬間移動"};
	private int[,] LevelUpSource = {{100,100,10,0,0,0,0,0},{400,300,10,0,0,0,0,0}};
	private Player playerNow;
	private GameObject player;
	
	
	// Use this for initialization
	void Start () {
		spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
		

		work = false;
		LevelUp = false;
		bombCount = 0;
		bombSet = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (LevelSpritePng.Length != 0) {
			spriteRenderer.sprite = LevelSpritePng[HouseLevel-1];
			
		}
	}
	void OnGUI () {

		if(LevelUp==true){
			if(HouseLevel<=5){
				string LevelText="";
				for(int i=0;i<8;i++){
					if(LevelUpSource[HouseLevel-1,i]!=0){
						LevelText+="\r\n"+sourceName[i]+":"+playerNow.source[i]+"/"+LevelUpSource[HouseLevel-1,i];
					}
				}
				
				GUI.BeginGroup(new Rect (Screen.width/10, Screen.height/10, Screen.width*4/5, Screen.height*4/5));
				GUI.Box (new Rect (0,0, Screen.width*4/5, Screen.height*4/5), "", guiSkin.box);
				GUI.Box (new Rect (Screen.width/15,Screen.height/12, Screen.width*2/7, Screen.width*2/7), LevelPng[HouseLevel], guiSkin.box);
				//GUI.Label (new Rect (Screen.width/10,Screen.height*3/10, Screen.width*1/5, Screen.width*1/25), HouseName[selectHouse], guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8, Screen.height* 9/ 16 , Screen.width/3, Screen.height/8 ), LevelText, guiSkin.label);
				GUI.Label (new Rect (Screen.width*3/8,Screen.height* 4/ 16, Screen.width/3, Screen.width*1/5),LevelInfo[HouseLevel-1], guiSkin.label);
				
				
				if (GUI.Button (new Rect (Screen.width*11/15, 0 , Screen.width/15, Screen.width/15 ),"X",guiSkin.button)){
					LevelUp=false;
					playerNow.click=false;
				}
				
				GUI.EndGroup();
				
				if(playerNow.source[0]>=LevelUpSource[HouseLevel-1,0] && playerNow.source[1]>=LevelUpSource[HouseLevel-1,1] && playerNow.source[2]>=LevelUpSource[HouseLevel-1,2]&& playerNow.source[3]>=LevelUpSource[HouseLevel-1,3]&& playerNow.source[4]>=LevelUpSource[HouseLevel-1,4]&& playerNow.source[5]>=LevelUpSource[HouseLevel-1,5]&&playerNow.source[6]>=LevelUpSource[HouseLevel-1,6]&& playerNow.source[7]>=LevelUpSource[HouseLevel-1,7]){
					GUI.enabled=true;
				}else{
					GUI.enabled=false;
					
				}
				
				if (GUI.Button (new Rect (Screen.width*2/10,Screen.height*4/10+Screen.width*1/5, Screen.width*1/5, Screen.width*1/15 ),"建造",guiSkin.button)){
					//SpriteRenderer spriteRenderer = this.transform.FindChild("House").gameObject.transform.FindChild("住宅").GetComponent<SpriteRenderer>();
					//spriteRenderer.sprite=LevelSprite[HouseLevel];
					LevelUp=false;
					for(int i =0;i<8;i++){
						
						playerNow.source[i]=playerNow.source[i]-LevelUpSource[HouseLevel-1,i];
					}
					spriteRenderer.sprite = LevelSpritePng[HouseLevel];
					
					HouseLevel++;
					playerNow.click=false;
					playerNow.infomationText("工作屋等級提升!");
					Vector3 Pos=new Vector3(this.gameObject.transform.position.x-1,this.gameObject.transform.position.y+5,this.gameObject.transform.position.z+1);
					GameObject animateNow=(GameObject) Instantiate(Building,Pos,Building.transform.rotation);
					Pos=new Vector3(this.transform.position.x,this.transform.position.y+4,this.transform.position.z);
					
					GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
					Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.1f,this.transform.position.z);
					
					GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
					
					Destroy(animateNow,3);
					Destroy(SomkeNow,3);
					Destroy(SmokeAnimateNow,3);
					Destroy(animateNow,3);
					
				}
				GUI.enabled=true;
				
			}else{
				playerNow.infomationText("以達到最高等級!");
				
				
			}
			
			
			
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			player=other.gameObject;
			playerNow=player.GetComponent<Player>();
		}
		
	}
	IEnumerator bomb_function(){
		
		yield return new WaitForSeconds(1);
		if(bomb.kind==2&&bombCount<2&& State.bombTotal<=5){
			State.bombTotal++;
			Instantiate(BombObject,this.transform.position+new Vector3(0,3,0),BombObject.transform.rotation);
			bombCount++;
		}
		yield return new WaitForSeconds(2);

		
		print( (State.bombTotal).ToString());
		Vector3 Pos=new Vector3(this.transform.position.x,this.transform.position.y+4,this.transform.position.z);
		
		GameObject SomkeNow=(GameObject) Instantiate(Smoke,Pos,Building.transform.rotation);
		Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.1f,this.transform.position.z);
		
		GameObject SmokeAnimateNow=(GameObject) Instantiate(SmokeAnimation,Pos,Building.transform.rotation);
		Pos=new Vector3(this.transform.position.x,this.transform.position.y+4.2f,this.transform.position.z);
		
		GameObject BombAnimateNow=(GameObject) Instantiate(BombAnimation,Pos,Building.transform.rotation);
		
		Destroy(SomkeNow,3);
		Destroy(SmokeAnimateNow,3);
		Destroy(BombAnimateNow,3);

		this.HP -= demage;
		if (this.HP <= 0) {
			bombCount = 0;
			State.bombTotal--;
			if(State.bombTotal<0) State.bombTotal=0;
			
			print( (State.bombTotal).ToString());
			Destroy (this.gameObject);		
		} else {
			yield return new WaitForSeconds (3);
			
			bombCount = 0;
			State.bombTotal--;
			if(State.bombTotal<0) State.bombTotal=0;
			
			print( (State.bombTotal).ToString());
		}
		

	}
	void OnTriggerEnter(Collider other){
		if (bombSet==null && other.tag == "bomb") {
			
			bombSet=other.gameObject;
			bomb=bombSet.GetComponent<Bomb>();	
			
			demage=bomb.power;
			StartCoroutine("bomb_function");
			
		}
	}

}
