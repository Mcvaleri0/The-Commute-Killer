using UnityEngine;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
    public List<Action.IDs> PossibleActions { get; protected set; }

    public void Start()
    {
        this.tag = "Selectable";
    }

    public virtual bool Interact(Action.IDs id) { return false; }

    public virtual bool CanInteract(Agent interactor, Action.IDs id) { return false; }

    protected bool ActionAvailable(Action.IDs id)
    {
        if(this.PossibleActions.FindIndex(x => x == id) != -1)
        {
            return true;
        }

        return false;
    }
}
