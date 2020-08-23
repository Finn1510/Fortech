using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    
    private List<Item> itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction )
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.Acid, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Coin, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        
    } 
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                    Debug.Log("Added Item to Inventory");
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }

        }
        else
        {
            itemList.Add(item);
            Debug.Log("Added new Item to Inventory");
        }

        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    } 

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
                Debug.Log("Amount of Stackable Item removed from Inventory");
            }

        }
        else
        {
            itemList.Remove(item);
            Debug.Log("Removed an Item from Inventory");
        }


        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    
    public void UseItem(Item item)
    {
        useItemAction(item);   
    }
    
    public List<Item> GetItemList()
    {
        return itemList;
    }
} 

