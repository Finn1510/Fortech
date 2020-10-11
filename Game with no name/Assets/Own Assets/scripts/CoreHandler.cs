using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CoreHandler : MonoBehaviour
{
    [Header("References")]
    CinemachineImpulseSource ImpulseGEN; 
    [SerializeField] Slider CoreHealthSlider;
    [SerializeField] TMP_Text CoreHealthText;
    AudioSource damageAudio;
    [Header("Parameters")]
    [SerializeField] float maxHealth = 500;

    public float Health;
    bool dead = false;

    private void Awake()
    {
        Health = maxHealth;
        if (ES3.KeyExists("CoreHealth"))
        {
            Load();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        damageAudio = GetComponent<AudioSource>();
        ImpulseGEN = GetComponent<CinemachineImpulseSource>();

        CoreHealthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DOTween.To(() => CoreHealthSlider.value, x => CoreHealthSlider.value = x, Health, 0.5f);
        CoreHealthText.SetText(Health.ToString()); 
        
        if(Health <= 0)
        {
            dead = true;
            Die();
        }
    }

    public void Damage(float amount)
    {
        if(Health > 0)
        {
            Health = Health - amount;

            //damageAudio.Play(); TODO: add that back in if we hav some soundzzzzz
            ImpulseGEN.GenerateImpulse(new Vector3(2, 2, 0));
        } 
        if(Health < 0)
        {
            Health = 0;
        }
    }

    void Die()
    {
        //Do something here
    }

    void Load()
    {
        Health = ES3.Load<float>("CoreHealth");
    } 

    void Save()
    {
        ES3.Save<float>("CoreHealth", Health);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

}
