using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class player_movement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float runSpeed = 40f;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float VignetteIntensity = 0.25f;
    [SerializeField] float VignetDuration = 0.2f;
    [SerializeField] KeyCode InventoryKey = KeyCode.I;
    [SerializeField] KeyCode altInventoryCloseKey = KeyCode.Escape;
    float Horizontalmove = 0f;
    float MouseXpos;

    [Space]

    [Header("References")]
    [SerializeField] CharacterController2D controller;
    [SerializeField] Text Healthtext;
    [SerializeField] Slider HealthSlider;
    [SerializeField] PostProcessVolume DiedPostProcess;
    [SerializeField] PostProcessVolume PostProcessing;
    [SerializeField] GameObject YouDiedtext;
    [SerializeField] GameObject DiedMenu;
    [SerializeField] UI_Inventory uiInventory;
    [SerializeField] GameObject UI_Inventory;
    [SerializeField] Transform holdPoint; 
    AudioSource damageAudio;
    CinemachineImpulseSource ImpulseGEN; 
    Rigidbody2D rigid;
    GameObject heldWeapon;
    Item heldItem;
    PlayerStats PlayerStats;

    GameObject WeaponPrefab;
    public float Health = 100;
    bool Jump = false;
    bool Playerdied = false;
    bool DiedMessageSent = false;
    bool RespawnMessageSent = false;
    bool VignetCoroutineDelayACTIVE = false;
    bool GamePaused = false;

    private Inventory inventory;
    
    //Post process vars
    ColorGrading ColorGrade = null;
    Vignette Vignet = null;

    

    void Awake()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);

        ItemWorld.SpawnItemWorld(new Vector3(3, 4), new Item { itemType = Item.ItemType.Acid, amount = 1 });

        if (ES3.KeyExists("inventoryList"))
        {
            Load();
        }

        
    }

    void Start()
    {
        Healthtext.text = Health.ToString();
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = Health;

        DiedPostProcess.sharedProfile.TryGetSettings<ColorGrading>(out ColorGrade);
        PostProcessing.sharedProfile.TryGetSettings<Vignette>(out Vignet);

        ImpulseGEN = GetComponent<CinemachineImpulseSource>();
        rigid = GetComponent<Rigidbody2D>();
        damageAudio = GetComponent<AudioSource>();
        PlayerStats = GameObject.FindGameObjectWithTag("PlayerStatBin").GetComponent<PlayerStats>();

        UI_Inventory.SetActive(false);

    }

    // Update is called once per frame
    
    void Update()
    {
        Horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed; 
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }

        //Flip player according mouse position
        MouseXpos = Input.mousePosition.x / Screen.width;
        if (MouseXpos < 0.5 && GamePaused == false && Playerdied == false)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if(MouseXpos > 0.5 && GamePaused == false && Playerdied == false)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        
        if (Health <= 0f)
        {
            Debug.Log("Health is <= 0 player ist dead");
            UI_Inventory.SetActive(false);
            Playerdied = true;

            //reset this value here so the player can actually repawn
            RespawnMessageSent = false;

            if (DiedMessageSent == false)
            {
                GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (GameObject go in gos)
                {
                    if (go && go.transform.parent == null)
                    {
                        go.gameObject.BroadcastMessage("PlayerDied", SendMessageOptions.DontRequireReceiver);
                    }
                }

                DiedMenu.SetActive(true);

                PlayerStats.DiedCount = PlayerStats.DiedCount + 1;

                DiedMenu.GetComponent<Animator>().SetTrigger("died");

                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.00001f, 1f);
                
                DOTween.To(() => DiedPostProcess.weight, x => DiedPostProcess.weight = x, 1, 0.5f);
                DiedMessageSent = true;

            }

        } 

        if(Input.GetKeyDown(InventoryKey) == true && Playerdied == false && GamePaused == false)
        {
            if (UI_Inventory.active == false)
            {
                UI_Inventory.SetActive(true);
            }
            else
            {
                UI_Inventory.SetActive(false);
            }
        }

        if (Input.GetKeyDown(altInventoryCloseKey) == true && Playerdied == false && GamePaused == false)
        {
            if (UI_Inventory.active == true)
            {
                UI_Inventory.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        controller.Move(Horizontalmove * Time.fixedDeltaTime , false, Jump);
        Jump = false;
    } 

    public void Damage (float amount)
    {
        Health = Health - amount;
        Healthtext.text = Health.ToString();
        DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, Health, 0.5f);
        damageAudio.Play();
        ImpulseGEN.GenerateImpulse(new Vector3(2, 2, 0));
        DOTween.To(() => Vignet.intensity.value, x => Vignet.intensity.value = x, VignetteIntensity, 0.2f);
        StartCoroutine(DamageVignetDelay());
        
        
    } 

    public void applyKnockback(Hashtable KnockbackDATA)
    {
        Vector3 Knockbackdirection = (Vector3)KnockbackDATA["Direction"];
        
        if (Knockbackdirection.x < transform.position.x)
        {
            rigid.AddForce(new Vector3(10, 2) * (float)KnockbackDATA["Strengh"]);
        }
        else
        {
            rigid.AddForce(new Vector3(-10, 2) * (float)KnockbackDATA["Strengh"]);
        }

    } 

    IEnumerator DamageVignetDelay()
    {
        if(VignetCoroutineDelayACTIVE == false)
        {
            VignetCoroutineDelayACTIVE = true;
            
            yield return new WaitForSeconds(VignetDuration);
            DOTween.To(() => Vignet.intensity.value, x => Vignet.intensity.value = x, 0, 0.2f);
            VignetCoroutineDelayACTIVE = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();  
        if (itemWorld != null)
        {
            //touching item 
            
            //Checking if Inventory is full
            int InventorySpace = 1;
            foreach(Item item in inventory.GetItemList())
            {
                InventorySpace++;
                
            }
            //Debug.Log(InventorySpace + " unique Items in Inventory");

            if (InventorySpace > 40)
            {
                Debug.Log("Inventory Full");
                return;
            }
            
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
        
    } 

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                //TODO some VFX 
                if (Health + 20 <= 100)
                {
                    Health += 20;
                    Healthtext.text = Health.ToString();
                    DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, Health, 0.5f);
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                }
                break;
            case Item.ItemType.Acid:
                if(heldWeapon != null)
                {
                    Destroy(heldWeapon);
                }

                WeaponPrefab = ItemAssets.Instance.AcidPrefab;
                heldItem = item;

                heldWeapon = Instantiate(WeaponPrefab,holdPoint.position,Quaternion.identity, holdPoint);
                UI_Inventory.SetActive(false);

                break;
            case Item.ItemType.Thunderbolt:
                if (heldWeapon != null)
                {
                    Destroy(heldWeapon);
                }

                WeaponPrefab = ItemAssets.Instance.ThunderboltPrefab;
                heldItem = item;

                heldWeapon = Instantiate(WeaponPrefab, holdPoint.position, Quaternion.identity, holdPoint);
                UI_Inventory.SetActive(false);

                break;
            case Item.ItemType.Lighting_Hawk:
                if (heldWeapon != null)
                {
                    Destroy(heldWeapon);
                }

                WeaponPrefab = ItemAssets.Instance.LightingHawkPrefab;
                heldItem = item;

                heldWeapon = Instantiate(WeaponPrefab, holdPoint.position, Quaternion.identity, holdPoint);
                UI_Inventory.SetActive(false);

                break;


        }
    } 

    public void removedItem(Item item)
    {
        bool heldWeaponstillinInventory = false;
        
        if(item.itemType != heldItem.itemType)
        {
            Debug.Log("removedItem was not helditem's type");
            return;
        }
        else
        {
            Debug.Log("removedItem was helditem's type investigating...");
            foreach (Item inventoryItem in inventory.GetItemList())
            {
                
                if (inventoryItem.itemType == heldItem.itemType)
                {
                    heldWeaponstillinInventory = true;
                    Debug.Log("an item with same itemType as heldItem is still in inventory");
                    
                }
            } 

            if(heldWeaponstillinInventory == false)
            {
                //destroy held weapon
                Debug.Log("no item with the same itemType as heldItem is in the inventory... deleting helditem...");
                heldItem = null;
                Destroy(heldWeapon);
                
            }
        }
    }

    public void Respawn()
    {
        Debug.Log("Player Respawned");
        UI_Inventory.SetActive(false);
        Playerdied = false;

        //reset pos
        transform.position = new Vector2(17, 6.5f);
        
        //change Health + Healthbar values to max possible health
        changeHealth(maxHealth);
        DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, maxHealth, 0.2f);


        //TODO figure out a penalty for dying + protect the player during the first few seconds

        if (RespawnMessageSent == false)
        {
            GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in gos)
            {
                if (go && go.transform.parent == null)
                {
                    go.gameObject.BroadcastMessage("PlayerRespawned", SendMessageOptions.DontRequireReceiver);
                }
            }

            DiedMenu.SetActive(false);

            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1f);

            DOTween.To(() => DiedPostProcess.weight, x => DiedPostProcess.weight = x, 0,1f);
            DiedMenu.GetComponent<Animator>().SetTrigger("respawned");
            
            RespawnMessageSent = true;

            //reset this value here so player can die again after respawning
            DiedMessageSent = false;

        }
    }

    void changeHealth(float amount)
    {
        Health = amount;
        DOTween.To(() => HealthSlider.value, x => HealthSlider.value = x, amount, 0.2f);
        Healthtext.text = amount.ToString();
    }

    public void GamePause()
    {
        GamePaused = true;
    }

    public void GameResume()
    {
        GamePaused = false;
    }

    public void Save()
    {
        ES3.Save<List<Item>>("inventoryList", inventory.GetItemList());
        ES3.Save<Item>("heldItemType", heldItem);
        
        ES3.Save<float>("PlayerHealth", Health);
        ES3.Save<Vector3>("PlayerPosition", transform.position);

        Debug.Log("Player Saved");
    }

    void Load()
    {
        Health = ES3.Load<float>("PlayerHealth");
        transform.position = ES3.Load<Vector3>("PlayerPosition");

        Item TestHeldItem = ES3.Load<Item>("heldItemType");

        //Equip weapon again
        if (ES3.KeyExists("heldItemType") == true)
        {
            if(TestHeldItem != null)
            {
                heldItem = ES3.Load<Item>("heldItemType");
                UseItem(heldItem);
            }
            
        }
    }

   
}
