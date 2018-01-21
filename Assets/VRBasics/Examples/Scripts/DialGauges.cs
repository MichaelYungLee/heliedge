using UnityEngine;
using System.Collections;

public class DialGauges : MonoBehaviour {

	public VRBasics_Lever dial;

	void Update(){
		transform.parent.gameObject.GetComponent<VRBasics_Hinge> ().SetAngle (dial.hinge.GetComponent<VRBasics_Hinge> ().angle);
	}
}
