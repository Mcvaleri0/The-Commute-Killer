using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    public enum ItemType
    {
        SharpWeapon,
        BluntWeapon,
        Tool,
        Food
    }

    private GameObject OriginalOwner { get; set; }

    public GameObject Owner { get; set; }

    public Vector3 PosRelative { get; set; }

    public Quaternion NewRotation { get; set; }

    public float Durability { get; set; }

    public List<ItemType> Types { get; set; }

    public string Name { get; set; }

    public int AnimationState = 0; // [ 0 - To Start | 1 - On Going | 2 - Finished ]

    public void Start()
    {
        this.PosRelative = new Vector3(1f, -0.5f, 0.25f);
        this.NewRotation = Quaternion.Euler(-90, 90, 90);

        this.Owner = this.transform.parent.gameObject;
        this.OriginalOwner = this.Owner;
    }

    //Add a way to tell it to animate and detect when it is done
    public virtual void Animate() { }

    #region === Interactable Methods ===

    /// <summary>
    /// Interactations with this kind of objects are only picking it up
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(Agent interactor)
    {
        if (this.CanInteract(interactor))
        {
            this.PickUp(interactor);
            return true;
        }

        return false;
    }


    /// <summary>
    /// It is only possible to interact if the interactor is close
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool CanInteract(Agent interactor)
    {
        var dist = Vector3.Distance(this.transform.position, interactor.transform.position);
        
        return (dist < 2);
    }

    #endregion

    #region === Pick Up Methods ===
    public bool PickUp(Agent interactor)
    {
        if(interactor.Inventory.Count < interactor.Inventory.Capacity)
        {
            this.GetComponent<Collider>().enabled     = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Renderer>().enabled     = false;

            this.Owner = interactor.gameObject;
            this.transform.parent = interactor.transform;

            return true;
        }

        return false;
    }


    public void Drop()
    {
        this.transform.position = this.Owner.transform.position;

        this.Owner = this.OriginalOwner;

        this.transform.parent = this.Owner.transform;

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
