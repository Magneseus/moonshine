using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{

    public string itemName = "";

    private void Start()
    {
        this.oneShot = true;
    }

    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null)
        {
           
            interactor.SetHeldObject(this.gameObject, false);
        }
    }
}
