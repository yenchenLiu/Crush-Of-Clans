using UnityEngine;
using System.Collections;

public class ToolAnimation : MonoBehaviour {

	bool flag = true;
	Vector3 temp;
	public float speed = 1.5f;
	public AudioClip sound;
	protected AudioSource m_sound;

	// Use this for initialization
	void Start () {
		temp = transform.eulerAngles;
		m_sound = this.audio;
		m_sound.PlayOneShot (sound);
	}
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			temp.z += speed;
			if(temp.z >= 91){
				flag = false; 
			}
		} else {
			temp.z -= speed;
			if(temp.z <= 50){
				flag = true;
			}
		}
		transform.eulerAngles = temp;
	}
}
