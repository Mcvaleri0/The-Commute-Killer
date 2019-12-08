using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractPopUp : MonoBehaviour
{

    public RectTransform contract;
    private int state = 0;

    // Start is called before the first frame update
    void Update()
    {
        switch (state)
        {
            case 0:
            break;

            //open
            case 1:
                contract.gameObject.SetActive(true);
                state = 0;
            break;

            //close
            case 2:
                contract.gameObject.SetActive(false);
                state = 0;
            break;


        }
    }

    void OnMouseDown()
    {
        state = 1;
    }

    void OnMouseUp()
    {
        state = 2;
    }
}
