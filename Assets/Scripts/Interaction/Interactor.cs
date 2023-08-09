using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask interactionLayer;
    PlayerAim playerAim;
    [SerializeField] float maxRange = 100;
    
    [SerializeField] float directionWeight = 1;
    [SerializeField] float distanceWeight = 1;
    [SerializeField] float priorityWeight = 1;
    [Range(-1, 1)]
    [SerializeField] float minDirectionValue = -1;
    [SerializeField] InputActionProperty input;

    Interactable interactable;
    public bool hasTarget;

	private void Start()
	{
        input.action.performed += Interact;
        playerAim = GetComponent<PlayerAim>();
	}
	private void OnEnable()
	{
		input.action.Enable();
	}
	private void OnDisable()
	{
		input.action.Disable();
	}

	private void Update()
	{
        Interactable tempInteractible;
		if (FindInteractionTarget(playerAim.aimDir, transform.position, out tempInteractible))
		{
            if(tempInteractible != interactable)
			{
                if (hasTarget)
                    interactable.ExitHover();
                interactable = tempInteractible;
                
                
			}
            interactable.OnHover();
            hasTarget = true;
		}
		else
		{
			if (hasTarget)
			{
                hasTarget = false;
                interactable.ExitHover();
            }
            
                
        }
        
	}

    void Interact(InputAction.CallbackContext context)
	{
        if(!hasTarget)
            return;
        interactable.Interact(this);
	}

	public bool FindInteractionTarget(Vector3 aimDir, Vector3 origin,out Interactable interactable)
    {
        

        Collider[] colliders = Physics.OverlapSphere(origin, maxRange, interactionLayer);
        if(colliders.Length == 0)
		{
            interactable = null;
            return false;
        }
            

        Interactable bestTarget = null;
        float bestVal = float.NegativeInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable target;
            if (colliders[i].TryGetComponent(out target))
            {


                Vector3 casterToTarget = target.transform.position - origin;
                casterToTarget.y = 0;

                Vector3 casterToTargetNorm = casterToTarget.normalized;
                float directionValue = Vector3.Dot(aimDir, casterToTargetNorm) * directionWeight;
                if (directionValue < minDirectionValue)
                    continue;
                float val = directionValue + (1 / casterToTarget.magnitude) * distanceWeight + target.priority * priorityWeight;
                if (val > bestVal)
                {
                    bestVal = val;
                    bestTarget = target;
                }

            }
        }
        interactable = bestTarget;
        return true;
    }
}
