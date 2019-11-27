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
    /// parent of this object while this object is not being picked
    /// </summary>
    public Transform OldParent { get; set; }


    #region === Unity Events ===
    
    private void OnMouseDown()
    {
        this.Interact(this.Interactor);
    }


    private void OnMouseUp()
    {
        this.transform.parent = this.OldParent;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
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
    public bool CanInteract(GameObject interactor)
    {
        return (Vector3.Distance(this.transform.position, interactor.transform.position) < 2);
    }

    #endregion


    #region === Pick Up Methods ===

    private void PickUp(GameObject interactor)
    {
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().useGravity = false;

        this.transform.position = interactor.transform.position;

        this.OldParent = this.transform.parent;
        this.transform.parent = interactor.transform;
    }

    #endregion
}
