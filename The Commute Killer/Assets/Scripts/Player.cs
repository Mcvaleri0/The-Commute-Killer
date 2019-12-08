using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class Player : Agent
{
    public ISelector Selector;

    private Transform PrevSelection;

    public Action.IDs DeterminedAction { get; private set; }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        this.Selector = GameObject.Find("Selection Manager").GetComponent<ISelector>();
    }

    // Update is called once per frame
    void Update()
    {
        var selection = this.Selector.GetSelection();

        if (selection != this.PrevSelection) 
        {
            this.PrevSelection = selection;

            if (selection != null)
            {
                this.DeterminedAction = DetermineAction(GetPossibleActions(selection.gameObject));
            }
            else
            {
                this.DeterminedAction = Action.IDs.None;
            }
        }

        // Drop Key
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ExecuteAction(Action.IDs.Drop, this.OnHand.gameObject);
            return;
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (this.DeterminedAction != Action.IDs.None)
            {
                ExecuteAction(this.DeterminedAction, selection.gameObject);

                return;
            }
        }
    }
}
