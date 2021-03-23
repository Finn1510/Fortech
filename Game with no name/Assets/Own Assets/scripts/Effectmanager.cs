using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectmanager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float EffectTimeSeconds = 2;
    [SerializeField] bool hasSoundEffect = false;
    [SerializeField] AudioClip SoundEffect;

    [Header("References")]
    [SerializeField] AudioSource Audio;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(hasSoundEffect == true)
        {
            Audio.clip = SoundEffect;
            Audio.Play();
        }

        StartCoroutine(EffectWait());
    }

    IEnumerator EffectWait()
    {
        yield return new WaitForSeconds(EffectTimeSeconds);
        Destroy(gameObject);
    }
}
