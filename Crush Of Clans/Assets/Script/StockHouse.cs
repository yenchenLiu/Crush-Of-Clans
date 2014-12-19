using UnityEngine;
using System.Collections;

public class StockHouse : MonoBehaviour {
	//資料庫
	/*
	StockHouse的欄位：
	PlayerID 玩家帳號
	HouseID(看需不需要)
	x(x座標)
	z(z座標)
	StockHouseLevel=1;
	StockHouseHP=100;(暫定耐久)
	StockSouce[0]=0(wood)
	StockSouce[1]=0(stone)
	StockSouce[2]=0(metal)
	...
	*/
	public int[] stockSource = {20,20,20};
	public int[] stocklimit = {100,10,2};
	public bool work;
	public int HP=100,Lelel=1;
	public string PlayerID;
	
	private int vSliderValue,limit;
	private float hSliderValue;
	private GameObject player;
	private int selectSource,playerSource;
	private Player playerNow;
	// Use this for initialization
	void Start () {
		hSliderValue = Screen.width / 8;
		selectSource = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (work == true) {
			playerNow.source [selectSource] = vSliderValue;
			//playerNow.weightNow+=(playerNow.source[selectSource]-playerSource)*playerNow.weight[selectSource];
			stockSource[selectSource]=stockSource[selectSource]+(playerSource-playerNow.source[selectSource]);
			//資料庫
				/*
				UPDATE Player
				Key:PlayerID
				欄位：
				source[0]=playerNow.source[0](wood)
				source[1]=playerNow.source[1](stone)
				source[2]=playerNow.source[2](metal)

				UPDATE StockHouse
				Key:PlayerID,x,z(,HouseID)
				欄位：			
				StockSouce[0]=stockSource[0](wood)
				StockSouce[1]=stockSource[1](stone)
				StockSouce[2]=stockSource[2](metal)
				

			*/
		}
	}
	void OnGUI () {
		if (work == true) {
			vSliderValue=(playerNow.source[selectSource])>0?playerNow.source[selectSource]:0;
			playerSource=playerNow.source [selectSource];

			GUI.Box (new Rect ( 0, 0, Screen.width, Screen.height),"");
			hSliderValue=GUI.HorizontalSlider(new Rect(Screen.width/8, Screen.height*5/6, Screen.width*6/8, Screen.height/8), hSliderValue,Screen.height * 1 /8 ,  Screen.width);
			if (GUI.Button (new Rect (hSliderValue, Screen.height/4 , Screen.width/4, Screen.height/2 ),"Wood\r\nStock:  "+stockSource[0].ToString())){
				selectSource=0;
				limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
				vSliderValue=playerNow.source[selectSource];
				playerSource=playerNow.source [selectSource];
				
			}
			if (GUI.Button (new Rect (hSliderValue+Screen.width/4, Screen.height/4 , Screen.width/4, Screen.height/2 ),"Stone\r\nStock:  "+stockSource[1].ToString())){
				selectSource=1;
				limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
				
				vSliderValue=playerNow.source[selectSource];
				playerSource=playerNow.source [selectSource];
				
			}
			if (GUI.Button (new Rect (hSliderValue+Screen.width*2/4, Screen.height/4 , Screen.width/4, Screen.height/2 ),"Metal\r\nStock:  "+stockSource[2].ToString())){
				selectSource=2;
				limit=playerNow.package[playerNow.cart]-playerNow.source[1]*playerNow.weight[1]-playerNow.source[2]*playerNow.weight[2];
				
				vSliderValue=playerNow.source[selectSource];
				playerSource=playerNow.source [selectSource];
				
			}

			vSliderValue = (int)GUI.VerticalSlider(new Rect(7*Screen.width/8, Screen.height/3, Screen.width/14, Screen.height*2/5), (float)vSliderValue, (float)(stockSource[selectSource]+playerNow.source[selectSource]),0 );

			
			//GUI.TextArea(new Rect (Screen.width* 1/8, Screen.height* 1/ 4 , Screen.width/3, Screen.height/2 ),text);
			if (GUI.Button (new Rect (9*Screen.width/10, Screen.height* 1/ 8 , Screen.width/10, Screen.height/8 ),"X")){
				work=false;		
				playerNow.click=false;
			}
			
		}
		
		
	}
	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			player=other.gameObject;
			playerNow=player.GetComponent<Player>();

		}
		
	}
//	int caculateLimit(int select){
//		for(i)

//	}
}
