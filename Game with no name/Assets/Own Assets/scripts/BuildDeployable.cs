using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class BuildDeployable : MonoBehaviour
{
    [SerializeField] GameObject movepoint;
    [SerializeField] GameObject Kek;
    [SerializeField] Camera cam;
    [SerializeField] player_movement PlayerObject;
    public Item ourItem;
 
    bool firstrayhit;
    bool PositionViable = false;
    bool PlacingDeployable = false;
    Vector3 LastVialbePos;

    private void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movement>();
        
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
                Debug.Log("SpawnPrefab is:" + spawnprefab);

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
                    Kek.transform.position = hit.point;
                    firstrayhit = true;
                    PositionViable = true;
                    LastVialbePos = hit.point;
                    Kek.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    firstrayhit = false;
                    PositionViable = false;
                    Kek.GetComponent<SpriteRenderer>().color = Color.red;
                }

                RaycastHit2D hit2 = Physics2D.Raycast(movepoint.transform.position, Vector2.up, 100f);
                if (hit2.collider is TilemapCollider2D)
                {
                    //we're inside the collider
                    if (firstrayhit == true)
                    {
                        PositionViable = false;
                        Kek.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
        
    }

    IEnumerator SpawnwithDelay(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        Instantiate(go, LastVialbePos, Quaternion.identity);
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

}
