using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class player_movement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float runSpeed = 40f;
    [SerializeField] float Health = 100;
    [SerializeField] float VignetteIntensity = 0.25f;
    [SerializeField] float VignetDuration = 0.2f;
    float Horizontalmove = 0f;

    [Space]

    [Header("References")]
    [SerializeField] CharacterController2D controller;
    [SerializeField] Text Healthtext;
    [SerializeField] Slider HealthSlider;
    [SerializeField] PostProcessVolume DiedPostProcess;
    [SerializeField] PostProcessVolume PostProcessing;
    [SerializeField] GameObject YouDiedtext;
    CinemachineImpulseSource ImpulseGEN;
    Rigidbody2D rigid;

    bool Jump = false;
    bool Playerdied = false;
    bool DiedMessageSent = false;
    bool VignetCoroutineDelayACTIVE = false;
    
    //Post process vars
    ColorGrading ColorGrade = null;
    Vignette Vignet = null;

    void Start()
    {
        Healthtext.text = Health.ToString();
        HealthSlider.maxValue = Health;
        HealthSlider.value = Health;

        DiedPostProcess.sharedProfile.TryGetSettings<ColorGrading>(out ColorGrade);
        PostProcessing.sharedProfile.TryGetSettings<Vignette>(out Vignet);

        ImpulseGEN = GetComponent<CinemachineImpulseSource>();
        rigid = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed; 
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }

        
        if (Health <= 0f)
        {
            Playerdied = true; 
            if(DiedMessageSent == false)
            {
                GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (GameObject go in gos)
                {
                    if (go && go.transform.parent == null)
                    {
                        go.gameObject.BroadcastMessage("PlayerDied", SendMessageOptions.DontRequireReceiver);
                    }
                }

                YouDiedtext.active = true;
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.00001f, 1f);
                
                DOTween.To(() => DiedPostProcess.weight, x => DiedPostProcess.weight = x, 1, 0.5f);
                DiedMessageSent = true;

            }

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
        DOTween.To(() => Vignet.intensity.value, x => Vignet.intensity.value = x, VignetteIntensity, 0.2f);
        StartCoroutine(DamageVignetDelay());
        
        
    } 

    public void applyKnockback(Hashtable KnockbackDATA)
    {
        Vector3 Knockbackdirection = (Vector3)KnockbackDATA["Direction"];
        
        if (Knockbackdirection.x < transform.position.x)
        {
            rigid.AddForce(new Vector3(10, 2) * (float)KnockbackDATA["Strengh"]);
        }
        else
        {
            rigid.AddForce(new Vector3(-10, 2) * (float)KnockbackDATA["Strengh"]);
        }

    } 

    IEnumerator DamageVignetDelay()
    {
        if(VignetCoroutineDelayACTIVE == false)
        {
            VignetCoroutineDelayACTIVE = true;
            
            yield return new WaitForSeconds(VignetDuration);
            DOTween.To(() => Vignet.intensity.value, x => Vignet.intensity.value = x, 0, 0.2f);
            VignetCoroutineDelayACTIVE = false;
        }
        
    }

}
