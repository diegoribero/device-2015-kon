using UnityEngine;

public class ExcavatorMovement : MonoBehaviour
{
	public int m_PlayerNumber = 1;              // Used to identify which excavator belongs to which player.
	public float m_Speed = 3f;                 // How fast the excavator moves forward and back.
	public float m_TurnSpeed = 40f;            // How fast the excavator turns in degrees per second.
	public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
	public AudioClip m_EngineIdling;            // Audio to play when the excavator isn't moving.
	public AudioClip m_EngineDriving;           // Audio to play when the excavator is moving.
	public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
  public Rigidbody baseCabinRiginbody;

	
	private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
	private string m_TurnAxisName;              // The name of the input axis for turning.
	private Rigidbody m_Rigidbody;              // Reference used to move the excavator.
	private float m_MovementInputValue;         // The current value of the movement input.
	private float m_TurnInputValue;             // The current value of the turn input.
	private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
	
	
	private void Awake ()
	{
		m_Rigidbody = GetComponent<Rigidbody> ();
	}
	
	
	private void OnEnable ()
	{
		m_Rigidbody.isKinematic = false; // When the excavator is turned on, make sure it's not kinematic.
		m_MovementInputValue = 0f; // Also reset the input values.
		m_TurnInputValue = 0f;
	}
	
	
	private void OnDisable ()
	{
		m_Rigidbody.isKinematic = true; // When the excavator is turned off, set it to kinematic so it stops moving.
	}
	
	
	private void Start ()
	{
		m_MovementAxisName = "Vertical";// + m_PlayerNumber;
		m_TurnAxisName = "Horizontal";// + m_PlayerNumber;

		m_OriginalPitch = m_MovementAudio.pitch; // Store the original pitch of the audio source.
	}
	
	
	private void Update ()
	{
		m_MovementInputValue = Input.GetAxis (m_MovementAxisName); // Store the value of both input axes.
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
		
		EngineAudio ();
	}
	
	
	private void EngineAudio ()
	{
		// If there is no input (the excavator is stationary)...
		if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
		{
			// ... and if the audio source is currently playing the driving clip...
			if (m_MovementAudio.clip == m_EngineDriving)
			{
				// ... change the clip to idling and play it.
				m_MovementAudio.clip = m_EngineIdling;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		}
		else
		{
			// Otherwise if the excavatir is moving and if the idling clip is currently playing...
			if (m_MovementAudio.clip == m_EngineIdling)
			{
				// ... change the clip to driving and play.
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play();
			}
		}
	}
	
	
	private void FixedUpdate ()
	{
		// Adjust the rigidbodies position and orientation in FixedUpdate.
		Move ();
		Turn ();
		LimitMovement();
    
	}
	

	private void LimitMovement(){
		if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f){
			m_Rigidbody.isKinematic = true;
		} else {
			m_Rigidbody.isKinematic = false;
		}
	}

	private void Move ()
	{
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime; // Create a vector in the direction the excavator is facing
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement); // Apply this movement to the rigidbody's position.
	}
	
	
	private void Turn ()
	{
		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime; // Determine the number of degrees to be turned.
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f); // Make this into a rotation in the y axis.
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation); // Apply this rotation to the rigidbody's rotation.
	}
}