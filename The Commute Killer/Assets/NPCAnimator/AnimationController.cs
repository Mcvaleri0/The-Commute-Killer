using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Player Player;
    protected Animator Anim;
    protected CharacterController Character;

    protected AutonomousAgent AutoAgent;

    //the number value is used in the animator
    public enum States
    {
        Walking = 0,
        Alert   = 1,
        Dead    = 2,
        Talking = 3
    }

    protected int State = (int) States.Walking;

    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        AutoAgent = gameObject.GetComponent<AutonomousAgent>();
        Character = gameObject.GetComponent<CharacterController>();
        
    }

    void Update()
    {
        //pass params to animator
        /*
        Vector3 velocity = Character.velocity;
        float speed = velocity.magnitude;
        */

        float speed = AutoAgent.GetVelocity();

        //Debug.Log(speed);
        Anim.SetFloat("Speed", speed);
  
    }

    //set State
    public void SetState(States state)
    {
        this.State = (int) state;
        Anim.SetInteger("State", State);
    }

}