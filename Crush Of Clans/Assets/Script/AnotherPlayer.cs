using UnityEngine;
using System.Collections;

public class AnotherPlayer : MonoBehaviour {

	private NavMeshAgent agent;
	private Vector3[] start_position = new Vector3[2];
	private Quaternion[] start_rotation = new Quaternion[2];
	public string PlayerID;
	private float x,z;

	// Use this for initialization
	IEnumerator server_function(){
			while (true) {
				yield return new WaitForSeconds (5);	
				State.PlayerUpdate[PlayerID]--;
				if (PlayerID != "" && PlayerID != null) {
					if (!State.PlayerID.ContainsKey (PlayerID)) {
						Destroy (this.gameObject);
						break;
					}
				x=State.PlayerPositionX[PlayerID];
				z=State.PlayerPositionZ[PlayerID];
				}
				yield return new WaitForSeconds (5);
			}
	}
	void Start () {
		StartCoroutine("server_function");
			
		this.transform.FindChild ("People").gameObject.animation ["Take 001"].speed *= 4;
		this.transform.FindChild ("People").gameObject.animation.Stop ();
		start_position [0] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localPosition;
		start_position [1] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localPosition;
		start_rotation [0] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localRotation;
		start_rotation [1] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localRotation;

		agent = GetComponent<NavMeshAgent> ();
		this.name = PlayerID;
	}
	
	// Update is called once per frame
	void Update () {
		//分別填入目的地的XYZ，Y固定1.5f，XZ填入伺服器送來的座標
		Vector3 temp = new Vector3 (x,1.5f,z);

		agent.destination = temp;

		if (agent.remainingDistance == 0) {
			this.transform.FindChild ("People").gameObject.animation.Stop();
			this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localPosition = start_position [0];
			this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localPosition = start_position [1];
			this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localRotation = start_rotation [0];
			this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localRotation = start_rotation [1];
		} else {
			this.transform.FindChild ("People").gameObject.animation.Play();
		}

	}
}
