using UnityEngine;
using System.Collections;

public class Buttons : VRBasics_Touchable {

	//a list of possible button types
	public enum ButtonTypes{Slider, Hinge};
	//instance of the list
	public ButtonTypes buttonType;
	private bool pushed = false;
	private bool isOn = false;

	new void Start(){
		
		//still run the Start function of the base class
		base.Start ();
		
		GetComponent<Rigidbody> ().isKinematic = true;
	}

	public bool GetIsOn(){
		return isOn;
	}

	void Update(){

		//use if you want the button to be clicked just by touching it
		//===============================================================================
		switch (buttonType) {
		case ButtonTypes.Slider:
			VRBasics_Slider slider = transform.parent.gameObject.GetComponent<VRBasics_Slider> ();
			if (isTouched && !pushed) {

				SetIsTouchable (false);

				GetComponent<Rigidbody> ().isKinematic = false;

				slider.useSpringToMin = false;
				slider.springToMax = 50.0f;
				slider.useSpringToMax = true;

				pushed = true;

			}

			if (pushed) {

				if (slider.percentage > 0.75f) {

					slider.springToMin = 50.0f;
					slider.useSpringToMin = true;
					slider.useSpringToMax = false;

					pushed = false;
				}
			}

			if (!pushed && !isTouchable) {

				if (slider.percentage < 0.25f) {
					SetIsTouchable (true);
				}
			}
			break;

		case ButtonTypes.Hinge:
			VRBasics_Hinge hinge = transform.parent.gameObject.GetComponent<VRBasics_Hinge> ();

			if (isTouched && !pushed) {

				SetIsTouchable (false);

				GetComponent<Rigidbody> ().isKinematic = false;	

				hinge.springToMin = 50.0f;
				hinge.useSpringToMin = true;
				hinge.useSpringToMax = false;

				pushed = true;
			}


			if (hinge.percentage < 0.5f && pushed && !isOn) {

				isOn = true;

				SetIsTouchable (true);
			}


			if (isTouched && isOn) {
				
				SetIsTouchable (false);
			
				hinge.useSpringToMin = false;
				hinge.springToMax = 50.0f;
				hinge.useSpringToMax = true;

				pushed = false;
			}

			if (hinge.percentage > 0.5f && !pushed && isOn) {

				isOn = false;

				SetIsTouchable (true);
			}

			break;
		}
		//===============================================================================


		//use if you want the button to be clicked by physically pushing it down with collision
		//===============================================================================
		/*
		if (isTouched && !pushed) {
			
			GetComponent<Rigidbody> ().isKinematic = false;

			pushed = true;
		}
		*/
		//===============================================================================
	}
}
