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

	public Optional<Image> cursor;

	float angleConstant;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        aimAction.action.performed += AimInput;
		angleConstant = Camera.main.transform.rotation.eulerAngles.x;
		angleConstant = Mathf.Tan(angleConstant * Mathf.Deg2Rad);
		angleConstant = 1 / angleConstant;
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
		Vector2 readVal = context.ReadValue<Vector2>();
		if (context.action.activeControl.parent.device == Mouse.current.device)
		{
			if (cursor.Enabled)
			{
				cursor.Value.transform.position = readVal;
				
			}
			RaycastHit hit;
			Vector3 hitPoint;
			float y;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(readVal), out hit, float.PositiveInfinity, validRayLayers))
			{
				hitPoint = hit.point;
				hitPoint.y = 0;
				y = hit.point.y;
				Vector3 tempAimDir = (hitPoint - transform.position - playerMovement.orientation.forward * ((transform.position.y + playerMovement.orientation.localPosition.y) - y) * angleConstant).normalized;
				aimDir = new Vector3(tempAimDir.x,0, tempAimDir.z);

			}
			
			
		}
		else
		{
			Vector3 tempAimDir = playerMovement.orientation.forward * readVal.y + playerMovement.orientation.right * readVal.x;
			aimDir = new Vector3(tempAimDir.x, 0, tempAimDir.z);
		}
		playerMovement.body.transform.forward = aimDir;
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
