﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : MonoBehaviour, Interactable
{
    private int State = 0; // [ 0 - Full | 1 - Spouting | 2 - Empty]

    public int Liters = 10 * 60;

    public ParticleSystem WaterSpout;

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
                Liters -= 1;

                if(Liters <= 0)
                {
                    State = 2;

                    WaterSpout.Stop();
                }

                break;

            case 2: // Empty

                break;
        }
    }

    public bool Interact(GameObject Interactor)
    {
        if(CanInteract(Interactor))
        {
            State = 1;

            WaterSpout.Play();
        }

        return false;
    }

    public bool CanInteract(GameObject Interactor)
    {
        if(State == 0 && Vector3.Distance(gameObject.transform.position, Interactor.transform.position) < 30)
        {
            return true;
        }

        return false;
    }
}
