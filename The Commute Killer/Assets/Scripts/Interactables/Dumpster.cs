﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Dumpster : Interactable
{
    private int State = 0; // [ 0 - Closed | 1 - Open ]

    public EventManager EventManager;

    private GameObject[] Doors;

    #region === MonoBehaviour Methods ===
    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };

        this.Doors = new GameObject[2];

        this.Doors[0] = transform.Find("door 1").gameObject;
        this.Doors[1] = transform.Find("door 2").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.State)
        {
            case 0: // Closed

                break;

            case 1: // Open
                
                break;
        }
    }
    #endregion

    #region === Interactable Methods
    override public bool Interact(Action.IDs id)
    {
        switch (id)
        {
            default:
                break;

            case Action.IDs.Use:
                Use();
                return true;
        }

        return false;
    }

    override public bool CanInteract(Agent Interactor, Action.IDs id)
    {
        if (this.ActionAvailable(id))
        {
            return true;
        }

        return false;
    }
    #endregion

    #region === Possible Action Methods ===
    private void Use()
    {
        switch (this.State)
        {
            case 0: // Closed
                Open();
                break;

            case 1: // Open
                Close();
                break;
        }
    }
    #endregion

    #region === Object Behaviour ===
    private void Open()
    {
        this.State = 1; // Open the Dumpster

        foreach(GameObject d in Doors)
        {
            DisableDoor(d);
        }
    }

    private void Close()
    {
        this.State = 0; // Close the Dumpster

        foreach (GameObject d in Doors)
        {
            EnableDoor(d);
        }
    }

    private void EnableDoor(GameObject door)
    {
        door.GetComponent<MeshRenderer>().enabled = true;
        door.GetComponent<MeshCollider>().enabled = true;
    }

    private void DisableDoor(GameObject door)
    {
        door.GetComponent<MeshRenderer>().enabled = false;
        door.GetComponent<MeshCollider>().enabled = false;
    }
    #endregion
}
