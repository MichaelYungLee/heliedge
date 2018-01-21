
//======================== VRBasics_SpeechEditor ==============================
//
// custom editor for VRBasics_Speech class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(VRBasics_Speech))]
[CanEditMultipleObjects]
public class VRBasics_SpeechEditor : Editor {

	SerializedProperty recognitionMethodProp;
	SerializedProperty confidenceLevelProp;
	SerializedProperty grammarFileNameProp;
	SerializedProperty timeOutSecondsProp;
	SerializedProperty initialTimeOutSecondsProp;
	SerializedProperty keywordsProp;

	void OnEnable(){
		//set up serialized properties
		recognitionMethodProp = serializedObject.FindProperty ("recognitionMethod");
		confidenceLevelProp = serializedObject.FindProperty ("confidenceLevel");
		grammarFileNameProp = serializedObject.FindProperty ("grammarFileName");
		timeOutSecondsProp = serializedObject.FindProperty ("timeOutSeconds");
		initialTimeOutSecondsProp = serializedObject.FindProperty ("initialTimeOutSeconds");
		keywordsProp = serializedObject.FindProperty ("keywords");
	}

	public override void OnInspectorGUI ()
	{
		//reference to the class of object
		VRBasics_Speech speech = (VRBasics_Speech) target;

		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//start listening for changes in inspector values
		EditorGUI.BeginChangeCheck ();

		//display serialized properties in inspector
		EditorGUILayout.PropertyField (recognitionMethodProp,  new GUIContent ("Type"));
		EditorGUILayout.PropertyField (confidenceLevelProp,  new GUIContent ("Confidence"));

		//conditional properties
		//DICTION
		if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Dictation) {
			EditorGUILayout.PropertyField (timeOutSecondsProp, new GUIContent ("Time Out Seconds"));
			EditorGUILayout.PropertyField (initialTimeOutSecondsProp, new GUIContent ("Initial Time Out Seconds"));
		
		//GRAMMAR
		} else if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Grammar) {
			EditorGUILayout.PropertyField (grammarFileNameProp, new GUIContent ("Grammar File Name"));
		
		//KEYWORDS
		} else if (speech.recognitionMethod == VRBasics_Speech.recognitionMethods.Keyword) {

			EditorGUILayout.PropertyField (keywordsProp, new GUIContent ("Keywords"), true);
		}

		//if there were any changes in inspector values
		if (EditorGUI.EndChangeCheck ()) {

			//apply changes to serialized properties
			serializedObject.ApplyModifiedProperties ();
		}
	}
}