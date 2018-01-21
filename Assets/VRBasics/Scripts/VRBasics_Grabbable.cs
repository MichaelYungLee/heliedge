
//======================= VRBasics_Grabbable ==================================
//
// an extention of the VRBasics_Touchable class
// place on objects that can not only be touched but also grabbed
//
//=========================== by Zac Zidik ====================================
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class VRBasics_Grabbable : VRBasics_Touchable {

	//options on what type of joint is created betwen the controller and the grabbed object
	public enum JointTypes{Fixed,Spring};
	//what type of joint is created betwen the controller and the grabbed object
	public JointTypes jointType;
	//which button on the controller is used to grab this object
	public GrabButtons grabButton;
	//state of grabbed object
	private bool isGrabbed = false;
	//moves the grabbed object a little closer to the grabber
	public bool isGrabnetized = false;
	//controls how much closer, between 0.0 and 1.0
	public float grabnetStrength = 1.0f;
	//amount of force to break the object from the grab
	public float grabBreakforce = Mathf.Infinity;
	//hides the rendered child objects of the controller (when grabbing an object) if true
	public bool grabHidesController = false;
	//determines if the object can be thrown
	public bool isThrowable = false;
	//a seperate attach point game object
	//used to orient a grabbable object into a specific position relative to the controller when grabbed
	public GameObject attach;

	public bool GetIsGrabbed(){
		return isGrabbed;
	}

	public void SetIsGrabbed(bool g){
		isGrabbed = g;
	}

	void FixedUpdate(){
		//if the object is not grabbed, or touched and is pushable by default but is not pushable
		if (!isGrabbed && !isTouched && isPushableDefault && !isPushable) {
			//make it pushable
			SetIsPushable (true);
		}
	}
}
