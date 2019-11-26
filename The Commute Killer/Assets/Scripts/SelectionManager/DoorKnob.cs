using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorKnob : MonoBehaviour, Interactable
{
    public GameObject Interactor;
    public GameObject targetDoor;

    void OnMouseDown()
    {
        this.Interact(this.Interactor);
    }


    public bool Interact(GameObject Interactor)
    {
        if (CanInteract(Interactor))
        {
            Vector3 targetPos = new Vector3(0, 0, 0);
            targetPos = targetDoor.transform.position;
            targetPos.y += 2;

            //var heading = Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;

            Interactor.transform.position = targetPos;
        }

        return false;
    }

    public bool CanInteract(GameObject Interactor)
    {
        if (Vector3.Distance(this.transform.position, Interactor.transform.position) < 30)
        {
            return true;
        }

        return false;
    }
}
