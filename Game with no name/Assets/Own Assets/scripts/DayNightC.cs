using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using DG.Tweening;

public class DayNightC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light2D SunLight;

    [Header("Parameters")]
    [Range(1f, 30f)]
    [SerializeField] float DayDurationMinutes = 10f;
    [Range(1f, 30f)]
    [SerializeField] float NightDurationMinutes = 5f;

    [SerializeField] float DayLightIntensity = 1;
    [SerializeField] float NightLightIntensity = 0.2f;

    float RealDayDuration;
    float RealNightDuration;

    private void Start()
    {
        RealDayDuration = DayDurationMinutes * 60;
        RealNightDuration = NightDurationMinutes * 60;
    }

    void Day()
    {
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, DayLightIntensity, RealDayDuration);
    }

    void Night()
    {
        DOTween.To(() => SunLight.intensity, x => SunLight.intensity = x, NightLightIntensity, RealNightDuration);
    }
}
