using UnityEngine;
using System.Collections;

public class LeverGauge : MonoBehaviour {

	//which lever is this gauge reading from
	public VRBasics_Lever lever;
	//how much does the UV texture more between stages
	private Vector2 uvOffset;
	//current stage of texture
	private int stage = 0;

	void LateUpdate () {

		ChangeStage(Mathf.RoundToInt ((lever.hinge.GetComponent<VRBasics_Hinge> ().percentage*100.0f)/10.0f));
	}

	void ChangeStage(int s){

		if (stage != s) {
			float xOffset = s * (0.7825f / 10.0f);
			uvOffset = new Vector2 (xOffset, 0.0f);
			GetComponent<Renderer> ().material.mainTextureOffset = uvOffset;
			stage = s;
		}
	}

}
