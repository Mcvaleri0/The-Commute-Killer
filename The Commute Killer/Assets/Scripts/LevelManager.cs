﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    #region /* Game Control */
    
    public bool Paused { get; private set; } = false;
    private bool GameFinished { get; set; }
    private GameOverPrompt GameOverPrompt { get; set; }
    private WinPrompt WinPrompt { get; set; }
    private ExitPrompt ExitPrompt { get; set; }

    private TimeManager TimeManager;

    private int CurrentDay;

    private List<AutonomousAgent> Agents;
    #endregion

    #region /* Auxiliar */

    private FirstPersonController PlayerController;

    #endregion


    #region === Unity Events ===

    // Use this for initialization
    void Start()
    {
        this.PlayerController = GameObject.Find("PlayerCharacter").GetComponent<FirstPersonController>();

        this.UnPause();

        Physics.gravity = new Vector3(0, -9.8f, 0);

        this.GameOverPrompt = GameObject.Find("Canvas").transform.Find("GameOverPrompt").GetComponent<GameOverPrompt>();
        this.WinPrompt = GameObject.Find("Canvas").transform.Find("WinPrompt").GetComponent<WinPrompt>();
        this.ExitPrompt = GameObject.Find("Canvas").transform.Find("ExitPrompt").GetComponent<ExitPrompt>();

        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.CurrentDay = this.TimeManager.GetCurrentTime().Day;

        this.Agents = new List<AutonomousAgent>();

        var agents = GameObject.FindGameObjectsWithTag("NPC");

        foreach(var agent in agents)
        {
            var aa = agent.GetComponent<AutonomousAgent>();

            if(aa != null)
            {
                this.Agents.Add(aa);
            }   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GameFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.Paused)
            {
                this.UnPause();
                this.ExitPrompt.Hide();
            }
            else
            {
                this.Pause();
                this.ExitPrompt.Draw();
            }
        }

        /*
        var currentTime = this.TimeManager.GetCurrentTime();

        if(this.CurrentDay != currentTime.Day)
        {
            this.CurrentDay = currentTime.Day;

            foreach(var agent in this.Agents)
            {
                var charController = agent.GetComponent<CharacterController>();

                charController.Move(agent.StartPosition);
            }
        }
        */
    }

    #endregion

    #region === Game Control Functions ===

    public void GameOver(string text)
    {
        this.GameFinished = true;
        this.Pause();

        this.GameOverPrompt.Draw(text);
    }

    public void Win()
    {
        this.GameFinished = true;
        this.Pause();

        this.WinPrompt.Draw();
    }

    public void Pause()
    {
        this.Paused = true;
        Time.timeScale = 0;
        this.PlayerController.m_MouseLook.SetCursorLock(false);
        this.PlayerController.enabled = false;
        AudioListener.volume = 0.3f;

    }

    public void UnPause()
    {
        this.Paused = false;
        Time.timeScale = 1;
        this.PlayerController.m_MouseLook.SetCursorLock(true);
        this.PlayerController.enabled = true;
        AudioListener.volume = 1.0f;

    }

    public void Resart()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    #endregion

    #region === Auxiliar Functions ===
    public void TrainHasArrived()
    {
        Debug.Log("ARRIVED");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject obj in npcs)
        {
            //do something - tell npc train has arrived.
        }

    }
    public void TrainGoingToDeparture()
    {
        Debug.Log("DEPARTURED");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject obj in npcs)
        {
            //do something - tell npc train has departured
        }
    }
    #endregion
}
