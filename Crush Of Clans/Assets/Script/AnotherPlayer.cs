using UnityEngine;
using System.Collections;

public class AnotherPlayer : MonoBehaviour {

	private NavMeshAgent agent;
	private Vector3[] start_position = new Vector3[2];
	private Quaternion[] start_rotation = new Quaternion[2];
	public string PlayerID;
	private float x,z;

	// Use this for initialization

	/*void destroyThisPlayer(){
		if (State.PlayerID.ContainsKey (PlayerID)) {
						State.PlayerID.Remove (PlayerID);
				}
		State.PlayerPositionX.Remove(PlayerID);
		State.PlayerPositionZ.Remove(PlayerID);
		State.PlayerStatus.Remove(PlayerID);
		State.PlayerUpdate.Remove (PlayerID);
		
		Destroy(this.gameObject);
	}
	IEnumerator server_function(){
			yield return new WaitForSeconds (2);	
		
			while (true) {

				if (PlayerID != "" && PlayerID != null) {
					if (!State.PlayerID.ContainsKey (PlayerID)) {
						destroyThisPlayer();
					
						//Destroy (this.gameObject);
						break;
					}
				x=State.PlayerPositionX[PlayerID];
				z=State.PlayerPositionZ[PlayerID];
				}
				yield return new WaitForSeconds (2);
			}
	}*/
	void Start () {
		//StartCoroutine("server_function");
			
		this.transform.FindChild ("People").gameObject.animation ["Take 001"].speed *= 4;
		this.transform.FindChild ("People").gameObject.animation.Stop ();
		start_position [0] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localPosition;
		start_position [1] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localPosition;
		start_rotation [0] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl001").transform.localRotation;
		start_rotation [1] = this.transform.FindChild ("People").gameObject.transform.FindChild ("ChamferCyl002").transform.localRotation;

		agent = GetComponent<NavMeshAgent> ();
		this.name = PlayerID;
		x = this.transform.position.x;
		z = this.transform.position.z;
		this.transform.FindChild("Name").GetComponent<TextMesh>().text = PlayerID;
		
	} 
	
	// Update is called once per frame
	void Update () {
		//分別填入目的地的XYZ，Y固定1.5f，XZ填入伺服器送來的座標
		
		Vector3 temp = new Vector3 (x, 1.5f, z);
		if (State.PlayerPositionX.ContainsKey (PlayerID) && State.PlayerPositionZ.ContainsKey (PlayerID)) {
			temp = new Vector3 (State.PlayerPositionX [PlayerID], 1.5f, State.PlayerPositionZ [PlayerID]);
				
		} 
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
