using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPrompt : MonoBehaviour
{

    public void Draw(string text)
    {
        this.transform.Find("Description").GetComponent<Text>().text = text;
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
        this.Hide();

        GameObject.Find("LevelManager").GetComponent<LevelManager>().Restart();
    }
}
