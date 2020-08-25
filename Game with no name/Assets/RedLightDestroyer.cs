using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightDestroyer : MonoBehaviour
{

    [SerializeField] float DelayWaitTimeSeconds = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteDelay());    
    }

    IEnumerator DeleteDelay()
    {
        yield return new WaitForSeconds(DelayWaitTimeSeconds);
        Destroy(this.gameObject);
    }
}
