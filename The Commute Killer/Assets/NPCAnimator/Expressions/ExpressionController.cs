using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionController : MonoBehaviour
{

    private GameObject Player;

    private Camera PlayerCamera;

    private GameObject SpeechBubble;

    private bool Render         = false;
    public bool Visible        = true;
    public float RenderDistance = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("PlayerCharacter");
        this.PlayerCamera = this.Player.GetComponentInChildren<Camera>();
        this.SpeechBubble = this.transform.Find("SpeechBubble").gameObject;

        this.SpeechBubble.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetVector = this.transform.position - PlayerCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(targetVector, PlayerCamera.transform.rotation * Vector3.up);

        //show
        if (Vector3.Distance(this.Player.transform.position, this.transform.position) <= RenderDistance && !this.Render)
        {
            StartCoroutine(ScaleLerp(this.SpeechBubble.transform, Vector3.one, 10));
            this.Render = true;
        }
        //hide
        if (Vector3.Distance(this.Player.transform.position, this.transform.position) > RenderDistance && this.Render)
        {
            StartCoroutine(ScaleLerp(this.SpeechBubble.transform, Vector3.zero, 10));
            this.Render = false;
        }
    }


    // Lerp Scale
    private IEnumerator ScaleLerp(Transform transform, Vector3 targetScale, float speed)
    {
        var _deltaTime = Time.deltaTime;
        var scale = transform.localScale;
        while (Vector3.Distance(scale, targetScale) > 0.1)
        {
            scale = Vector3.Lerp(scale, targetScale, _deltaTime * speed);
            transform.localScale = scale;
            yield return new WaitForSeconds(_deltaTime);
        }

        transform.localScale = targetScale;
    }

}
