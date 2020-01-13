using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with something");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("no more collision");
    }
}
