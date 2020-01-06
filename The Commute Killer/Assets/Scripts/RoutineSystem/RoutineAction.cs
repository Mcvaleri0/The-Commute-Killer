using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineAction : MonoBehaviour
{
    public bool Executed = false; 

    private Action.IDs Action;
    private Agent      Agent;

    //time window
    private DateTime IniTime;
    private DateTime EndTime;

    //dependencies to initiate routine action
    private Vector3             IniTargetPosition;
    private GameObject          IniTargetObject;
    private float               IniMinDistance;
    private List<RoutineAction> IniActionDependencies;

    //dependencies to end routine action
    private Vector3         EndTargetPosition;
    private GameObject      EndTargetObject;
    private float           EndMinDistance;


    public RoutineAction(Action.IDs action, DateTime iniTime, DateTime endTime)
    {
        this.Action  = action;
        this.IniTime = iniTime;
        this.EndTime = endTime;
    }


    public Action.IDs GetAction()
    {
        return this.Action;
    }

    public void SetAgent(Agent agent)
    {
        this.Agent = agent;
    }

    //check if routine action can start
    public bool CanStart(DateTime currentTime)
    {
        //check that it hasn't already been executed
        if (this.Executed)
        {
            return false;
        }
        //check time
        if (currentTime < this.IniTime)
        {
            return false;
        }
        //check distance to target position
        if (this.IniTargetPosition != null)
        {
            var agentPos = this.Agent.GetPosition();
            if (Vector3.Distance(agentPos, this.IniTargetPosition) > this.IniMinDistance)
            {
                return false;
            }
        }
        //check distance to target object
        if (this.IniTargetPosition != null)
        {
            var agentPos = this.Agent.GetPosition();
            if (Vector3.Distance(agentPos, IniTargetObject.transform.position) > this.IniMinDistance)
            {
                return false;
            }
        }
        //check if actions in dependency list were completed
        foreach (RoutineAction act in IniActionDependencies)
        {
            if(act.Executed != true)
            {
                return false;
            }
        } 

        return true;
    }


    public bool WillEnd(DateTime currentTime)
    {
        //check time
        if (currentTime > EndTime)
        {
            return true;
        }
        //check distance to target position
        if (EndTargetPosition != null)
        {
            var agentPos = this.Agent.GetPosition();
            if (Vector3.Distance(agentPos, EndTargetPosition) <= this.EndMinDistance)
            {
                return true;
            }
        }
        //check distance to target object
        if (EndTargetObject != null)
        {
            var agentPos = this.Agent.GetPosition();
            if (Vector3.Distance(agentPos, EndTargetObject.transform.position) <= this.EndMinDistance)
            {
                return true;
            }
        }

        return true;
    }

}
