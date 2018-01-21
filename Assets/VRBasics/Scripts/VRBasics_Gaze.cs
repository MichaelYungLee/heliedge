
//========================== VRBasics_Gaze ====================================
//
// uses the headset to identify when the user is looking at VRBasics_Gazable objects
// uses an optional indicator in front of the camera that points in the direction of the gaze
// this object, called the reticle, can snap to the object or the normal of the collider surface
//
//=========================== by Zac Zidik ====================================

using UnityEngine;
using System.Collections;

public class VRBasics_Gaze : MonoBehaviour {

	public Transform gazeCamera;
	public float rayLength = 500f;
	public float rayDuration = 1f;
	public bool useDebugRay;
	public Transform reticle;
	public float reticleDefaultDistance = 2.0f;
	private Quaternion reticleDefaultRotation;
	public bool onlyHitGazables = false;
	public bool useNormal = false;

	//the current object under gaze
	private VRBasics_Gazable curGazedObject;
	//the previous object under gaze
	private VRBasics_Gazable prevGazedObject;

	void Awake(){
		
		//when using a reticle
		if(reticle){

			//store the initial rotation of the reticle object
			reticleDefaultRotation = reticle.localRotation;
		}
	}

	void Gaze(){

		// Create a ray that points forwards from the camera.
		Ray ray = new Ray(gazeCamera.position, gazeCamera.forward);
		RaycastHit hit;

		//raycast and check for hit
		if (Physics.Raycast (ray, out hit, rayLength)) {

			//if gaze is only used to hit gazable objects
			if (onlyHitGazables) {

				//if the object hit by the gaze is a gazable object
				if (hit.collider.GetComponent<VRBasics_Gazable> ()) {

					//if the object hit by the raycast is a gazable object
					VRBasics_Gazable gazabledObject = hit.collider.GetComponent<VRBasics_Gazable> ();

					//assign the current object under gaze
					curGazedObject = gazabledObject;

					//if the gazable object is not the same as the previously gazed object
					//stops from continously identifying the same object
					if (gazabledObject && gazabledObject != prevGazedObject) {

						//set the gazable object to active
						gazabledObject.Activate ();
					}

					//if there was previously gazed object
					if (gazabledObject != prevGazedObject && prevGazedObject != null) {

						//deactivate it
						prevGazedObject.Deactivate ();
					}

					//assign the previous object under gaze
					prevGazedObject = gazabledObject;

					//when using a reticle
					if (reticle) {

						//set the position of the reticle to the hit
						SetReticlePosition (hit);
					}
				
				//if the object hit by the gaze is not a gazable object
				} else {

					//if there was previously gazed object
					if (prevGazedObject != null){

						//deactivate it
						prevGazedObject.Deactivate ();
						prevGazedObject = null;
					}

					//clear the current gazed object as well
					curGazedObject = null;

					//when using a reticle
					if (reticle) {

						//set it to it's fault distance and position from the camera
						SetReticlePosition ();
					}
				}

			//if the gaze can hit any object with a collider
			} else {
				
				//if the object hit by the raycast is a gazable object
				VRBasics_Gazable gazabledObject = hit.collider.GetComponent<VRBasics_Gazable> ();

				//assign the current object under gaze
				curGazedObject = gazabledObject;

				//if the gazable object is not the same as the previously gazed object
				//stops from continously identifying the same object
				if (gazabledObject && gazabledObject != prevGazedObject) {

					//set the gazable object to active
					gazabledObject.Activate ();
				}

				//if there was previously gazed object
				if (gazabledObject != prevGazedObject && prevGazedObject != null) {

					//deactivate it
					prevGazedObject.Deactivate ();
				}

				//assign the previous object under gaze
				prevGazedObject = gazabledObject;

				//when using a reticle
				if (reticle) {

					//set the position of the reticle to the hit
					SetReticlePosition (hit);
				}
			}
		
		//when nothing is hit when the raycast
		} else {
			
			//if there was previously gazed object
			if (prevGazedObject != null){

				//deactivate it
				prevGazedObject.Deactivate ();
				prevGazedObject = null;
			}

			//clear the current gazed object as well
			curGazedObject = null;

			//when using a reticle
			if (reticle) {

				//set it to it's fault distance and position from the camera
				SetReticlePosition ();
			}
		}

		//when using a debug ray
		if (useDebugRay){

			//draw the debug ray
			Debug.DrawRay(gazeCamera.position, gazeCamera.forward * rayLength, Color.red, rayDuration);
		}
	}

	//sets the position of the reticle when there is no hit by the raycast of the gaze
	void SetReticlePosition(){

		//put the reticle in front of the camera at a set distance
		reticle.position = gazeCamera.position + gazeCamera.forward * reticleDefaultDistance;

		//keeps the reticle at its initial rotation in front of the camera
		reticle.localRotation = reticleDefaultRotation;
	}

	//sets the position of the reticle when there is a hit by the raycast of the gaze
	void SetReticlePosition (RaycastHit hit){

		//put the reticle at the hit point of the raycast
		reticle.position = hit.point;

		//when using normal detection
		if (useNormal){

			//rotates the reticle to lay along the normal of the hit of the raycast
			reticle.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
		}else{

			//keeps the reticle at its initial rotation in front of the camera
			reticle.localRotation = reticleDefaultRotation;
		}
	}

	void Update () {

		//continually check for gaze 
		Gaze ();	
	}
}
