using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPopUp : MonoBehaviour
{

    public RectTransform Map;
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
                Map.gameObject.SetActive(true);
                state = 0;
                break;

            //close
            case 2:
                Map.gameObject.SetActive(false);
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
