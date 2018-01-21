using UnityEngine;
using System.Collections;

public class TankBelt : MonoBehaviour {

	private GameObject rig;
	private GameObject head;
	public VRBasics_Hinge lever01;
	public VRBasics_Hinge lever02;

	public float beltHeight = 0.8f;
	public float speedRegulator = 0.05f;

	private float rotationSpeed = 0.0f;
	private float moveSpeed = 0.0f;

	void Start(){
		rig = GameObject.Find ("[CameraRig]");
		head = GameObject.Find ("Camera (head)");
	}

	void Update () {

		Vector3 newPos = head.transform.position;
		newPos.y = rig.transform.position.y + beltHeight;
		transform.position = newPos;

		RotateRig ();

		MoveRig ();
	}

	void MoveRig(){

		moveSpeed = ((lever01.angle + lever02.angle) * speedRegulator) * -1;

		if (Mathf.Abs(lever01.angle) > 1.0f && Mathf.Abs(lever02.angle) > 1.0f && Mathf.Abs (moveSpeed) > 1.0f) {
			
			rig.transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed);
		}
	}

	void RotateRig(){

		rotationSpeed = lever01.angle + (lever02.angle * -1.0f);

		if (Mathf.Abs(rotationSpeed) > 1.0f) {

			rig.transform.RotateAround (head.transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
		}
	}
}
