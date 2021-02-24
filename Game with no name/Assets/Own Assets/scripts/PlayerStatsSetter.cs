using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsSetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text CrazyEyesKilledText;
    [SerializeField] TMP_Text ZombiesKilledText;
    [SerializeField] TMP_Text DeathsText;
    [SerializeField] TMP_Text DamageDealt;
    [SerializeField] TMP_Text ShotsFired;

    [Header("Parameters")]
    [SerializeField] float UpdateDelaySeconds = 4;

    PlayerStats PlayerStatsData;
    bool timerfinished = true;

    void updatePlayerStats()
    {
        PlayerStatsData = GetComponent<PlayerStats>();
        
        CrazyEyesKilledText.text = "CrazyEyes: " + PlayerStatsData.CrazyEyesKilled.ToString();
        ZombiesKilledText.text = "Zombies: " + PlayerStatsData.ZombiesKilled.ToString();
        DeathsText.text = "Deaths: " + PlayerStatsData.DiedCount.ToString();
        DamageDealt.text = "Damage dealt: " + PlayerStatsData.DamageDealed.ToString();
        ShotsFired.text = "Shots fired: " + PlayerStatsData.ShotsFired.ToString();
    }

    void Update()
    {
        if(timerfinished == true)
        {
            StartCoroutine(Wait(UpdateDelaySeconds));
            updatePlayerStats();
        }  
    }

    IEnumerator Wait(float time)
    {
        timerfinished = false;
        yield return new WaitForSeconds(time);
        timerfinished = true;
    }

    
    
}
