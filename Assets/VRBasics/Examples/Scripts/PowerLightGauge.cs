using UnityEngine;
using System.Collections;

public class PowerLightGauge : MonoBehaviour {

	public GameObject button;
	private bool lit = false;

	void Update(){

		if (button.GetComponent<Buttons>().GetIsOn() && !lit) {
			Light ();
		}

		if (!button.GetComponent<Buttons>().GetIsOn() && lit) {
			Unlight ();
		}
	}

	void Light(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color(255.0f, 0.0f, 0.0f));
		lit = true;
	}

	void Unlight(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
		lit = false;
	}
}
