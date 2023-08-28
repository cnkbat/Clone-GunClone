using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.instance.KnockbackPlayer();
        GetComponent<BoxCollider>().enabled = false;
    }
}
