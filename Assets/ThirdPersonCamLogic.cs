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
	private Vector3 target;
	// Use this for initialization
	void Start () {
		follow = GameObject.FindWithTag ("CameraController").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {
		target = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		transform.position = Vector3.Lerp (transform.position, target, Time.deltaTime * smooth);
		transform.LookAt (follow);
	}
}
