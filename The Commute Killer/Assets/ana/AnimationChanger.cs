using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanger : MonoBehaviour
{
    public Player player;
    static Animator anim;
    public GameObject alertSign;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (anim.GetBool("isDying"))
        {
            anim.SetBool("isIdling", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAlert", false);
            alertSign.SetActive(false);
        }

        if (anim.GetBool("isWalking"))
        {
            this.transform.Translate(0, 0, 0.001f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetBool("isIdling", true);
            alertSign.SetActive(false);
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("isIdling", false);
            anim.SetBool("isWalking", true);
            alertSign.SetActive(false);
        }


        Vector3 direction = player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        bool hasDangerous = hasDangerousItem();
            
        if ((Vector3.Distance(player.transform.position, this.transform.position) < 3 && angle < 30 && hasDangerous ) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Just pressed 2");
            anim.SetBool("isIdling", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAlert", true);
            alertSign.SetActive(true);

            Debug.Log(anim.GetBool("isAlert"));
            Debug.Log(anim.GetBool("isWalking"));
        }

        
    }

    bool hasDangerousItem()
    {
        Item weapon = player.OnHand;
        if (weapon)
        {
            return true;
        }
        return false;
    }
}