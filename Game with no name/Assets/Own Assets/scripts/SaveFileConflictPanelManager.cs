using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveFileConflictPanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text CloudSaveDateText;
    [SerializeField] TMP_Text LocalSaveDateText;
    [SerializeField] Button CloudSaveFileButton;
    [SerializeField] Button LocalSaveFileButton;

    [SerializeField] Animator Anim;

    //Enables the Butoons again when the GameObject gets enabled again
    void OnEnable()
    {
        changeButtons(true);  
    }

    //gets called by the databaseSync script to forward the lastSaved Date information for both SaveFiles (online and local)
    public void EntryInformation(string[] Data)
    {
        string CloudsaveDate = Data[1];
        string LocalSaveDate = Data[0];
        
        //Fill out information provided by the databaseSync script
        CloudSaveDateText.SetText(CloudsaveDate);
        LocalSaveDateText.SetText(LocalSaveDate);
    }
    
    //gets called when the cloud save button has been clicked
    public void CloudSaveButtonClicked()
    {
        changeButtons(false);
        Anim.SetTrigger("ButtonClicked");
        GameObject.FindGameObjectWithTag("DatabaseManager").GetComponent<databaseSync>().SendMessage("SetSaveFileConflictDecision", 1);
    }

    //gets called when the local save button has been clicked
    public void LocalSaveButtonClicked()
    {
        changeButtons(false);
        Anim.SetTrigger("ButtonClicked");
        GameObject.FindGameObjectWithTag("DatabaseManager").GetComponent<databaseSync>().SendMessage("SetSaveFileConflictDecision", 2);
    }

    //Enables/Disables Buttons to prevent them from beeing clicked (even tho they are like 1pixel wide after the animation)
    void changeButtons(bool change)
    {
        CloudSaveFileButton.interactable = change;
        LocalSaveFileButton.interactable = change;
    }

}
