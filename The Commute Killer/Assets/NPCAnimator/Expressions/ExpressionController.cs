using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionController : MonoBehaviour
{

    private GameObject Player;
    private Camera PlayerCamera;

    private SpeechBubbleController BubbleController;
    private DetectionMeterController DetectionMeter;

    // Start is called before the first frame update
    void Start()
    {
        this.Player           = GameObject.Find("PlayerCharacter");
        this.PlayerCamera     = this.Player.GetComponentInChildren<Camera>();

        this.BubbleController = this.GetComponent<SpeechBubbleController>();
        this.DetectionMeter   = this.GetComponent<DetectionMeterController>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetVector = this.transform.position - PlayerCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(targetVector, PlayerCamera.transform.rotation * Vector3.up);

    }
}
