using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{

    private TimeManager TimeManager;

    public int LampState = 0;

    public AnimationCurve Flicker = AnimationCurve.Linear(0f, 1f, 1f, 0f);
    private float FlickerTime = 0; 

    private Light Light;

    private MeshRenderer Renderer;

    // Start is called before the first frame update
    void Start()
    {
        this.TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.Light = transform.Find("Spot Light").gameObject.GetComponent<Light>();

        this.Renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float eval = 0;
        var time = TimeManager.GetCurrentTime().Hour;

        switch (LampState)
        {
            //off
            case 0:
                this.Renderer.enabled = false;
                this.Light.intensity = 0;

                if (time >= 19)
                {
                    LampState = 1;
                    FlickerTime = 0;
                }
                break;
            
            //turning on
            case 1:
                FlickerTime += 2*Time.deltaTime;

                eval = Flicker.Evaluate(FlickerTime);
                this.Renderer.enabled = (eval > 0.5f) ? true : false;
                this.Light.intensity  = (eval > 0.5f) ? 2 : 0;

                if (FlickerTime >= 1)
                {
                    LampState = 2;
                }
                break;

            //on
            case 2:
                this.Renderer.enabled = true;
                this.Light.intensity = 2;

                if (time < 19 && time >= 4)
                {
                    LampState = 3;
                    FlickerTime = 1;
                }
                break;

            //turning off
            case 3:
                FlickerTime -= 2*Time.deltaTime;

                eval = Flicker.Evaluate(FlickerTime);
                this.Renderer.enabled = (eval > 0.5f) ? true : false;
                this.Light.intensity = (eval > 0.5f) ? 2 : 0;

                if (FlickerTime <= 0)
                {
                    LampState = 0;
                }

                break;
        }
    }
}
