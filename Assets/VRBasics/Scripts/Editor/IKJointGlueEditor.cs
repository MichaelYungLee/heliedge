
//========================= IKJointGlueEditor =================================
//
// custom editor for VRBasics_FixedJointGlue class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics_IKJointGlue))]
[CanEditMultipleObjects]
public class IKJointGlueEditor : Editor {

	void OnEnable() {
		//auto fill the connected body of the fixed joint with the object's parent
		VRBasics_IKJointGlue fjg = (VRBasics_IKJointGlue) target;
		fjg.GetComponent<FixedJoint> ().connectedBody = fjg.transform.parent.gameObject.GetComponent<Rigidbody> ();

		//disables gravity by default
		fjg.transform.GetComponent<Rigidbody> ().useGravity = false;
	}
}

