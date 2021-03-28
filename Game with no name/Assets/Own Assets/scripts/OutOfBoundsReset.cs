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
        if (collision.tag == "Player")
        {
            Player.transform.position = PlayerResetPosition;
        }
    }


}
