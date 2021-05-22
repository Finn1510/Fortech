using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryHack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Button AddToInventoryButton;
    [SerializeField] TMP_Dropdown ItemTypeDropDown;
    [SerializeField] TMP_InputField AmountInputfield;
    [SerializeField] player_movement InventoryGO;

    [SerializeField] List<Item> AllItems;

    string kek;
    
    public void AddToInventory()
    {
        int amount = 1;
        try
        {
            amount = Convert.ToInt32(AmountInputfield.text);
        }
        //we dont really want to do anything on catch
        catch { }

        Item item = AllItems[ItemTypeDropDown.value];

        Item generatedItem = item;
        item.amount = amount;

        InventoryGO.inventory.AddItem(item);
        
    }
}
