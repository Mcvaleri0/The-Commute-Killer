using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickUpAble : MonoBehaviour, Interactable
{
    /// <summary>
    /// Object the interacts with this
    /// </summary>
    public GameObject Interactor;

    /// <summary>
    /// position that the object has while is being picked up
    /// </summary>
    public GameObject newPos;

    /// <summary>
    /// parent of this object while this object is not being picked
    /// </summary>
    public Transform OldParent { get; set; }

    /// <summary>
    /// Is true if the object is being picked up
    /// </summary>
    public bool BeingPicked { get; set; }


    #region === Unity Events ===
    
    private void OnMouseDown()
    {
        this.Interact(this.Interactor);
    }


    private void OnMouseUp()
    {
        this.Release();
    }

    #endregion


    #region === Interactable Methods ===

    /// <summary>
    /// Interactations with this kind of objects are only picking it up
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool Interact(GameObject interactor)
    {
        if (this.CanInteract(interactor))
        {
            this.PickUp();
            return true;
        }

        return false;
    }


    /// <summary>
    /// It is only possible to interact if the interactor is close
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool CanInteract(GameObject interactor)
    {
        var dist = Vector3.Distance(this.transform.position, interactor.transform.position);
        Debug.Log(dist);
        return (dist < 5);
    }

    #endregion


    #region === Pick Up Methods ===

    public void PickUp()
    {
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().useGravity = false;

        this.transform.position = this.newPos.transform.position;

        this.OldParent = this.transform.parent;
        this.transform.parent = this.newPos.transform;
    
        this.BeingPicked = true;
    }

    
    public void Release()
    {
        if (this.BeingPicked)
        {
            this.transform.parent = this.OldParent;

            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Collider>().enabled = true;

            this.BeingPicked = false;
        }
    }

    #endregion
}
