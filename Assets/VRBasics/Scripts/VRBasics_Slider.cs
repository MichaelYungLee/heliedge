
//========================= VRBasics_Slider ===================================
//
// draws a gizmo that indicates the position of a slider
// this child of a rail can be used for any object that moves back and forth along a single axis
// examples: drawers, sliding switches
// use the percentage property to get the position of the slider between 0.0 and 1.0
//
//=========================== by Zac Zidik ====================================
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(ConfigurableJoint))]
public class VRBasics_Slider : MonoBehaviour {

	//options to where a spring can move to
	public enum SpringTo{none,max,mid,min}

	//indicates the percentage of the slider along the rail (0.0 - 1.0)
	//0 - at the minimum, 1- at the maximum
	public float percentage;
	//used to set the start position of the slider  (0.0 - 1.0)
	//0 - at the minimum, 1- at the maximum
	public float position;
	//where is the joint currently springing to
	private SpringTo springTo;
	//spring a joint to the max limit
	public bool useSpringToMax = false;
	//spring amount to max
	public float springToMax;
	//damper amount to max
	public float damperToMax;
	//spring a joint to the middle of the total possible movement
	public bool useSpringToMid = false;
	//spring amount to middle
	public float springToMid;
	//damper amount to middle
	public float damperToMid;
	//spring a joint to the min limit
	public bool useSpringToMin = false;
	//spring amount to min
	public float springToMin;
	//damper amount to min
	public float damperToMin;

	//the percentage of the total amount of movement (0.0 - 1.0)
	public void CalcPercentage(){
		//an empty game object used to aid in positioning
		GameObject dummyTrans = GetDummy();
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//set the dummy at the position and rotation of the rail transform
		dummyTrans.transform.position = rail.transform.position;
		dummyTrans.transform.eulerAngles = rail.transform.eulerAngles;
		//include the anchor move of the rail
		dummyTrans.transform.position += dummyTrans.transform.up * rail.anchorMove;
		//reference to the joint of the slider
		ConfigurableJoint configJoint = GetComponent<ConfigurableJoint> ();
		//the minimum position along the rail
		Vector3 minLimitPos = dummyTrans.transform.position + (transform.up.normalized * configJoint.linearLimit.limit);
		//the distance between the slider and the minimum position along the rail
		float dist = Vector3.Distance (transform.position, minLimitPos);
		//convert the distance to a percentage of the rail length
		percentage = ((dist*100)/rail.length)/100;
		percentage = Mathf.Clamp (percentage, 0.0f, 1.0f);
		//remove the dummy
		DestroyImmediate (dummyTrans);
	}

