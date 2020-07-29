using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class player_movement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float runSpeed = 40f;
    [SerializeField] float Health = 100;
    float Horizontalmove = 0f;

    [Space]

    [Header("References")]
    [SerializeField] CharacterController2D controller;
    [SerializeField] Text Healthtext;
    [SerializeField] Slider HealthSlider;
    CinemachineImpulseSource ImpulseGEN;

    bool Jump = false;

    void Start()
    {
        Healthtext.text = Health.ToString();
        HealthSlider.maxValue = Health;
        HealthSlider.value = Health;

        ImpulseGEN = GetComponent<CinemachineImpulseSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed; 
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(Horizontalmove * Time.fixedDeltaTime , false, Jump);
        Jump = false;
    } 

    public void Damage (float amount)
    {
        Health = Health - amount;
        Healthtext.text = Health.ToString();
        HealthSlider.value = Health;
        ImpulseGEN.GenerateImpulse(new Vector3(2, 2, 0));
    }

}
