using UnityEngine;
using System.Collections;

public class BombAnimation : MonoBehaviour {
	public int speed = 5;
	public Sprite[] bombAnimation;
	SpriteRenderer temp;
	int flag;
	int speed_flag = 0;
	// Use this for initialization
	void Start () {
		flag = 0;
		temp = renderer as SpriteRenderer;
	}
	
	// Update is called once per frame
	void Update () {
		if (speed_flag > speed) {
			temp.sprite = bombAnimation [flag++];
			if (flag >= bombAnimation.Length) {
					flag = 0;
			}
			speed_flag = 0;
		} 
		speed_flag++;
	}
}