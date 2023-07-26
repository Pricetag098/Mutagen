using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionProperty moveDir;
    [SerializeField] GameObject body;
    public Transform orientation;
	[SerializeField] PlayerStats playerStats;
	[SerializeField] LayerMask groundingLayer;
	[SerializeField] float groundingRange;

	[Header("MovementSettings")]
	[SerializeField] float walkMaxSpeed = 10; 
	[SerializeField] float walkAcceleration = 1000; 
	[SerializeField] float walkSlowForce = 1000;
	[SerializeField] float walkControlForce = 1000;
	[SerializeField] float gravityForce = 1000;

	[Header("Animation Settings")]
	[SerializeField] Optional<Animator> animator;
	[SerializeField] string forwardVelocityParam;
	[SerializeField] string horizontalVelocityParam;
	Rigidbody rb;
    Vector2 inputDir;

	public Vector3 movementDir;
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

	private void OnEnable()
	{
        moveDir.action.Enable();
	}

	private void OnDisable()
	{
		moveDir.action.Disable();
	}
	// Update is called once per frame
	void Update()
    {
        inputDir = moveDir.action.ReadValue<Vector2>();
		movementDir = orientation.forward * inputDir.y + orientation.right * inputDir.x;
    }

	private void FixedUpdate()
	{
		Move(walkMaxSpeed,walkAcceleration,walkSlowForce,walkControlForce);
	}

	void Move(float maxSpeed, float acceleration, float slowForce, float controlForce)
	{

		acceleration *= Time.fixedDeltaTime;
		slowForce *= Time.fixedDeltaTime;
		controlForce *= Time.fixedDeltaTime;
		
		Vector3 force = Vector3.zero;

		Vector3 playerVel = rb.velocity;

		//Project player velocity to relative vectors on the player
		//basicaly calculating what the velocity is in the players forward/backward and left/right direction
		float fwVel = Vector3.Dot(playerVel, orientation.forward * Mathf.Sign(inputDir.y));
		float rightVel = Vector3.Dot(playerVel, orientation.right * Mathf.Sign(inputDir.x));


		if (fwVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.y))
		{
			Vector3 forceDir = orientation.forward * inputDir.y * acceleration * playerStats.accelerationMulti;

			if (Mathf.Sign(fwVel) < 0)
			{
				forceDir *= 2;
			}
			force += forceDir;
		}

		if (rightVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.x))
		{
			Vector3 forceDir = orientation.right * inputDir.x * acceleration * playerStats.accelerationMulti;
			if (Mathf.Sign(rightVel) < 0)
			{
				forceDir *= 2;
			}
			force += forceDir;
		}

		//reduce velocity in directions were not moving
		if (inputDir.y == 0)
			force += slowForce * Vector3.Dot(playerVel, orientation.forward) * -orientation.forward;
		if (inputDir.x == 0)
			force += slowForce * Vector3.Dot(playerVel, orientation.right) * -orientation.right;
		



		//project the forward velocity onto the floor for walking on slopes
		RaycastHit hit;
		if (Physics.Raycast(orientation.position, -orientation.up, out hit, groundingRange, groundingLayer))
		{
			force = Vector3.ProjectOnPlane(force, hit.normal);
		}
		else
		{
			rb.AddForce(-orientation.up *gravityForce * Time.fixedDeltaTime,ForceMode.VelocityChange);
		}


		rb.AddForce(force);
		if (rb.velocity.magnitude > maxSpeed)
		{
			rb.AddForce(-rb.velocity.normalized * controlForce);
		}


		if (animator.Enabled)
		{
			animator.Value.SetFloat(forwardVelocityParam, fwVel);
			animator.Value.SetFloat(horizontalVelocityParam, rightVel);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawRay(orientation.position, -orientation.up* groundingRange);
	}
}
