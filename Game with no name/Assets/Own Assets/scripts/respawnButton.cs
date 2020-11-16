using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnButton : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    private player_movement playerScript;

    private void Start()
    {
        playerScript = player.GetComponent<player_movement>();   
    }

    public void Respawn()
    {
        playerScript.Respawn();
    }
}
