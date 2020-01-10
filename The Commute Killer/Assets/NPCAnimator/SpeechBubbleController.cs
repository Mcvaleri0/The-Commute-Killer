using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleController : LerpScaleElement
{
    private Transform SpeechBubble;
    private SpriteRenderer SpeechExpression;

    public enum Expressions
    {
        Happy,
        Sad,
        Crying,
        Cute,
        Laughing,
        Mad,
        Surprised,
    }

    public Dictionary<Expressions, Sprite> ExpressionSprites;

    private IEnumerator LastCourotine;


    // Start is called before the first frame update
    new void Start()
    {
        this.SpeechBubble = this.transform.Find("SpeechBubble");
        this.SpeechExpression = this.SpeechBubble.transform.Find("SpeechExpression").GetComponent<SpriteRenderer>();

        this.Element = this.SpeechBubble;
        this.Visible = false;
        base.Start();

        this.ExpressionSprites = new Dictionary<Expressions, Sprite>();
        this.ExpressionSprites.Add(Expressions.Happy,     Resources.Load<Sprite>("Sprites/faces/face_happy"));
        this.ExpressionSprites.Add(Expressions.Sad,       Resources.Load<Sprite>("Sprites/faces/face_sad"));
        this.ExpressionSprites.Add(Expressions.Crying,    Resources.Load<Sprite>("Sprites/faces/face_crying"));
        this.ExpressionSprites.Add(Expressions.Cute,      Resources.Load<Sprite>("Sprites/faces/face_cute"));
        this.ExpressionSprites.Add(Expressions.Laughing,  Resources.Load<Sprite>("Sprites/faces/face_laughing"));
        this.ExpressionSprites.Add(Expressions.Mad,       Resources.Load<Sprite>("Sprites/faces/face_mad"));
        this.ExpressionSprites.Add(Expressions.Surprised, Resources.Load<Sprite>("Sprites/faces/face_surprised"));
    }

    public void NewBubble(SpeechBubbleController.Expressions expression, float time)
    {
        //end current bubble if there is one
        if (this.Visible)
        {
            StopCoroutine(LastCourotine);
            this.EndBubble();
        }

        this.SpeechExpression.sprite = ExpressionSprites[expression];
        this.Visible = true;

        this.LastCourotine = EndBubbleAfterTime(time);
        StartCoroutine(this.LastCourotine);
    }


    protected IEnumerator EndBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.EndBubble();

    }


    public void EndBubble()
    {
        this.Visible = false;
    }
}
