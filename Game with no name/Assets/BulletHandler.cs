using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BulletHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ImpactParticle;
    [SerializeField] private GameObject RedLight;
    
    [Header("Parameters")]
    [SerializeField] private float BulletDamage = 10f;

    CinemachineImpulseSource ImpulseCOM;


    // Start is called before the first frame update
    void Start()
    {
        ImpulseCOM = GetComponent<CinemachineImpulseSource>();
        StartCoroutine(destroyDelay());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with something");
        if(collision.gameObject.tag == "Enemy")
        {
            ImpulseCOM.GenerateImpulse(new Vector3(2, 2, 0));
            Instantiate(ImpactParticle, transform.position, transform.rotation);
            Instantiate(RedLight, transform.position, transform.rotation);
            collision.gameObject.SendMessage("EnemyDamage", BulletDamage);
            
            Destroy(this.gameObject);
        }
        else
        {
            Instantiate(ImpactParticle, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
