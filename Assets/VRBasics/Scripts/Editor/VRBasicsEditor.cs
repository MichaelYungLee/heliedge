
//========================== VRBasicsEditor ===================================
//
// custom editor for VRBasics class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics))]
[CanEditMultipleObjects]
public class VRBasicsEditor : Editor {

	SerializedProperty vrTypeProp;
	SerializedProperty ignoreCollisionsLayerProp;


	void OnEnable(){
		//set up serialized properties
		vrTypeProp = serializedObject.FindProperty ("vrType");
		ignoreCollisionsLayerProp = serializedObject.FindProperty ("ignoreCollisionsLayer");
	}

	public override void OnInspectorGUI ()
	{
		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//display serialized properties in inspector
		EditorGUILayout.PropertyField (vrTypeProp,  new GUIContent ("VR Type"));
		EditorGUILayout.PropertyField (ignoreCollisionsLayerProp, new GUIContent ("Ignore Collisions"));

		//apply changes to serialized properties
		serializedObject.ApplyModifiedProperties ();
	}
}
