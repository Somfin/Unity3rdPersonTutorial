using UnityEngine;
using System.Collections;

public class CharacterMovementLogic : MonoBehaviour {
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private float directionDampTime = 0.1f;
	[SerializeField]
	private ThirdPersonCamLogic gamecam;
	[SerializeField]
	private float directionSpeed = 3.0f;
	[SerializeField]
	private float rotationDegreesPerSecond = 120f;

	private float speed = 0.0f;
	private float direction = 0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	private AnimatorStateInfo stateInfo;

	private int m_LocomotionID = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		if (anim.layerCount >= 2) {
			anim.SetLayerWeight (1, 1);
		}

		m_LocomotionID = Animator.StringToHash ("Base Layer.Running");
	}
	
	// Update is called once per frame
	void Update () {
		if (anim)
		{
			stateInfo = anim.GetCurrentAnimatorStateInfo(0);
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");
			
			StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);

			anim.SetFloat("Speed", speed);
			anim.SetFloat ("Direction", direction, directionDampTime, Time.deltaTime);

		}
	}

	void FixedUpdate() {
		if (IsInLocomotion() && ((direction >= 0 && horizontal >=0) || (direction < 0 && horizontal < 0))){
			Vector3 rotationAmount = Vector3.Lerp (Vector3.zero, new Vector3 (0f, rotationDegreesPerSecond * (horizontal < 0f? -1f : 1f), 0f), Mathf.Abs(horizontal));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);
		}
	}

	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut){
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3 (horizontal, 0, vertical);
		speedOut = stickDirection.sqrMagnitude;

		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);

		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross (moveDirection, rootDirection);

		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		angleRootToMove /= 180f;
		directionOut = angleRootToMove * directionSpeed;
	}

	public bool IsInLocomotion(){
		return stateInfo.nameHash == m_LocomotionID;;
	}
}	