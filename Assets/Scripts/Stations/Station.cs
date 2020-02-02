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

    protected float timeElapsed = 0;
    protected AnimationCurve curve;
    protected float animationLength;

    public void Start()
    {
        curve = AnimationUtility.GetEditorCurve(clip, AnimationUtility.GetCurveBindings(clip)[0]);
        timeElapsed = 0;

        animationLength = curve.keys[curve.length - 1].time;
    }

    public void FixedUpdate()
    {
        BuildPressure();

        current = curve.Evaluate(timeElapsed);
        if (current >= 100)
        {
            Explode();
            timeElapsed = animationLength;
        }
    }

    protected virtual void BuildPressure()
    {
       timeElapsed += Time.deltaTime * speedMultiplier;
    }

    protected virtual void Tend()
    {
        
    }

    protected virtual void Explode()
    {
        //Debug.Log("Boom.");
    }
}
