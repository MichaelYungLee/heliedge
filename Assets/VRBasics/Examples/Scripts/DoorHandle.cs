
//============================ DoorHandle =====================================
//
// this is an example of how you might use VRBasics for a door and handle or a drawer and handle
// it indicates a door (or drawer) is open when it passes the half way point of it's potential amount of movement
// also alters the state of when both the handle and the door can be touched and pushed
// when a door is closed it can not be touched or pushed, only the handle can be touched
// when a door is open, both the door and handle can be pushed
// this prevents the colliders from being able to open the door (or drawer) from the inside 
//
//=========================== by Zac Zidik ====================================
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

//this is an extension of the VRBasics_Grabbale class
public class DoorHandle : VRBasics_Grabbable {

	//used to determine the state of the door
	public bool isOpen = false;
	//what type of widget is attached the to the door
	private string widgetType;
	//the parent game object the widget is attached to
	private GameObject widgetObject;


	//create a new Start function seperate from the base class
	new void Start(){

		//still run the Start function of the base class
		base.Start ();
		//set to unpushable by default
		SetIsPushableDefault (false);
		//set to it's default pushable state
		ReturnIsPushableToDefault ();
		//find the parent gameobject that contains a widget
		CheckParentsForWidget (this.gameObject);
		//set up the parent door
		InitailizeDoor();
	}
		
	//sets the isOpen variable
	//isOpen = true when the door is past the half way point of movement
	void CheckIsOpen(){
		switch (widgetType) {
		case "Hinge": 
			//the Hinge widget class of the object
			VRBasics_Hinge hinge = widgetObject.GetComponent<VRBasics_Hinge> ();

			//if the door is past the half way point
			if (hinge.percentage > 0.5f) {
				//it will be considered open
				isOpen = true;
			} else {
				//otherwise it will be considered closed
				isOpen = false;
			}
			break;

		case "Slider":
			//the Slider widget class of the object
			VRBasics_Slider slider = widgetObject.GetComponent<VRBasics_Slider> ();

			//if the drawer is past the half way point
			if (slider.percentage > 0.5f) {
				//it will be considered open
				isOpen = true;
			} else {
				//otherwise it will be considered closed
				isOpen = false;
			}
			break;
		}
	}

	//check the object's parents up the hierarchy for either a Hinge or a Slider to know what type of door
	void CheckParentsForWidget(GameObject obj){

		if (obj.transform.parent != null) {

			GameObject parent = obj.transform.parent.gameObject;

			//if the parent is a Hinge widget
			if (parent.GetComponent<VRBasics_Hinge> ()) {

				widgetType = "Hinge";
				widgetObject = parent;

				//if the parent is a Slider widget
			} else if (parent.GetComponent<VRBasics_Slider> ()) {

				widgetType = "Slider";
				widgetObject = parent;

				//check the next parent up the hierarchy
			} else {

				CheckParentsForWidget (parent);
			}

		//at end of hierarchy
		} else {
			Debug.Log ("The DoorHandle has no widget!");
		}
	}

	void DefinePushables(){

		//the parent of the handle should be a door
		GameObject door = transform.parent.gameObject;

		//if the handle is not being grabbed
		if (!GetIsGrabbed ()) {
			//when the door is open
			if (isOpen) {
				//if the handle is not pushable
				if (!isPushable) {
					//make it pushable
					SetIsPushable (true);
				}

				//if the door is not pushable
				if (!door.GetComponent<VRBasics_Touchable> ().isPushable) {
					//make it pushable
					door.GetComponent<VRBasics_Touchable> ().SetIsPushable (true);

					door.layer = VRBasics.Instance.ignoreCollisionsLayer;
				}
			}
		} 

		//if the handle is being grabbed or the door is not open
		if (GetIsGrabbed () || !isOpen) {
			//if the handle is pushable
			if (isPushable) {
				//make it not pushable
				SetIsPushable (false);
			}
			//if the door is pushable
			if (door.GetComponent<VRBasics_Touchable> ().isPushable) {
				//make it not pushable
				door.GetComponent<VRBasics_Touchable> ().SetIsPushable (false);

				door.layer = 0;
			}
		}
	}

	void InitailizeDoor(){

		//the parent of the handle should be a door
		GameObject door = transform.parent.gameObject;

		//make sure the parent door is Touchable
		if (!door.GetComponent<VRBasics_Touchable> ()) {
			door.AddComponent<VRBasics_Touchable> ();
		}

		door.GetComponent<VRBasics_Touchable> ().SetIsTouchable (true);

		//but not Grabbable
		if (door.GetComponent<VRBasics_Grabbable> ()) {
			Destroy(door.AddComponent<VRBasics_Grabbable> ());
		}

		//set to unpushable by default
		door.GetComponent<VRBasics_Touchable>().SetIsPushableDefault (false);
		//set to it's default pushable state
		door.GetComponent<VRBasics_Touchable>().ReturnIsPushableToDefault ();
		//do not use a touch color
		door.GetComponent<VRBasics_Touchable>().useTouchColor = false;
	}

	void CheckForHandleGrab(){

		//the parent of the handle should be a door
		GameObject door = transform.parent.gameObject;

		if (GetIsGrabbed ()) {
			this.gameObject.layer = VRBasics.Instance.ignoreCollisionsLayer;
			door.layer = VRBasics.Instance.ignoreCollisionsLayer;
		} else {
			this.gameObject.layer = 0;
			door.layer = 0;
		}
	}

	void FixedUpdate(){
		
		CheckForHandleGrab ();
	}

	void Update () {
		
		//check if the door is open or closed
		CheckIsOpen ();

		//alter what objects can be pushed
		DefinePushables();
	}
}



