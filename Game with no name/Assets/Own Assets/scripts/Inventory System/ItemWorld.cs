using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

public class ItemWorld : MonoBehaviour
{
    static GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemworld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Transform dropPosition, Item item)
    {
        Vector3 randomDir = new Vector3(Random.Range(-1, 1), Random.Range(0.3f, 1), 0);
        Vector3 randomdir;
        
        if(dropPosition.localScale.x == 1)
        {
            randomDir = new Vector3(Random.Range(0.2f , 1), Random.Range(0.3f, 1), 0);
        }
        if (dropPosition.localScale.x == -1)
        {
            randomDir = new Vector3(Random.Range(-1, -0.2f), Random.Range(0.3f, 1), 0);
        }
        
        ItemWorld itemWorld = SpawnItemWorld(dropPosition.position + randomDir * 5f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 5f, ForceMode2D.Impulse);
        
        //Check if held weapon is still in Inventory
        Player.GetComponent<player_movement>().removedItem(item);
        
        return itemWorld;
    }
    
    
    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro amountText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        amountText = transform.Find("Text").GetComponent<TextMeshPro>();
        
    }

    public void SetItem (Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite(); 
        if (item.amount > 1)
        {
            amountText.SetText(item.amount.ToString());
        }
        else
        {
            amountText.SetText("");
        }
    } 

    public Item GetItem()
    {
        return item;
    } 

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
