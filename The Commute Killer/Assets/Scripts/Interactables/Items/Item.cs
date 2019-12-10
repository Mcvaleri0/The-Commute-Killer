using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum ItemType
    {
        SharpWeapon,
        BluntWeapon,
        Tool,
        Food
    }

    #region /* Chain of Ownership */
    public GameObject OriginalOwner { get; protected set; }

    public GameObject Owner { get; protected set; }
    #endregion

    #region /* Scene */
    private GameObject OriginalParent { get; set; }

    public Vector3 PosRelative { get; set; }

    public Quaternion NewRotation { get; set; }
    #endregion

    #region /* Item Attributes */
    public float Durability { get; protected set; }

    public List<ItemType> Types { get; protected set; }

    public string Name { get; protected set; }

    public List<Action.IDs> EnabledActions { get; protected set; }

    public Action.IDs DefaultAction { get; protected set; } = Action.IDs.None;
    #endregion

    public int AnimationState = 0; // [ 0 - To Start | 1 - On Going | 2 - Finished ]

    new public void Start()
    {
        base.Start();

        this.PosRelative = new Vector3(0.9f, -0.4f, 0.5f);
        this.NewRotation = Quaternion.Euler(-90, 90, 90);

        if(this.transform.parent != null) this.OriginalParent = this.transform.parent.gameObject;

        this.Types = new List<ItemType>();
    }

    public void Update()
    {
    }

    //Add a way to tell it to animate and detect when it is done
    public virtual void Animate() { }

    #region === Interactable Methods ===
    public bool Interact(Action.IDs id, Agent interactor)
    {
        switch(id)
        {
            default:
                break;

            case Action.IDs.PickUp:
                this.PickUp(interactor);
                return true;

            case Action.IDs.Drop:
                this.Drop();
                return true;
        }

        return false;
    }

    public bool CanInteract(Action.IDs id)
    {
        return true;
    }
    #endregion

    #region === Pick Up Methods ===
    public bool PickUp(Agent interactor)
    {
        this.GetComponent<Collider>().enabled     = false;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Renderer>().enabled     = false;

        this.Owner = interactor.gameObject;
        this.transform.parent = interactor.transform;

        return true;
    }

    public void Drop()
    {
        this.transform.position = this.Owner.transform.position;

        this.Owner = null;

        if(this.OriginalParent != null)
        {
            this.transform.parent = this.OriginalParent.transform;
        }
        else
        {
            this.transform.parent = null;
        }

        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Collider>().enabled     = true;
        this.GetComponent<Renderer>().enabled     = true;
    }
    #endregion

    #region === Equiping Methods ===
    public void Equip()
    {
        this.GetComponent<Renderer>().enabled = true;

        this.transform.localPosition = this.PosRelative;
        this.transform.localRotation = this.NewRotation;
    }

    public void Unequip()
    {
        this.GetComponent<Renderer>().enabled = false;
    }
    #endregion
}
