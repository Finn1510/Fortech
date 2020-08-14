using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitsoundDeleter : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float deletedelay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Deletedelay());        
    }
    
    IEnumerator Deletedelay()
    {
        yield return new WaitForSeconds(deletedelay);
        Destroy(this.gameObject);
    }
}
