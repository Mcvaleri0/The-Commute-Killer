using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Equipable : PickUpAble
{
    public bool IsEquiped { get; set; }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.IsEquiped)
            {
                this.Unequip();
            }
            else
            {
                this.Equipe();
            }
        }
    }


    public void Equipe()
    {
        this.transform.localPosition = new Vector3(0, 0, 0.5f);

        this.BeingPicked = false;
        this.IsEquiped = true;
    }


    public void Unequip()
    {
        this.IsEquiped = false;
        this.BeingPicked = true;
        this.Release();
    }

}
