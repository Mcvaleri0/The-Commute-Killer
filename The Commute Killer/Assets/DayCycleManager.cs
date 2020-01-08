using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{

    private TimeManager TimeManager;
    private Material SkyboxMaterial;
    private Light Sun;
    public float blend;

    // Start is called before the first frame update
    void Start()
    {
        this.TimeManager    = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        this.SkyboxMaterial = RenderSettings.skybox;

        this.Sun = GameObject.Find("Sun").GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        var time = this.TimeManager.GetCurrentTime();
        var hour = time.Hour;
        var min = time.Minute;

        var totalmins = hour * 60 + min;

        blend = 0;
        if(totalmins >= 0 && totalmins < 720f)
        {
            blend = totalmins / 720f;
        }
        else if (totalmins >= 720f && totalmins < 1440f)
        {
            blend = 1 - (( totalmins - 720f) / 720f);
        }
        else
        {
            blend = 0f;
        }

        //update skybox
        this.SkyboxMaterial.SetFloat("_Blend", blend);

        //update sun light
        float H, S, V; 
        Color.RGBToHSV(Sun.color, out H, out S, out V);
        Sun.color = Color.HSVToRGB(0.8f, 1 - blend, V);
        Sun.intensity = Mathf.Clamp(blend, 0.1f, 1);

        //update enviroment
        DynamicGI.UpdateEnvironment();
    }
}
