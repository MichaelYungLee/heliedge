using UnityEngine;
using System.Collections;

public class SliderGauge : MonoBehaviour {

	//which slider is this gauge reading from
	public VRBasics_Slider slider;
	//how much does the UV texture more between stages
	private Vector2 uvOffset;
	//current stage of texture
	private float stage = 0;

	void LateUpdate () {

		ChangeStage(slider.percentage);
	}

	void ChangeStage(float s){

		if (stage != s) {
			float xOffset = s * 0.5f;
			uvOffset = new Vector2 (-xOffset, 0.0f);
			GetComponent<Renderer> ().material.mainTextureOffset = uvOffset;
			stage = s;
		}

	}
}
