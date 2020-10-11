using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CoreHandler : MonoBehaviour
{
    [Header("References")]
    CinemachineImpulseSource ImpulseGEN;
    AudioSource damageAudio;
    [Header("Parameters")]
    [SerializeField] float maxHealth = 500;

    public float Health;
    
    // Start is called before the first frame update
    void Start()
    {
        damageAudio = GetComponent<AudioSource>();
        ImpulseGEN = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount)
    {
        Health = Health - amount;
        
        damageAudio.Play();
        ImpulseGEN.GenerateImpulse(new Vector3(2, 2, 0));
        

    }

}
