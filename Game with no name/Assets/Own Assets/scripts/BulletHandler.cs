using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BulletHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ImpactParticle;
    [SerializeField] private GameObject RedLight;
    [SerializeField] private GameObject hitsound;
    CinemachineImpulseSource ImpulseCOM;
    PlayerStats PlayerStats;

    [Header("Parameters")]
    [SerializeField] private float BulletDamage = 10f;
    [SerializeField] private bool EnableScreenshake = true;
    [SerializeField] private bool ShowHitLight = true;

    


    // Start is called before the first frame update
    void Start()
    {
        ImpulseCOM = GetComponent<CinemachineImpulseSource>();
        PlayerStats = GameObject.FindGameObjectWithTag("PlayerStatBin").GetComponent<PlayerStats>();
        StartCoroutine(destroyDelay());    
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Enemy")
        {
            //instanciate Hinsound
            Instantiate(hitsound, transform.position, Quaternion.identity);
            
            if(EnableScreenshake == true)
            {
                //do some Screenshake
                ImpulseCOM.GenerateImpulse(new Vector3(2, 2, 0));
            }
            //Instanciate Impactparticle
            Instantiate(ImpactParticle, transform.position, transform.rotation);
            
            if(ShowHitLight == true)
            {
                //Instanciate red Hitlight
                Instantiate(RedLight, transform.position, transform.rotation);
            }
            
            //send damage to collided Object
            collision.gameObject.SendMessage("EnemyDamage", BulletDamage, SendMessageOptions.DontRequireReceiver);

            //Update PlayerStats 
            PlayerStats.DamageDealed = PlayerStats.DamageDealed + BulletDamage;
            
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
