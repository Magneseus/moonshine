using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Station
{
    public float percentagePerLog = 25;
    public float logStrengthVariance = 5;

    public float tendDelaySeconds = 2;

    private WaitForSeconds tendDelay;
    public void Start()
    {
        base.Start();
        tendDelay = new WaitForSeconds(tendDelaySeconds);
    }

    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null && interactor.GetHeldObject() != null)
        {
            GameObject heldItem = interactor.GetHeldObject();
            interactor.ConsumeHeldObject();
            Tend();
        }
    }

    protected override void Tend()
    {
        base.Tend();

        // fire is tended to a configurable time later
        StartCoroutine(DelayedTend());
    }

    protected void Overburn()
    {
        Debug.Log("Overburn!");
    }

    private IEnumerator DelayedTend()
    {
        yield return tendDelay;

        float currentPercentage = timeElapsed / animationLength;
        // random value between [percPerLog - variance, perPerLog + variance]
        float logEffect = (percentagePerLog + (UnityEngine.Random.value * logStrengthVariance * 2) - logStrengthVariance) / 100.0f;
        Debug.Log("Log saved " + logEffect);
        currentPercentage -= logEffect;
        timeElapsed = currentPercentage * animationLength;

        // check for overburn!
        if (timeElapsed < 0)
        {
            Overburn();
            timeElapsed = 0;
        }
    }
}
