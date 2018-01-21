
//======================== VRBasics_LeverEditor ===============================
//
// custom editor for VRBasics_Lever class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics_Lever))]
[CanEditMultipleObjects]
public class VRBasics_LeverEditor : Editor {

	SerializedProperty hingeProp;
	SerializedProperty lengthProp;

	void OnEnable(){
		//set up serialized properties
		hingeProp = serializedObject.FindProperty ("hinge"); 
		lengthProp = serializedObject.FindProperty ("length");
	}

	public override void OnInspectorGUI ()
	{
		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//display serialized properties in inspector
		EditorGUILayout.PropertyField (hingeProp, new GUIContent ("Hinge Object"));
		EditorGUILayout.PropertyField (lengthProp, new GUIContent ("Length"));

		//always apply serialized properties at end of OnInspectorGUI
		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI() {
		//reference to the class of object used to display gizmo
		VRBasics_Lever lever = (VRBasics_Lever) target;

		VRBasics_Hinge hinge = lever.hinge.GetComponent<VRBasics_Hinge> ();

		//DRAW LEVER
		lever.DrawGizmo();

		//DRAW HINGE
		hinge.DrawGizmo();
	}
}
