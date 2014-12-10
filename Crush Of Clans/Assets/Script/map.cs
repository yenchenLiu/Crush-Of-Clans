using UnityEngine;
using System.Collections;

public class map : MonoBehaviour {
	private float starttime,gametime;

	private Rigidbody BuildNow;
	public GameObject[] source={null,null};//wood,stone;
	// Use this for initialization
	void Start () {
		starttime = Time.time;
		//System.Random r = new System.Random(1000);
	

	}
	
	// Update is called once per frame
	void Update () {
		//print (Time.time - starttime);
		if ( (int)(Time.time - starttime )% 10 == 0 ) {
			GameObject[] temp = GameObject.FindGameObjectsWithTag("Source");

			if(temp.Length<=5000){
				int x, z, kind;
				for(int i=0;i<100;i++){
					x= 5*(UnityEngine.Random.Range(0, 2000)/5);//r.Next(2000);
					z= 5*(UnityEngine.Random.Range(0, 2000)/5);//r.Next(2000);
					kind= UnityEngine.Random.Range(0, 2);//r.Next(2000);
					
					Instantiate(source[kind],new Vector3(x,0,z),source[kind].transform.rotation);					
				}

			}	
		}
	}
}
