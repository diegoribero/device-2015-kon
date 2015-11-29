﻿using UnityEngine;
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

	public bool useLimits = false;

	private float baseLimitsMin = -135;
	private float baseLimitsMax = 45;
	private float armLimitsMin = -60;
	private float armLimitsMax = 60;
	private float bucketLimitsMin = -60;
	private float bucketLimitsMax = 65;

	JointLimits baseLimits;
	JointLimits armLimits;
	JointLimits bucketLimits;
	JointLimits unionLimits;


	// Use this for initialization
	void Start () {
		baseJoint.useMotor = true;
		armJoint.useMotor = true;
		bucketJoint.useMotor = true;
		unionJoint.useMotor = true;
		baseMotor = baseJoint.motor;
		baseMotor.force = 100;
		armMotor = armJoint.motor;
		armMotor.force = 100;
		bucketMotor = bucketJoint.motor;
		bucketMotor.force = 100;
		unionMotor = unionJoint.motor;
		unionMotor.force = 200;
		if(useLimits) {
			setLimits(baseJoint, baseLimits);
			setLimits(armJoint, armLimits);
			setLimits(bucketJoint, bucketLimits);
		}
	}

	void setLimits(HingeJoint joint, JointLimits limits){
		limits.min = joint.angle;
		limits.max = joint.angle+1;
		joint.limits = limits;
		joint.useLimits = true;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)){
			//baseJoint.useLimits = false;
			baseLimits.min = baseLimitsMin;
			baseLimits.max = baseLimitsMax;
			if(useLimits) {
				baseJoint.limits = baseLimits;
			}
		} else if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)){
			if(useLimits) {
				setLimits(baseJoint, baseLimits);
			}
		}

		if(Input.GetKey(KeyCode.R)){
			armAcceleration = 1;
		}

		if(Input.GetKey(KeyCode.F)){
			armAcceleration = -1;
		}

		if(Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F)){
			//armJoint.useLimits = false;
			armLimits.min = armLimitsMin;
			armLimits.max = armLimitsMax;
			if(useLimits) {
				armJoint.limits = armLimits;
			}
		} else if(Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.F)){
			armAcceleration = 0;
			if(useLimits) {
				setLimits(armJoint, armLimits);
			}
		}


		if(Input.GetKey(KeyCode.T)){
			bucketAcceleration = 1;
		}
		
		if(Input.GetKey(KeyCode.G)){
			bucketAcceleration = -1;
		}


		if(Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.G)){
			//bucketJoint.useLimits = false;
			bucketLimits.min = bucketLimitsMin;
			bucketLimits.max = bucketLimitsMax;
			if(useLimits) {
				bucketJoint.limits = bucketLimits;
			}
		} else if(Input.GetKeyUp(KeyCode.T) || Input.GetKeyUp(KeyCode.G)){
			bucketAcceleration = 0;
			if(useLimits) {
				setLimits(bucketJoint, bucketLimits);
			}
		}

		baseAcceleration = Input.GetAxis("Vertical");
		baseMotor.targetVelocity = power * baseAcceleration;
		baseJoint.motor = baseMotor;
			
		armMotor.targetVelocity = power * armAcceleration;
		armJoint.motor = armMotor;

		bucketMotor.targetVelocity = power * bucketAcceleration;
		bucketJoint.motor = bucketMotor;

		unionAcceleration = Input.GetAxis("Horizontal");
		unionMotor.targetVelocity = power * unionAcceleration;
		unionJoint.motor = unionMotor;
		/*
		if(baseJoint.angle < -135 || baseJoint.angle > 45)
			baseJoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);

		if(armJoint.angle < -60 || armJoint.angle > 60)
			armJoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);

		if(bucketJoint.angle < -60 || bucketJoint.angle > 65)
			bucketJoint.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);
	*/
		//Base 45 -135
		//Arm 60 -60
		//Bucket 65 -60

		//Debug.Log("Force: " + baseMotor.force + " Pow: " + power + " Acc: " + baseAcceleration);
		//Debug.Log("Base angle: " + baseJoint.angle + " Arm angle: " + armJoint.angle + " Bucket angle: " + bucketJoint.angle);
	}
	
}
