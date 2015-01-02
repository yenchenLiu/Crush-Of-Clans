using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	public int power;
	public int kind;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 3);
		print ("X");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
