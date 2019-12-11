using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadaver : Item
{
    new public void Start()
    {
        base.Start();

        this.Name = "Cadaver";

        this.EnabledActions = new List<Action.IDs>()
        {
            Action.IDs.Insert
        };
    }
}
