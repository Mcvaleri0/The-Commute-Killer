﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;



public class TimeManager : MonoBehaviour
{
    #region Variables setted in Unity Editor

    public float TimeMultiplier;

    public int StoryYear;
    public int StoryMonth;
    public int StoryDay;

    public double DaysToKill;

    public int VictimWorkHours;

    #endregion

    #region Time Variables

    private DateTime InitialTime { get; set; }
    private DateTime CurrentTime { get; set; }
    private DateTime NextDay { get; set; }
    private DateTime TimeLimit { get; set; }
    private DateTime TimeForVictimToMove { get; set; }

    #endregion

    #region Prompt Variables

    private DayPrompt DayPrompt { get; set; }
    private GameOverPrompt GameOverPrompt { get; set; }
    private bool PromptOpen { get; set; }
    private DateTime TimeToClosePrompt { get; set; }

    #endregion

    #region Auxiliar Variables

    private bool Pause { get; set; }

    private AutonomousAgent VictimController { get; set; }

    #endregion



    #region === Unity Events ===

    private void Start()
    {
        this.InitializeTime();

        this.VictimController = GameObject.Find("Victim").GetComponent<AutonomousAgent>();

        this.Pause = false;

        this.InitializePrompt();
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

            this.VerifyEvents();
        }
    }

    private void OnGUI()
    {
        this.DrawTime();
    }

    #endregion


    #region === Time Functions ===

    private void InitializeTime()
    {
        this.InitialTime = new DateTime(this.StoryYear, this.StoryMonth, this.StoryDay, 8, 30, 0);    // 6/9/1989  8:30

        this.CurrentTime = this.InitialTime;
        this.UpdateNextDay();
        this.TimeLimit = this.CurrentTime.AddDays(this.DaysToKill);

        // Time when victim goes home after work
        this.TimeForVictimToMove = DateTime.MaxValue;
    }

    private void UpdateCurrentTime()
    {
        this.CurrentTime = this.CurrentTime.AddSeconds(Time.deltaTime * this.TimeMultiplier);
    }

    private void ResetCurrentTime()
    {
        this.CurrentTime = this.InitialTime;
        this.UpdateNextDay();
    }

    private void UpdateNextDay()
    {
        this.NextDay = new DateTime(this.CurrentTime.Year, this.CurrentTime.Month, this.CurrentTime.Day + 1, 0, 0, 0);
    }

    public void UpdateTimeForVictimToMove()
    {
        if (VictimController.GoalHome)
        {
            // If the victim got home then his next action will be in the next
            // day when he has to go to work
            this.TimeForVictimToMove = new DateTime(this.NextDay.Year, this.NextDay.Month, this.NextDay.Day,
                                                    this.InitialTime.Hour, this.InitialTime.Minute, this.InitialTime.Second);
        }
        else
        {
            // If the victim got at his work place then his next action will be 
            // after 8 hours when he has to go home
            this.TimeForVictimToMove = this.CurrentTime.AddHours(this.VictimWorkHours);
        }

        Debug.Log("TimeManager -> Time For Victim To Move: " + this.TimeForVictimToMove.ToString());

        this.FastFoward();
    }

    private void NewDay()
    {
        this.DrawDayPrompt();
        this.UpdateNextDay();
    }

    private void VerifyEvents()
    {
        if (this.CurrentTime >= this.TimeLimit)
        {
            this.GameOver();
        }

        else if (this.CurrentTime >= this.NextDay)
        {
            this.NewDay();
        }

        // Time to close day prompt
        if (this.PromptOpen && (this.CurrentTime >= this.TimeToClosePrompt))
        {
            this.HidePrompt();
        }

        if (this.TimeForVictimToMove != DateTime.MaxValue &&    // If TimeForVictimToMove is set
            this.CurrentTime >= this.TimeForVictimToMove)       // and the time has come
        {
            this.MoveVictim();
        }
    }

    private void DrawTime()
    {
        var width = Screen.width / 15;
        var height = Screen.height / 15;
        var padding = 10;

        GUI.Box(new Rect(Screen.width - width - padding, padding / 2, width, height), 
                this.CurrentTime.ToString("dd'-'MM'-'yy'\n'H':'mm':'ss"));
    }

    private void FastFoward()
    {
        this.TimeMultiplier = 2000;
    }

    private void NormalSpeed()
    {
        this.TimeMultiplier = 10;
    }

    #endregion


    #region === Prompt Functions ===

    private void InitializePrompt()
    {
        var canvas = GameObject.Find("Canvas").transform;
        this.DayPrompt = canvas.Find("DayPrompt").GetComponent<DayPrompt>();

        this.GameOverPrompt = canvas.Find("GameOverPrompt").GetComponent<GameOverPrompt>();

        this.DayPrompt.Initialize();
        this.DrawDayPrompt();
    }

    private void DrawDayPrompt()
    {
        // prompt stays open during 1 second in real life
        this.TimeToClosePrompt = this.CurrentTime.AddSeconds(1 * this.TimeMultiplier);

        var text = "Day " + (this.CurrentTime.Day - this.InitialTime.Day + 1);
        this.DayPrompt.Draw(text);

        this.PromptOpen = true;
    }

    private void DrawGameOverPrompt()
    {
        this.GameOverPrompt.Draw();
    }

    private void HidePrompt()
    {
        this.PromptOpen = false;
        this.DayPrompt.Hide();
    }

    #endregion


    #region === Auxiliar Functions ===

    private void PauseUnpauseGame()
    {
        this.Pause = !this.Pause;
    }

    private void GameOver()
    {
        this.PauseUnpauseGame();
        this.DrawGameOverPrompt();
        this.TurnCursorVisible();
    }

    public void ResetTime()
    {
        this.CurrentTime = this.InitialTime;
        this.UpdateNextDay();
        this.TimeLimit = this.CurrentTime.AddDays(this.DaysToKill);

        this.PauseUnpauseGame();
    }

    private void TurnCursorVisible()
    {
        var playerController = GameObject.Find("PlayerCharacter").GetComponent<FirstPersonController>();

        playerController.m_MouseLook.SetCursorLock(false);
    }

    private void MoveVictim()
    {
        Debug.Log("TimeManger -> Victim Move!!!");

        this.NormalSpeed();
        this.TimeForVictimToMove = DateTime.MaxValue;
        this.VictimController.ToogleGoalPosition();
    }

    #endregion

}