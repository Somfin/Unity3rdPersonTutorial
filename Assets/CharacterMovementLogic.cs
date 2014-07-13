using UnityEngine;
using System.Collections;

public class CharacterMovementLogic : MonoBehaviour {
	private Animator anim;
	private float directionDampTime = 0.1f;
	private float Speed = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		if (anim.layerCount >= 2) {
			anim.SetLayerWeight (1, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (anim)
		{
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");

			Speed = new Vector2 (h, v).sqrMagnitude;

			anim.SetFloat("Speed", Speed);
			anim.SetFloat ("Direction", h, directionDampTime, Time.deltaTime);
		}
	}
}	