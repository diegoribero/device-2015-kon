using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour {

  public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
  public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
  public float m_Speed = 12f;                 // How fast the tank moves forward and back.

  private Rigidbody mRigidbody;
  private float horizontal;
  private float vertical; 
  private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
  private string m_TurnAxisName;              // The name of the input axis for turning.
  


  // Use this for initialization
  void Start () {
    mRigidbody = GetComponent<Rigidbody>();
    // The axes names are based on player number.
    m_MovementAxisName = "Vertical";
    m_TurnAxisName = "Horizontal";
  }
	
	// Update is called once per frame
	void Update () {
    // Store the value of both input axes.
    vertical = Input.GetAxis(m_MovementAxisName);
    horizontal = Input.GetAxis(m_TurnAxisName);
  }

  private void FixedUpdate() {
    // Adjust the rigidbodies position and orientation in FixedUpdate.
    Move();
    Turn();
	LimitMovement();
  }

	private void LimitMovement(){
		if (Mathf.Abs (vertical) < 0.1f && Mathf.Abs (horizontal) < 0.1f){
			mRigidbody.isKinematic = true;
		} else {
			mRigidbody.isKinematic = false;
		}
	}

  private void Move() {
    // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
    Vector3 movement = transform.forward * vertical * m_Speed * Time.deltaTime;
    // Apply this movement to the rigidbody's position.
    mRigidbody.MovePosition(mRigidbody.position + movement);
  }


  private void Turn() {
    // Determine the number of degrees to be turned based on the input, speed and time between frames.
    float turn = horizontal * m_TurnSpeed * Time.deltaTime;

    // Make this into a rotation in the y axis.
    Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

    // Apply this rotation to the rigidbody's rotation.
    mRigidbody.MoveRotation(mRigidbody.rotation * turnRotation);
  }
}
