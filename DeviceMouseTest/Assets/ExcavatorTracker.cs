using UnityEngine;
using System.Collections;

public class ExcavatorTracker : MonoBehaviour {

	//
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
	
	JointLimits baseLimits;
	JointLimits armLimits;
	JointLimits bucketLimits;
	JointLimits unionLimits;
	//

	public int sensor;
	private Quaternion currentRotation;
	float test = 0;
	Vector3 vect;
	float x, y, z, newX, newY, newZ;

	float minAngleThreshold = 25;
	float maxAngleThreshold = 50;
	// Use this for initialization
	void Start () {
		//
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
		//
		VRPNEventManager.StartListeningTracker(VRPNManager.Tracker_Types.vrpn_Tracker_RazerHydra, VRPNDeviceConfig.Device_Names.Tracker0, trackerUpdate);
	}

	void setLimits(HingeJoint joint, JointLimits limits){
		limits.min = joint.angle;
		limits.max = joint.angle;
		joint.limits = limits;
		joint.useLimits = true;
	}

	float getAcceleration(float minLimit, float maxLimit, float value){
		float result, tempValue;
		if(value >= minLimit){
			tempValue = (value > maxLimit) ? maxLimit : value;
			result = (tempValue - minLimit) / (maxLimit - minLimit);
			
		} else if(value <= -minLimit){
			tempValue = (value < -maxLimit) ? -maxLimit : value;
			result = (tempValue + minLimit) / (maxLimit - minLimit);
		} else {
			result = 0;
		}

		return result;
	}

	// Update is called once per frame
	void Update () {
		currentRotation.ToAngleAxis(out test, out vect);
		x = test*vect.x;
		y = test*vect.y;
		z = test*vect.z;
		Debug.Log("X: " + test*vect.x + " Y: " + test*vect.y + " Z: " + test*vect.z);
		//Debug.Log("Test: " + test + " Vect: " + vect);

		if(sensor == 0){
			unionAcceleration = getAcceleration(minAngleThreshold, maxAngleThreshold, x);
		} else if (sensor == 1){
			bucketAcceleration = getAcceleration(minAngleThreshold, maxAngleThreshold, x);
		}

		if(sensor == 0){
			armAcceleration = getAcceleration(minAngleThreshold, maxAngleThreshold, z);
		} else if (sensor == 1){
			baseAcceleration = getAcceleration(minAngleThreshold, maxAngleThreshold, z);
		}

		Debug.Log("unionAcceleration: " + unionAcceleration + " bucketAcceleration: " + bucketAcceleration + " armAcceleration: " + armAcceleration + " baseAcceleration: " + baseAcceleration);

		//getAcceleration(minAngleThreshold, maxAngleThreshold, y);

		if(armAcceleration != 0){
			armLimits.min = -60;
			armLimits.max = 60;
			if(useLimits) {
				armJoint.limits = armLimits;
			}
		} else {
			if(useLimits) {
				setLimits(armJoint, armLimits);
			}
		}

		if(bucketAcceleration != 0){
			bucketLimits.min = -60;
			bucketLimits.max = 65;
			if(useLimits) {
				bucketJoint.limits = bucketLimits;
			}
		} else {
			if(useLimits) {
				setLimits(bucketJoint, bucketLimits);
			}
		}

		if(baseAcceleration != 0){
			baseLimits.min = -135;
			baseLimits.max = 45;
			if(useLimits) {
				baseJoint.limits = baseLimits;
			}
		} else {
			if(useLimits) {
				setLimits(baseJoint, baseLimits);
			}
		}



		if(sensor == 0){
			armMotor.targetVelocity = power * armAcceleration;
			armJoint.motor = armMotor;
			unionMotor.targetVelocity = power * unionAcceleration;
			unionJoint.motor = unionMotor;
		} else if (sensor == 1){
			baseMotor.targetVelocity = power * baseAcceleration;
			baseJoint.motor = baseMotor;
			bucketMotor.targetVelocity = power * bucketAcceleration;
			bucketJoint.motor = bucketMotor;
		}

		//Debug.Log("NewVel: " + power * bucketAcceleration);
		//Debug.Log("bucketMotor.targetVelocity: " + bucketMotor.targetVelocity);			

	}

	//Método que se encarga de estar pendiente de los mensajes (posición y orientación) que se envían desde el tracker del dispositivo
	void trackerUpdate(string name, VRPNTracker.TrackerReport report)
	{
		//if (sensor == 0 && report.sensor == 0 || sensor == 1 && report.sensor == 1)
		if (sensor == 0 && report.sensor == 0 || sensor == 1 && report.sensor == 1)
		{

			//Posición: Notar que 'y' y 'z' se encuentran intercambiados
			//this.transform.position = new Vector3((float)report.pos[0] * 10, (float)report.pos[2] * 10, (float)report.pos[1] * 10);
			//Rotación: Notar que es un cuaternión y no ángulos de euler, y que nuevamente 'y' y 'z' se encuentran intercambiados, además que 'x', 'y', y 'z' se encuentran invertidos
			this.transform.localRotation = new Quaternion(-1 * (float)report.quat[0], -1 * (float)report.quat[2], -1 * (float)report.quat[1], (float)report.quat[3]);
			currentRotation = new Quaternion(-1 * (float)report.quat[0], -1 * (float)report.quat[2], -1 * (float)report.quat[1], (float)report.quat[3]);
		}
	}
}
