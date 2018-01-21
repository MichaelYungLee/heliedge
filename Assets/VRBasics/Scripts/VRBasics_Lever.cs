
//======================== VRBasics_Lever ===================================
//
// draws a gizmo that indicates the position and length of a lever prefab
// levers provide a solid mount for the child hinge to rotate around a single axis
//
//=========================== by Zac Zidik ====================================

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class VRBasics_Lever : MonoBehaviour {

	public GameObject hinge;
	//the radius of the arc
	public float length = 1.0f;

	#if UNITY_EDITOR
	public void DrawGizmo(){
		//color of gizmo
		Handles.color = Color.magenta;
		//get the position of the end of the lever according to how long it is
		Vector3 endOfLever = transform.position + (hinge.transform.up.normalized * length);
		//draw a line from the hinge joint to the end of the lever
		Handles.DrawLine (transform.position, endOfLever);
		//draw a solid dot at the hinge end of lever
		Handles.DrawSolidDisc (transform.position, transform.right, 0.01f);
		//draw an solid dot at the end of the lever
		Handles.DrawSolidDisc (endOfLever, hinge.transform.right, 0.01f);
	}
	#endif
}





