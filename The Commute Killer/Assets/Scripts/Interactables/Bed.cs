using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{

    public GameObject SleepTransition;
    protected CanvasGroup Group;

    protected int State = 0;

    protected TimeManager TimeManager;

    public int WakeUpHour = 8;

    private int StartingDay;

    public GameObject AlarmClock;

    private Player Player;

    #region === MonoBehaviour Methods ===
    new void Start()
    {
        base.Start();

        this.PossibleActions = new List<Action.IDs>()
        {
            Action.IDs.Sleep
        };

        this.Group = this.SleepTransition.GetComponent<CanvasGroup>();

        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.Player = GameObject.Find("PlayerCharacter").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            //normal
            case 0:
                this.SleepTransition.SetActive(false);
                break;
            
            //fade to black
            case 1:
                StartCoroutine(DoFade(1, 4));
                this.State = 3;

                break;
            
            //fade to normal
            case 2:
                StartCoroutine(DoFade(0, 0));
                this.State = 3;
                break;

            //fadding
            case 3:
                this.SleepTransition.SetActive(true);
                break;

            //wait
            case 4:
                this.TimeManager.SleepFastFoward();
                var time = this.TimeManager.GetCurrentTime();
                var hour = time.Hour;
                var day = time.Day;

                if(day != this.StartingDay && hour == this.WakeUpHour)
                {
                    this.State = 2;

                    this.Wake();
                }
                
                break;
        }
    }
    #endregion

    IEnumerator DoFade(float targetAlpha, int NextState)
    {
        while (Mathf.Abs(this.Group.alpha - targetAlpha) > 0.001) {

            this.Group.alpha = Mathf.Lerp(this.Group.alpha, targetAlpha, Time.deltaTime * 4f);
            yield return null;
        }

        this.Group.alpha = targetAlpha;
        this.State = NextState;
        yield return null;
    }


    #region === Interactable Methods
    override public bool Interact(Action.IDs id)
    {
        switch (id)
        {
            default:
                break;

            case Action.IDs.Sleep:
                Sleep();
                return true;
        }

        return false;
    }

    override public bool CanInteract(Agent Interactor, Action.IDs id)
    {
        if (this.State == 0 && this.ActionAvailable(id))
        {

            return true;

        }
        return false;
    }
    #endregion

    #region === Possible Action Methos ===
    private void Sleep()
    {
        Debug.Log("Sleep Time");

        this.TimeManager.SleepFastFoward();
        
        this.State = 1;
        var time = this.TimeManager.GetCurrentTime();
        this.StartingDay = time.Day;

        var clock = this.AlarmClock.GetComponent<AlarmClock>();
        clock.StartTicking();

        //lock player 
        this.Player.LockMovement();
    }

    private void Wake()
    {
        this.TimeManager.NormalSpeed();
        var clock = this.AlarmClock.GetComponent<AlarmClock>();
        clock.StartRinging();

        //unlock player
        this.Player.UnlockMovement();
    }

    #endregion
}
