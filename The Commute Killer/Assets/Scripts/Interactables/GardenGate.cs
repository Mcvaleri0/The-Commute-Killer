using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenGate : MonoBehaviour
{
    private int State = 0; // [ 0 - Open | 1 - Moving | 2 - Closed ]
    private int NextState = -1;

    private Quaternion Door1TargetAngle;
    private Quaternion Door2TargetAngle;

    private Transform Door1;
    private Transform Door2;

    public EventManager EventManager;

    public float smooth = 5.0f;

    #region === MonoBehaviour Methods ===
    void Start()
    {
        Door1 = this.transform.GetChild(0).transform;
        Door2 = this.transform.GetChild(1).transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case 0: // Open

                break;

            case 1: //Moving (update transform to meet the target rot)

                Door1.rotation = Quaternion.Slerp(Door1.rotation, Door1TargetAngle, Time.deltaTime * smooth);
                Door2.rotation = Quaternion.Slerp(Door2.rotation, Door2TargetAngle, Time.deltaTime * smooth);

                if (Door1.rotation == Door1TargetAngle)
                {
                    State = NextState;
                }

                break;

            case 2: // Closed

                break;

        }
    }
    #endregion
    

    #region === Object Behaviour ===

    public void Trigger()
    {
        //Debug.Log("TRIGGERED");

        if (State == 0)
        {
            Close();
        }
        else if (State == 2)
        {
            Open();
        }
    }


    private void Open()
    {
        State = 1;

        Door1TargetAngle = Door1.rotation * Quaternion.Euler(0, -90, 0);
        Door2TargetAngle = Door2.rotation * Quaternion.Euler(0, 90, 0);  

        NextState = 0;

        var evName = gameObject.name + "_Open";
        evName = evName.Replace(" ", "_");
        evName = evName.Replace("(", string.Empty);
        evName = evName.Replace(")", string.Empty);
        var ev = (Event)Enum.Parse(typeof(Event), evName);
        this.EventManager.TriggerEvent(ev);
    }

    private void Close()
    {
        State = 1;

        Door1TargetAngle = Door1.rotation * Quaternion.Euler(0, 90, 0);
        Door2TargetAngle = Door2.rotation * Quaternion.Euler(0, -90, 0);  

        NextState = 2;

        var evName = gameObject.name + "_Close";
        evName = evName.Replace(" ", "_");
        evName = evName.Replace("(", string.Empty);
        evName = evName.Replace(")", string.Empty);
        var ev = (Event)Enum.Parse(typeof(Event), evName);
        this.EventManager.TriggerEvent(ev);
    }
    #endregion


}
