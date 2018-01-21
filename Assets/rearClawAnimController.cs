using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rearClawAnimController : MonoBehaviour {

	public Animator rearClawAnim;


	// Use this for initialization
	void Start () {
		rearClawAnim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			rearClawAnim.Play ("clawTailRotorInsert");
			//rearClawAnim.Stop ("clawTailRotorInsert");
		}
	}
}
