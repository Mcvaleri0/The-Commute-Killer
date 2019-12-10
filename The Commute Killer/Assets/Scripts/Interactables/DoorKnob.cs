using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnob : Interactable
{
    public GameObject Interactor;

    public GameObject TargetDoor;

    public bool Locked = false;

    #region === MonoBehaviour Methods ===
    void Start()
    {

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };
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
        if (!this.Locked) {
            
             return true;
       
        }
        return false;
    }
    #endregion

    #region === Possible Action Methos ===
    private void Use()
    {
        Vector3 doorPos = this.TargetDoor.transform.position;

        float angle = (this.TargetDoor.transform.eulerAngles.y * Mathf.Deg2Rad);
        int distance = 2;

        float xx = Mathf.Cos(angle);
        float zz = Mathf.Sin(angle);

        Vector3 dirVec = new Vector3(xx, 00, zz);

        Vector3 targetPos = doorPos + (dirVec.normalized * distance);
        targetPos.y += 2;

        this.Interactor.GetComponent<Player>().Teleport(targetPos, this.TargetDoor.transform.eulerAngles);
    }
    #endregion
}
