using UnityEngine;
using System.Collections;

public class ButtonLightGuages : MonoBehaviour {

	public VRBasics_Slider button;
	public string buttonColor = "red";
	private bool pushed = false;
	//public Light light;
	private bool lit = false;

	void Update(){

		//after push, has the button returned to up
		if (pushed) {
			if (button.percentage < 0.5f) {
				pushed = false;
			}
		}

		if (button.percentage > 0.5f && !pushed && !lit) {
			Light ();
		}

		if (button.percentage > 0.5f && !pushed && lit) {
			Unlight ();
		}
	}

	void Light(){
		pushed = true;
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");

		switch (buttonColor) {
		case "red":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color(255.0f, 0.0f, 0.0f));
			break;
		case "yellow":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color(241.0f, 255.0f, 0.0f));
			break;
		case "green":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", new Color(0.0f, 255.0f, 0.0f));
			break;
		}

		//light.intensity = 1;
		lit = true;
	}

	void Unlight(){
		pushed = true;
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");

		switch (buttonColor) {
		case "red":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
			break;
		case "yellow":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
			break;
		case "green":
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
			break;
		}

		//light.intensity = 0;
		lit = false;
	}
}
