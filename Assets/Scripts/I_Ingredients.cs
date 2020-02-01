using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Ingredients : Interactable
{
    public GameObject ingredientObject;

    private void Start()
    {
        this.oneShot = true;
    }

    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null)
        {
            interactor.SetHeldObject(ingredientObject);
        }
    }
}
