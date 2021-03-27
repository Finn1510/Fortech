using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsReset : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] GameObject Player;
    [SerializeField] BoxCollider2D Bound;

    [Header("Parameters")]
    [SerializeField] Vector3 PlayerResetPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("something went into our net");
        if (collision.tag == "Player")
        {
            Debug.Log("it was the player");
            Player.transform.position = PlayerResetPosition;
        }
        else
        {
            Debug.Log("it was not the player");
        }
    }


}
