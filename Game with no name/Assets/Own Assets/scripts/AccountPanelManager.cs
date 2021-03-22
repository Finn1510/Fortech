using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using TMPro;
using UnityEngine.UI;

public class AccountPanelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] databaseSync DatabaseSystem;
    [Space]
    [SerializeField] GameObject Panel;
    [SerializeField] Camera InputCamera;
    [SerializeField] RawImage RawImageOverlay;
    [SerializeField] TMP_InputField UsernameInputfield;
    [SerializeField] TMP_InputField PasswordInputfield;
    [SerializeField] Animator PanelAnimator;
    [SerializeField] AudioSource PanelAudioSource;
    [Space]
    [SerializeField] AudioClip PanelEntryAudio;
    [SerializeField] AudioClip PanelExitAudio;
    [Space]
    [SerializeField] Button LoginButton;
    [SerializeField] Button RegisterButton;
    
    private bool MouseOverPanel = false;
    private bool MouseOverUsernameInputfield = false;
    private bool MouseOverPasswordInputfield = false;
    private bool MouseOverLoginButton = false;
    private bool MouseOverRegisterButton = false;
    private bool pastValue = false;

    private void Start()
    {
        AdjustRenderTextureToScreenRes();
    }

    void AdjustRenderTextureToScreenRes()
    {
        if (InputCamera.targetTexture != null)
        {
            InputCamera.targetTexture.Release();
        }
         
        RenderTexture adjustedRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        
        InputCamera.targetTexture = adjustedRenderTexture;
        Panel.GetComponent<Image>().material.SetTexture("_BlurTex", adjustedRenderTexture);
        RawImageOverlay.texture = adjustedRenderTexture;
    }

    public void LoginButtonClicked()
    {
        string Username = UsernameInputfield.text;
        string Password = PasswordInputfield.text;

        DatabaseSystem.ExecuteLogin(Username, Password);
    }

    public void RegisterButtonClicked()
    {
        string Username = UsernameInputfield.text;
        string Password = PasswordInputfield.text;

        DatabaseSystem.ExecuteRegister(Username, Password);
    }

    //This is a terrible way of doing this but I couldn't think of anything diffrent because the panel 
    //always thought it has to close when the pointer is over a button and not over the panel

    private void Update()
    {
        if(UsernameInputfield.text == "")
        {
            LoginButton.interactable = false;
            RegisterButton.interactable = false;
        }
        else if(PasswordInputfield.text == "")
        {
            LoginButton.interactable = false;
            RegisterButton.interactable = false;
        }
        else
        {
            LoginButton.interactable = true;
            RegisterButton.interactable = true;
        }
        
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
