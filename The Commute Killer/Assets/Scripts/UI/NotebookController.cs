using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    private int State = 0; //[ 0 - Closed | 1- Notebook Open | 2- Map Open ]

    private bool unlocked = false;

    private AudioClip ContractOpenSound;
    private AudioClip ContractCloseSound;

    private AudioClip MapOpenSound;
    private AudioClip MapCloseSound;

    private AudioSource AudioSource;

    public void Start()
    {
        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;

        this.ContractOpenSound = (AudioClip)Resources.Load("Audio/contract_open");
        this.ContractCloseSound = (AudioClip)Resources.Load("Audio/contract_close");

        this.MapOpenSound = (AudioClip)Resources.Load("Audio/map_open");
        this.MapCloseSound = (AudioClip)Resources.Load("Audio/map_close");
    }

    public void Update()
    {
        switch (State)
        {
            case 0: // Closed

                if (Input.GetKeyDown(KeyCode.N))
                {
                    openNotebook();
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    openMap();
                }
                break;

            case 1: // Notebook Opened

                if (Input.GetKeyDown(KeyCode.N))
                {
                    closeNotebook();
                }
               
                break;

            case 2: // Map Opened

                if (Input.GetKeyDown(KeyCode.M))
                {
                    closeMap();
                }

                break;
        }
       
    }

    public void openNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(true);

        this.State = 1;

        this.AudioSource.PlayOneShot(ContractOpenSound);
    }

    public void openMap()
    {
        var Map = this.transform.Find("PaperMap");

        Map.gameObject.SetActive(true);

        this.State = 2;

        this.AudioSource.PlayOneShot(MapOpenSound);
    }

    public void closeNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(false);

        this.State = 0;

        this.AudioSource.PlayOneShot(ContractCloseSound);
    }

    public void closeMap()
    {
        var Map = this.transform.Find("PaperMap");

        Map.gameObject.SetActive(false);

        this.State = 0;

        this.AudioSource.PlayOneShot(MapCloseSound);
    }
}
