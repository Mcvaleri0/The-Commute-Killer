﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Item
{
    public int nAttackFrames;

    public int CurrentAttackFrames { get; set; }


    new public void Start()
    {
        base.Start();

        this.Name = "Knife";

        this.CurrentAttackFrames = this.nAttackFrames;

        this.Types.Add(ItemType.SharpWeapon);

        this.EnabledActions = new List<Action.IDs>()
        {
        };

        this.DefaultAction = Action.IDs.Stab;

        this.ActionSounds.Add(Action.IDs.PickUp, (AudioClip)Resources.Load("Audio/knife_on"));
        this.ActionSounds.Add(Action.IDs.Drop,   (AudioClip)Resources.Load("Audio/knife_off"));
        this.ActionSounds.Add(Action.IDs.Stab,   (AudioClip)Resources.Load("Audio/knife_stab1"));
    }


    new public void Update()
    {
        base.Update();

        if (this.AnimationState == 1)
        {
            if (this.CurrentAttackFrames > (this.nAttackFrames / 2))
            {
                var pos = this.transform.localPosition;
                this.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z);
                this.CurrentAttackFrames--;
            }
            else if (this.CurrentAttackFrames > 0)
            {
                var pos = this.transform.localPosition;
                this.transform.localPosition = new Vector3(pos.x, pos.y - 5, pos.z);
                this.CurrentAttackFrames--;
            }
            else
            {
                this.AnimationState = 0;
                this.CurrentAttackFrames = this.nAttackFrames;
            }
        }
    }


    override public void Animate()
    {
        if(this.AnimationState == 0)
        {
            this.AnimationState = 1;
        }
    }

}