	#if UNITY_EDITOR
	public void DrawGizmo(){
		//color of gizmo
		Handles.color = Color.magenta;
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//draw a solid dot at the joint
		Handles.DrawSolidDisc (transform.position, rail.transform.right, 0.01f);
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

	public void SetConnectedAnchorPos(){
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//reference to the configurable joint the gizmo displays
		ConfigurableJoint configJoint = GetComponent<ConfigurableJoint> ();
		configJoint.autoConfigureConnectedAnchor = false;
		Vector3 newAnchorPos = configJoint.connectedAnchor;
		newAnchorPos.x = 0.0f;
		newAnchorPos.y = rail.anchorMove;
		newAnchorPos.z = 0.0f;
		configJoint.connectedAnchor = newAnchorPos;
	}

	public void SetLinearLimit(){
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//reference to the configurable joint the gizmo displays
		ConfigurableJoint configJoint = GetComponent<ConfigurableJoint> ();
		//lock limits according to length of rail
		SoftJointLimit linearLimit = configJoint.linearLimit;
		//the limit of the joint is half of the rail length
		if (linearLimit.limit != rail.length * 0.5f) {
			linearLimit.limit = rail.length * 0.5f;
			configJoint.linearLimit = linearLimit;
		}
	}

	public void EditorSetPosition(){
		//an empty game object used to aid in positioning
		GameObject dummyTrans = GetDummy();
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//set the dummy at the position and rotation of the rail transform
		dummyTrans.transform.position = rail.transform.position;
		dummyTrans.transform.eulerAngles = rail.transform.eulerAngles;
		//include the anchor move of the rail
		dummyTrans.transform.position += dummyTrans.transform.up * rail.anchorMove;
		//reference to the joint of the slider
		ConfigurableJoint configJoint = GetComponent<ConfigurableJoint> ();
		//the minimum position along the rail
		Vector3 minLimitPos = dummyTrans.transform.position + (transform.up.normalized * configJoint.linearLimit.limit);
		//set the dummy to the minimum position
		dummyTrans.transform.position = minLimitPos;
		//translate the position number into a distance along the rail
		float dist = ((position * 100) * rail.length) / 100;
		//move the dummy along the rail
		dummyTrans.transform.position -= dummyTrans.transform.up * dist;
		//move the slide to the dummy position
		transform.position = dummyTrans.transform.position;
		//remove the dummy
		DestroyImmediate (dummyTrans);
	}

	//p must be between 0 and 1
	public void SetPosition(float p){
		//clamp p
		p = Mathf.Clamp(p, 0.0f, 1.0f);
		//an empty game object used to aid in positioning
		GameObject dummyTrans = GetDummy();
		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();
		//set the dummy at the position and rotation of the rail transform
		dummyTrans.transform.position = rail.transform.position;
		dummyTrans.transform.eulerAngles = rail.transform.eulerAngles;
		//include the anchor move of the rail
		dummyTrans.transform.position += dummyTrans.transform.up * rail.anchorMove;
		//reference to the joint of the slider
		ConfigurableJoint configJoint = GetComponent<ConfigurableJoint> ();
		//the minimum position along the rail
		Vector3 minLimitPos = dummyTrans.transform.position + (transform.up.normalized * configJoint.linearLimit.limit);
		//set the dummy to the minimum position
		dummyTrans.transform.position = minLimitPos;
		//translate the position number into a distance along the rail
		float dist = ((p * 100) * rail.length) / 100;
		//move the dummy along the rail
		dummyTrans.transform.position -= dummyTrans.transform.up * dist;
		//before moving the slider with script, best make it kinematic
		GetComponent<Rigidbody> ().isKinematic = true;
		//move the slide to the dummy position
		transform.position = dummyTrans.transform.position;
		//after moving the slider with script, best make it not kinematic
		GetComponent<Rigidbody> ().isKinematic = false;
		//remove the dummy
		DestroyImmediate (dummyTrans);
	}

	public void SetSpring(){

		//if using any of the springs
		if(useSpringToMax || useSpringToMid || useSpringToMin){

			//store the drive of the joint
			JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

			//if using spring to maximum, middle and minimum
			if (useSpringToMax && useSpringToMid && useSpringToMin) {

				//use the percentage to set where the slider springs towards
				if (percentage >= 0.75f) {

					//set spring settings to maximum
					if (springTo != SpringTo.max || yDrive.positionSpring != springToMax ||	yDrive.positionDamper != damperToMax) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMax();
					}

				} else if (percentage >= 0.25f && percentage < 0.75f) {

					//set spring settings to middle
					if (springTo != SpringTo.mid || yDrive.positionSpring != springToMid ||	yDrive.positionDamper != damperToMid) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMid();
					}

				} else {

					//set spring settings to minimum
					if (springTo != SpringTo.min || yDrive.positionSpring != springToMin ||	yDrive.positionDamper != damperToMin) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMin();
					}
				}

			//if using spring to maximum and middle but not to minimum
			}else if (useSpringToMax && useSpringToMid && !useSpringToMin) {

				//use the percentage to set where the slider springs towards
				if (percentage >= 0.75f) {

					//set spring settings to maximum
					if (springTo != SpringTo.max || yDrive.positionSpring != springToMax ||	yDrive.positionDamper != damperToMax) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMax();
					}

				} else {

					//set spring settings to middle
					if (springTo != SpringTo.mid || yDrive.positionSpring != springToMid ||	yDrive.positionDamper != damperToMid) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMid();
					}
				}

			//if using spring to middle and minimum but not to maximum
			}else if (!useSpringToMax && useSpringToMid && useSpringToMin) {

				//use the percentage to set where the slider springs towards
				if (percentage >= 0.25f) {

					//set spring settings to middle
					if (springTo != SpringTo.mid || yDrive.positionSpring != springToMid ||	yDrive.positionDamper != damperToMid) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMid();
					}

				} else {

					//set spring settings to minimum
					if (springTo != SpringTo.min || yDrive.positionSpring != springToMin ||	yDrive.positionDamper != damperToMin) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMin();
					}
				}

			//if using spring to maximum and minimum but not to middle
			}else if (useSpringToMax && !useSpringToMid && useSpringToMin) {

				//use the percentage to set where the slider springs towards
				if (percentage >= 0.5f) {

					//set spring settings to maximum
					if (springTo != SpringTo.max || yDrive.positionSpring != springToMax ||	yDrive.positionDamper != damperToMax) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMax();
					}

				} else {

					//set spring settings to minimum
					if (springTo != SpringTo.min || yDrive.positionSpring != springToMin ||	yDrive.positionDamper != damperToMin) {
						//set the values to the slider's spring
						GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMin();
					}

				}

			//if only using spring to maximum
			} else if (useSpringToMax && !useSpringToMid && !useSpringToMin) {

				//set spring settings to maximum
				if (springTo != SpringTo.max || yDrive.positionSpring != springToMax ||	yDrive.positionDamper != damperToMax) {
					//set the values to the slider's spring
					GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMax();
				}

			//if only using spring to middle
			} else if (!useSpringToMax && useSpringToMid && !useSpringToMin) {

				//set spring settings to middle
				if (springTo != SpringTo.mid || yDrive.positionSpring != springToMid ||	yDrive.positionDamper != damperToMid) {
					//set the values to the slider's spring
					GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMid();
				}

			//if only using spring to minimum
			} else if (!useSpringToMax && !useSpringToMid && useSpringToMin) {

				//set spring settings to minimum
				if (springTo != SpringTo.min || yDrive.positionSpring != springToMin ||	yDrive.positionDamper != damperToMin) {
					//set the values to the slider's spring
					GetComponent<ConfigurableJoint> ().yDrive = SetSpringToMin();
				}
			}

		//using none of the springs
		}else{

			//store the drive of the joint
			JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

			//set spring settings to none
			if (springTo != SpringTo.none || yDrive.positionSpring != 0.0f ||	yDrive.positionDamper != 0.0f) {
				//set the values to the slider's spring
				GetComponent<ConfigurableJoint> ().yDrive = SetSpringToNone();
			}
		}
	}

	JointDrive SetSpringToNone(){

		//store the drive of the joint
		JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

		//set spring settings
		yDrive.positionSpring = 0.0f;
		yDrive.positionDamper = 0.0f;
		GetComponent<ConfigurableJoint> ().targetPosition = Vector3.zero;

		//set where the spring is currently springing to
		springTo = SpringTo.none;

		return yDrive;
	}

	JointDrive SetSpringToMax(){

		//store the drive of the joint
		JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();

		//set spring settings
		yDrive.positionSpring = springToMax;
		yDrive.positionDamper = damperToMax;
		GetComponent<ConfigurableJoint> ().targetPosition = new Vector3 (0, rail.length * 0.5f, 0);

		//set where the spring is currently springing to
		springTo = SpringTo.max;

		return yDrive;
	}

	JointDrive SetSpringToMid(){

		//store the drive of the joint
		JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

		//set spring settings
		yDrive.positionSpring = springToMid;
		yDrive.positionDamper = damperToMid;
		GetComponent<ConfigurableJoint> ().targetPosition = new Vector3 (0, 0, 0);

		//set where the spring is currently springing to
		springTo = SpringTo.mid;

		return yDrive;
	}

	JointDrive SetSpringToMin(){

		//store the drive of the joint
		JointDrive yDrive = GetComponent<ConfigurableJoint> ().yDrive;

		//reference to the parent rail
		VRBasics_Rail rail = transform.parent.gameObject.GetComponent<VRBasics_Rail>();

		//set spring settings
		yDrive.positionSpring = springToMin;
		yDrive.positionDamper = damperToMin;
		GetComponent<ConfigurableJoint> ().targetPosition = new Vector3 (0, -(rail.length * 0.5f), 0);

		//set where the spring is currently springing to
		springTo = SpringTo.min;

		return yDrive;
	}
		
	void Update(){
		//calculate the percentage of the slider along the rail
		CalcPercentage ();
		//keep the position the same as the percentage at runtime
		position = percentage;
		//keeps the spring at the proper settings
		SetSpring();
	}
}

