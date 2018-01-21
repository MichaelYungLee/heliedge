using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redTailRotorAnimController : MonoBehaviour {

	public Animator redTailRotorAnim;
	bool redTailRotorInstalled;


	// Use this for initialization
	void Start () {
		redTailRotorAnim = GetComponent<Animator>();
		redTailRotorInstalled = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1") && redTailRotorInstalled == false) {
			redTailRotorAnim.Play ("tailRotorActualMove");
			//redTailRotorAnim.Stop ("tailRotorActualMove");
			redTailRotorInstalled = true;
		}
	}

}
