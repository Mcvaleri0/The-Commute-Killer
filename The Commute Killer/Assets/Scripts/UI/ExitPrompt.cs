using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPrompt : MonoBehaviour
{
    private Transform Prompt;
    private Button Yes;
    private Button No;

    private bool PromptActive;

    // Start is called before the first frame update
    void Start()
    {
        this.Prompt = transform.Find("ExitPromptWindow");

        Button[] buttonObjects = GetComponentsInChildren<Button>();

        this.No  = buttonObjects[0];
        this.Yes = buttonObjects[1];

        this.No.onClick.AddListener(()  => NoButton());
        this.Yes.onClick.AddListener(() => YesButton());

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
