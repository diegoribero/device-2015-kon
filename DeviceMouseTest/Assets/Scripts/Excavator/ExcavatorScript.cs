using UnityEngine;
using System.Collections;

public class ExcavatorScript : MonoBehaviour {
	
	public HingeJoint bucketJoint;
	public HingeJoint armJoint;
	public HingeJoint baseJoint;
	public HingeJoint unionJoint;
	private JointMotor baseMotor;
	private JointMotor armMotor;
	private JointMotor bucketMotor;
	private JointMotor unionMotor;
	private float baseAcceleration = 0;
	private float armAcceleration = 0;
	private float bucketAcceleration = 0;
	private float unionAcceleration = 0;
	private float power = 200;	
	
	/*
	public bool enableLimits = false;
	private float baseLimitsMin = -135;
	private float baseLimitsMax = 45;
	private float armLimitsMin = -60;
	private float armLimitsMax = 60;
	private float bucketLimitsMin = -60;
	private float bucketLimitsMax = 65;
	
	private JointLimits baseLimits;
	private JointLimits armLimits;
	private JointLimits bucketLimits;
	private JointLimits unionLimits;
	*/

	private string m_ExcavatorRotationAxisName;
	private string m_BaseRotationAxisName;
	private string m_ArmRotationAxisName;
	private string m_BucketRotationAxisName;
	
	
	// Use this for initialization
	void Start () {
		baseJoint.useMotor = true;
		armJoint.useMotor = true;
		bucketJoint.useMotor = true;
		unionJoint.useMotor = true;
		baseMotor = baseJoint.motor;
		baseMotor.force = 400;
		armMotor = armJoint.motor;
		armMotor.force = 100;
		bucketMotor = bucketJoint.motor;
		bucketMotor.force = 100;
		unionMotor = unionJoint.motor;
		unionMotor.force = 200;

		m_ExcavatorRotationAxisName = "Rotate Excavator";
		m_BaseRotationAxisName = "Rotate Base";
		m_ArmRotationAxisName = "Rotate Arm";
		m_BucketRotationAxisName = "Rotate Bucket";

		/*if(enableLimits) {
			setLimits(baseJoint, baseLimits);
			setLimits(armJoint, armLimits);
			setLimits(bucketJoint, bucketLimits);
		}*/
	}
	
	void setLimits(HingeJoint joint, JointLimits limits){
		limits.min = joint.angle;
		limits.max = joint.angle+1;
		joint.limits = limits;
		joint.useLimits = true;
	}

	private void setExcavatorRotation(){
		unionAcceleration = Input.GetAxis (m_ExcavatorRotationAxisName);

		/*
		if(Input.GetKey(KeyCode.E)){
			unionAcceleration = -1;
		}
		
		if(Input.GetKey(KeyCode.Q)){
			unionAcceleration = 1;
		}
		
		if(Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E)){
			unionAcceleration = 0;
			/*if(enableLimits) {
				setLimits(unionJoint, unionLimits);
			}*/
		//}
	}

	private void rotateExcavator(){
		unionMotor.targetVelocity = power * unionAcceleration;
		unionJoint.motor = unionMotor;
	}

	private void setBaseRotation(){

		baseAcceleration = Input.GetAxis(m_BaseRotationAxisName);

		/*
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			baseAcceleration = 1;
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			baseAcceleration = -1;
		} else {
			baseAcceleration = 0;
		}*/
	}

	private void rotateBase(){
		baseMotor.targetVelocity = power * baseAcceleration;
		baseJoint.motor = baseMotor;
	}

	private void setArmRotation(){

		armAcceleration = Input.GetAxis(m_ArmRotationAxisName);
		/*
		if(Input.GetMouseButton(0)){
			armAcceleration = 1;
		}
		
		if(Input.GetMouseButton(1)){
			armAcceleration = -1;
		}
		
		if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
			/*armJoint.useLimits = false;
			armLimits.min = armLimitsMin;
			armLimits.max = armLimitsMax;
			if(enableLimits) {
				armJoint.limits = armLimits;
			}*/
		/*
		} else if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)){
			armAcceleration = 0;
			/*if(enableLimits) {
				setLimits(armJoint, armLimits);
			}*/
		//}
	}

	private void rotateArm(){
		armMotor.targetVelocity = power * armAcceleration;
		armJoint.motor = armMotor;
	}


	private void setBucketRotation(){
		bucketAcceleration = Input.GetAxis(m_BucketRotationAxisName);
	}

	private void rotateBucket(){
		bucketMotor.targetVelocity = power * bucketAcceleration;
		bucketJoint.motor = bucketMotor;
	}


	void Update () {
	
		setArmRotation();
		rotateArm();

		setBaseRotation();
		rotateBase();

		setExcavatorRotation();
		rotateExcavator();

		setBucketRotation();
		rotateBucket ();

		//Base 45 -135
		//Arm 60 -60
		//Bucket 65 -60
	}
	
}
