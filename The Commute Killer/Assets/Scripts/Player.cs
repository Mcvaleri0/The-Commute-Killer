using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class Player : Agent
{
    public SelectionManager Select;

    // Start is called before the first frame update
    void Start()
    {
        this.Select = GameObject.Find("Selection Manager").GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var selection = this.Select.getSelection();


        }
    }
}
