using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoutineManager : MonoBehaviour
{

    private List<Routine> Routines = new List<Routine>();

    public void addRoutine(Routine routine) {
        this.Routines.Add(routine);
    }

    public Routine getRoutine(int id) {
        return this.Routines.Find(x => x.GetId() == id);
    }

   
    void Start()
    {
        // Routine1  FIXME
        Agent agent1 = new Agent();
        List<RoutineAction> routineActions = new List<RoutineAction>() {
            new RoutineAction(Action.IDs.PickUp, new DateTime(1989, 9, 6, 8, 30, 0), new DateTime(1989, 9, 6, 8, 30, 0)),
            new RoutineAction(Action.IDs.PickUp, new DateTime(1989, 9, 6, 8, 30, 0), new DateTime(1989, 9, 6, 8, 30, 0))
        };
        DateTime iniTime = new DateTime(1989, 9, 6, 8, 30, 0);
        DateTime endTime = new DateTime(1989, 9, 6, 9, 00, 0);
        var routine1 = new Routine(0, agent1, routineActions, iniTime, endTime);
    }
}
