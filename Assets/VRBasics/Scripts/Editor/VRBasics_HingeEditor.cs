
//======================== VRBasics_HingeEditor ===============================
//
// custom editor for VRBasics_Hinge class
// automatic handling of multi-object handling, undo and prefab overrides
//
//=========================== by Zac Zidik ====================================

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(VRBasics_Hinge))]
[CanEditMultipleObjects]
public class VRBasics_HingeEditor : Editor {

	SerializedProperty useLimitsProp;
	SerializedProperty limitMinProp;
	SerializedProperty limitMaxProp;
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

	void OnEnable(){

		//set up serialized properties
		useLimitsProp = serializedObject.FindProperty ("useLimits");
		limitMinProp = serializedObject.FindProperty ("limitMin");
		limitMaxProp = serializedObject.FindProperty ("limitMax");
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

	public override void OnInspectorGUI ()
	{
		//reference to the class of object used to display gizmo
		VRBasics_Hinge hinge = (VRBasics_Hinge) target;

		//always update serialized properties at start of OnInspectorGUI
		serializedObject.Update ();

		//start listening for changes in inspector values
		EditorGUI.BeginChangeCheck ();

		//display serialized properties in inspector
		EditorGUILayout.PropertyField (useLimitsProp, new GUIContent ("Use Limits"));
		if (hinge.useLimits) {
			//limited to -179.9 because -180 would be the same as 180, would effect spring negatively
			EditorGUILayout.Slider (limitMinProp, 0.0f, -179.9f, new GUIContent ("Limit Minimum"));
			EditorGUILayout.Slider (limitMaxProp, 0.0f, 180.0f, new GUIContent ("Limit Maximum"));
		}

		EditorGUILayout.PropertyField (useSpringToMaxProp, new GUIContent ("Use Spring To Max"));
		if (hinge.useSpringToMax) {
			showSpringToMax = EditorGUILayout.Foldout (showSpringToMax, "Spring To Max");
			if (showSpringToMax) {
				EditorGUILayout.PropertyField (springToMaxProp, new GUIContent ("Spring"));
				EditorGUILayout.PropertyField (damperToMaxProp, new GUIContent ("Damper"));
			}
		}

		EditorGUILayout.PropertyField (useSpringToMidProp, new GUIContent ("Use Spring To Middle"));
		if (hinge.useSpringToMid) {
			showSpringToMid = EditorGUILayout.Foldout (showSpringToMid, "Spring To Middle");
			if (showSpringToMid) {
				EditorGUILayout.PropertyField (springToMidProp, new GUIContent ("Spring"));
				EditorGUILayout.PropertyField (damperToMidProp, new GUIContent ("Damper"));
			}
		}

		EditorGUILayout.PropertyField (useSpringToMinProp, new GUIContent ("Use Spring To Min"));
		if (hinge.useSpringToMin) {
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

			//adjust the limits of the hinge
			hinge.SetLimits ();

			//adjust the spring of the hinge
			hinge.SetSpring();
		}

		//only show the angle in editor when in play mode
		if (Application.isPlaying) {
			EditorGUILayout.LabelField ("Angle", hinge.angle.ToString ());
			EditorGUILayout.LabelField ("Percentage", hinge.percentage.ToString ());
			EditorGUILayout.LabelField ("Angle Frame Change", hinge.angleFrameChange.ToString ());
			EditorGUILayout.LabelField ("Total Rotation", hinge.totalRotation.ToString ());
			EditorGUILayout.LabelField ("Total Revolutions", hinge.totalRevolutions.ToString ());
		}
	}

	void OnSceneGUI() {

		//reference to the class of object used to display gizmo
		VRBasics_Hinge hinge = (VRBasics_Hinge) target;

		//reference to the hinge joint the gizmo displays
		HingeJoint hingeJoint = hinge.GetComponent<HingeJoint> ();

		if (hingeJoint.useLimits != hinge.useLimits) {
			hingeJoint.useLimits = hinge.useLimits;
		}

		if (hingeJoint.limits.min < -179.9f) {
			hinge.limitMin = -179.9f;
			//adjust the limits of the hinge
			hinge.SetLimits ();
		}

		if (hingeJoint.limits.max < 0.0f) {
			hinge.limitMax = 0.0f;
			//adjust the limits of the hinge
			hinge.SetLimits ();
		}

		//this checks if the limits of the hinge joint were changed on the hinge joint itself
		if (hingeJoint.limits.max != hinge.limitMax || hingeJoint.limits.min != hinge.limitMin) {
			hinge.limitMax = hingeJoint.limits.max;
			hinge.limitMin = hingeJoint.limits.min;
			//adjust the limits of the hinge
			hinge.SetLimits ();
		}

		//in edit mode
		if (!Application.isPlaying) {
			//at runtime this is taken care of by the physics engine
			//prevent the hinge from moving away from the fulcrum of the lever during edit mode
			hinge.transform.localPosition = Vector3.zero;
			//prevents the hinge from rotating away from the fulcrum of the parent
			hinge.transform.localEulerAngles = Vector3.zero;
		}

		//reference to the parent class
		VRBasics_Lever lever = hinge.transform.parent.gameObject.GetComponent<VRBasics_Lever> ();

		//DRAW HINGE
		hinge.DrawGizmo();

		//DRAW LEVER
		lever.DrawGizmo();
	}
}