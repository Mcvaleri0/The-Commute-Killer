using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayPrompt : MonoBehaviour
{
    private Text PromptText { get; set; }


    public void Initialize()
    {
        this.PromptText = this.transform.Find("Text").GetComponent<Text>();
    }

    public void Draw(string text)
    {
        this.PromptText.text = text;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
