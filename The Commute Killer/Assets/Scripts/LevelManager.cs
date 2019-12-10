using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;



public class LevelManager : MonoBehaviour
{
    public bool Paused { get; private set; } = false;

    private GameObject Player;


    #region === Unity Events ===

    // Use this for initialization
    void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter");

        var fpc = this.Player.GetComponent<FirstPersonController>();

        fpc.m_MouseLook.SetCursorLock(true);

        Physics.gravity = new Vector3(0, -9.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            this.Paused = !this.Paused;

            var fpc = this.Player.GetComponent<FirstPersonController>();

            fpc.m_MouseLook.SetCursorLock(!this.Paused);
        }
    }

    #endregion

    #region === Game Control Functions ===
    public void GameOver()
    {
        //this.PauseUnpauseGame();
        //this.DrawGameOverPrompt();
        //this.TurnCursorVisible();

    }

    #endregion

    #region === Auxiliar Functions ===
    private void TooglePause()
    {
        this.Paused = !this.Paused;
    }

    #endregion
}
