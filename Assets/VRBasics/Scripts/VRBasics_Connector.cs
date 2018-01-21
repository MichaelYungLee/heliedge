
//========================= VRBasics_Connector ================================
//
// Connectors are trigger colliders that attach their parent game objects to
// another connector's parent game object.
// For a connection to be made, the connectors must be of opposite type (male / female).
// They must also share a coupleID.
//
// The male connector will present several options for how a connection can be broken.
// Never - the connection can not be broken once made
// Force - if the proper amount of force is applied to the joint of the connection the connection will break
// Grab - if either parent object in the connection is grabbed the connection will break
//
// examples: plug and outlet, lid and jar, Lego blocks (although something this complex, may require addtional code)
//
//=========================== by Zac Zidik ====================================

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

public class VRBasics_Connector : MonoBehaviour {

	public enum connectorType{male,female}
	public enum detachMethods{never,force,grab}

	//male or female
	public connectorType type;
	//a compatible connector currently attached to (male and female)
	private GameObject attachedTo;
	//indication that the connector is connected to a compatible connector (male and female)
	private bool isConnected = false;
	//a way to identify compatible connectors (male and female)
	public int coupleID = 0;
	//the original parent of the parent object of the connector (male and female)
	//this needed because when a connection is made the connector repositions it's parent using a dummy parent
	//after positioning, the connector's parent is returned to it's orginal parent
	private GameObject originalParent;
	//a compatible connector currently touching (male and female)
	private GameObject touching;

	//MALE ONLY PROPERTIES
	//what action triggers the break of the connection (male)
	public detachMethods detachMethod;
	//amount of force needed to break joint when detach method is Force (male)
	public float breakForce = Mathf.Infinity;
	//a joint between connectors (male)
	private FixedJoint joint;
	//is the object in a connection time out (male)
	private bool inTimeOut = false;
	//how long is the object unattachable after detach (male)
	//this is need so that objects can be pushed away from other objects by force
	//without immediately reattaching
	public float timeOutSeconds = 1.0f;


	void Start(){

		//if the parent object's parent is the world
		if (transform.parent.transform.parent == null) {
			originalParent = null;
		} else {
			//store the parent object's parent
			originalParent = transform.parent.transform.parent.transform.gameObject;
		}
	}

