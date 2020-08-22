using CodeMonkey.Utils;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform PlayerPosition;
    
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }


    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    } 
    
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 130f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use Item
            };

            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop item 
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                inventory.RemoveItem(item);
                ItemWorld.DropItem(PlayerPosition, duplicateItem);
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Icon").GetComponent<Image>();
            image.sprite = item.GetSprite();
            Text uiText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            
            if(item.amount > 1)
            {
                uiText.text = item.amount.ToString();
            }
            else
            {
                uiText.text = "";
            }
            


            x++; 
            if (x > 8)
            {
                x = 0;
                y++;
            }
            
        }
    }

}
