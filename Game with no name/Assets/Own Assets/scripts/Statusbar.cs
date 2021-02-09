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
    [SerializeField] Rigidbody2D LoadingBob;
    [SerializeField] Transform Checkmark;
    Vector3 CheckmarkScale = new Vector3(0.066f, 0.066f, 0.066f);
    [Space]

    [Header("Colors")]
    [SerializeField] Color32 ConnectingToDatabaseColor = new Color32(231, 207, 0, 255);
    [SerializeField] Color32 ConnectedToDatabaseColor = new Color32(56, 169, 39, 255);
    [SerializeField] Color32 LoggingInColor = new Color32(231, 207, 0, 255);
    [SerializeField] Color32 LoggedInColor = new Color32(56, 169, 39, 255);
    [SerializeField] Color32 SyncingColor = new Color32(0, 225, 231, 255);
    [SerializeField] Color32 SyncedColor = new Color32(56, 169, 39, 255);
    [SerializeField] Color32 RegisteringColor = new Color32(231, 207, 0, 255);
    [SerializeField] Color32 RegisteredColor = new Color32(56, 169, 39, 255);


    //same principle as StatusID this is used to check if we already tweened the color for this statusID
    int TweenStatus;

    void Start()
    {
        Checkmark.transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (Databasesystem.StatusID)
        {
            case 0:
                Statustext.SetText("Connecting to Database");
                SetLoadingBobRotationVelocity(500);
                if (TweenStatus != 0)
                {
                    //StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 1).From();
                    SwitchToLoadingBob();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, ConnectingToDatabaseColor, 1);
                    TweenStatus = 0;
                }
                break;
            case 1:
                Statustext.SetText("Connected to Database");
                if (TweenStatus != 1)
                {
                    //StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 1).From();
                    SwitchToCheckmark();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, ConnectedToDatabaseColor, 1);
                    TweenStatus = 1;
                    StopLoadingBob();
                }
                break;
            case 2:
                Statustext.SetText("logging in");
                SetLoadingBobRotationVelocity(500);
                if (TweenStatus != 2)
                {
                    //StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 1).From();
                    SwitchToLoadingBob();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, LoggingInColor, 1);
                    TweenStatus = 2;
                }
                break;
            case 3:
                Statustext.SetText("logged in");
                if (TweenStatus != 3)
                {
                    //StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 1).From();
                    SwitchToCheckmark();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, LoggedInColor, 1);
                    TweenStatus = 3;
                    StopLoadingBob();
                }
                break;
            case 4:
                Statustext.SetText("Syncing");
                SetLoadingBobRotationVelocity(500);
                if (TweenStatus != 4)
                {
                    //StatusbarPanel.DOColor(new Color32(0, 225, 231, 255), 1).From();
                    SwitchToLoadingBob();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, SyncingColor, 1);
                    TweenStatus = 4;
                }
                break;
            case 5:
                Statustext.SetText("Synced");
                if (TweenStatus != 5)
                {
                    //StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 1).From();
                    SwitchToCheckmark();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, SyncedColor, 1);
                    TweenStatus = 5;
                    StopLoadingBob();
                }
                break;
            case 6:
                Statustext.SetText("Registering");
                SetLoadingBobRotationVelocity(500);
                if (TweenStatus != 6)
                {
                    //StatusbarPanel.DOColor(new Color32(231, 207, 0, 255), 1).From();
                    SwitchToLoadingBob();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, RegisteringColor, 1);
                    TweenStatus = 6;
                }
                break;
            case 7:
                Statustext.SetText("Registered");
                if (TweenStatus != 7)
                {
                    //StatusbarPanel.DOColor(new Color32(56, 169, 39, 255), 1).From();
                    SwitchToCheckmark();
                    DOTween.To(() => StatusbarPanel.color, x => StatusbarPanel.color = x, RegisteredColor, 1);
                    TweenStatus = 7;
                    StopLoadingBob();
                }
                break;
        } 
        
        void SetLoadingBobRotationVelocity(float amount)
        {
            LoadingBob.angularVelocity = amount;
        }

        void StopLoadingBob()
        {
            DOTween.To(() => LoadingBob.angularVelocity, x => LoadingBob.angularVelocity = x, 0f, 1);
        }

        void SwitchToCheckmark()
        {
            if(Checkmark.localScale == Vector3.zero)
            {
                LoadingBob.transform.DOScale(0, 0.1f).SetEase(Ease.InOutCubic);
                Checkmark.DOScale(CheckmarkScale, 1f).SetEase(Ease.OutElastic);
            }
        }

        void SwitchToLoadingBob()
        {
            if (Checkmark.localScale != Vector3.zero)
            {
                Checkmark.DOScale(0, 0.1f).SetEase(Ease.InOutCubic);
                LoadingBob.transform.DOScale(1, 0.5f).SetEase(Ease.InOutCubic);
            }
            
        }
        
    }
}
