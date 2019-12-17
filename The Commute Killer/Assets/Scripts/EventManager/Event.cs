using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    //Hydrant_0_ON, Hydrant_0_OFF,
    Hydrant_ON, Hydrant_OFF,
    Hydrant_1_ON, Hydrant_1_OFF,
    VictimStartEnd, VictimEndStart, VictimAtGoal, VictimAtDumpster,
    InteractibleGardenGate_Close, InteractibleGardenGate_Open,
    InteractibleGardenGate_1_Close, InteractibleGardenGate_1_Open,
    Killed
}