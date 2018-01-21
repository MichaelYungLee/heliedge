
//====================== VRBasics_IKJointGlue ==============================
//
// used to better simulate IK physics up the hierarchy of objects joined by fixed joints
// used to remove looseness in the fixed joints, glues child object firmly with parent object
//
// may adversely effect some physics simulations, test for your application
//
//=========================== by Zac Zidik ====================================


using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(FixedJoint))]
public class VRBasics_IKJointGlue : MonoBehaviour {

	private Vector3 localStartPos;
	private Vector3 localStartRot;

	void Start(){
		//stores the starting local postion and rotation
		localStartPos = transform.localPosition;
		localStartRot = transform.localEulerAngles;
	}

	void LockLocals(){
		//maintains the local position and rotation relative to parent
		transform.localPosition = localStartPos;
		transform.localEulerAngles = localStartRot;
	}

	void FixedUpdate () {
		//during physics update
		LockLocals ();
	}

	void Update () {
		//during frame update
		LockLocals ();
	}
}