﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum ItemType
    {
        SharpWeapon,
        BluntWeapon,
        Tool,
        Food
    }

    #region /* Chain of Ownership */
    public GameObject OriginalOwner { get; protected set; }

    public GameObject Owner { get; protected set; } = null;
    #endregion

    #region /* Scene */
    private GameObject OriginalParent { get; set; }

    public Vector3 PosRelative { get; set; }

    public Quaternion NewRotation { get; set; }
    #endregion

    #region /* Item Attributes */
    public float Durability { get; protected set; }

    public List<ItemType> Types { get; protected set; }

    public string Name { get; protected set; }

    public List<Action.IDs> EnabledActions { get; protected set; }

    public Action.IDs DefaultAction { get; protected set; } = Action.IDs.None;
    #endregion

    public AudioSource AudioSource;
    public Dictionary<Action.IDs, AudioClip> ActionSounds;

    public int AnimationState = 0; // [ 0 - To Start | 1 - On Going | 2 - Finished ]

    new public void Start()
    {
        base.Start();

        this.PosRelative = new Vector3(0.9f, -0.4f, 0.5f);
        this.NewRotation = Quaternion.Euler(-90, 90, 90);

        if(this.transform.parent != null) this.OriginalParent = this.transform.parent.gameObject;

        this.Types = new List<ItemType>();

        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;
        this.ActionSounds = new Dictionary<Action.IDs, AudioClip>();
    }

    public void Update()
    {

        var ps = SelectableParticles.GetComponent<ParticleSystem>();

        var emiss = ps.emission;
        if (Owner != null)
        {
            emiss.enabled = false;
        }
        else {
            emiss.enabled = true;
        }

        this.transform.localPosition = Vector3.zero;
    }

    //Add a way to tell it to animate and detect when it is done
    public virtual void Animate() { }

    #region === Interactable Methods ===
    public bool Interact(Action.IDs id, Agent interactor)
    {
        switch(id)
        {
            default:
                break;

            case Action.IDs.PickUp:
                this.PickUp(interactor);
                return true;

            case Action.IDs.Drop:
                this.Drop();
                return true;
        }

        return false;
    }

    public bool CanInteract(Action.IDs id)
    {
        return true;
    }
    #endregion

    #region === Pick Up Methods ===
    public bool PickUp(Agent interactor)
    {
        this.GetComponent<Collider>().enabled = false;

        if(GetComponent<Rigidbody>() != null)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
        }

        if (GetComponent<CharacterController>() != null)
        {
            Destroy(this.GetComponent<CharacterController>());
        }

        if(this.GetComponent<Renderer>() != null)
        {
            this.GetComponent<Renderer>().enabled = false;
        }
        
        this.Owner = interactor.gameObject;
        this.transform.parent = interactor.HandPivot.transform;

        return true;
    }

    public void Drop()
    {
        this.transform.position = this.Owner.transform.position;

        this.Owner = null;

        if(this.OriginalParent != null)
        {
            this.transform.parent = this.OriginalParent.transform;
        }
        else
        {
            this.transform.parent = null;
        }

        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Collider>().enabled     = true;
        this.GetComponent<Renderer>().enabled     = true;
    }
    #endregion

    #region === Equiping Methods ===
    public void Equip()
    {
        if(this.GetComponent<Renderer>() != null)
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        
        this.transform.localPosition = Vector3.zero;

        var rotation = new Quaternion();

        this.transform.localRotation = rotation;
    }

    public void Unequip()
    {
        this.GetComponent<Renderer>().enabled = false;
    }
    #endregion

    public void PlayActionSound(Action.IDs action)
    {
        if (this.ActionSounds != null)
        {
            var sound = this.ActionSounds[action];
            if (sound != null)
            {
                this.AudioSource.PlayOneShot(sound);
            }
        }
    }
}
