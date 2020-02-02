using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class Refinery : Interactable
{
    public List<Pickupable> ingredients;

    private float processTime;


    // Start is called before the first frame update
    void Start()
    {
        ingredients = new List<Pickupable>();
    }

    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null)
        {
            GameObject heldObject = interactor.GetHeldObject();

            if (heldObject != null)
            {
                Pickupable p = heldObject.GetComponent<Pickupable>();
                if (p != null)
                {
                    ingredients.Add(p);
                    interactor.ConsumeHeldObject();
                    if (ingredients.Count >= 3)
                    {
                        Produce();
                    }
                }
            }
        }
    }

    protected void Produce()
    {
        processTime = 0;


    }
}
