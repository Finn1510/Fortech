using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject DebugMenuParent;
    [SerializeField] KeyCode DebugMenuActivateKey = KeyCode.O;
    [Space]
    
    [Header("TimeMultiplier")]
    [SerializeField] Slider TimeMultiplierSlider;
    [SerializeField] TMP_Text TimeMultipliertNumberText;

    bool DebugMenuActivated = false;

    void Awake()
    {
        DebugMenuParent.SetActive(false);    
    } 



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(DebugMenuActivateKey))
        {
            
            if (DebugMenuActivated == false)
            {
                
                DebugMenuParent.SetActive(true);
                DebugMenuActivated = true;
                return;
            } 
            if(DebugMenuActivated == true)
            {
                
                DebugMenuParent.SetActive(false);
                DebugMenuActivated = false; 
                return;
            } 
        }

        TimeControll();
    } 

    void TimeControll()
    {
        TimeMultipliertNumberText.SetText(Time.timeScale.ToString());
        Time.timeScale = TimeMultiplierSlider.value;
    }
}
