using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class BuildDeployable : MonoBehaviour
{
    [SerializeField] float IconPreviewAlpha;
    [SerializeField] GameObject movepoint;
    [SerializeField] GameObject Buildpoint;
    [SerializeField] Camera cam;
    [SerializeField] player_movement PlayerObject;

    DeployAbleSaveManager DContainer;
    public Item ourItem;
    bool firstrayhit;
    bool PositionViable = false;
    bool PlacingDeployable = false;
    Vector3 LastVialbePos;

    private void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movement>();
        DContainer = GameObject.FindGameObjectWithTag("DeployableContainer").GetComponent<DeployAbleSaveManager>();

        //Buildpoint.GetComponent<SpriteRenderer>().sprite = GetItemSprite(ourItem); -this doesnt look that good

    }

    // Update is called once per frame
    void Update()
    {
        Pickground();
        
        if(Input.GetMouseButtonDown(0))
        {
            if(PositionViable == true)
            {
                PlacingDeployable = true;

                //Place DeployAble Prefab
                GameObject spawnprefab = GetItemPrefab(ourItem);
                
                StartCoroutine(SpawnwithDelay(spawnprefab, 0.5f));

                movepoint.transform.parent = null;
                movepoint.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);
                movepoint.transform.DOMoveY(movepoint.transform.position.y - 10, 0.5f).SetEase(Ease.InExpo);
                movepoint = null;

                PlayerObject.heldItem = null;
                PlayerObject.removeItem(ourItem);

            }
            else
            {
                //give some feedback that the position is not viable
            }

        }
    }

    void Pickground()
    {
        if(PlacingDeployable == false)
        {
            movepoint.transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)) - transform.position, 0.5f);

            RaycastHit2D hit = Physics2D.Raycast(movepoint.transform.position, -Vector2.up, 200f);
            if (hit.collider != null)
            {
                if (hit.collider is TilemapCollider2D)
                {
                    Buildpoint.transform.position = hit.point;
                    firstrayhit = true;
                    PositionViable = true;
                    LastVialbePos = hit.point;
                    Buildpoint.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    firstrayhit = false;
                    PositionViable = false;
                    Buildpoint.GetComponent<SpriteRenderer>().color = Color.red;
                }

                RaycastHit2D hit2 = Physics2D.Raycast(movepoint.transform.position, Vector2.up, 100f);
                if (hit2.collider is TilemapCollider2D)
                {
                    //we're inside the collider
                    if (firstrayhit == true)
                    {
                        PositionViable = false;
                        Buildpoint.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
        
    }

    IEnumerator SpawnwithDelay(GameObject go, float duration)
    {
        DContainer.AddToContainer(ourItem, LastVialbePos);
        yield return new WaitForSeconds(duration);
        GameObject SpawnedDeployable = Instantiate(go, LastVialbePos, Quaternion.identity);
        Destroy(gameObject); 
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

    Sprite GetItemSprite(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Box:
                return ItemAssets.Instance.BoxSprite;
        }

        return null;
    }

}
