using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new MessageBox" , menuName = "MessageBox")]
public class MessageBoxScriptableObject : ScriptableObject
{
    public string Title;
    public string messageText;
}
