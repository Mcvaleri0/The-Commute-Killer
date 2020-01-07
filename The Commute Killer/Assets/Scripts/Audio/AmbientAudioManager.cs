using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioManager : MonoBehaviour
{
    static private Dictionary<string, AudioClip> AmbientSounds;

    private GameObject Player;

    private AudioSource AmbientAudioSource;

    private List<Collider> AmbientColliders;

    private AudioClip currentAmbientClip;


    // Start is called before the first frame update
    void Start()
    {
        //player
        Player = GameObject.Find("PlayerCharacter");

        //Ambient Sound Source
        AmbientAudioSource = Player.GetComponent<AudioSource>();

        //sounds dictionary
        AmbientSounds = new Dictionary<string, AudioClip>()
        {
            ["Default"] = (AudioClip)Resources.Load("Audio/Ambient_outside_street"),
            ["Office"] = (AudioClip)Resources.Load("Audio/Ambient_inside_office"),
        };

        //current clip
        currentAmbientClip = AmbientSounds["Default"];

        //get all Ambients
        AmbientColliders = new List<Collider>();

        var ambientBounds = transform.Find("AmbientBounds");
        if (ambientBounds != null)
        {
            foreach (Transform child in ambientBounds.transform)
            {
                var gameObj = child.gameObject;
                var collider = gameObj.GetComponent<BoxCollider>();

                if (collider != null)
                {
                    AmbientColliders.Add(collider);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        var newClip = this.GetCurrentAmbient();

        if (this.currentAmbientClip != newClip)
        {
            this.currentAmbientClip = newClip;

            //play sound
            this.AmbientAudioSource.clip = this.currentAmbientClip;
            this.AmbientAudioSource.Play();
        }
    }

    protected AudioClip GetCurrentAmbient()
    {
        foreach (Collider collider in AmbientColliders)
        {
            if (collider.bounds.Contains(Player.transform.position))
            {
                return AmbientSounds[collider.name];
            }
        }

        return AmbientSounds["Default"];

    }

    public void setVolume(float volume)
    {
        this.AmbientAudioSource.volume = volume;
        this.AmbientAudioSource.Play();
    }

}

