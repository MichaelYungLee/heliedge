using UnityEngine;
using System.Collections;

public class Crank : MonoBehaviour {

	public VRBasics_Lever lever;
	public VRBasics_Hinge hinge;

	private float smoothAngle;
	private float smoothFrameChange = 0.1f;
	private float velOfChange = 0.0f;

	//private Vector3 newPos;

	public GameObject car;

	void Start () {

		//newPos = car.transform.position;
	}

	void Update () {

		if (!GetComponent<VRBasics_Grabbable> ().GetIsGrabbed ()) {
			
			smoothAngle = hinge.angleFrameChange;
		}

		if (GetComponent<VRBasics_Grabbable> ().GetIsGrabbed () && hinge.angleFrameChange != 0.0f) {

			smoothAngle = Mathf.SmoothDamp (smoothAngle, hinge.angleFrameChange, ref velOfChange, smoothFrameChange);

			float dist = (2 * Mathf.PI * lever.length) * (smoothAngle / 360);

			car.transform.position += car.transform.forward * dist;

			//newPos += car.transform.forward * dist;

		}	

		//car.transform.position = Vector3.Lerp (car.transform.position, newPos, Time.deltaTime * smoothTime);
	}
}
