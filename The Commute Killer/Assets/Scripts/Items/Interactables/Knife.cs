using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Knife : Item
{

    /// <summary>
    /// Number of frames that the attack takes
    /// </summary>
    public int nAttackFrames;

    public int CurrentAttackFrames { get; set; }

    public bool Attacking { get; set; }


    new public void Start()
    {
        base.Start();

        this.CurrentAttackFrames = this.nAttackFrames;
    }


    public void Update()
    {
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
                this.AnimationState = 2;
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
