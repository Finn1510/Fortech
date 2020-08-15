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

    float temphealth;


    void Awake()
    {
        ES3.Load<float>("PlayerHealth", PlayerHealth);    
    }

    void Start()
    {
        HealthBar.maxValue = 100f;
        HealthBar.value = PlayerHealth;
        temphealth = PlayerHealth;
       
    }

    // Update is called once per frame
    void Update()
    {
        HealthbarSmoothChange(); 
       
    } 

    public void Damage(float Damage)
    {
        PlayerHealth = PlayerHealth - Damage;
        
    } 

    void HealthbarSmoothChange()
    {
        if(temphealth != PlayerHealth)
        {
            DOTween.To(() => HealthBar.value, x => HealthBar.value = x, PlayerHealth, 0.5f);
        }    
    }

    private void OnApplicationQuit()
    {
        ES3.Save<float>("PlayerHealth", PlayerHealth);
    }

}
