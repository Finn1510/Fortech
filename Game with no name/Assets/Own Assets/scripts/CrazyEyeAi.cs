using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CrazyEyeAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject target;

    [Header("BodyParts")]
    [SerializeField] private GameObject LeftWing;
    [SerializeField] private GameObject RightWing;
    [SerializeField] private GameObject Eye;

    [Header("Parameters")]
    [SerializeField] private float EnemyHealth = 30f;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float PathUpdateDelay = 0.5f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    bool Dead = false;
    bool DeadStarted = false;
    Seeker seeker;
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("CORE");

        InvokeRepeating("UpdatePath", 0f, PathUpdateDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealth <= 0f && DeadStarted == false)
        {
            Dead = true;
            Die(); 
        }
        
    }

    void FixedUpdate()
    {
        if(Dead != true)
        {
            movement();
        }
         
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }
    
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void movement()
    {


        if (path == null)
            return; 

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        } 
        else
        {
            reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        

        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //only change flip when waypoint not reached to prevent looking back to passed waypoint
        if (distance > nextWaypointDistance)
        {
            if (force.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (force.x <= -0.01f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        if(distance < nextWaypointDistance)
        {
            currentWaypoint = currentWaypoint + 1;
        } 

        

        
    }
    
    
    
    void EnemyDamage(float gottenDamage)
    {
        EnemyHealth = EnemyHealth - gottenDamage;
        target = GameObject.FindGameObjectWithTag("Player");
    }

  
    void Die()
    {
        DeadStarted = true;
        anim.SetTrigger("Die");

        LeftWing.AddComponent<Rigidbody2D>();
        LeftWing.transform.parent = null;
        LeftWing.GetComponent<PolygonCollider2D>().enabled = true;
        

        RightWing.AddComponent<Rigidbody2D>();
        RightWing.transform.parent = null;
        RightWing.GetComponent<PolygonCollider2D>().enabled = true;
        

        Eye.GetComponent<Rigidbody2D>().gravityScale = 1;
        Eye.GetComponent<Animator>().enabled = false;
        //Eye.transform.parent = null; Eye IS parent ! ??
        Eye.GetComponent<CircleCollider2D>().enabled = true;
        Eye.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)));

        

    }

}
