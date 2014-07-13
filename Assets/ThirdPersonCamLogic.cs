using UnityEngine;
using System.Collections;

public class ThirdPersonCamLogic : MonoBehaviour {

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform follow;
	[SerializeField]
	private Vector3 offset = new Vector3 (1f, 1.5f, 0f);
	[SerializeField]
	private float dampingTime = 0.1f;

	private Vector3 velocityCamSmooth = Vector3.zero;
	private Vector3 lookDir;
	private Vector3 target;

	// Use this for initialization
	void Start () {
		follow = GameObject.FindWithTag ("CameraController").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {
		Vector3 characterOffset = follow.position + offset;

		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0;
		lookDir.Normalize ();
		Debug.DrawRay (this.transform.position, lookDir, Color.green);
		
		target = characterOffset + follow.up * distanceUp - lookDir * distanceAway;
		Debug.DrawLine (follow.position, target, Color.green);
		//Debug.DrawRay (follow.position, -1f * follow.forward * distanceAway, Color.red);
		//Debug.DrawRay (follow.position, Vector3.up * distanceUp, Color.blue);
		smoothPosition (this.transform.position, target);
		//transform.position = Vector3.Lerp (transform.position, target, Time.deltaTime * smooth);



		transform.LookAt (follow.position + offset);
	}

	private void smoothPosition(Vector3 from, Vector3 to){
		this.transform.position = Vector3.SmoothDamp (from, to, ref velocityCamSmooth, dampingTime);
	}
}
