
//======================= VRBasics_Controller ================================
//
// attach to the controller game objects
// this script is the go between for the VR controllers (depending on type) and the VRBasics scripts
// the type is held in the vrType property of VRBasics instance
//
// currently the only type available is SteamVR for the Vive
// will update as other controllers for VR devices are released
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//which buttons on the controller are used to grab objects
public enum GrabButtons
{
	grip,touchpad,trigger
}

public class VRBasics_Controller : MonoBehaviour {

	//holds the index of the input device
	private int deviceIndex;

	//state of the buttons on the controller
	private bool isGrip = false;
	private bool isHairTrigger = false;
	private bool isTouchPad = false;
	private bool isTrigger = false;

	//secondary state of the buttons
	//used primarily for grabbing objects
	//when a joint is broken between a grabbed object and the controller by force
	//the grab button may still be down
	//but we don't want to be able to grab things with a closed grab
	//gets set to true by the VRBascis_GrabManager when an object is grabbed using this button
	//set back to false when that button is released
	private bool isGripGrab = false;
	private bool isHairTriggerGrab = false;
	private bool isTouchPadGrab = false;
	private bool isTriggerGrab = false;

	//is the controller model being rendered
	private bool isHidden = false;

	void FixedUpdate()
	{
		switch(VRBasics.Instance.vrType.ToString()){
		case "SteamVR":
			//constantly update the index of this device cause it might change
			deviceIndex = GetDeviceIndex ();

			//TRIGGER
			if (SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
				isTrigger = false;
				isTriggerGrab = false;
			}
			if (SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
				isTrigger = true;
			}

			//HAIRTRIGGER
			if (SteamVR_Controller.Input (deviceIndex).GetHairTrigger ()) {
				isHairTrigger = true;
			} else {
				isHairTrigger = false;
				isHairTriggerGrab = false;
				//this also means the tigger is not down
				isTrigger = false;
			}

			//GRIP
			if (SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Grip)) {
				isGrip = false;
				isGripGrab = false;
			}
			if (SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
				isGrip = true;
			}

			//TOUCHPAD
			if (SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
				isTouchPad = false;
				isTouchPadGrab = false;
			}
			if (SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
				isTouchPad = true;
			}
			break;
		}
	}

	public Vector3 GetAngularVelocity(){
		Vector3 angularVelocity = Vector3.zero;
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			angularVelocity = SteamVR_Controller.Input (deviceIndex).angularVelocity;
			break;
		}
		return angularVelocity;
	}

	public int GetDeviceIndex(){
		int deviceIndex = -1;
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			deviceIndex = (int)GetComponent<SteamVR_TrackedObject> ().index;
			break;
		}
		return deviceIndex;
	}

	public bool GetisGrip(){
		return isGrip;
	}

	public bool GetisHairTrigger(){
		return isHairTrigger;
	}

	public bool GetisTouchPad(){
		return isTouchPad;
	}

	public bool GetisTrigger(){
		return isTrigger;
	}

	public bool GetisGripGrab(){
		return isGripGrab;
	}

	public void SetisGripGrab(bool g){
		isGripGrab = g;
	}

	public bool GetisHairTriggerGrab(){
		return isHairTriggerGrab;
	}

	public void SetisHairTriggerGrab(bool g){
		isHairTriggerGrab = g;
	}

	public bool GetisTouchPadGrab(){
		return isTouchPadGrab;
	}

	public void SetisTouchPadGrab(bool g){
		isTouchPadGrab = g;
	}

	public bool GetisTriggerGrab(){
		return isTriggerGrab;
	}

	public void SetisTriggerGrab(bool g){
		isTriggerGrab = g;
	}

	public Transform GetOrigin(){
		Transform origin = null;
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			origin = GetComponent<SteamVR_TrackedObject> ().origin ? GetComponent<SteamVR_TrackedObject> ().origin : GetComponent<SteamVR_TrackedObject> ().transform.parent;
			break;
		}
		return origin;
	}

	public List<GameObject> GetTouchers(){
		
		List<GameObject> touchers = new List<GameObject> ();

		int numChildren = transform.childCount;
		for (int ch = 0; ch < numChildren; ch++) {
			if (transform.GetChild (ch).name == "Toucher") {
				touchers.Add (transform.GetChild (ch).gameObject);
			}
		}

		return touchers;
	}

	public List<GameObject> GetPushers(){
		
		List<GameObject> touchers = GetTouchers ();
		List<GameObject> pushers = new List<GameObject> ();

		int numTouchers = touchers.Count;
		for (int t = 0; t < numTouchers;t++) {
			int numChildren = touchers [t].transform.childCount;
			for (int ch = 0; ch < numChildren; ch++) {
				if (touchers [t].transform.GetChild (ch).name == "Pusher") {
					pushers.Add (touchers [t].transform.GetChild (ch).gameObject);
				}
			}
		}

		return pushers;
	}

	public Vector3 GetVelocity(){
		Vector3 velocity = Vector3.zero;
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			velocity = SteamVR_Controller.Input (deviceIndex).velocity;
			break;
		}
		return velocity;
	}

	public void HapticPulse(ushort microSeconds){
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			SteamVR_Controller.Input (deviceIndex).TriggerHapticPulse (microSeconds);
			break;
		}
	}
		
	public void HideController(){
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			Renderer[] renderers = GetComponentsInChildren<Renderer> ();
			for (int i = 0; i < renderers.Length; i++) {
				renderers [i].enabled = false;
			}
			break;
		}

		isHidden = true;
	}


	public void DisableTouchersAndPushers(bool tp){
		DisableTouchers (tp);
		DisablePushers (tp);
	}


	public void DisableTouchers(bool tp){
		//if true, no collisions should occur with the Touchers of this Controller
		if (tp) {
			
			//place all Touchers on layer where collisions are ignored
			List<GameObject> touchers = GetTouchers();
			int numTouchers = touchers.Count;
			for (int t = 0; t < numTouchers; t++) {
				touchers[t].layer = VRBasics.Instance.ignoreCollisionsLayer;
			}

		//if false
		} else if (!tp) {
			
			//place all Touchers on layer where collisions are enabled
			List<GameObject> touchers = GetTouchers();
			int numTouchers = touchers.Count;
			for (int t = 0; t < numTouchers; t++) {
				touchers[t].layer = 0;
			}
		}
	}

	public void DisablePushers(bool tp){
		
		//if true, no collisions should occur with the Pushers of this Controller
		if (tp) {
			
			//place all Pushers on layer where collisions are ignored
			List<GameObject> pushers = GetPushers();
			int numPushers = pushers.Count;
			for (int p = 0; p < numPushers; p++) {
				pushers[p].layer = VRBasics.Instance.ignoreCollisionsLayer;
			}

		//if false
		} else if (!tp) {
			
			//place all Pushers on layer where collisions are enabled
			List<GameObject> pushers = GetPushers();
			int numPushers = pushers.Count;
			for (int p = 0; p < numPushers; p++) {
				pushers[p].layer = 0;
			}
		}
	}

	public void CollideWithTouchers(GameObject other){

		List<GameObject> touchers = GetTouchers();
		int numTouchers = touchers.Count;
		for (int t = 0; t < numTouchers; t++) {
			Physics.IgnoreCollision (touchers [t].GetComponent<Collider> (), other.GetComponent<Collider> (), false);
		}
	}

	public void IgnoreTouchers(GameObject other){

		List<GameObject> touchers = GetTouchers();
		int numTouchers = touchers.Count;
		for (int t = 0; t < numTouchers; t++) {
			Physics.IgnoreCollision (touchers [t].GetComponent<Collider> (), other.GetComponent<Collider> ());
		}
	}

	public void CollideWithPushers(GameObject other){

		List<GameObject> pushers = GetPushers();
		int numPushers = pushers.Count;
		for (int p = 0; p < numPushers; p++) {
			Physics.IgnoreCollision (pushers [p].GetComponent<Collider> (), other.GetComponent<Collider> (), false);
		}
	}

	public void IgnorePushers(GameObject other){

		List<GameObject> pushers = GetPushers();
		int numPushers = pushers.Count;
		for (int p = 0; p < numPushers; p++) {
			Physics.IgnoreCollision (pushers [p].GetComponent<Collider> (), other.GetComponent<Collider> ());
		}
	}

	public void ShowController(){
		switch (VRBasics.Instance.vrType.ToString ()) {
		case "SteamVR":
			Renderer[] renderers = GetComponentsInChildren<Renderer> ();
			for (int i = 0; i < renderers.Length; i++) {
				renderers [i].enabled = true;
			}
			break;
		}

		isHidden = false;
	}
}