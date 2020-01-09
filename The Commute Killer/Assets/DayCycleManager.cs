using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{

    private TimeManager TimeManager;
    private Material SkyboxMaterial;
    private Light Sun;
    private SkyboxCamera SkyboxCam;
    public float blend;

    public AnimationCurve BlendCurve = AnimationCurve.Linear(0,1,1,1);

    // Start is called before the first frame update
    void Start()
    {
        this.TimeManager    = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.SkyboxMaterial = RenderSettings.skybox;

        this.Sun = GameObject.Find("Sun").GetComponent<Light>();

        this.SkyboxCam = GameObject.Find("SkyBoxCamera").GetComponent<SkyboxCamera>();

    }

    // Update is called once per frame
    void Update()
    {
        var time = this.TimeManager.GetCurrentTime();
        var hour = time.Hour;
        var min = time.Minute;
        var totalmins = hour * 60 + min;

        blend = 0;
        float rot = 0;
        if (totalmins >= 0 && totalmins < 720f)
        {
            blend = totalmins / 720f;
            rot = 180 + (180 * blend);
        }
        else
        {
            blend = 1 - (( totalmins - 720f) / 720f);
            rot = 180 - (180 * blend);
        }

        //modulate blend
        blend = BlendCurve.Evaluate(blend);

        //update skybox
        this.SkyboxMaterial.SetFloat("_Blend", blend);

        //update sun light
        float H, S, V; 
        Color.RGBToHSV(Sun.color, out H, out S, out V);
        Sun.color = Color.HSVToRGB(0.8f, 1 - blend, V);
        Sun.intensity = Mathf.Clamp(blend, 0.1f, 1);

        //update sun angle
        Sun.transform.rotation = Quaternion.Euler(90 - rot,-90,-90);

        //update skybox rotation
        this.SkyboxCam.SetSkyBoxRotation(new Vector3(0, 0, rot));

        //update enviroment
        DynamicGI.UpdateEnvironment();
    }
}
