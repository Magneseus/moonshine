using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure : Station
{
    protected override void OnInteract(Player interactor = null)
    {
        Tend();
    }

    protected override void Tend()
    {
        base.Tend();

        // relieving pressure
        float currentPercentage = timeElapsed / animationLength;
        float percentRelieved = Time.deltaTime / timeToRelieve;
        currentPercentage -= percentRelieved;

        timeElapsed = Math.Max(animationLength * currentPercentage, 0);
    }

    protected override void BuildPressure()
    {
        if (interactors.Count == 0)
            base.BuildPressure();
    }
}
