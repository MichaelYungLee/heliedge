
//======================= VRBasics_Touchable =================================
//
// go between the SteamVR_TrackedObject and the VRBasics scripts
// although this is currently set up for the Steam VR Controller,
// it is generic enough to work with any VR controller
// this is the only VRBasics script that would need the specific references
// to the Steam VR Controller swapped
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider))]
public class VRBasics_Touchable : MonoBehaviour {

	//can the object be moved by the push collider of the controller
	public bool isPushable = true;
	//the default start value of isPushable
	protected bool isPushableDefault;
	//this is true by default, because you put this script
	//however, it can be turned off for conditional disabling of touching
	protected bool isTouchable = true;
	//is the object currently being touched
	protected bool isTouched = false;
	//does the object change color when touched
	public bool useTouchColor = true;
	//if so, to what color
	public Color touchedColor;

	//the start position of the object
	private Vector3 startPos;
	//the start rotation of the object
	private Vector3 startRot;
	//stores the orginal color of the object
	//private Color untouchedColor;

	public virtual void Awake(){
		//the start position of the object
		startPos = transform.position;
		//the start rotation of the object
		startRot = transform.eulerAngles;
		//store the start color of the object
		//untouchedColor = GetComponent<Renderer> ().material.color;
	}

	public virtual void Start(){
		//store the start value of isPushable
		isPushableDefault = isPushable;
		//starts the object in the correct layer
		SetIsPushable (isPushable);
	}

	public virtual void DisplayTouchedColor(){
		if (useTouchColor) {
			//GetComponent<Renderer> ().material.color = touchedColor;
			GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
			GetComponent<Renderer> ().material.SetColor ("_EmissionColor", touchedColor);
		}
	}

	public virtual void DisplayUntouchedColor(){
		//GetComponent<Renderer> ().material.color = untouchedColor;
		GetComponent<Renderer> ().material.EnableKeyword("_EMISSION");
		GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
	}

	public bool GetIsPushableDefault(){
		return isPushableDefault;
	}

	public bool GetIsTouched(){
		return isTouched;
	}

	//set the isPushable to it's default start value
	public void ReturnIsPushableToDefault(){
		isPushable = isPushableDefault;
		//move to correct layer
		SetIsPushable (isPushableDefault);
	}

	//you might need this for objects that are throwable
	//once users start throwing objects around, you might need to return them to the original locations
	public void ReturnToStart(){
		//if there is a rigidbody
		if (GetComponent<Rigidbody> ()) {
			//kill all velocity
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			GetComponent<Rigidbody> ().Sleep ();
		}

		//reset to start position and rotation
		transform.position = startPos;
		transform.eulerAngles = startRot;
	}

	public void SetIsPushable(bool p){
		//if pushable is false
		if (p == false) {

			//tell this object to ignore collision with all Pushers
			VRBasics.Instance.IgnoreAllPushers(this.gameObject);

		} else if (p == true) {

			//tell this object to collide with all Pushers
			VRBasics.Instance.CollideWithAllPushers(this.gameObject);
		}

		isPushable = p;
	}

	public void SetIsTouchable(bool t){
		//if touchable is false
		if (t == false) {

			//tell this object to ignore collision with all Touchers
			VRBasics.Instance.IgnoreAllTouchers(this.gameObject);

		} else if (t == true) {

			//tell this object to collide with all Touchers
			VRBasics.Instance.CollideWithAllTouchers(this.gameObject);
		}

		isTouchable = t;
	}

	public void SetIsPushableDefault(bool p){
		isPushableDefault = p;
	}

	public void SetIsTouched(bool t){
		isTouched = t;
	}
}