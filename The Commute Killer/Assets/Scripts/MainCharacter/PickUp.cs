using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform newPos;
    private Transform oldParent;

    void OnMouseDown()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = newPos.position;
        this.oldParent = this.transform.parent;
        this.transform.parent = GameObject.Find("hand").transform;
    }

    void OnMouseUp()
    {
        this.transform.parent = this.oldParent;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
    }
}
