using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recticle : MonoBehaviour {

    private RectTransform RTransform;

    public enum Mode
    {
        Circle,
        Point,
        Grab
    }

    private Dictionary<Mode, Transform> ModeTransforms;

    Mode Current;

    int State = 0;

    [Range(10f, 250f)]
    public float Size = 20f;

    private Player Player;
    private Transform Selected;

    private void Start() {

        this.RTransform = GetComponent<RectTransform>();

        this.ModeTransforms = new Dictionary<Mode, Transform>()
        {
            [Mode.Circle] = this.transform.Find("Circle"),
            [Mode.Point]  = this.transform.Find("Point"),
            [Mode.Grab]   = this.transform.Find("Grab")
        };

        this.Current = Mode.Circle;

        this.Player = GameObject.Find("PlayerCharacter").GetComponent<Player>();
    }

    private void SwitchRecticle(Mode mode) {
        this.ModeTransforms[this.Current].gameObject.SetActive(false);

        this.ModeTransforms[mode].gameObject.SetActive(true);

        this.Current = mode;
    }

    private void Update() {
        var currentTransform = this.ModeTransforms[this.Current];

        currentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(this.Size, this.Size);

        var rectMode = Mode.Circle;

        switch (GetActionID())
        {
            // --- Normal
            default:
            case Action.IDs.None:
            break;

            // --- Point
            case Action.IDs.Use:
            case Action.IDs.PickUp: // FIXME: This should be in Grab when we have another recticle
                rectMode = Mode.Point;
                break;

            // --- Grab
            
        }

        if (this.Current != rectMode)
        {
            SwitchRecticle(rectMode);
        }
    }

    private Action.IDs GetActionID()
    {
        var aID = Action.IDs.None;
        var action = this.Player.DeterminedAction;

        if (action != null)
        {
            aID = action.ID;
        }

        return aID;
    }
}

