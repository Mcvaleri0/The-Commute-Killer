using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Player player;
    protected Animator anim;
    protected CharacterController character;

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
        anim = gameObject.GetComponent<Animator>();
        character = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        //pass params to animator
        Vector3 velocity = character.velocity;
        float speed = velocity.magnitude;
        anim.SetFloat("Speed", speed);
  
    }

    //set State
    public void SetState(States state)
    {
        this.State = (int) state;
        anim.SetInteger("State", State);
    }

}