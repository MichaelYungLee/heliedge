
//=================== VRBasics_TouchAndPushManager ==========================
//
// gets attached to the Toucher prefab object which should be a child object of a Controller
// handles the effects of the trigger collider for the Toucher
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using System.Collections;

public class VRBasics_TouchAndPushManager : MonoBehaviour {

	//an object being touched
	public GameObject touchedObject;

	public virtual void OnTriggerEnter(Collider other){
		if (other.gameObject.GetComponent<VRBasics_Touchable> ()) {
			
			//tell the touched object to change to the touch material
			other.gameObject.GetComponent<VRBasics_Touchable> ().DisplayTouchedColor ();
			//indicate the object is being touched 
			other.gameObject.GetComponent<VRBasics_Touchable> ().SetIsTouched(true);
			//define the touchObject
			touchedObject = other.gameObject;

			//if the object being touched is pushable
			if (other.gameObject.GetComponent<VRBasics_Touchable> ().isPushable) {
				//trigger a haptic feedback pusle
				VRBasics_Controller controller = this.gameObject.transform.parent.gameObject.GetComponent<VRBasics_Controller> ();
				//microseconds must be between 1 and 3999
				controller.HapticPulse (3000);
			}
		}
	}

	public virtual void OnTriggerExit(Collider other){
		if (other.gameObject.GetComponent<VRBasics_Touchable> ()) {
			
			//tell the touched object to change to the touch material
			other.gameObject.GetComponent<VRBasics_Touchable> ().DisplayUntouchedColor();
			//indicate the object is being touched
			other.gameObject.GetComponent<VRBasics_Touchable> ().SetIsTouched(false);
			//define the touchObject
			touchedObject = null;

			VRBasics_Controller controller = this.gameObject.transform.parent.gameObject.GetComponent<VRBasics_Controller>();

			//if the object is pushable by default
			if (other.gameObject.GetComponent<VRBasics_Touchable> ().isPushable) {

				//make sure collsions between the Pusher and this object are enabled
				//it may have been disable by the GrabManager if the object was grabbed
				if (other.gameObject.GetComponent<VRBasics_Grabbable> ()) {
					if (!other.gameObject.GetComponent<VRBasics_Grabbable> ().GetIsGrabbed()) {
						controller.CollideWithPushers (other.gameObject);
					}
				} else {
					controller.CollideWithPushers (other.gameObject);
				}

			}
		}
	}
}