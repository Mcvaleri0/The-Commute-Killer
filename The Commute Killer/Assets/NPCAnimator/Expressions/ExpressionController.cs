using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionController : MonoBehaviour
{

    private GameObject Player;

    private Camera PlayerCamera;

    private GameObject SpeechBubble;
    private GameObject SpeechExpression;

    public bool  Visible        = true;
    public float RenderDistance = 5.0f;

    public int VisibilityState = 0;
    private bool FinishedScaling = false;


    // Start is called before the first frame update
    void Start()
    {
        this.Player           = GameObject.Find("PlayerCharacter");
        this.PlayerCamera     = this.Player.GetComponentInChildren<Camera>();
        this.SpeechBubble     = this.transform.Find("SpeechBubble").gameObject;
        this.SpeechExpression = this.transform.Find("SpeechExpression").gameObject;

        this.SpeechExpression.transform.localScale = Vector3.zero;
        this.SpeechBubble.transform.localScale     = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 targetVector = this.transform.position - PlayerCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(targetVector, PlayerCamera.transform.rotation * Vector3.up);
        
        var dist = Vector3.Distance(this.Player.transform.position, this.transform.position);
        var inRange = (dist <= RenderDistance);
        
        switch (VisibilityState)
        {
            //Visible
            case 0:

                if (!inRange || !this.Visible)
                {
                    Hide();
                    VisibilityState = 1;
                }
                break;

            //going to Hide
            case 1:

                if (this.FinishedScaling)
                {
                    this.FinishedScaling = false;
                    VisibilityState = 2;
                }
                break;

            //Hidden
            case 2:

                if(inRange && this.Visible)
                {
                    Show();
                    VisibilityState = 3;
                }
                break;

            //going to Visible
            case 3:

                if (this.FinishedScaling)
                {
                    this.FinishedScaling = false;
                    VisibilityState = 0;
                }
                break;
        }
    }


    // hide
    protected void Hide()
    {
        StartCoroutine(ScaleLerp(this.SpeechBubble.transform, Vector3.zero, 10));
    }


    // show
    protected void Show()
    {
        StartCoroutine(ScaleLerp(this.SpeechBubble.transform, Vector3.one, 10));
    } 


    // Lerp Scale
    private IEnumerator ScaleLerp(Transform transform, Vector3 targetScale, float speed)
    {
        var scale = transform.localScale;

        while (Vector3.Distance(scale, targetScale) > 0.1)
        {
            scale = Vector3.Lerp(scale, targetScale, Time.deltaTime * speed);
            transform.localScale = scale;
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = targetScale;
        this.FinishedScaling = true;
    }
}
