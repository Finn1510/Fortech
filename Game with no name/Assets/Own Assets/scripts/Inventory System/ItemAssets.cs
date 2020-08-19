using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{

    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemworld;
    
    public Sprite AcidSprite; 
    public Sprite ThunderboltSprite;
    public Sprite CoinSprite;
    public Sprite HealthPotionSprite;
    public Sprite LightingBoltSprite;
    

}
