using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// Ratio to real-time
    /// </summary>
    public float TimeMultiplier;

    public int WakeUpHour;
    public int WakeUpMinute;

    public DateTime CurrentTime { get; set; }

    public bool Pause { get; set; }


    void Start()
    {
        this.CurrentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 
                                        this.WakeUpHour  , this.WakeUpMinute , 0);

        this.Pause = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.PauseUnpauseGame();
        }

        if (!this.Pause)
        {
            this.UpdateCurrentTime();
        }
    }


    private void OnGUI()
    {
        this.DrawTime();
    }


    public void DrawTime()
    {
        var width = Screen.width / 15;
        var height = Screen.height / 15;
        var padding = 10;

        GUI.Box(new Rect(Screen.width - width - padding, padding / 2, width, height), 
                this.CurrentTime.ToString("dd'-'MM'-'yy'\n'H':'mm':'ss"));
    }


    public void PauseUnpauseGame()
    {
        this.Pause = !this.Pause;
    }


    public void UpdateCurrentTime()
    {
        this.CurrentTime = this.CurrentTime.AddSeconds(Time.deltaTime * this.TimeMultiplier);
    }
}
