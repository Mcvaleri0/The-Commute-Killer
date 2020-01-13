using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{

    public Player Player;
    protected AnimationController Anim;

    protected DetectionMeterController DetectMeter;

    public float VisionAngle   = 40;
    public float VisionDistance = 6;

    protected int State = 0;
    protected bool InSight = false;
    protected bool Threat = false;

    void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter").GetComponent<Player>();
        this.Anim = Player.gameObject.GetComponent<AnimationController>();
        this.DetectMeter = GetComponentInChildren<DetectionMeterController>();
    }

   
    void Update()
    {
        //see if player is within sight
        Vector3 direction = this.Player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        float distance = Vector3.Distance(this.Player.transform.position, this.transform.position);
        
        this.InSight = false;
        if (distance < this.VisionDistance && angle < this.VisionAngle)
        {
            this.InSight = true;
        }

        //see if player is a threat
        this.Threat = IsPlayerThreat();

        switch (this.State)
        {
            //NPC sees nothing
            case 0:

                if(InSight && Threat)
                {
                    DetectMeter.StartDetection();
                    this.State = 1;
                }

                break;
            

            //NPC sees player as a threat
            case 1:

                if (DetectMeter.IsDetected())
                {
                    DetectMeter.EndDetection();
                    this.State = 2;
                }

                if (!InSight)
                {
                    DetectMeter.EndDetection();
                    this.State = 0;
                }

                break;
            

            //NPC has seen for long enough
            case 2:

                GameObject.Find("EventManager").GetComponent<EventManager>().TriggerEvent(Event.CaughtByNPC);
                this.State = 0;

                break;
        }
    }

    protected bool IsPlayerThreat()
    {
        // player is threat if they have a knife
        if(this.Player.OnHand != null && this.Player.OnHand.Types.Contains(Item.ItemType.SharpWeapon))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (this.InSight)
        {
            Gizmos.DrawSphere(transform.position, 0.4f);    
        }
    }
}
