using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        HealthPotion, 
        Acid, 
        Thunderbolt, 
        Coin, 
        Lighting_Hawk,
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
            case ItemType.Lighting_Hawk: return ItemAssets.Instance.LightingBoltSprite;
        }
    }
}
