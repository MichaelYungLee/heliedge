using UnityEngine;
using System.Collections;

public class CabnetLight : MonoBehaviour {

	public DoorHandle door;
	public GameObject innerLight;
	private bool lit = false;

	void Update(){
		if (door.isOpen && !lit) {
			Light ();
		}

		if (!door.isOpen && lit) {
			Unlight ();
		}
	}

	void Light(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.white);
		innerLight.GetComponent<Light>().intensity = 1;
		lit = true;
	}

	void Unlight(){
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
		innerLight.GetComponent<Light>().intensity = 0;
		lit = false;
	}
}
