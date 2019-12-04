using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorKnob : MonoBehaviour, Interactable
{
    public GameObject Interactor;
    public GameObject targetDoor;

    public float maxDistance = 3;

    public bool Interact(Agent Interactor)
    {
        if (CanInteract(Interactor))
        {

            //targetPos = targetDoor.transform.position;

            //Debug.Log(newRotation.ToString("F4"));

            Vector3 doorPos = targetDoor.transform.position;

            float angle = targetDoor.transform.eulerAngles.y * Mathf.Deg2Rad;
            int distance = 2;

            float xx = Mathf.Cos(angle);
            float zz = Mathf.Sin(angle);

            Vector3 dirVec = new Vector3(xx, 00, zz);

            Vector3 targetPos = doorPos + (dirVec.normalized * distance);
            targetPos.y += 1;

            Interactor.transform.position = targetPos;
        }

        return false;
    }

    public bool CanInteract(Agent Interactor)
    {
        if (Vector3.Distance(this.transform.position, Interactor.transform.position) < maxDistance)
        {
            return true;
        }

        return false;
    }
}
