using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployAbleSaveManager : MonoBehaviour
{
    public List<Item> ItemContainer = new List<Item>();
    public List<Vector3> PositionContainer = new List<Vector3>();

    int Index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        if (ES3.KeyExists("ItemContainer"))
        {
            Load();

            //Spawn in Saved Deployables
            foreach(Item item in ItemContainer)
            {
                Item itemType = item;
                Vector3 pos = PositionContainer[Index];
                Index = Index + 1;
                Instantiate(GetItemPrefab(item), pos, Quaternion.identity);
            }

        }
        
            
    }

    public void AddToContainer(Item itemType, Vector3 position)
    {
        ItemContainer.Add(itemType);
        PositionContainer.Add(position);
    }

    GameObject GetItemPrefab(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Box:
                return ItemAssets.Instance.BoxPrefab;
        }

        return null;
    }

    public void Save()
    {
        ES3.Save<List<Item>>("ItemContainer", ItemContainer);
        ES3.Save<List<Vector3>>("PositionContainer", PositionContainer);
        Debug.Log("Deployables Saved");
    }

    private void Load()
    {
        ItemContainer = ES3.Load<List<Item>>("ItemContainer");
        PositionContainer = ES3.Load<List<Vector3>>("PositionContainer");
    }

    
}
