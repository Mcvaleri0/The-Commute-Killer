using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    Hydrant_ON, Hydrant_OFF,
    Hydrant_1_ON, Hydrant_1_OFF,
    VictimStartEnd, VictimEndStart, VictimAtGoal, VictimAtDumpster,
    InteractibleGardenGate_Close, InteractibleGardenGate_Open,
    InteractibleGardenGate_1_Close, InteractibleGardenGate_1_Open,
    Killed, TrainArrival, TrainDeparture
}