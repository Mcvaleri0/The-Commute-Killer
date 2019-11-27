using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    private int State = 0; //[ 0 - Closed | 1- Open ]

    private bool unlocked = false;

    public void Start()
    {
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            switch (State)
            {
                case 0: // Closed
                    openNotebook();
                    break;

                case 1: // Open
                    closeNotebook();
                    break;
            }
        }
    }

    public void openNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(true);

        this.State = 1;
    }

    public void closeNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(false);

        this.State = 0;
    }
}
