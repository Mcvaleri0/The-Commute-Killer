using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public bool move;
    private Vector3 initialPosition;
    private Vector3 finalPosition;
    private Vector3 middlePosition;

    private Vector3 speed;
    public GameObject train;
    private bool middle;

    // Start is called before the first frame update
    void Start()
    {
        this.move = false;
        this.finalPosition = new Vector3(5.78f, -1.55f, 2.7f);
        this.initialPosition = new Vector3(5.78f, -1.55f, 92.1f);
        this.middlePosition = new Vector3(5.78f, -1.55f, 45.6f);
        this.speed = new Vector3(0, 0, 10f);
        this.middle = false; ;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = GetComponent<Transform>().position;
        if (this.move && (currentPos.z > finalPosition.z || currentPos.z > middlePosition.z))
        {
            transform.Translate(- speed * Time.deltaTime);
        }
         if (this.move && currentPos.z <= finalPosition.z)
        {
            this.move = false;
            transform.position = this.initialPosition;
            train.SetActive(false);
            this.middle = false;
        }
         if (this.move && !this.middle && currentPos.z <= middlePosition.z)
        {
            this.middle = true;
            this.move = false;
        }
    }
}
