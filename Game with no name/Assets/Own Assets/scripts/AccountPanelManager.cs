using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using TMPro;

public class AccountPanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] databaseSync DatabaseSystem;
    [Space]
    [SerializeField] TMP_InputField UsernameInputfield;
    [SerializeField] TMP_InputField PasswordInputfield;
    [SerializeField] Animator PanelAnimator;
    [SerializeField] AudioSource PanelAudioSource;
    [Space]
    [SerializeField] AudioClip PanelEntryAudio;
    [SerializeField] AudioClip PanelExitAudio;
    
    private bool MouseOverPanel = false;
    private bool MouseOverUsernameInputfield = false;
    private bool MouseOverPasswordInputfield = false;
    private bool MouseOverLoginButton = false;
    private bool MouseOverRegisterButton = false;
    private bool pastValue = false;
    
    //This is a terrible way of doing this but I couldn't think of anything diffrent because the panel 
    //always thought it has to close when the pointer is over a button and not over a panel

    public void LoginButtonClicked()
    {
        string Username = UsernameInputfield.text;
        string Password = PasswordInputfield.text;

        DatabaseSystem.Login(Username, Password);
    }

    public void RegisterButtonClicked()
    {
        string Username = UsernameInputfield.text;
        string Password = PasswordInputfield.text;

        DatabaseSystem.Register(Username, Password);
    }

    private void Update()
    {
        if(MouseOverPanel == true)
        {
            PanelAnimator.SetBool("MouseOverPanel", true);
            
        }
        else
        {
            if(MouseOverUsernameInputfield == true)
            {
                PanelAnimator.SetBool("MouseOverPanel", true);
            }
            else
            {
                if(MouseOverPasswordInputfield == true)
                {
                    PanelAnimator.SetBool("MouseOverPanel", true);
                }
                else
                {
                    if(MouseOverLoginButton == true)
                    {
                        PanelAnimator.SetBool("MouseOverPanel", true);
                    }
                    else
                    {
                        if(MouseOverRegisterButton == true)
                        {
                            PanelAnimator.SetBool("MouseOverPanel", true);
                        }
                        else
                        {
                            PanelAnimator.SetBool("MouseOverPanel", false);
                        }
                    }
                }
            }
        }

        //make diffrent clicking sounds when panel is opening and closing
        if(PanelAnimator.GetBool("MouseOverPanel") != pastValue)
        {
            if(PanelAnimator.GetBool("MouseOverPanel") == true)
            {
                PanelAudioSource.panStereo = 0.3f;
                PanelAudioSource.PlayOneShot(PanelEntryAudio); 
            }
            if (PanelAnimator.GetBool("MouseOverPanel") == false)
            {
                PanelAudioSource.panStereo = -0.3f;
                PanelAudioSource.PlayOneShot(PanelExitAudio);
            }
            
            pastValue = PanelAnimator.GetBool("MouseOverPanel");
        }
    
    
    }

    //__Functions for recieving Events from UI__

    public void MouseOverPanelTrue()
    {
        MouseOverPanel = true;
    } 

    public void MousOverPanelFalse()
    {
        MouseOverPanel = false;
    } 

    public void MouseOverUsernameInputFieldTrue()
    {
        MouseOverUsernameInputfield = true;   
    }

    public void MouseOverUsernameInputFieldFalse()
    {
        MouseOverUsernameInputfield = false;
    }

    public void MouseOverPasswordInputFieldTrue()
    {
        MouseOverPasswordInputfield = true;
    }

    public void MouseOverPasswordInputFieldFalse()
    {
        MouseOverPasswordInputfield = false;
    } 

    public void MouseOverLoginButtonTrue()
    {
        MouseOverLoginButton = true;
    }

    public void MouseOverLoginButtonFalse()
    {
        MouseOverLoginButton = false;
    }

    public void MouseOverRegisterButtonTrue()
    {
        MouseOverRegisterButton = true;
    }

    public void MouseOverRegisterButtonFalse()
    {
        MouseOverRegisterButton = false;
    }
}
