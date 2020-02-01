using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Station : Interactable
{
    public AnimationClip clip;
    public float timeToRelieve = 3.0f; // how long it takes to relieve pressure from full
    public float speedMultiplier = 1.0f;
    public float current;

    private float timeElapsed = 0;
    private AnimationCurve curve;
    private float animationLength;

    public void Start()
    {
        curve = AnimationUtility.GetEditorCurve(clip, AnimationUtility.GetCurveBindings(clip)[0]);
        timeElapsed = 0;

        animationLength = curve.keys[curve.length - 1].time;
    }

    public void FixedUpdate()
    {
        buildPressure();

        current = curve.Evaluate(timeElapsed);
        if (current >= 100)
        {
            explode();
        }
    }

    protected virtual void buildPressure()
    {
       timeElapsed += Time.deltaTime * speedMultiplier;
    }

    protected virtual void tend()
    {
        // relieving pressure
        Debug.Log("relieving pressure");
        float currentPercentage = timeElapsed / animationLength;
        float percentRelieved = Time.deltaTime / timeToRelieve;
        currentPercentage -= percentRelieved;

        timeElapsed = animationLength * currentPercentage;
    }

    protected virtual void explode()
    {
        //Debug.Log("Boom.");
    }
}
