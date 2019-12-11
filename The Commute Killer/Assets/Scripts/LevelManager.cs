using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;



public class LevelManager : MonoBehaviour
{
    #region /* Game Control */
    
    public bool Paused { get; private set; } = false;
    private bool GameFinished { get; set; }
    private GameOverPrompt GameOverPrompt { get; set; }
    private WinPrompt WinPrompt { get; set; }

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

    }

    // Update is called once per frame
    void Update()
    {
        if(!this.GameFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            this.Paused = !this.Paused;

            this.PlayerController.m_MouseLook.SetCursorLock(!this.Paused);
        }
    }

    #endregion

    #region === Game Control Functions ===

    public void GameOver()
    {
        this.GameFinished = true;
        this.Paused = true;

        this.DrawGameOverPrompt();

        this.PlayerController.m_MouseLook.SetCursorLock(false);
    }

    public void Win()
    {
        this.GameFinished = true;
        this.Paused = true;

        this.DrawWinPrompt();

        this.PlayerController.m_MouseLook.SetCursorLock(false);
    }

    #endregion

    #region === Auxiliar Functions ===

    private void DrawGameOverPrompt()
    {
        this.GameOverPrompt.Draw();
    }

    private void DrawWinPrompt()
    {
        this.WinPrompt.Draw();
    }

    #endregion
}
