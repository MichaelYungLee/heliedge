using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tailRotorAnimController : MonoBehaviour {

	public Animator tailRotorAnim;
	public redTailRotorAnimController redTailRotorAnimCont;	
	public bool rotorInstalled;

	// Use this for initialization
	void Start () {
		tailRotorAnim = GetComponent<Animator>();
		redTailRotorAnimCont = GetComponent<redTailRotorAnimController>();
	}


	// Update is called once per frame
	void Update () {
		//rotorInstalled = redTailRotorAnimCont.getInstalled ();
		//Why is this always NULL????
		if (Input.GetKeyDown ("2")) {
			tailRotorAnim.Play ("tailRotorSpin");
		}
	}
}
