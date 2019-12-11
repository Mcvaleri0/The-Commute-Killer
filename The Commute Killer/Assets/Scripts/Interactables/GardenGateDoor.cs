using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenGateDoor : Interactable
{

    //public EventManager EventManager;

    private GardenGate Gate;

    #region === MonoBehaviour Methods ===
    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };

        Gate = GetComponentInParent<GardenGate>();
    }

    // Update is called once per frame
    void Update()
    {

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
        Gate.Trigger();
    }
    #endregion
}
