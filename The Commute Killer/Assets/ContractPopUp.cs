using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        openNotebook();
    }

    public void openNotebook()
    {
        var NoteBook = GameObject.Find("Canvas").transform.Find("NotebookController");

        NoteBook.gameObject.SetActive(true);

    }

    public void closeNotebook()
    {
        var NoteBook = this.transform.Find("Notebook");

        NoteBook.gameObject.SetActive(false);

    }
}
