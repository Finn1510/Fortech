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

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxValue = PlayerHealth;
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
   
}
