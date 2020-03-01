using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyAI : MonoBehaviour
{

    public float raydistance = 2f;
    public float movespeed = 20f;
    GameObject Player;
    Rigidbody2D rb;
    Vector2 hitPoint;
    public bool RIGHT;
    public bool LEFT;

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
        }
        else
        {
            RIGHT = false;
        }

        if (gameObject.transform.position.x > Player.transform.position.x)
        {
            LEFT = true;
        }
        else
        {
            LEFT = false;
        }

        Debug.Log(rb.velocity);
    }

    void FixedUpdate()
    {
        if (LEFT = true)
        {
            rb.velocity = new Vector2(-movespeed, 0);
        }
        if (RIGHT = true)
        {
            rb.velocity = new Vector2(movespeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, raydistance);
        hit.point = hitPoint;
        if (hit.collider != null) //&& hit.transform.tag == "obstacle"
        {
            jump();

        }

        void jump()
        {
            Debug.Log("I wanna Jump lol");
        }
    }
}
