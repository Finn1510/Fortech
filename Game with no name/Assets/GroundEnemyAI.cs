using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyAI : MonoBehaviour
{

    public float raydistance = 2f;
    public float movespeed = 20f;
    public float jumpForce = 20f;
    
    GameObject Player;
    Rigidbody2D rb;
    
    public bool RIGHT;
    public bool LEFT;
    

    public Transform raystart;
    public Transform rayend;

    public bool Jump = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < Player.transform.position.x)
        {
            RIGHT = true;
            LEFT = false;
            
        }
        else
        {
            RIGHT = false;
            LEFT = true;
        }

        

        if (rb.velocity.x < 0)
        {
                    
        }
        
    }

    void FixedUpdate()
    {
        if (LEFT == true)
        {
            rb.AddForce(new Vector2(-movespeed, 0));
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
            
        }
        
        else if (RIGHT == true)
        {
            rb.AddForce(new Vector2(movespeed, 0));
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
            
        }

        Debug.DrawLine(raystart.position, rayend.position, Color.cyan);
        Jump = Physics2D.Linecast(raystart.position, rayend.position, 1 << LayerMask.NameToLayer("obstacle")); 

        if (Jump == true)
        {
            rb.AddForce(new Vector2(0, 60));
        }

        
    }
}
