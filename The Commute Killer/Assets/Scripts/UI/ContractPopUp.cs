using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractPopUp : MonoBehaviour
{

    private AudioClip OpenSound;
    private AudioClip CloseSound;

    private AudioSource AudioSource;

    public RectTransform contract;
    private int state = 0;


    private void Start()
    {
        this.AudioSource = gameObject.AddComponent<AudioSource>();
        this.AudioSource.playOnAwake = false;

        this.OpenSound = (AudioClip)Resources.Load("Audio/contract_open");
        this.CloseSound = (AudioClip)Resources.Load("Audio/contract_close");
    }

    // Start is called before the first frame update
    void Update()
    {
        switch (state)
        {
            case 0:
            break;

            //open
            case 1:
                contract.gameObject.SetActive(true);
                state = 0;

                this.AudioSource.PlayOneShot(OpenSound);
                break;

            //close
            case 2:
                contract.gameObject.SetActive(false);
                state = 0;

                this.AudioSource.PlayOneShot(CloseSound);
                break;


        }
    }

    void OnMouseDown()
    {
        state = 1;
    }

    void OnMouseUp()
    {
        state = 2;
    }
}
