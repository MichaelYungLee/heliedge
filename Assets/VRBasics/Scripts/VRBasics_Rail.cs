
//========================== VRBasics_Rail ====================================
//
// draws a gizmo that indicates the position and length of a rail prefab
// rails provide a solid mount for the child slider to move back and forth along a single axis
//
//=========================== by Zac Zidik ====================================
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

//typically a kinematic rigidbody
//serves as a stable object for a slider object to move across
[RequireComponent (typeof(Rigidbody))]
public class VRBasics_Rail : MonoBehaviour {

	//the gameobject the moves along the rail
	public GameObject slider;
	//the length of the rail
	public float length = 1.0f;
	//the location of the anchor along the rail between 0.0 and 1.0
	public float anchor = 0.5f;
	//the amount the anchor has moved from the orginal position
	public float anchorMove;

	#if UNITY_EDITOR
	public void DrawGizmo(){

		//an empty game object used to aid in positioning
		GameObject dummyTrans = GetDummy();

		//reference to the configurable joint the gizmo displays
		ConfigurableJoint configJoint = slider.GetComponent<ConfigurableJoint> ();

		//color of gizmo
		Handles.color = Color.cyan;

		dummyTrans.transform.position = transform.position;
		dummyTrans.transform.eulerAngles = transform.eulerAngles;
		dummyTrans.transform.position += dummyTrans.transform.up * anchorMove;

		Vector3 minLimitPos = dummyTrans.transform.position + (transform.up.normalized * configJoint.linearLimit.limit);
		Vector3 maxLimitPos = dummyTrans.transform.position - (transform.up.normalized * configJoint.linearLimit.limit);

		//draw a empty dot at lower limit
		Handles.DrawWireDisc (minLimitPos, transform.right, 0.015f);
		//draw a empty dot at upper limit
		Handles.DrawWireDisc (maxLimitPos, transform.right, 0.015f);
		//draw a line
		Handles.DrawLine (minLimitPos, maxLimitPos);

		//remove the dummy
		DestroyImmediate (dummyTrans);
	}
	#endif

	public GameObject GetDummy(){
		GameObject dummyTrans;
		//if one exist
		if (GameObject.Find ("Dummy")) {
			//get the Dummy transform object
			dummyTrans = GameObject.Find ("Dummy");
			//if one doesnt exist
		} else {
			//create one
			dummyTrans = new GameObject ();
			dummyTrans.name = "Dummy";
		}
		return dummyTrans;
	}

	public void SetAnchorMove(float m){
		//store the value of the distance the achor has moved
		anchorMove = m;
		//reposition the connected anchor on the slider joint
		slider.GetComponent<VRBasics_Slider> ().SetConnectedAnchorPos ();
	}
}


