using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : Agent
{
    private SelectionManager SelectionM;

    public GameObject DeterminedSelection;

    public Action DeterminedAction;

    public Action ExecutingAction { get; private set; }

    private bool reactivateController = false;

    private FirstPersonController Controller;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        this.SelectionM = GameObject.Find("Selection Manager").GetComponent<SelectionManager>();

        //reactivate Controller (FIXME: fps controller is fucky)
        reactivateController = false;
        this.Controller = this.GetComponent<FirstPersonController>();
}

    // Update is called once per frame
    new void Update()
    {
        if (this.Attributes[Attribute.HP] <= 0)
        {
            Die();
            return;
        }

        // If an action is being executed
        if(this.ExecutingAction != null)
        {
            this.ExecutingAction.Update();

            if(this.ExecutingAction.State == 2)
            {
                this.ExecutingAction = null;
            }
        }

        // If no action is in execution
        else
        {
            DetermineSelection();

            DetermineAction();

            #region Player Controls
            // Drop Item On Hand
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ExecuteAction(CreateAction(Action.IDs.Drop, this.DeterminedSelection));
                return;
            }

            // General Actions
            if (Input.GetMouseButtonDown(0))
            {
                ExecuteAction(this.DeterminedAction);
            }
            #endregion
        }
    }

    void FixedUpdate()
    {
        //Reactivate Controller
        if (reactivateController)
        {
            Controller.enabled = true;
            reactivateController = false;
        }
    }

    protected void DetermineSelection()
    {
        GameObject selection = null;

        var sT = this.SelectionM.CurrentSelection;

        if (sT != null)
        {
            selection = sT.gameObject;
        }

        this.DeterminedSelection = selection; 
    }

    protected void DetermineAction()
    {
        var target = this.DeterminedSelection;

       // Check Actions enabled by the On Hand Item
        if(this.OnHand != null)
        {
            foreach (Action.IDs id in this.OnHand.EnabledActions)
            {
                var action = CreateAction(id, target);

                if(action.CanExecute())
                {
                    this.DeterminedAction = action;
                    return;
                }
            }
        }

        // Check Available Actions
        foreach (Action.IDs id in this.AvailableActions)
        {
            var action = CreateAction(id, target);
                
            if (action.CanExecute())
            {
                this.DeterminedAction = action;
                return;
            }
        }

        // Check On Hand Item Fall Back Action
        if(this.OnHand != null)
        {
            // If there is a Fall Back Action
            if(this.OnHand.DefaultAction != Action.IDs.None)
            {
                var action = CreateAction(this.OnHand.DefaultAction);

                if(action.CanExecute())
                {
                    this.DeterminedAction = action;
                    return;
                }
            }
        }

        this.DeterminedAction = null;
        return;
    }

    public void Teleport(Vector3 targetPos, Vector3 targetAngles)
    {
        Controller.enabled = false;
        this.transform.position = targetPos;
        this.transform.eulerAngles = new Vector3(0, 2, 0);
        reactivateController = true;

    }
}
