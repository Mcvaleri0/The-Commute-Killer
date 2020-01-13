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
        Grab,
        Wrench,
        Eye,
        Insert,
        Sleep
    }

    private Dictionary<Mode, Transform> ModeTransforms;

    Mode Current;

    int State = 0;

    [Range(10f, 250f)]
    public float DefaultSize = 25f;

    private Player Player;
    private Transform Selected;

    private void Start() {

        this.RTransform = GetComponent<RectTransform>();

        this.ModeTransforms = new Dictionary<Mode, Transform>()
        {
            [Mode.Circle] = this.transform.Find("Circle"),
            [Mode.Point]  = this.transform.Find("Point"),
            [Mode.Grab]   = this.transform.Find("Grab"),
            [Mode.Wrench] = this.transform.Find("Wrench"),
            [Mode.Eye]    = this.transform.Find("Eye"),
            [Mode.Insert] = this.transform.Find("Insert"),
            [Mode.Sleep]  = this.transform.Find("Sleep")
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

        var size = this.DefaultSize;

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

            // --- Sabotage
            case Action.IDs.Sabotage:
                rectMode = Mode.Wrench;
                size = 35;
                break;
            
            // --- Read
            case Action.IDs.Read:
                rectMode = Mode.Eye;
                break;

            // --- Trash
            case Action.IDs.Trash:
                rectMode = Mode.Insert;
                size = 30;
                break;

            // --- Sleep
            case Action.IDs.Sleep:
                rectMode = Mode.Sleep;
                size = 30;
                break;

        }

        currentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);


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

