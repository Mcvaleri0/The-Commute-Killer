using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : Interactable
{
    private int State = 0;
    private AudioSource AudioSource;
    private AudioClip SoundTicking;
    private AudioClip SoundRinging;
    private AudioClip SoundDeactivate;

    #region === MonoBehaviour Methods ===
    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };

        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;
        this.AudioSource.loop = true;
        this.AudioSource.volume = 0.3f;
        this.AudioSource.spatialBlend = 1.0f;

        this.SoundTicking    = (AudioClip)Resources.Load("Audio/clock_ticking");
        this.SoundRinging    = (AudioClip)Resources.Load("Audio/clock_ringing");
        this.SoundDeactivate = (AudioClip)Resources.Load("Audio/button_click");

}

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case 0: // Normal

                break;

            case 1: // Ringing

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

            case Action.IDs.Use:
                Use();
                return true;
        }

        return false;
    }

    override public bool CanInteract(Agent Interactor, Action.IDs id)
    {
        if (this.ActionAvailable(id))
        {
            return true;
        }

        return false;
    }
    #endregion

    #region === Possible Action Methods ===
    private void Use()
    {
        Deactivate();
    }
    #endregion

    #region === Object Behaviour ===
    public void StartTicking()
    {
        this.AudioSource.clip = this.SoundTicking;
        this.AudioSource.Play();
        this.State = 0;
    }

    public void StartRinging() 
    {
        this.AudioSource.clip = this.SoundRinging;
        this.AudioSource.Play();
        this.State = 1;
    }

    private void Deactivate()
    {
        this.AudioSource.Stop();
        this.AudioSource.PlayOneShot(this.SoundDeactivate);
        this.State = 0;
    }
    #endregion
}
