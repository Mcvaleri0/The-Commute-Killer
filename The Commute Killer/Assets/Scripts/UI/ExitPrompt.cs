using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPrompt : MonoBehaviour
{
    private Transform Prompt;

    private bool PromptActive;

    // Start is called before the first frame update
    void Start()
    {
        this.Prompt = transform.Find("ExitPromptWindow");

        this.Prompt.gameObject.SetActive(false);
        this.PromptActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PromptActive = !PromptActive;

            Prompt.gameObject.SetActive(PromptActive);
        }
    }

    public void YesButton()
    {
        Prompt.gameObject.SetActive(false);

        PromptActive = false;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void NoButton()
    {
        Prompt.gameObject.SetActive(false);

        PromptActive = false;
    }
}
