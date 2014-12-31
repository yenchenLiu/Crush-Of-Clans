using UnityEngine;
using System.Collections;

public class joy_control : MonoBehaviour {
	
	Rect joy_pixel;
	float start_x;
	float start_y;
	float start_pixel_x;
	float start_pixel_y;
	float r;
	bool joy_flag;

	float zoom_temp;

	float slide_temp;
	float test_slide;

	Player Player_script;

	GameObject Player_ob;
	Vector3[] Player_start_position = new Vector3[2];
	Quaternion[] Player_start_rotation = new Quaternion[2];

	// Use this for initialization
	void Start () {
		//取得player.cs資料
		Player_script = GameObject.Find ("Player").GetComponent<Player>();

		joy_flag = false;
		joy_pixel = this.guiTexture.pixelInset;
		start_x = Screen.width * this.transform.position.x + this.guiTexture.pixelInset.x + this.guiTexture.pixelInset.width / 2;
		start_y = Screen.height * this.transform.position.y + this.guiTexture.pixelInset.y + this.guiTexture.pixelInset.height / 2;
		start_pixel_x = this.guiTexture.pixelInset.x;
		start_pixel_y = this.guiTexture.pixelInset.y;
		r = (this.transform.FindChild("joy_back").guiTexture.pixelInset.width - this.guiTexture.pixelInset.width) / 2;

		test_slide = 0;

		Player_ob = GameObject.Find ("Player").transform.FindChild ("People").gameObject;
		Player_ob.animation ["Take 001"].speed *= 4;
		Player_ob.animation.Stop ();
		Player_start_position [0] = Player_ob.transform.FindChild ("ChamferCyl001").transform.localPosition;
		Player_start_position [1] = Player_ob.transform.FindChild ("ChamferCyl002").transform.localPosition;
		Player_start_rotation [0] = Player_ob.transform.FindChild ("ChamferCyl001").transform.localRotation;
		Player_start_rotation [1] = Player_ob.transform.FindChild ("ChamferCyl002").transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		//move player
		if (Input.touchCount == 1 && joy_flag) {
			switch(Input.GetTouch(0).phase){
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					float temp_x = start_pixel_x + (Input.GetTouch(0).position.x - start_x);
					float temp_y = start_pixel_y + (Input.GetTouch(0).position.y - start_y);
					
					//range limit
					if(Mathf.Pow (temp_x - start_pixel_x, 2) + Mathf.Pow(temp_y - start_pixel_y, 2) > Mathf.Pow (r, 2)){
						float u = Mathf.Atan2 (temp_y - start_pixel_y, temp_x - start_pixel_x);
						u = u * 180 / Mathf.PI;
					
						temp_x = start_pixel_x + r * Mathf.Cos (u * Mathf.PI / 180);
						temp_y = start_pixel_y + r * Mathf.Sin (u * Mathf.PI / 180);
					}
					this.guiTexture.pixelInset = new Rect(temp_x,temp_y, this.guiTexture.pixelInset.width, this.guiTexture.pixelInset.height);
					
					//action range
					if(Mathf.Pow (temp_x - start_pixel_x, 2) + Mathf.Pow(temp_y - start_pixel_y, 2) > Mathf.Pow (r / 2, 2)){
						float temp =(Mathf.Atan2 (temp_y - start_pixel_y,temp_x - start_pixel_x) * 180 / Mathf.PI + 90) * -1;
						//Angle
						GameObject.Find ("Player").transform.FindChild("People").gameObject.transform.localEulerAngles = new Vector3(0, temp, 0);
						
						temp = temp * Mathf.PI / 180;

						float start = Mathf.Pow(Mathf.Pow (Player_script.player_temp.x - Player_script.point[0], 2) + Mathf.Pow (Player_script.player_temp.z - Player_script.point[1], 2), 0.5f);
						float end = Mathf.Pow(Mathf.Pow ((Player_script.player_temp.x - Player_script.move_speed * Mathf.Sin (temp)) - Player_script.point[0], 2) + Mathf.Pow ((Player_script.player_temp.z - Player_script.move_speed * Mathf.Cos(temp)) - Player_script.point[1], 2), 0.5f);
						float player_x = Player_script.player_temp.x;
						float player_z = Player_script.player_temp.z;
						Player_script.player_temp.x += Player_script.move_speed * Mathf.Sin (temp) + Player_script.move_speed * Mathf.Cos (temp);
						Player_script.player_temp.z -= Player_script.move_speed * Mathf.Sin (temp) - Player_script.move_speed * Mathf.Cos (temp);

						if(Player_script.move_flag){
							if(end >= start){
								Player_script.player_temp.x = player_x; 
								Player_script.player_temp.z = player_z;
							}	 
						}
						GameObject.Find("Player").transform.position = Player_script.player_temp;
					}
					Player_ob.animation.Play();
					break;
				case TouchPhase.Ended:
					//restart
					this.guiTexture.pixelInset = joy_pixel;
					joy_flag =false;
					Player_ob.animation.Stop();
					Player_ob.transform.FindChild ("ChamferCyl001").transform.localPosition = Player_start_position [0];
					Player_ob.transform.FindChild ("ChamferCyl002").transform.localPosition = Player_start_position [1];
					Player_ob.transform.FindChild ("ChamferCyl001").transform.localRotation = Player_start_rotation [0];
					Player_ob.transform.FindChild ("ChamferCyl002").transform.localRotation = Player_start_rotation [1];
					break;
			}
		}

		//view zoom
		if (Input.touchCount == 2 && !joy_flag) {
			bool both_move = true;
			float[,] point = new float[2, 2];
			for(int i = 0; i < 2; i++){
				point[i, 0] = Input.GetTouch(i).position.x;
				point[i, 1] = Input.GetTouch(i).position.y;
				if(Input.GetTouch (i).phase != TouchPhase.Moved){
					both_move = false;
				}
			}

			if(both_move){
				float distance = Mathf.Pow(Mathf.Pow(point[0, 0] - point[1, 0], 2) + Mathf.Pow(point[0, 1] - point[1, 1], 2), 0.5f);
				if(distance > zoom_temp){
					if(GameObject.Find ("Player").transform.FindChild("Main Camera").camera.fieldOfView < 100){
						GameObject.Find ("Player").transform.FindChild("Main Camera").camera.fieldOfView += 4;
					}
				}else if(distance < zoom_temp){
					if(GameObject.Find ("Player").transform.FindChild("Main Camera").camera.fieldOfView > 30){
						GameObject.Find ("Player").transform.FindChild("Main Camera").camera.fieldOfView -= 4;
					}
				}
				zoom_temp = distance;
			}
		}

		//slide:up and down
		if (Input.touchCount == 1 && !joy_flag) {
			if(Input.GetTouch (0).phase == TouchPhase.Moved){
				if(Input.GetTouch(0).position.y > slide_temp){
					test_slide += 1;
				}else if(Input.GetTouch(0).position.y < slide_temp){
					test_slide -= 1;
				}
				slide_temp = Input.GetTouch(0).position.y;
			}
		}
	}

	void OnMouseDown(){
		if (Input.touchCount == 1 && !joy_flag) {
			joy_flag = true;
		}
	}
}