	public void ConnectTo() {

		//all connections are controlled through the male connector
		//if currently touching a compatible connector
		//and not in a connection time out
		if (type == connectorType.male && touching != null && !inTimeOut) {
			
			//define the attached connector
			//male
			attachedTo = touching;
			//female
			attachedTo.GetComponent<VRBasics_Connector> ().attachedTo = this.gameObject;

			//eventhough this collision will be canceled by the creation of the joint
			//we must immediately cancel it so we can first properly position the male without collisions interfearing
			Physics.IgnoreCollision (transform.parent.GetComponent<Collider> (), attachedTo.transform.parent.GetComponent<Collider> ());

			//set the male position relative to the connected female
			PositionMale ();

			//add a joint to the male connector
			joint = transform.parent.gameObject.AddComponent<FixedJoint> ();

			//connect the joint to the parent of the female connector
			joint.connectedBody = attachedTo.transform.parent.gameObject.GetComponent<Rigidbody> ();

			//keep the joint together unless pulled apart by grab
			float newBreakForce = Mathf.Infinity;

			//if set to never detach
			if (detachMethod == detachMethods.never) {

				//keep the joint together
				newBreakForce = Mathf.Infinity;

			}else{

				//if detach method set to force
				if (detachMethod == detachMethods.force) {

					//the male's break forced will be used
					newBreakForce = breakForce;
				}
			}

			//asign the breakforce to the joint
			joint.breakForce = newBreakForce;

			//indicate the connectors are connected to a compatible connector
			//male
			SetIsConnected(true);
			//female
			attachedTo.GetComponent<VRBasics_Connector>().SetIsConnected(true);

			//kill all velocity as a result of connection
			//male
			transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			//female
			attachedTo.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			attachedTo.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

	public void DetachFrom() {

		//destroy the joint
		//male
		if (type == connectorType.male && attachedTo != null) {
			//destroy the joint between the connectors
			//this type
			Object.DestroyImmediate (joint);

			//clear the joint
			//this type
			joint = null;

		//female
		}else if (type == connectorType.female && attachedTo != null) {
			//destroy the joint between the connectors
			//this type
			Object.DestroyImmediate (attachedTo.GetComponent<VRBasics_Connector> ().joint);

			//clear the joint
			//this type
			attachedTo.GetComponent<VRBasics_Connector> ().joint = null;
		}

		//remove the attachment
		if (attachedTo != null) {
			
			//re-enable the collisions between the male and female parent objects
			Physics.IgnoreCollision (transform.parent.GetComponent<Collider> (), attachedTo.transform.parent.GetComponent<Collider> (), false);

			//kill all velocity as a result of connection
			//this sex
			transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			//opposite sex
			attachedTo.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
			attachedTo.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			//indicate the connectors are not connected to a compatible connector
			//this sex
			SetIsConnected (false);
			//opposite sex
			attachedTo.GetComponent<VRBasics_Connector> ().SetIsConnected (false);

			//clear the object
			//this sex
			attachedTo.GetComponent<VRBasics_Connector> ().attachedTo = null;
			//opposite sex
			attachedTo = null;
		}
	}

	#if UNITY_EDITOR
	//label with connector information in scene editor
	public void DrawGizmo(){

		GUIStyle style = new GUIStyle();
		style.fontSize = 12;
		style.alignment = TextAnchor.MiddleCenter;


		if (type == connectorType.male) {
			
			style.normal.textColor = Color.cyan;
			Handles.Label (transform.position, "Male Connector (" + coupleID.ToString () + ")", style);


		} else if (type == connectorType.female) {
			
			style.normal.textColor = Color.magenta;
			Handles.Label (transform.position, "Female Connector (" + coupleID.ToString () + ")", style);
		}
	}
	#endif

	public GameObject GetDummy(){
		GameObject dummyTrans;
		//if one exist
		if (GameObject.Find ("Dummy")) {
			//get the Dummy transform object
			dummyTrans = GameObject.Find ("Dummy");
			//if one doesnt exist
		} else {
			//create one
			dummyTrans = new GameObject ();
			dummyTrans.name = "Dummy";
		}
		return dummyTrans;
	}

	void OnTriggerEnter(Collider other){

		//if both objects in this collision are connectors
		if (GetComponent<VRBasics_Connector> () && other.GetComponent<VRBasics_Connector> ()) {
			
			//if a male connector comes into contact with a female connector who share a coupleID
			//and niether are currently connected to another connector
			if (type == connectorType.male &&
			   other.GetComponent<VRBasics_Connector> ().type == connectorType.female &&
			   coupleID == other.GetComponent<VRBasics_Connector> ().coupleID &&
			   !isConnected &&
			   !other.GetComponent<VRBasics_Connector> ().isConnected && !inTimeOut) {

				//let both connectors know which game object they are touching
				//male
				SetTouching (other.gameObject);
				//female
				other.GetComponent<VRBasics_Connector> ().SetTouching (this.gameObject);

				//male
				if (transform.parent.gameObject.GetComponent<VRBasics_Touchable> ()) {
					transform.parent.gameObject.GetComponent<VRBasics_Touchable> ().DisplayTouchedColor ();
				}

				//female
				if (other.transform.parent.gameObject.GetComponent<VRBasics_Touchable> ()) {
					other.transform.parent.gameObject.GetComponent<VRBasics_Touchable> ().DisplayTouchedColor ();
				}
			}
		}
	}

	void OnTriggerExit(Collider other){

		//if both objects in this collision are connectors
		if (GetComponent<VRBasics_Connector> () && other.GetComponent<VRBasics_Connector> ()) {
			
			//if a male connector exits contact with a female connector who share a coupleID
			if (type == connectorType.male &&
			    other.GetComponent<VRBasics_Connector> ().type == connectorType.female &&
			    coupleID == other.GetComponent<VRBasics_Connector> ().coupleID) {

				//let both connectors know they are not touching any compatible connectors
				//male
				SetTouching (null);
				//female
				other.GetComponent<VRBasics_Connector> ().SetTouching(null);

				//male
				if (transform.parent.gameObject.GetComponent<VRBasics_Touchable> ()) {
					transform.parent.gameObject.GetComponent<VRBasics_Touchable> ().DisplayUntouchedColor ();
				}

				//female
				if (other.transform.parent.gameObject.GetComponent<VRBasics_Touchable> ()) {
					other.transform.parent.gameObject.GetComponent<VRBasics_Touchable> ().DisplayUntouchedColor ();
				}
			}
		}
	}

	//if the male's parent's rigid body is kinematic
	//then the female assumes the position of the male
	void PositionFemale(){
		
		//if the male's parent's rigid body is kinematic
		if (attachedTo.transform.parent.GetComponent<Rigidbody> ().isKinematic) {

			//an empty game object used to aid in positioning
			GameObject dummy = GetDummy ();
			dummy.transform.position = transform.position;
			dummy.transform.rotation = transform.rotation;

			//make the parent of the parent object for the male connector the dummy
			transform.parent.transform.parent = dummy.transform;

			//move the dummy to the position and rotation of the female connector
			dummy.transform.rotation = attachedTo.transform.rotation;
			dummy.transform.position = attachedTo.transform.position;

			//return the parent of the parent object of the male connector to the original parent
			if (originalParent == null) {
				transform.parent.transform.parent = null;
			} else {
				transform.parent.transform.parent = originalParent.transform;
			}

			//remove the dummy
			DestroyImmediate (dummy);
		}
	}

	//if the male's parent's rigid body is not kinematic
	//then the male assumes the position of the female
	void PositionMale(){
		
		//if the male's parent's rigid body is not kinematic
		if (!transform.parent.GetComponent<Rigidbody> ().isKinematic) {

			//an empty game object used to aid in positioning
			GameObject dummy = GetDummy ();
			dummy.transform.position = transform.position;
			dummy.transform.rotation = transform.rotation;

			//make the parent of the parent object for the male connector the dummy
			transform.parent.transform.parent = dummy.transform;

			//move the dummy to the position and rotation of the female connector
			dummy.transform.rotation = attachedTo.transform.rotation;
			dummy.transform.position = attachedTo.transform.position;

			//return the parent of the parent object of the male connector to the original parent
			if (originalParent == null) {
				transform.parent.transform.parent = null;
			} else {
				transform.parent.transform.parent = originalParent.transform;
			}

			//remove the dummy
			DestroyImmediate (dummy);
		}
	}

	public void SetInTimeOut(bool t) {
		inTimeOut = t;
	}

	public void SetIsConnected(bool c) {
		isConnected = c;
	}

	public void SetTouching(GameObject t) {
		touching = t;
	}

	void ConnectionCheck(){
		//CHECK FOR CONNECTIONS
		//check if the parent object of the connector is grabbable
		if (transform.parent.GetComponent<VRBasics_Grabbable> ()) {

			//is not currently grabbed
			if (!transform.parent.GetComponent<VRBasics_Grabbable> ().GetIsGrabbed ()) {

				//is not currently attached to a compatible connector but is touching one
				if (attachedTo == null && touching != null) {

					//if the connector it is touching is also not attached
					if (touching.GetComponent<VRBasics_Connector> ().attachedTo == null) {

						//make a connection
						ConnectTo ();
					}
				}

				//is currently grabbed
			} else if (transform.parent.GetComponent<VRBasics_Grabbable> ().GetIsGrabbed ()) {

				//is not currently attached to a compatible connector but is touching one
				if (attachedTo == null && touching != null) {

					//check if the parent object of the touching connector is grabbable
					if (touching.transform.parent.GetComponent<VRBasics_Grabbable> ()) {

						//if the connector it is touching is not currently grabbed
						if (!touching.transform.parent.GetComponent<VRBasics_Grabbable> ().GetIsGrabbed ()) {

							//if the connector it is touching is also not attached
							if (touching.GetComponent<VRBasics_Connector> ().attachedTo == null) {

								//make a connection
								ConnectTo ();
							}
						}

						//if the parent object of the touching connector is not grabbable
					} else {

						//if the connector it is touching is also not attached
						if (touching.GetComponent<VRBasics_Connector> ().attachedTo == null) {

							//make a connection
							ConnectTo ();
						}
					}
				}
			}

		//if the parent object is not grabbable
		} else {

			//is not currently attached to a compatible connector but is touching one
			if (attachedTo == null && touching != null) {

				//if the connector it is touching is also not attached
				if (touching.GetComponent<VRBasics_Connector> ().attachedTo == null) {

					//make a connection
					ConnectTo ();
				}
			}
		}
	}

	void DetachCheck(){
		detachMethods CheckMethod = detachMethods.never;

		//male's control the detach methods
		if (type == connectorType.male && attachedTo != null) {

			CheckMethod = detachMethod;

		}else if (type == connectorType.female && attachedTo != null){

			CheckMethod = attachedTo.GetComponent<VRBasics_Connector> ().detachMethod;
		}


		//DETACH METHODS
		//situations that would result the seperation of two connectors
		switch (CheckMethod) {

		//DETACH - BY FORCE
		//this connector will be detached from a connection by force
		case detachMethods.force:

			DetachByForce ();
			break;

		//DETACH - ON GRAB
		//this connector will be detached from a connection by a grab
		case detachMethods.grab:

			//check if the parent object of the connector is grabbable
			if (transform.parent.GetComponent<VRBasics_Grabbable> ()) {

				//is the parent object of the male connector currently grabbed and is it currently attached to a compatible connector
				if (transform.parent.GetComponent<VRBasics_Grabbable> ().GetIsGrabbed () && attachedTo != null) {

					if (type == connectorType.male) {

						//check if the male is not in a connection time out
						if (!inTimeOut) {

							StartCoroutine ("ConnectionTimeOut");
						}	

					} else if (type == connectorType.female) {

						//check if the male is not in a connection time out
						if (!attachedTo.GetComponent<VRBasics_Connector> ().inTimeOut) {

							StartCoroutine ("ConnectionTimeOut");	
						}
					}

				}
			}
			break;
		}
	}

	void FixedUpdate(){
		ConnectionCheck ();
		DetachCheck ();
	}

	void LateUpdate(){
		ConnectionCheck ();
		DetachCheck ();
	}

	IEnumerator ConnectionTimeOut(){

		if (type == connectorType.male) {

			//break a connection
			DetachFrom ();
			
			inTimeOut = true;

			yield return new WaitForSeconds (timeOutSeconds);

			StopCoroutine ("ConnectionTimeOut");

			inTimeOut = false;
		}

		if (type == connectorType.female) {

			GameObject prevAttachedTo = attachedTo;

			//break a connection
			DetachFrom ();

			prevAttachedTo.GetComponent<VRBasics_Connector> ().inTimeOut = true;

			yield return new WaitForSeconds (prevAttachedTo.GetComponent<VRBasics_Connector> ().timeOutSeconds);

			StopCoroutine ("ConnectionTimeOut");

			prevAttachedTo.GetComponent<VRBasics_Connector> ().inTimeOut = false;
		}
	}

	void DetachByForce(){
		
		//is this a male connector
		//this is important because the male gets the actual fixed joint component
		//so it will know if the joint is null after it has been destroyed
		if (type == connectorType.male && attachedTo != null) {

			//if the joint has been destoryed
			if (joint == null || transform.parent.GetComponent<Rigidbody>().velocity.magnitude > breakForce) {

				//check if the male is not in a connection time out
				if (!inTimeOut) {

					StartCoroutine ("ConnectionTimeOut");
				}	
			}
		}

		//is this a female connector
		if (type == connectorType.female && attachedTo != null) {

			//check the male for the joint
			if (attachedTo.GetComponent<VRBasics_Connector> ().joint == null  || transform.parent.GetComponent<Rigidbody>().velocity.magnitude > breakForce) {

				//check if the male is not in a connection time out
				if (!attachedTo.GetComponent<VRBasics_Connector> ().inTimeOut) {

					StartCoroutine ("ConnectionTimeOut");	
				}
			}
		}
	}

	void MaintainPositions(){

		if (type == connectorType.male && attachedTo != null) {

			//maintain the male position relative to the connected female
			PositionMale ();

		} else if (type == connectorType.female && attachedTo != null) {

			//maintain the female position relative to the connected male
			PositionFemale ();
		}
	}
}

