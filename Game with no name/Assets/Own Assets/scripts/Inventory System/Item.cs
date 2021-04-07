using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public enum ItemType
    {
        HealthPotion, 
        Acid, 
        Thunderbolt, 
        Coin, 
        Lighting_Hawk,
        RustGun,
        Box,
    }

    public ItemType itemType;
    public int amount; 

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Acid: return ItemAssets.Instance.AcidSprite;
            case ItemType.Thunderbolt: return ItemAssets.Instance.ThunderboltSprite; 
            case ItemType.HealthPotion: return ItemAssets.Instance.HealthPotionSprite;
            case ItemType.Coin: return ItemAssets.Instance.CoinSprite;
            case ItemType.Lighting_Hawk: return ItemAssets.Instance.LightingHawkSprite;
            case ItemType.RustGun: return ItemAssets.Instance.RustGunSprite;
            case ItemType.Box: return ItemAssets.Instance.BoxSprite;
        }
    } 

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Acid:
            case ItemType.Lighting_Hawk:
            case ItemType.Thunderbolt:
            case ItemType.RustGun:
            case ItemType.Box:
                return false;
            case ItemType.Coin:
            case ItemType.HealthPotion:
                return true;
            
        }
    }
}
