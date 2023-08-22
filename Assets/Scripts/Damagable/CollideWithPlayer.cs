using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithPlayer : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("interact");
        if(transform.parent.TryGetComponent(out IInteractable interactable))     
        {
            Debug.Log("if");
            interactable.Interact();
        }
    }


   
}
