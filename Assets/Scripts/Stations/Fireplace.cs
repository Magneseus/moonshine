using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Station
{
    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null && interactor.GetHeldObject() != null)
        {
            GameObject heldItem = interactor.GetHeldObject();
            interactor.ConsumeHeldObject();
            Debug.Log("ou burned");
        }
    }
}
