using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

	void OnTriggerEnter(Collider other){

		//if the object is set to toss and retrieve
		if (other.gameObject.GetComponent<TossedAndRetrieved> ()) {

			if (other.gameObject.GetComponent<Rigidbody> ()) {
				other.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			}

			//reposition it
			other.gameObject.transform.position = other.gameObject.GetComponent<TossedAndRetrieved> ().reappearPosition;
		}
	}
}
