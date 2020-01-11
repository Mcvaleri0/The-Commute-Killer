using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : Interactable
{

    protected AudioClip OpenSound;
    protected AudioClip CloseSound;

    private AudioSource AudioSource;

    public RectTransform RectTransform;
    private int state = 0;


    new protected void Start()
    {
        base.Start();

        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;

        this.OpenSound = (AudioClip)Resources.Load("Audio/map_open");
        this.CloseSound = (AudioClip)Resources.Load("Audio/map_close");

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Use
        };
    }

    // Start is called before the first frame update
    void Update()
    {
        switch (state)
        {
            case 0:
                break;
               

            //opening
            case 1:
                RectTransform.gameObject.SetActive(true);
                state = 3;

                this.AudioSource.PlayOneShot(OpenSound);

                break;

            //close
            case 2:
                RectTransform.gameObject.SetActive(false);
                state = 0;

                this.AudioSource.PlayOneShot(CloseSound);
                break;
            
            //open
            case 3:

                if (Input.GetMouseButtonUp(0))
                {
                    state = 2;
                }
                break;

        }
    }


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


    private void Use()
    {
        state = 1;
    }
}