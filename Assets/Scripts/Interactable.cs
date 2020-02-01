using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool oneShot = false;
    protected List<Player> interactors = new List<Player>();

    protected void Update()
    {
        if (!oneShot && this.interactors.Capacity > 0)
            OnInteract(null);
    }

    public void OnInteractStart(Player player)
    {
        if (!oneShot)
            interactors.Add(player);
        else
            OnInteract(player);
    }

    public void OnInteractExit(Player player)
    {
        if (!oneShot)
            interactors.Remove(player);
    }

    protected abstract void OnInteract(Player interactor = null);
}
