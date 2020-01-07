using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPrompt : MonoBehaviour
{
    private Transform Prompt;

    private bool PromptActive;

    private AudioClip ClickSound;
    private AudioClip HoverSound;
    private AudioClip SnapSound;

    private AudioSource AudioSource;

    private AmbientAudioManager Ambient;

    // Start is called before the first frame update
    void Start()
    {
        this.Prompt = transform.Find("ExitPromptWindow");

        this.Prompt.gameObject.SetActive(false);
        this.PromptActive = false;

        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;
        this.AudioSource.volume = 0.2f;

        this.ClickSound = (AudioClip)Resources.Load("Audio/UI_click");
        this.HoverSound = (AudioClip)Resources.Load("Audio/UI_hover");
        this.SnapSound = (AudioClip)Resources.Load("Audio/UI_snap");

        Ambient = GameObject.Find("AudioManager").GetComponent<AmbientAudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    PromptActive = !PromptActive;

        //    Prompt.gameObject.SetActive(PromptActive);
        //}
    }

    public void Draw()
    {
        this.PromptActive = true;
        Prompt.gameObject.SetActive(true);

        this.AudioSource.PlayOneShot(SnapSound);
    }

    public void Hide()
    {
        this.PromptActive = false;
        Prompt.gameObject.SetActive(false);

        this.AudioSource.PlayOneShot(SnapSound);
    }

    public void YesButton()
    {
        this.Hide();
        this.AudioSource.PlayOneShot(ClickSound);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void NoButton()
    {
        this.Hide();
        this.AudioSource.PlayOneShot(ClickSound);
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UnPause();

    }

    public void HoverButton()
    {
        this.AudioSource.PlayOneShot(HoverSound);
    }
}
