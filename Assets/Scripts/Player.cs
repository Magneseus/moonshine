using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Player : MonoBehaviour
{
    // Public Vars
    public int playerId = 0;
    public float moveSpeed = 0.5f;

    // Private Vars
    private Rewired.Player rewiredPlayer;
    private bool input_interacting = false;
    private float input_horizontal, input_vertical, input_look_horizontal, input_look_vertical;
    private HashSet<Interactable> interactables = new HashSet<Interactable>();

    // Components
    public GameObject holdLocation;
    private GameObject heldObject;
    private CharacterController controller;

    //Fetch the Animator
    Animator m_Animator;
    public Animation anim;
    public Transform joint;

    private void Awake()
    {
        rewiredPlayer = ReInput.players.GetPlayer(playerId);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (holdLocation == null)
        {
            Debug.LogError("No hold location for player! Please fix!");
        }

        m_Animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        UpdateInputs();

        // Movement
        Vector3 movement = new Vector3(input_horizontal, 0.0f, input_vertical);
        movement *= moveSpeed;
        controller.Move(movement);

        // Setting anim params
        m_Animator.SetFloat("Speed", Math.Abs(movement.x) + Math.Abs(movement.z));

        // Looking direction
        Vector3 lookDir = new Vector3(input_look_horizontal, 0.0f, input_look_vertical);
        lookDir += controller.transform.position;
        controller.transform.LookAt(lookDir);

        // Apply gravity if falling
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0.0f, -0.5f, 0.0f));
        }

        if (rewiredPlayer.GetButton("Interact"))
        {

            if (interactables.Count > 0)
            {
                // Find the closest interactable and interact with it
                float minDist = float.MaxValue;
                float dist;
                Interactable closestInteractable = null;

                foreach (Interactable interactable in interactables)
                {
                    if (interactable.gameObject == heldObject)
                        continue;

                    dist = Vector3.Distance(this.transform.position, interactable.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestInteractable = interactable;
                    }
                }
                if (closestInteractable != null)
                {
                    closestInteractable.OnInteractStart(this);
                }
                //anim["Pickup"].AddMixingTransform(joint, true);
            }
        }
        else if (rewiredPlayer.GetButtonUp("Interact"))
        {
            // Dropping items
            RemoveAllInteractables();
        }

        if (rewiredPlayer.GetButtonUp("Drop"))
        {
            DropHeldObject();
        }
    }

    private void UpdateInputs()
    {
        //input_horizontal = Input.GetAxis("Horizontal");
        //input_vertical = Input.GetAxis("Vertical");
        input_horizontal = rewiredPlayer.GetAxis("MHorizontal");
        input_vertical = rewiredPlayer.GetAxis("MVertical");

        input_look_horizontal = rewiredPlayer.GetAxis("LHorizontal");
        input_look_vertical = rewiredPlayer.GetAxis("LVertical");

        //bool newInteract = Input.GetAxis("Submit") != 0.0f;
        input_interacting = rewiredPlayer.GetButton("Interact");
    }

    public bool SetHeldObject(GameObject heldObject, bool create=true)
    {

        if (this.heldObject)
            return false;

        if (create)
        {
            this.heldObject = Instantiate(heldObject, holdLocation.transform);
        }
        else
        {
            this.heldObject = heldObject;
            heldObject.transform.parent = holdLocation.transform;
            heldObject.transform.position = holdLocation.transform.position;
            heldObject.transform.localRotation = Quaternion.identity;
        }
        this.heldObject.GetComponent<Rigidbody>().isKinematic = true;
        m_Animator.SetTrigger("IsPickingUp");

        return true;
    }

    public GameObject GetHeldObject()
    {
        return this.heldObject;
    }

    public void DropHeldObject()
    {
        if (this.heldObject != null)
        {
            this.heldObject.transform.parent = null;
            this.heldObject.GetComponent<Rigidbody>().isKinematic = false;
            this.heldObject = null;
        }
    }

    public void ConsumeHeldObject()
    {
        if (this.heldObject != null)
        {
            m_Animator.SetTrigger("IsPickingUp");

            OnTriggerExit(this.heldObject.GetComponent<Collider>());
            Destroy(this.heldObject);
            this.heldObject = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Called when the character hits another object with a collider
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForce(hit.moveDirection * hit.moveLength);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable;
        if (other.gameObject.TryGetComponent<Interactable>(out interactable))
        {
            // If there is a valid interactable, add it to the list
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable;
        if (other.gameObject.TryGetComponent<Interactable>(out interactable))
        {
            // If there is a valid interactable, stop interaction
            interactables.Remove(interactable);

            if (IsInteracting())
            {
                interactable.OnInteractExit(this);
            }
        }
    }

    public bool IsInteracting()
    {
        return input_interacting;
    }

    private void RemoveAllInteractables()
    {
        foreach (Interactable item in interactables)
        {
            item.OnInteractExit(this);
        }
    }
}
