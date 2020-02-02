using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool oneShot = false;
    protected HashSet<Player> interactors = new HashSet<Player>();

    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out _) || !TryGetComponent<Collider>(out _))
        {
            if (GetComponentInChildren<Collider>() == null || GetComponentInChildren<Rigidbody>() == null)
            {
                Debug.LogError(string.Format("Interactable {0} missing a rigidbody or collider! Please fix!", this.name));
            }
        }
    }

    protected void Update()
    {
        if (!oneShot && this.interactors.Count > 0)
            OnInteract(null);
    }

    public void OnInteractStart(Player player)
    {
        Debug.Log("interacting " + this + " and " + player);
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
