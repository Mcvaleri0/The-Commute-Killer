using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : MonoBehaviour, Interactable
{
    private int State = 0; // [ 0 - Full | 1 - Spouting | 2 - Empty]

    private float StartTime;

    public float Duration = 2 * 60;

    public ParticleSystem WaterSpout;

    public GameObject Interactor;

    public EventManager EventManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case 0: // Full

                break;

            case 1: // Spouting
                if(StartTime + Duration <= Time.time)
                {
                    State = 2;

                    WaterSpout.Stop();

                    this.EventManager.TriggerEvent(gameObject.name + "_OFF");
                }

                break;

            case 2: // Empty

                break;
        }
    }

    public bool Interact(Agent Interactor)
    {
        if(CanInteract(Interactor))
        {
            State = 1;

            WaterSpout.Play();

            StartTime = Time.time;

            this.EventManager.TriggerEvent(gameObject.name + "_ON");
        }

        return false;
    }

    public bool CanInteract(Agent Interactor)
    {
        if(State == 0 && Vector3.Distance(this.transform.position, Interactor.transform.position) < 30)
        {
            return true;
        }

        return false;
    }
}
