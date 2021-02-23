using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFileConflictPanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text CloudSaveDateText;
    [SerializeField] TMP_Text LocalSaveDateText;
    [SerializeField] Animator Anim;

    //gets called by the databaseSync script to forward the lastSaved Date information for both SaveFiles (online and local)
    public void EntryInformation(string[] Data)
    {
        string CloudsaveDate = Data[1];
        string LocalSaveDate = Data[0];
        
        //Fill out information provided by the databaseSync script
        CloudSaveDateText.SetText(CloudsaveDate);
        LocalSaveDateText.SetText(LocalSaveDate);
    }

    public void CloudSaveButtonClicked()
    {
        Anim.SetTrigger("ButtonClicked");
        //something
    }

    public void LocalSaveButtonClicked()
    {
        Anim.SetTrigger("ButtonClicked");
        //something
    }
}
