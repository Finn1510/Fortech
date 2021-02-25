using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using DG.Tweening;

public class Gunsystem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float BulletForce = 1500;
    [SerializeField] float GunDelaySeconds = 0.02f;
    [SerializeField] KeyCode FireKey = KeyCode.Mouse0;
    [SerializeField] float LightMuzzleFlashIntensity = 2f;
    [SerializeField] float LightMuzzleFlashDurationSeconds = 0.05f;

    [Header("References")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform Firepoint;
    [SerializeField] Light2D LightMuzzleflash;
    [SerializeField] GameObject GunfireVFX;
    [SerializeField] AudioSource GunAudio;
    GameObject UI_Inventory;
    PlayerStats PlayerStats;
    
    bool GunDelayOver = true;
    bool Playerdied = false;
    bool GamePaused = false;
    Transform Player;

    private void Awake()
    {
        UI_Inventory = GameObject.FindGameObjectWithTag("UI_Inventory");
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerStats = GameObject.FindGameObjectWithTag("PlayerStatBin").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Playerdied != true && GamePaused == false)
        {
            aimtwrdsmouse();
            flipLikePlayer();
        }  
        
        if(Input.GetKey(FireKey) == true && GunDelayOver == true && Playerdied == false && UI_Inventory.active == false && GamePaused == false)
        {
            GunDelayOver = false;
            
            //Instantiate Bullet & add force
            GameObject Firedbullet = Instantiate(Bullet, Firepoint.position, transform.rotation);
            Firedbullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(BulletForce, 0));

            //Play GunShootSound 
            GunAudio.Play();

            //Instanciate Gunfire VFX
            Instantiate(GunfireVFX, Firepoint.position, Quaternion.identity);

            //Fade 2d light MuzzleFlash In and out
            DOTween.To(() => LightMuzzleflash.intensity, x => LightMuzzleflash.intensity = x, LightMuzzleFlashIntensity, LightMuzzleFlashDurationSeconds);
            StartCoroutine(MuzzleFlashDelay());

            //Update Player Stats
            PlayerStats.ShotsFired = PlayerStats.ShotsFired + 1;

            //init Delay Coroutines
            StartCoroutine(GunDelay()); 
            
        }
    } 

    IEnumerator GunDelay()
    {
        yield return new WaitForSeconds(GunDelaySeconds);
        GunDelayOver = true;
    } 

    IEnumerator MuzzleFlashDelay()
    {
        yield return new WaitForSeconds(LightMuzzleFlashDurationSeconds);
        DOTween.To(() => LightMuzzleflash.intensity, x => LightMuzzleflash.intensity = x, 0, LightMuzzleFlashDurationSeconds);
    }
    
    void aimtwrdsmouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    } 

    void flipLikePlayer()
    {
        transform.localScale = new Vector3(Player.localScale.x, Player.localScale.x, transform.localScale.z);
            
    }

    public void PlayerDied()
    {
        Playerdied = true;
    }

    public void PlayerRespawned()
    {
        Playerdied = false;
    }

    public void GamePause()
    {
        GamePaused = true;
    }

    public void GameResume()
    {
        GamePaused = false;
    }
}
