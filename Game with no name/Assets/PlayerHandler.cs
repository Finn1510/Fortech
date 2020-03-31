using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class PlayerHandler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float PlayerHealth = 100f;
    [SerializeField] Slider HealthBar;

    float healthTemp;

    PostProcessVolume volume;
    Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxValue = PlayerHealth;
        PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);
        vignette.intensity.value = 0f;
        vignette.color.value = Color.red;
        vignette.enabled.Override(true);

    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = PlayerHealth;
        DamageEffects();
    } 

    public void Damage(float Damage)
    {
        PlayerHealth = PlayerHealth - Damage;
        
    } 

    void DamageEffects()
    {
        if(PlayerHealth < healthTemp)
        {
            healthTemp = PlayerHealth;
            if (vignette.intensity.value < 1f)
            {
                DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.5f, 1f);
            }
        }
    }
}
