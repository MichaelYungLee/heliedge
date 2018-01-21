
//========================= VRBasics_Gazable ==================================
//
// place on objects that can be activate by VRBasics_Gaze
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class VRBasics_Gazable : MonoBehaviour {

	public bool useGazeColor = true;
	public Color gazeColor;

	public void Activate(){
		if (useGazeColor) {
			DisplayActiveColor ();
		}
	}

	public void Deactivate(){
		DisplayInactiveColor ();
	}

	public void DisplayActiveColor(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", gazeColor);
	}

	public void DisplayInactiveColor(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
	}
}
