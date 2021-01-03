using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxManager : MonoBehaviour
{
    [SerializeField] GameObject ErrorMessageBoxPrefab;
    [SerializeField] GameObject RegularMessageBoxPrefab; 

    public void ErrorMessageBox(MessageBoxScriptableObject messageContent)
    {
        GameObject MessageBox = Instantiate(ErrorMessageBoxPrefab, Vector3.zero, Quaternion.identity);
        MessageBox.GetComponent<MessageBox>().messageboxContent = messageContent;
    }

    public void MessageBox(MessageBoxScriptableObject messageContent)
    {
        GameObject MessageBox = Instantiate(RegularMessageBoxPrefab, Vector3.zero, Quaternion.identity);
        MessageBox.GetComponent<MessageBox>().messageboxContent = messageContent;
    }
}
