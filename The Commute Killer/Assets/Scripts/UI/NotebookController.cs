using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    private int State = 0; //[ 0 - Closed | 1- Notebook Open | 2- Map Open ]

    private bool unlocked = false;

    public void Start()
    {
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
    }

    public void openMap()
    {
        var Map = this.transform.Find("PaperMap");

        Map.gameObject.SetActive(true);

        this.State = 2;
    }

    public void closeNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(false);

        this.State = 0;
    }

    public void closeMap()
    {
        var Map = this.transform.Find("PaperMap");

        Map.gameObject.SetActive(false);

        this.State = 0;
    }
}
