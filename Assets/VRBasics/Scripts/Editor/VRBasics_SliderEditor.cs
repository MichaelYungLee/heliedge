
//======================= VRBasics_SliderEditor ===============================
//
// custom editor for VRBasics_Slider class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics_Slider))]
[CanEditMultipleObjects]
public class VRBasics_SliderEditor : Editor {

	SerializedProperty positionProp;
	SerializedProperty useSpringToMaxProp;
	SerializedProperty springToMaxProp;
	SerializedProperty damperToMaxProp;
	SerializedProperty useSpringToMidProp;
	SerializedProperty springToMidProp;
	SerializedProperty damperToMidProp;
	SerializedProperty useSpringToMinProp;
	SerializedProperty springToMinProp;
	SerializedProperty damperToMinProp;

	protected static bool showSpringToMax = false;
	protected static bool showSpringToMid = false;
	protected static bool showSpringToMin = false;

	void OnEnable() {
		//set up serialized properties
		positionProp = serializedObject.FindProperty ("position");
		useSpringToMaxProp = serializedObject.FindProperty ("useSpringToMax");
		springToMaxProp = serializedObject.FindProperty ("springToMax");
		damperToMaxProp = serializedObject.FindProperty ("damperToMax");
		useSpringToMidProp = serializedObject.FindProperty ("useSpringToMid");
		springToMidProp = serializedObject.FindProperty ("springToMid");
		damperToMidProp = serializedObject.FindProperty ("damperToMid");
		useSpringToMinProp = serializedObject.FindProperty ("useSpringToMin");
		springToMinProp = serializedObject.FindProperty ("springToMin");
		damperToMinProp = serializedObject.FindProperty ("damperToMin");
	}

	public override void OnInspectorGUI () {

		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//start listening for changes in inspector values
		EditorGUI.BeginChangeCheck ();

		//reference to the slider
		VRBasics_Slider slider = (VRBasics_Slider) target;

		//in edit mode
		if (!Application.isPlaying) {
			//used to position the slider at the start
			//at runtime it will change automatically to what the percentage of the slider is
			EditorGUILayout.Slider (positionProp, 0.0f, 1.0f, new GUIContent ("Position"));
		}

		EditorGUILayout.PropertyField (useSpringToMaxProp, new GUIContent ("Use Spring To Max"));
		if (slider.useSpringToMax) {			
			showSpringToMax = EditorGUILayout.Foldout (showSpringToMax, "Spring To Max");
			if (showSpringToMax) {
				EditorGUILayout.PropertyField (springToMaxProp, new GUIContent ("Spring"));
				EditorGUILayout.PropertyField (damperToMaxProp, new GUIContent ("Damper"));
			}
		}

		EditorGUILayout.PropertyField (useSpringToMidProp, new GUIContent ("Use Spring To Middle"));
		if (slider.useSpringToMid) {			
			showSpringToMid = EditorGUILayout.Foldout (showSpringToMid, "Spring To Middle");
			if (showSpringToMid) {
				EditorGUILayout.PropertyField (springToMidProp, new GUIContent ("Spring"));
				EditorGUILayout.PropertyField (damperToMidProp, new GUIContent ("Damper"));
			}
		}

		EditorGUILayout.PropertyField (useSpringToMinProp, new GUIContent ("Use Spring To Min"));
		if (slider.useSpringToMin) {
			showSpringToMin = EditorGUILayout.Foldout (showSpringToMin, "Spring To Min");
			if (showSpringToMin) {
				EditorGUILayout.PropertyField (springToMinProp, new GUIContent ("Spring"));
				EditorGUILayout.PropertyField (damperToMinProp, new GUIContent ("Damper"));
			}
		}

		//if there were any changes in inspector values
		if (EditorGUI.EndChangeCheck ()) {

			//apply changes to serialized properties
			serializedObject.ApplyModifiedProperties ();

			//adjust the spring of the slider
			slider.SetSpring();
		}

		//in play mode
		if (Application.isPlaying) {
			EditorGUILayout.LabelField ("Percentage", slider.percentage.ToString ());
		}
	}

	void OnSceneGUI() {

		//reference to the class of object used to display gizmo
		VRBasics_Slider slider = (VRBasics_Slider) target;

		//in edit mode
		if (!Application.isPlaying) {
			//at runtime these are taken care of by the physics engine
			//prevent the slider from moving away from the rail
			slider.transform.localPosition = Vector3.zero;
			//prevents the slider from rotating away from the rail
			slider.transform.localEulerAngles = Vector3.zero;
			//use the inspector to postion the slider along the rail
			slider.EditorSetPosition ();
			//calculate the percentage of the slider along the rail
			slider.CalcPercentage ();
		} else {
			//calculate the percentage of the slider along the rail
			slider.CalcPercentage ();
			//keep the position the same as the percentage at runtime
			slider.position = slider.percentage;
		}

		//reference to the parent rail
		VRBasics_Rail rail = slider.transform.parent.gameObject.GetComponent<VRBasics_Rail>();

		//DRAW RAIl
		rail.DrawGizmo();

		//DRAW SLIDER
		slider.DrawGizmo();
	}
}
