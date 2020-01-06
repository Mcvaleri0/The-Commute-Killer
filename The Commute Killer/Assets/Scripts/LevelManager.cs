using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region /* Game Control */
    
    public bool Paused { get; private set; } = false;
    private bool GameFinished { get; set; }
    private GameOverPrompt GameOverPrompt { get; set; }
    private WinPrompt WinPrompt { get; set; }
    private ExitPrompt ExitPrompt { get; set; }

    #endregion

    #region /* Auxiliar */

    private FirstPersonController PlayerController;

    #endregion


    #region === Unity Events ===

    // Use this for initialization
    void Start()
    {
        this.PlayerController = GameObject.Find("PlayerCharacter").GetComponent<FirstPersonController>();

        this.PlayerController.m_MouseLook.SetCursorLock(true);

        Physics.gravity = new Vector3(0, -9.8f, 0);

        this.GameOverPrompt = GameObject.Find("Canvas").transform.Find("GameOverPrompt").GetComponent<GameOverPrompt>();
        this.WinPrompt = GameObject.Find("Canvas").transform.Find("WinPrompt").GetComponent<WinPrompt>();
        this.ExitPrompt = GameObject.Find("Canvas").transform.Find("ExitPrompt").GetComponent<ExitPrompt>();
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
    }

    #endregion

    #region === Game Control Functions ===

    public void GameOver()
    {
        this.GameFinished = true;
        this.Pause();

        this.GameOverPrompt.Draw();
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
    }

    public void UnPause()
    {
        this.Paused = false;
        Time.timeScale = 1;
        this.PlayerController.m_MouseLook.SetCursorLock(true);
        this.PlayerController.enabled = true;
    }

    public void Resart()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
    }

    #endregion

    #region === Auxiliar Functions ===

    #endregion
}
