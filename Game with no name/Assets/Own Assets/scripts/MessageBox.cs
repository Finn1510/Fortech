using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;

public class MessageBox : MonoBehaviour
{
    public MessageBoxScriptableObject messageboxContent;
    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text message;
    [SerializeField] Animator MessageBoxAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Title.text = messageboxContent.Title;
        message.text = messageboxContent.messageText;
    } 

    public void ExitButtonClick()
    {
        MessageBoxAnimator.SetTrigger("MessageBoxExit");
        StartCoroutine(DestroyDelay());
    }

    //delete Popup window after 5 seconds
    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
