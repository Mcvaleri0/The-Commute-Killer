using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSchedule : MonoBehaviour
{
    private DateTime[] departures;
    private DateTime[] arrivals;
    private TimeManager manager;
    private bool inStation;
    public GameObject obj;
    // Start is called before the first frame update
    private void Start()
    {
        this.arrivals = new DateTime[5];
        this.arrivals[0] = new DateTime(1, 1, 1, 8, 30, 0);
        this.arrivals[1] = new DateTime(1, 1, 1, 10, 00, 0);
        this.arrivals[2] = new DateTime(1, 1, 1, 16, 30, 0);
        this.arrivals[3] = new DateTime(1, 1, 1, 18, 30, 0);
        this.arrivals[4] = new DateTime(1, 1, 1, 20, 30, 0);

        this.departures = new DateTime[5];
        this.departures[0] = new DateTime(1, 1, 1, 8, 40, 0);
        this.departures[1] = new DateTime(1, 1, 1, 10, 10, 0);
        this.departures[2] = new DateTime(1, 1, 1, 16, 40, 0);
        this.departures[3] = new DateTime(1, 1, 1, 18, 40, 0);
        this.departures[4] = new DateTime(1, 1, 1, 20, 40, 0);       

        this.manager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        this.inStation = false;
        this.obj.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("updating");
        DateTime[] aux;
        DateTime currentTime = this.manager.GetCurrentTime();

        if (this.inStation)
        {
            aux = this.departures;
        }
        else
        {
            aux = this.arrivals;
        }

        for(int i=0; i < aux.Length; i++)
        {
            if(currentTime.Hour == aux[i].Hour && currentTime.Minute == aux[i].Minute)
            {
                Debug.Log("IT TIMEEE");
                this.inStation = !this.inStation;
                Debug.Log(this.inStation);
                if (this.inStation)
                {
                    this.obj.SetActive(true);
                }
                this.obj.GetComponent<TrainMovement>().move = true;
                return;
            }
        }
    }
}
