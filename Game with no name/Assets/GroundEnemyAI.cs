using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyAI : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField] private float raydistance = 2f;
    [SerializeField] private float movespeed = 20f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float Damage = 2f;
    [SerializeField] private float Damagedelay = 2f;
    [SerializeField] private Transform raystart;
    [SerializeField] private Transform rayend;
    [SerializeField] private float maxDamageDistance;

    GameObject Player;
    Rigidbody2D rb;

    bool Jump = false;
    bool RIGHT;
    bool LEFT;

    bool delay = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        DamagePlayer();
    }

    void FixedUpdate()
    {
        turning();   

    } 

    void movement()
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

    void turning()
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

    void DamagePlayer()
    {
        if (delay == false)
        {
            float dist = Vector3.Distance(Player.transform.position, transform.position);

            if (dist <= maxDamageDistance)
            {
                Player.GetComponent<PlayerHandler>().Damage(Damage);
                Debug.Log("Damaged Player");

                StartCoroutine(DamageDelay());
            }
        }
        
        
    } 

    IEnumerator DamageDelay()
    {
        delay = true;
        yield return new WaitForSeconds(Damagedelay);
        delay = false;
    }
}
