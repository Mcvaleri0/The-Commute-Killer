using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Equipable : PickUpAble
{
    public bool IsEquiped { get; set; }

    /// <summary>
    /// local position used when the object is equiped
    /// </summary>
    public Vector3 PosRelative { get; set; }

    /// <summary>
    /// rotation of the object while equiped
    /// </summary>
    public Quaternion NewRotation { get; set; }

    public virtual void Start()
    {
        this.PosRelative = new Vector3(1f, -0.5f, 0.25f);
        this.NewRotation = Quaternion.Euler(-90, 90, 90);
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.Equipe();
        }
        // Mouse Left Click
        else if (Input.GetMouseButtonDown(0))
        {
            this.Attack();
        }
    }


    public void Equipe()
    {
        if (this.IsEquiped)
        {
            // Unequip
            this.IsEquiped = false;
            this.BeingPicked = true;
            this.Release();
        }
        else
        {
            // Equip
            this.transform.localPosition = this.PosRelative;
            this.transform.localRotation = this.NewRotation;

            this.BeingPicked = false;
            this.IsEquiped = true;
        }
    }


    public abstract void Attack();

}
