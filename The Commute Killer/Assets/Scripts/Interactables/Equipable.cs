using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Equipable : PickUpAble
{
    public bool IsEquiped { get; set; }

    /// <summary>
    /// local position used when the object is equiped
    /// </summary>
    public Vector3 PosRelative { get; set; }


    public void Start()
    {
        this.PosRelative = new Vector3(1f, -0.5f, 0.25f);
    }

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
        this.transform.localPosition = this.PosRelative;
        this.transform.localRotation = Quaternion.Euler(-90, 90, 90);

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
