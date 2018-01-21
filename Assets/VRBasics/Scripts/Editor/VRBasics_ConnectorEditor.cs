
//====================== VRBasics_ConnectorEditor =============================
//
// custom editor for VRBasics_Connector class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics_Connector))]
[CanEditMultipleObjects]
public class VRBasics_ConnectorEditor : Editor {

	SerializedProperty typeProp;
	SerializedProperty coupleIDProp;
	SerializedProperty detachMethodProp;
	SerializedProperty breakForceProp;
	SerializedProperty timeOutSecondsProp;

	void OnEnable(){
		//set up serialized properties
		typeProp = serializedObject.FindProperty ("type");
		coupleIDProp = serializedObject.FindProperty ("coupleID");
		detachMethodProp = serializedObject.FindProperty ("detachMethod");
		breakForceProp = serializedObject.FindProperty ("breakForce");
		timeOutSecondsProp = serializedObject.FindProperty ("timeOutSeconds");
	}

	public override void OnInspectorGUI ()
	{
		//reference to the class of object
		VRBasics_Connector connector = (VRBasics_Connector) target;

		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//start listening for changes in inspector values
		EditorGUI.BeginChangeCheck ();

		//display serialized properties in inspector
		EditorGUILayout.PropertyField (typeProp,  new GUIContent ("Type"));
		EditorGUILayout.PropertyField (coupleIDProp,  new GUIContent ("Couple ID"));

		//these properties are controlled by a male connector
		if (connector.type == VRBasics_Connector.connectorType.male) {

			EditorGUILayout.PropertyField (detachMethodProp, new GUIContent ("Detach Method"));

			if (connector.detachMethod == VRBasics_Connector.detachMethods.force) {
				EditorGUILayout.PropertyField (breakForceProp, new GUIContent ("Break Force"));

			}

			EditorGUILayout.PropertyField (timeOutSecondsProp, new GUIContent ("Time Out Seconds"));
		}

		//if there were any changes in inspector values
		if (EditorGUI.EndChangeCheck ()) {

			//apply changes to serialized properties
			serializedObject.ApplyModifiedProperties ();

			if (Application.isPlaying) {
				connector.DetachFrom ();
				connector.SetInTimeOut(false);
				connector.StopCoroutine("ConnectionTimeOut");
			}
		}
	}

	void OnSceneGUI() {

		//reference to the class of object used to display gizmo
		VRBasics_Connector connector = (VRBasics_Connector) target;

		//DRAW GIZMO
		connector.DrawGizmo();
	}
}

