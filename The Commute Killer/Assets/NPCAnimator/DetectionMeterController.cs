using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionMeterController : LerpScaleElement
{
    private Transform DetectionMeter;

    private Transform Meter;

    protected float Progress = 0f;

    private IEnumerator LastCourotine;

    public float Increment = 0.00001f;
    public float UpdateTime = 0.01f;

    // Start is called before the first frame update
    new void Start()
    {
        this.DetectionMeter = this.transform.Find("DetectionMeter");
        this.Meter = this.DetectionMeter.Find("Pupil/Meter");

        this.Element = this.DetectionMeter;
        this.Visible = false;
        base.Start();
    }

    new void Update()
    {
        base.Update();

        this.Meter.localScale = new Vector3(this.Progress, this.Progress, this.Progress);

        if(this.Progress != 0)
        {
            DetectionMeter.localScale = Vector3.one + new Vector3(this.Progress, this.Progress, this.Progress);
        }
    }

    public bool IsDetected()
    {
        return (this.Progress >= 1);
    }

    public void StartDetection()
    {
        //end current meter if there is one
        if (this.Visible)
        {
            this.EndDetection();
        }

        this.Visible = true;

        this.LastCourotine = TickProgress();
        StartCoroutine(this.LastCourotine);

    }

    protected IEnumerator TickProgress()
    {
        while (this.Progress < 1)
        {
            this.Progress += this.Increment;
            yield return new WaitForSeconds(this.UpdateTime);
        }
        this.Progress = 1;
    }

    public void EndDetection()
    {
        if(this.LastCourotine != null)
        {
            StopCoroutine(this.LastCourotine);
        }
        
        this.Visible = false;
        this.Progress = 0f;
    }
    

    
}