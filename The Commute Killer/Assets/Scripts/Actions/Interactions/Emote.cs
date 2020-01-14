using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote : Interaction
{
    protected SpeechBubbleController SpeechBubbleController;

    protected SpeechBubbleController.Expressions Expression { get; set; }

    public Emote(Agent agent, SpeechBubbleController.Expressions exp, float duration, GameObject target = null) : base(agent, target) {
        this.ID = IDs.Emote;

        this.Expression = exp;

        this.Duration = duration;

        this.Instrument = null;

        var tr = agent.gameObject.transform;

        this.SpeechBubbleController = tr.Find("ExpressionController").GetComponent<SpeechBubbleController>();
    }

    public override void Update()
    {
        switch (this.State)
        {
            case 0: // To Start
                this.Execute();
                this.State = 1;
                this.Agent.GetComponent<AnimationController>().SetState(AnimationController.States.Talking);
                break;

            case 1: // In Progress
                if (!this.SpeechBubbleController.Visible)
                {
                    this.State = 2;
                    this.Agent.GetComponent<AnimationController>().SetState(AnimationController.States.Walking);
                }
                break;

            case 2: // Finished
                break;
        }
    }

    override public bool CanExecute()
    {
        #region Interactable
        // If we are interacting with another agent
        if (this.Interactable != null)
        {
            #region Distance
            if (Vector3.Distance(this.Interactable.transform.position, this.Agent.transform.position) > 5f)
            {
                return false;
            }
            #endregion
        }
        #endregion

        return true;
    }

    public override void Execute()
    {
        this.SpeechBubbleController.NewBubble(this.Expression, this.Duration);
    }
}
