using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionProperty moveDir;
    public GameObject body;
    public Transform orientation;
	[SerializeField] PlayerStats playerStats;
	[SerializeField] LayerMask groundingLayer;
	[SerializeField] float groundingRange;

    
    [SerializeField]MovementData walk, sprint;

    [SerializeField] Transform groundingPoint;
    [SerializeField] float groundingRadius;
    [SerializeField] float minSurface;
    [SerializeField] float surfaceCheckRange;
    [SerializeField] float gravityForce = 1000;
    [SerializeField] Vector3 gravityDir;

    [SerializeField] float sprintDelay = 5;
	public float timeSinceLastInteruption;
    

    Rigidbody rb;
    Vector2 inputDir;

	public Vector3 movementDir;
	Vector3 lastSafeLocation;

    [System.Serializable]
    class MovementData
    {
        public float speed;
        public float accleration;
    }
    // Start is called before the first frame update
    void Start()
    {
        lastSafeLocation = transform.position;
        rb = GetComponent<Rigidbody>();
		GetComponent<Health>().OnHit += ResetSprint;
    }
	void ResetSprint(DamageData data)
	{
		timeSinceLastInteruption = 0;
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
		timeSinceLastInteruption += Time.deltaTime;
        if(movementDir == Vector3.zero)
        {
            timeSinceLastInteruption = 0;
        }
    }

	private void FixedUpdate()
	{
        IsGrounded();
		if(timeSinceLastInteruption < sprintDelay)
		{
            Move(walk);
        }
		else
		{
			Move(sprint);
		}
		
	}
    bool touchingSurface;
    Vector3 surfaceNormal;
    RaycastHit lastSurface;
    bool IsGrounded()
    {
        RaycastHit hit;
        bool isGrounded = Physics.CheckSphere(groundingPoint.position, groundingRadius, groundingLayer) && Vector3.Dot(surfaceNormal, orientation.up) > minSurface;
        if(isGrounded)
        {
            lastSafeLocation = transform.position;
        }
        if (Physics.SphereCast(orientation.position, groundingRadius, -orientation.up, out hit, surfaceCheckRange * 5, groundingLayer))
        {
            surfaceNormal = hit.normal;
            touchingSurface = hit.distance <= surfaceCheckRange;
            lastSurface = hit;
        }
        else
        {
            touchingSurface = false;
        }
        return isGrounded;
    }
    void Move(MovementData data)
    {
        Vector3 idealVel = Vector3.ProjectOnPlane((orientation.forward * inputDir.y + orientation.right * inputDir.x) * data.speed * Mathf.Max(0, playerStats.speedMulti), surfaceNormal);
        Vector3 vel = rb.velocity;
        Vector3 turningForce = idealVel - vel;
        rb.AddForce(turningForce * data.accleration);

        if (!touchingSurface)
        {
            rb.AddForce(gravityDir);
        }
    }

    //void Move(float maxSpeed, float acceleration, float slowForce, float controlForce, float altDirectionMulti)
    //{

    //	acceleration *= Time.fixedDeltaTime;
    //	slowForce *= Time.fixedDeltaTime;
    //	controlForce *= Time.fixedDeltaTime;

    //	Vector3 force = Vector3.zero;

    //	Vector3 playerVel = rb.velocity;

    //	//Project player velocity to relative vectors on the player
    //	//basicaly calculating what the velocity is in the players forward/backward and left/right direction
    //	float fwVel = Vector3.Dot(playerVel, orientation.forward * Mathf.Sign(inputDir.y));
    //	float rightVel = Vector3.Dot(playerVel, orientation.right * Mathf.Sign(inputDir.x));


    //	if (fwVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.y))
    //	{
    //		Vector3 forceDir = orientation.forward * inputDir.y * acceleration * playerStats.accelerationMulti;

    //		if (Mathf.Sign(fwVel) < 0)
    //		{
    //			forceDir *= altDirectionMulti;
    //		}
    //		force += forceDir;
    //	}

    //	if (rightVel < maxSpeed * playerStats.speedMulti * Mathf.Abs(inputDir.x))
    //	{
    //		Vector3 forceDir = orientation.right * inputDir.x * acceleration * playerStats.accelerationMulti;
    //		if (Mathf.Sign(rightVel) < 0)
    //		{
    //			forceDir *= altDirectionMulti;
    //		}
    //		force += forceDir;
    //	}

    //	//reduce velocity in directions were not moving
    //	if (inputDir.y == 0)
    //		force += slowForce * Vector3.Dot(playerVel, orientation.forward) * -orientation.forward;
    //	if (inputDir.x == 0)
    //		force += slowForce * Vector3.Dot(playerVel, orientation.right) * -orientation.right;




    //	//project the forward velocity onto the floor for walking on slopes
    //	RaycastHit hit;
    //	if (Physics.Raycast(orientation.position, -orientation.up, out hit, groundingRange, groundingLayer))
    //	{
    //		force = Vector3.ProjectOnPlane(force, hit.normal);
    //		lastSafeLocation = transform.position;
    //	}
    //	else
    //	{
    //		rb.AddForce(-orientation.up *gravityForce * Time.fixedDeltaTime,ForceMode.VelocityChange);
    //	}



    //	rb.AddForce(force);
    //	if (rb.velocity.magnitude > maxSpeed * playerStats.speedMulti)
    //	{
    //		rb.AddForce(-rb.velocity.normalized * controlForce);
    //	}


    //	//if (animator.Enabled)
    //	//{
    //	//	animator.Value.SetFloat(forwardVelocityParam, fwVel);
    //	//	animator.Value.SetFloat(horizontalVelocityParam, rightVel);
    //	//}
    //}

    private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawRay(orientation.position, -orientation.up* groundingRange);
	}

	public void ResetPos()
	{
		transform.position = lastSafeLocation;
		rb.velocity = Vector3.zero;
	}
}
