using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.IO;
using UnityEngine.SceneManagement;

public class CoreHandler : MonoBehaviour
{
    [Header("References")]
    CinemachineImpulseSource ImpulseGEN; 
    [SerializeField] Slider CoreHealthSlider;
    [SerializeField] TMP_Text CoreHealthText;
    [SerializeField] CinemachineVirtualCamera Camera;
    [SerializeField] GameObject CoreDestroyedMenu;
    [SerializeField] TMP_Text CoreDestroyedMenuWavesFinishedText;
    [SerializeField] WaveManager waveManager;
    public Volume PostProcessing;
    AudioSource damageAudio;
    [Header("Parameters")]
    [SerializeField] float maxHealth = 500;
    [SerializeField] string SaveFileName = "SaveData.es3";

    public float Health;
    bool dead = false;
    bool DiedMessageSent = false;
    string LocalSaveFilePath;

    ChromaticAberration chromaticA;
    FilmGrain filmGrain;

    Sequence TimeSequence;

    private void Awake()
    {
        Health = maxHealth;
        if (ES3.KeyExists("CoreHealth"))
        {
            Load();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeSequence = DOTween.Sequence();

        damageAudio = GetComponent<AudioSource>();
        ImpulseGEN = GetComponent<CinemachineImpulseSource>();

        PostProcessing.sharedProfile.TryGet(out chromaticA);
        PostProcessing.sharedProfile.TryGet(out filmGrain);

        CoreHealthSlider.maxValue = maxHealth;

        LocalSaveFilePath = Application.persistentDataPath + "/" + SaveFileName;
    }

    // Update is called once per frame
    void Update()
    {
        DOTween.To(() => CoreHealthSlider.value, x => CoreHealthSlider.value = x, Health, 0.5f);
        CoreHealthText.SetText(Health.ToString()); 
        
        if(Health <= 0)
        {
            dead = true;
            Die();
        }
    }

    public void Damage(float amount)
    {
        if(Health > 0)
        {
            Health = Health - amount;

            //damageAudio.Play(); TODO: add that back in if we hav some soundzzzzz
            ImpulseGEN.GenerateImpulse(new Vector3(2, 2, 0));
        } 
        if(Health < 0)
        {
            Health = 0;
        }
    }

    void Die()
    {
        if (DiedMessageSent == false)
        {
            Camera.Follow = gameObject.transform;

            filmGrain.intensity.value = 0;
            filmGrain.active = true;
            DOTween.To(() => filmGrain.intensity.value, x => filmGrain.intensity.value = x, 1, 1f).SetUpdate(true);

            chromaticA.intensity.value = 0;
            chromaticA.active = true;
            DOTween.To(() => chromaticA.intensity.value, x => chromaticA.intensity.value = x, 0.7f, 1f).SetUpdate(true);


            GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in gos)
            {
                if (go && go.transform.parent == null)
                {
                    go.gameObject.BroadcastMessage("CoreDestroyed", SendMessageOptions.DontRequireReceiver);
                }
            }

            CoreDestroyedMenuWavesFinishedText.text = "You survived " + waveManager.lastCompletedWavenumber.ToString() + " Waves";

            CoreDestroyedMenu.SetActive(true);

            TimeSequence.Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 1f));

            DiedMessageSent = true;

        }
    }

    //restart the game
    public void RestartGame()
    {
        TimeSequence.Kill();
        DOTween.KillAll();

        //wipe local SaveFile
        File.WriteAllText(LocalSaveFilePath, "{}");
        Debug.Log("SaveFile has been wiped... restarting scene");
        filmGrain.active = false;
        chromaticA.active = false;
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }

    void Load()
    {
        Health = ES3.Load<float>("CoreHealth");
    } 

    void Save()
    {
        ES3.Save<float>("CoreHealth", Health);
    }

    private void OnApplicationQuit()
    {
        filmGrain.active = false;
        chromaticA.active = false;
        Save();
    }

}
