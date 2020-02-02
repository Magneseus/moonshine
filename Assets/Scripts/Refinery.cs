using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEditor.UIElements;
using UnityEngine;

public class Refinery : Interactable
{
    public List<string> ingredients;
    public float timeToProcess = 5;

    public float processTime;
    public bool isProcessing = false;
    public bool processingDone = false;

    public Pickupable wineObject;
    public Pickupable beerObject;
    public Pickupable vodkaObject;

    public Pickupable producedObject;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new List<string>();
        producedObject = null;
    }

    void Update()
    {
        if (isProcessing)
        {
            processTime += Time.deltaTime;
            checkProcessing();
        }
    }

    protected override void OnInteract(Player interactor = null)
    {
        if (interactor != null)
        {

            // refinery that's not making anything
            if (!isProcessing && !processingDone)
            {
                GameObject heldObject = interactor.GetHeldObject();

                if (heldObject != null)
                {
                    Pickupable p = heldObject.GetComponent<Pickupable>();
                    if (p != null && "Grapes".Equals(p.itemName) || "Potato".Equals(p.itemName) || "Wheat".Equals(p.itemName))
                    {
                        if (ingredients.Count == 0 || ingredients[0].Equals(p.itemName))
                        {
                            ingredients.Add(p.itemName);
                            interactor.ConsumeHeldObject();
                            if (ingredients.Count >= 3)
                            {
                                Produce();
                            }
                        }
                    }
                }
            }
            else if (processingDone)
            {
                interactor.DropHeldObject();
                interactor.SetHeldObject(producedObject.gameObject, true);
                processingDone = false;
                producedObject = null;
            }
        }
    }

    protected void Produce()
    {
        processTime = 0;
        isProcessing = true;
        processingDone = false;
    }

    protected void checkProcessing()
    {
        if (processTime >= timeToProcess)
        {
            isProcessing = false;
            processingDone = true;

            if ("Wheat".Equals(ingredients[0]))
            {
                producedObject = beerObject;
            }
            else if ("Grapes".Equals(ingredients[0]))
            {
                producedObject = wineObject;
            }
            else if ("Potato".Equals(ingredients[0]))
            {
                producedObject = vodkaObject;
            }
            else
            {
                producedObject = null;
                processingDone = false;
            }

            ingredients.Clear();
        }
    }
}
