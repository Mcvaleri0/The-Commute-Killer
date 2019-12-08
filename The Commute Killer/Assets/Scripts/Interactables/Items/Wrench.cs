using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : Item
{

    public int nAttackFrames;

    public int CurrentAttackFrames { get; set; }

    public bool Attacking { get; set; }

    new public void Start()
    {
        base.Start();

        this.Name = "Wrench";

        this.CurrentAttackFrames = this.nAttackFrames;

        this.Types.Add(ItemType.Tool);
        this.Types.Add(ItemType.BluntWeapon);

        this.EnabledActions = new List<Action.IDs>()
        {
            Action.IDs.Sabotage
        };
    }


    new public void Update()
    {
        base.Update();

        if (this.AnimationState == 1)
        {
            if (this.CurrentAttackFrames > (this.nAttackFrames / 2))
            {
                var pos = this.transform.localPosition;
                this.transform.localPosition = new Vector3(pos.x, pos.y, pos.z + 0.5f);
                this.CurrentAttackFrames--;
            }
            else if (this.CurrentAttackFrames > 0)
            {
                var pos = this.transform.localPosition;
                this.transform.localPosition = new Vector3(pos.x, pos.y, pos.z - 0.5f);
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
