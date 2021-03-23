using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GameStateManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Animator PauseMenuAnim;
    [SerializeField] KeyCode PauseKey = KeyCode.Escape;
    [SerializeField] GameObject Ui_Inventory;

    [Header("Parameters")]
    [SerializeField] float InventoryInputDelaySeconds = 0.25f;

    bool GamePaused = false;
    bool DelayDone = true;
    
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseKey ) && Ui_Inventory.active == false)
        {
            if (GamePaused == false && DelayDone == true)
            {
                StartCoroutine(Delay(InventoryInputDelaySeconds));
                PauseGame();
                GamePaused = true;
            }
            else if (GamePaused == true && DelayDone == true)
            {
                StartCoroutine(Delay(InventoryInputDelaySeconds));
                GamePaused = false;
                ResumeGame();
            }
        }   
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        PauseMenuAnim.SetBool("GamePaused", true);
        Time.timeScale = 0;
        MessageAll("GamePause");
    }

    public void ResumeGame()
    {
        PauseMenuAnim.SetBool("GamePaused", false);
        Time.timeScale = 1;
        MessageAll("GameResume");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator Delay(float Delay)
    {
        DelayDone = false;
        yield return new WaitForSecondsRealtime(Delay);
        DelayDone = true;
    }

    void MessageAll(string msg)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go != null)
            {
                go.gameObject.SendMessage(msg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }


}
