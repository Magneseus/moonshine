using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public Vars
    public float moveSpeed = 0.5f;

    // Private Vars
    private bool input_interacting = false;
    private float input_horizontal = 0.0f;
    private float input_vertical = 0.0f;
    private List<Interactable> interactables = new List<Interactable>();

    // Components
    public GameObject holdLocation;
    private GameObject heldObject;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (holdLocation == null)
        {
            Debug.LogError("No hold location for player! Please fix!");
        }
    }

    void Update()
    {
        UpdateInputs();

        // Movement
        Vector3 movement = new Vector3(input_horizontal, 0.0f, input_vertical);
        movement *= moveSpeed;
        controller.Move(movement);

        // Apply gravity if falling
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0.0f, -0.5f, 0.0f));
        }
    }

    public bool SetHeldObject(GameObject heldObject)
    {
        if (this.heldObject)
            return false;

        this.heldObject = Instantiate(heldObject, holdLocation.transform);
        
        return true;
    }

    public void DropHeldObject()
    {
        if (this.heldObject != null)
        {
            this.heldObject.transform.parent = null;
            this.heldObject = null;
        }
    }

    private void UpdateInputs()
    {
        input_horizontal = Input.GetAxis("Horizontal");
        input_vertical = Input.GetAxis("Vertical");

        bool newInteract = Input.GetAxis("Submit") != 0.0f;
        if (!newInteract && input_interacting)
        {
            RemoveAllInteractables();
            if (heldObject)
            {
                DropHeldObject();
            }
        }
        input_interacting = newInteract;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Called when the character hits another object with a collider
        if (hit.rigidbody)
            hit.rigidbody.AddForce(hit.moveDirection * hit.moveLength);
    }

    private void OnTriggerStay(Collider other)
    {
        Interactable interactable;
        if (IsInteracting() && other.gameObject.TryGetComponent<Interactable>(out interactable))
        {
            // If there is a valid interactable, interact with it
            AddInteractable(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable;
        if (IsInteracting() && other.gameObject.TryGetComponent<Interactable>(out interactable))
        {
            // If there is a valid interactable, stop interaction
            RemoveInteractable(interactable);
        }
    }

    public bool IsInteracting()
    {
        return input_interacting;
    }

    private void AddInteractable(Interactable interactable)
    {
        interactable.OnInteractStart(this);
        interactables.Add(interactable);
    }

    private void RemoveInteractable(Interactable interactable)
    {
        interactable.OnInteractExit(this);
        interactables.Remove(interactable);
    }

    private void RemoveAllInteractables()
    {
        foreach (Interactable item in interactables)
        {
            item.OnInteractExit(this);
        }

        interactables.Clear();
    }
}
