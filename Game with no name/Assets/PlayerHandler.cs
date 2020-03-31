using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHandler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float PlayerHealth = 100f;
    [SerializeField] Slider HealthBar;

    PostProcessVolume volume;
    Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxValue = PlayerHealth;
        PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);

    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = PlayerHealth;    
    } 

    public void Damage(float Damage)
    {
        PlayerHealth = PlayerHealth - Damage;
    }
}
