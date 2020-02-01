using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure : Station
{
    public bool playerInteracting = false;
    protected override void OnInteract(Player interactor = null)
    {
        tend();
    }

    protected override void buildPressure()
    {
        if (interactors.Count == 0)
            base.buildPressure();
    }
}
