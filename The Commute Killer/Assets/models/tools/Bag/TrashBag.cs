using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : Item
{

    protected bool ContainsVictim = false;

    // Start is called before the first frame update
    new public void Start()
    {
        base.Start();

        this.Name = "TrashBag";

        this.EnabledActions = new List<Action.IDs>()
        {
            Action.IDs.Insert
        };

        this.ActionSounds.Add(Action.IDs.PickUp, (AudioClip)Resources.Load("Audio/cadaver_on"));
        this.ActionSounds.Add(Action.IDs.Drop, (AudioClip)Resources.Load("Audio/cadaver_off"));

        if (this.ContainsVictim)
        {
            var bagging_sound = (AudioClip)Resources.Load("Audio/bag");
            this.AudioSource.PlayOneShot(bagging_sound);
        }
    }


    public void SetContainsVictim(bool a)
    {
        this.ContainsVictim = a;
    }

    public bool GetContainsVictim()
    {
        return this.ContainsVictim;
    }
}
