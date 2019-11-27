using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Knife : Equipable
{

    /// <summary>
    /// Number of frames that the attack takes
    /// </summary>
    public int nAttackFrames;

    public int CurrentAttackFrames { get; set; }

    public bool Attacking { get; set; }


    public override void Start()
    {
        base.Start();

        this.CurrentAttackFrames = this.nAttackFrames;
    }


    public override void Update()
    {
        if (this.Attacking)
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
                this.Attacking = false;
                this.CurrentAttackFrames = this.nAttackFrames;
            }
        }
        else
        {
            base.Update();
        }
    }


    public override void Attack()
    {
        if (this.IsEquiped)
        {
            this.Attacking = true;
        }
    }

}
