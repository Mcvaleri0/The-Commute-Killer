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
        this.DetectionMeter = this.GetComponent<DetectionMeterController>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetVector = this.transform.position - PlayerCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(targetVector, PlayerCamera.transform.rotation * Vector3.up);


        if (Input.GetKeyDown(KeyCode.H))
        {
            this.BubbleController.NewBubble(SpeechBubbleController.Expressions.Happy, 5);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            this.BubbleController.NewBubble(SpeechBubbleController.Expressions.Sad, 3);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            this.BubbleController.NewBubble(SpeechBubbleController.Expressions.Cute, 1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            this.BubbleController.EndBubble();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            this.DetectionMeter.StartDetection();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            this.DetectionMeter.EndDetection();
        }
    }
}
