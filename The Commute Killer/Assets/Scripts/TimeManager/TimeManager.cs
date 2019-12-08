using System;
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

    public int WakeUpHour;
    public int WakeUpMinute;

    public double DaysToKill;

    #endregion

    #region Time Variables

    private DateTime InitialTime { get; set; }
    private DateTime CurrentTime { get; set; }
    private DateTime NextDay { get; set; }
    private DateTime TimeLimit { get; set; }

    #endregion

    #region Prompt Variables

    private DayPrompt DayPrompt { get; set; }
    private GameOverPrompt GameOverPrompt { get; set; }
    private bool PromptOpen { get; set; }
    private DateTime TimeToClosePrompt { get; set; }

    #endregion

    #region Auxiliar Variables

    private bool Pause { get; set; }

    #endregion



    #region === Unity Events ===

    private void Start()
    {
        this.InitializeTime();
        
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
        this.InitialTime = new DateTime(this.StoryYear, DateTime.Now.Month, DateTime.Now.Day,
                                        this.WakeUpHour, this.WakeUpMinute, 0);

        this.CurrentTime = this.InitialTime;
        this.UpdateNextDay();
        this.TimeLimit = this.CurrentTime.AddDays(this.DaysToKill);
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
        else if (this.PromptOpen && (this.CurrentTime >= this.TimeToClosePrompt))
        {
            this.HidePrompt();
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

    #endregion


    #region === Prompt Functions ===

    private void InitializePrompt()
    {
        var canvas = this.transform.Find("Canvas");
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

    private void TurnCursorVisible()
    {
        var playerController = GameObject.Find("PlayerCharacter").GetComponent<FirstPersonController>();

        playerController.m_MouseLook.SetCursorLock(false);
    }

    #endregion

}
