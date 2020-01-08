using System;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : Interactable
{
    private int State = 0; // [ 0 - Closed | 1 - Open | 2 - Empty ]

    private float StartTime;

    public float Duration = 2 * 60;

    public ParticleSystem WaterSpout;

    public EventManager EventManager;

    private AudioSource AudioSource;

    #region === MonoBehaviour Methods ===
    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Sabotage
        };

        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;
        this.AudioSource.loop = true;
        this.AudioSource.volume = 0.3f;
        this.AudioSource.spatialBlend = 1.0f;
        this.AudioSource.clip = (AudioClip)Resources.Load("Audio/hydrant_splash");
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case 0: // Closed

                break;

            case 1: // Open
                if(StartTime + Duration <= Time.time)
                {
                    this.State = 2; // Go to empty

                    this.WaterSpout.Stop(); // Stop particles

                    var ev = (Event)Enum.Parse(typeof(Event), gameObject.name + "_OFF");
                    this.EventManager.TriggerEvent(ev);
                }

                break;

            case 2: // Empty

                break;
        }
    }
    #endregion

    #region === Interactable Methods
    override public bool Interact(Action.IDs id)
    {
        switch (id)
        {
            default:
                break;

            case Action.IDs.Sabotage:
                Sabotage();
                return true;
        }

        return false;
    }

    override public bool CanInteract(Agent Interactor, Action.IDs id)
    {
        if (this.State != 2 && this.ActionAvailable(id))
        {
            return true;
        }

        return false;
    }
    #endregion

    #region === Possible Action Methods ===
    private void Sabotage()
    {
        Activate();
    }
    #endregion

    #region === Object Behaviour ===
    private void Activate()
    {
        this.State = 1; // Open the Hydrant

        this.WaterSpout.Play(); // Start particles

        this.StartTime = Time.time; // Start timer

        var ev = (Event)Enum.Parse(typeof(Event), gameObject.name + "_ON");
        this.EventManager.TriggerEvent(ev);

        this.AudioSource.Play();
    }

    private void Deactivate()
    {
        this.State = 0; // Close the Hydrant

        this.WaterSpout.Stop(); // Stop particles

        var timeSpent = Time.time - this.StartTime; // Time it was on

        this.Duration -= timeSpent; // Decrement timer

        var ev = (Event)Enum.Parse(typeof(Event), gameObject.name + "_OFF");
        this.EventManager.TriggerEvent(ev);

        this.AudioSource.Stop();
    }
    #endregion
}
