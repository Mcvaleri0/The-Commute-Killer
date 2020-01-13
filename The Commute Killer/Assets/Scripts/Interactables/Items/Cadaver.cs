using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadaver : Interactable
{

    protected GameObject Bag;

    private AudioSource AudioSource;

    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };

        //make new colision box
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.center = new Vector3(-0.0447044f, 0.02904758f, 0.5598322f);
        boxCollider.size   = new Vector3( 0.5890946f, 0.3413701f, 0.9909107f);

        this.Bag = Resources.Load<GameObject>("Items/TrashBag");

    }


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

    public void Use()
    {
        //spawn trash bag item
        var bag = Instantiate(Bag);
        bag.transform.position = transform.position;
        bag.GetComponent<TrashBag>().SetContainsVictim(true);

        Destroy(gameObject);

    }
}
