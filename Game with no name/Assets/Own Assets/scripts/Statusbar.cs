using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; 


public class Statusbar : MonoBehaviour
{

    [SerializeField] databaseSync Databasesystem;
    [SerializeField] TMP_Text Statustext;
    [SerializeField] Image StatusbarPanel;
    
    // Update is called once per frame
    void Update()
    {
        //TODO fix having thousands of Tweens cuz its every frame
        switch (Databasesystem.StatusID)
        {
            case 0:
                Statustext.SetText("Connecting to Database");
                StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 2).From();
                break;
            case 1:
                Statustext.SetText("Connected to Database");
                StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 2).From();
                break;
            case 2:
                Statustext.SetText("logging in");
                StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 2).From();
                break;
            case 3:
                Statustext.SetText("logged in");
                StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 2).From();
                break;
            case 4:
                Statustext.SetText("Syncing");
                StatusbarPanel.DOColor(new Color32(0, 225, 231, 255), 2).From();
                break;
            case 5:
                Statustext.SetText("Synced");
                StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 2).From();
                break;
            case 6:
                Statustext.SetText("Registering");
                StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 2).From();
                break;
            case 7:
                Statustext.SetText("Registered");
                StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 2).From();
                break;
        }
    }
}
