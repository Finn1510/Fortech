using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using DG.Tweening;
using UnityEngine.Rendering;
using System.Reflection;
using UnityEditorInternal;
using System;

public class DayNightC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light2D SunLight;

    [Header("Parameters")]
    [Range(0.1f, 10f)]
    [SerializeField] float stateDurationMinutes = 1f;
    

    [SerializeField] float DayLightIntensity = 1;
    [SerializeField] float NightLightIntensity = 0.2f;
    [SerializeField] string WorldStartState = "State3";


    float RealStateDuration;
    int StateIndex;
    string methodName;

    private void Start()
    {
        RealStateDuration = stateDurationMinutes * 60;

        if(ES3.KeyExists("currentStateIndex") == true)
        {
            StateIndex = ES3.Load<int>("currentStateIndex");
            if (StateIndex != 10)
            {
                methodName = "State" + StateIndex;
            }
            else
            {
                methodName = "State1";
            }

            
        }

        else
        {
            methodName = WorldStartState;
        }

        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(methodName);
        theMethod.Invoke(this, null);

    }


    public void State1()
    {
        Debug.Log("State1");
        StateIndex = 1;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.36f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State2()
    {
        Debug.Log("State2");
        StateIndex = 2;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.52f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State3()
    {
        Debug.Log("State3");
        StateIndex = 3;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.68f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State4()
    {
        Debug.Log("State4");
        StateIndex = 4;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.84f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State5()
    {
        Debug.Log("State5");
        StateIndex = 5;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 1, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State6()
    {
        Debug.Log("State6");
        StateIndex = 6;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.84f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State7()
    {
        Debug.Log("State7");
        StateIndex = 7;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.68f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State8()
    {
        Debug.Log("State8");
        StateIndex = 8;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.52f, RealStateDuration);
        StartCoroutine(StateWait());
    }

    public void State9()
    {
        Debug.Log("State9");
        StateIndex = 9;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.36f, RealStateDuration);
        StartCoroutine(StateWait());
    }


    public void State10()
    {
        Debug.Log("State10");
        StateIndex = 10;
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, 0.2f, RealStateDuration);
        StartCoroutine(StateWait());
    }
    
    
    IEnumerator StateWait()
    {
        yield return new WaitForSeconds(RealStateDuration);
        
        if(StateIndex != 10)
        {
            methodName = "State" + (StateIndex + 1 );
        } 
        else
        {
            methodName = "State1";
        }

        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(methodName);
        theMethod.Invoke(this, null);
    }  

    void Save()
    {
        ES3.Save<int>("currentStateIndex", StateIndex);
    }

    private void OnApplicationQuit()
    {
        Save();
    }


}
