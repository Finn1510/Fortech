using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;

public class AccountPanelManager : MonoBehaviour
{
    [SerializeField] Animator PanelAnimator;
    private bool MouseOverPanel = false;
    
    public void MouseOverPanelTrue()
    {
        MouseOverPanel = true;
        Debug.Log("MouseOverUI");
        PanelAnimator.SetBool("MouseOverPanel", true);
    } 

    public void MousOverPanelFalse()
    {
        MouseOverPanel = false;
        Debug.Log("MouseNotOverUI");
        PanelAnimator.SetBool("MouseOverPanel", false);
    }
}
