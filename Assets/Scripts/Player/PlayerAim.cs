using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerAim : MonoBehaviour
{
    [SerializeField] InputActionProperty aimAction;
	[SerializeField] LayerMask validRayLayers = 1;
    PlayerMovement playerMovement;
    public Vector3 aimDir;

	public bool useMouse;
	[SerializeField] Optional<Animator> animator;
	Rigidbody rb;
	public Optional<Image> cursor;
	[SerializeField] string fwVelAnimationKey = "fwVel";
	[SerializeField] string lrVelAnimationKey = "lrVel";
	float angleConstant;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        aimAction.action.performed += AimInput;
		angleConstant = Camera.main.transform.rotation.eulerAngles.x;
		angleConstant = Mathf.Tan(angleConstant * Mathf.Deg2Rad);
		angleConstant = 1 / angleConstant;
		rb = GetComponent<Rigidbody>();
	}

	

	private void OnEnable()
	{
		aimAction.action.Enable();
	}
	private void OnDisable()
	{
		aimAction.action.Disable();
	}

    void AimInput(InputAction.CallbackContext context)
	{
		if(useMouse) { return; }
		Vector2 readVal = context.ReadValue<Vector2>();
		
		
		Vector3 tempAimDir = playerMovement.orientation.forward * readVal.y + playerMovement.orientation.right * readVal.x;
		aimDir = new Vector3(tempAimDir.x, 0, tempAimDir.z);
		
		playerMovement.body.transform.forward = aimDir;
	}

	private void Update()
	{
		float fwVel = Vector3.Dot(aimDir, rb.velocity);
		float lrVel = Vector3.Dot(Vector3.Cross(aimDir,Vector3.up),rb.velocity);
		if (animator.Enabled)
		{
			animator.Value.SetFloat(fwVelAnimationKey, fwVel);
			animator.Value.SetFloat(lrVelAnimationKey, lrVel);
		}
		if (useMouse)
		{
			
			RaycastHit hit;
			Vector3 hitPoint;
			float y;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, float.PositiveInfinity, validRayLayers))
			{
				hitPoint = hit.point;
				hitPoint.y = 0;
				y = hit.point.y;
				Vector3 tempAimDir = (hitPoint - transform.position - playerMovement.orientation.forward * ((transform.position.y + playerMovement.orientation.localPosition.y) - y) * angleConstant).normalized;
				aimDir = new Vector3(tempAimDir.x, 0, tempAimDir.z);

			}


		}
	}

	private void OnDestroy()
	{
        aimAction.action.performed -= AimInput;
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
        if(Application.isPlaying)
        Gizmos.DrawRay(playerMovement.orientation.position, aimDir * 5);
	}
}
