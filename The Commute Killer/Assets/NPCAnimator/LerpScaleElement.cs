using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpScaleElement : MonoBehaviour
{
    protected Transform Element;

    protected GameObject Player;
    public float RenderDistance = 5.0f;

    public bool Visible = true;
    protected int VisibilityState = 0;
    protected bool FinishedScaling = false;

    // Start is called before the first frame update
    protected void Start()
    {
        this.Element.transform.localScale = Vector3.zero;
        this.Player = GameObject.Find("PlayerCharacter");
    }

    protected void Update()
    {
        var dist = Vector3.Distance(this.Player.transform.position, this.transform.position);
        var inRange = (dist <= RenderDistance);

        switch (VisibilityState)
        {
            //Visible
            case 0:

                if (!inRange || !this.Visible)
                {
                    Hide(this.Element);
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

                if (inRange && this.Visible)
                {
                    Show(this.Element);
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
    protected void Hide(Transform transf)
    {
        StartCoroutine(ScaleLerp(transf, Vector3.zero, 10));
    }


    // show
    protected void Show(Transform transf)
    {
        StartCoroutine(ScaleLerp(transf, Vector3.one, 10));
    }


    // Lerp Scale
    protected IEnumerator ScaleLerp(Transform transform, Vector3 targetScale, float speed)
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
