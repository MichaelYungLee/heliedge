using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tailRotorAnimController : MonoBehaviour {

	public Animator tailRotorAnim;


	// Use this for initialization
	void Start () {
		tailRotorAnim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("2")) {
			tailRotorAnim.Play ("tailRotorSpin");
		}
	}
}
