using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithPlayer : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        if(transform.parent.TryGetComponent(out IInteractable interactable))     
        {
            interactable.Interact();
        }
    }


   
}
