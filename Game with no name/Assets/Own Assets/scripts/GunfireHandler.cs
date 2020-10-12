using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunfireHandler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float DestroyDelay = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDelayCorut());    
    }

    IEnumerator DestroyDelayCorut()
    {
        yield return new WaitForSeconds(DestroyDelay);
        Destroy(gameObject);
    }
}
