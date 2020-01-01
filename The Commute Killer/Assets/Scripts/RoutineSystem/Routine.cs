using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Routine : MonoBehaviour
{
    private TimeManager TimeM;
    private RoutineManager RoutineM;

    //routine id
    private int Id { get; set; }

    //State, used to track the routine
    private int State { get; set; } = 0; // [ 0 - To Start | 1 - In Progress | 2 - Finished ]

    //agent
    private Agent Agent { get; set; }

    //Routine actions that make up this routine
    private List<RoutineAction> RoutineActions { get; set; } = new List<RoutineAction>();
    private int CurrentAction_i = 0;

    //time window
    private DateTime IniTime { get; set; }
    private DateTime EndTime { get; set; }

    public Routine(int id, Agent agent, List<RoutineAction> routineActions, DateTime iniTime, DateTime endTime)
    {
        this.Id             = id;
        this.Agent          = agent;
        this.RoutineActions = routineActions;
        this.IniTime        = iniTime;
        this.EndTime        = endTime;

        foreach (RoutineAction action in this.RoutineActions)
        {
            action.SetAgent(this.Agent);
        }
    }

    #region === Unity Events ===

    // Start is called before the first frame update
    void Start()
    {
        TimeM = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        RoutineM = GameObject.Find("RoutineManager").GetComponent<RoutineManager>();
    }


    #endregion

    #region === Getters and setter ===

    public int GetId()
    {
        return Id;
    }

    public int GetCurrentTime()
    {
        return Id;
    }

    #endregion

    #region === Routine Methods ===

    //gives the next action that should be performed
    protected RoutineAction ActionUpdate()
    {
        var time = this.TimeM.GetCurrentTime();

        var currentAction = this.RoutineActions[CurrentAction_i];

        if(currentAction.WillEnd(time))
        {
            for(var i = CurrentAction_i+1; i < this.RoutineActions.Count; i++)
            {
                var action = this.RoutineActions[i];

                if (action.CanStart(time))
                {
                    this.CurrentAction_i = i;
                    currentAction.Executed = true;
                    return action;
                }
            }
        }
        return null;
    }

    //get current action, if no new action to perform, return is null
    public Action.IDs? GetCurrentAction()
    {
        switch (this.State) {

            case 0: //To Start

                if(this.TimeM.GetCurrentTime() >= IniTime)
                {
                   this.State = 1;
                }
                break;

            case 1: //In progress

                if (this.TimeM.GetCurrentTime() >= EndTime)
                {
                    this.State = 2;
                    break;
                }

                var newAction = ActionUpdate();
                if (newAction != null) {
                    return newAction.GetAction();
                }
                break;

            case 2: //Finished
            default:
                break;
        }
        return null;
    }

    #endregion
}
