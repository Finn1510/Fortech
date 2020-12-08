using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsSetter : MonoBehaviour
{
    [SerializeField] TMP_Text CrazyEyesKilledText;
    [SerializeField] TMP_Text ZombiesKilledText;
    [SerializeField] TMP_Text DeathsText;
    [SerializeField] TMP_Text DamageDealt;
    [SerializeField] TMP_Text ShotsFired;

    PlayerStats PlayerStatsData;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStatsData = GetComponent<PlayerStats>();

        CrazyEyesKilledText.text = "CrazyEyes: " + PlayerStatsData.CrazyEyesKilled.ToString();
        ZombiesKilledText.text = "Zombies: " + PlayerStatsData.ZombiesKilled.ToString();
        DeathsText.text = "Deaths: " + PlayerStatsData.DiedCount.ToString();
        DamageDealt.text = "Damage dealt: " + PlayerStatsData.DamageDealed.ToString();
        ShotsFired.text = "Shots fired: " + PlayerStatsData.ShotsFired.ToString();
    }

    
    
}
