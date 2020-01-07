using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private AudioClip ClickSound;
    private AudioClip HoverSound;

    private AudioSource AudioSource;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //audio source 
        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;
        this.AudioSource.volume = 0.2f;

        this.ClickSound = (AudioClip)Resources.Load("Audio/UI_click");
        this.HoverSound = (AudioClip)Resources.Load("Audio/UI_hover");
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("City", LoadSceneMode.Single);
        this.AudioSource.PlayOneShot(ClickSound);
    }

    public void QuitButton()
    {
        Application.Quit();
        this.AudioSource.PlayOneShot(ClickSound);
    }

    public void HoverButton()
    {
        this.AudioSource.PlayOneShot(HoverSound);
    }
}
