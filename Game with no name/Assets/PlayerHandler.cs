using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] float PlayerHealth = 100f;
    [SerializeField] Slider HealthBar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxValue = PlayerHealth;   
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
