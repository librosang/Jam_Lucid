using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    
    public IInteractable GetInteractable()
    {
        List<IInteractable> interactables = new List<IInteractable>();

        float interactRange = 2.5f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }
        
        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }
        
        return closestInteractable;
    }
    
}
