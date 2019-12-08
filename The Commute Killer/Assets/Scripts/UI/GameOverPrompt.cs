using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPrompt : MonoBehaviour
{
    public void Draw()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void MainMenuButton()
    {
        this.Hide();

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ResetButton()
    {
        Debug.Log("Resart");
    }
}
