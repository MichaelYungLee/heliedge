using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainRotorAnimController : MonoBehaviour {

	public Animator mainRotorAnim;
	public bool mainRotorInstalled;

	// Use this for initialization
	void Start () {
		mainRotorAnim = GetComponent<Animator>();
		mainRotorInstalled = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("3") && !mainRotorInstalled) {
			mainRotorAnim.Play ("mainRotorInstall");
			mainRotorInstalled = true;
		} else if (Input.GetKeyDown ("4") && mainRotorInstalled) {
			mainRotorAnim.Play ("mainRotorSpin");
		}
	}
}
