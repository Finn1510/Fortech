using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CrazyEyeAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject target;
    
    [Header("Parameters")]
    [SerializeField] private float EnemyHealth = 30f;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float PathUpdateDelay = 0.5f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
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
        if(EnemyHealth <= 0f)
        {
            Die();
        }
        
    }

    void FixedUpdate()
    {
        movement();    
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

        if(distance < nextWaypointDistance)
        {
            currentWaypoint = currentWaypoint + 1;
        } 

        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } 
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        
    }
    
    
    
    void EnemyDamage(float gottenDamage)
    {
        EnemyHealth = EnemyHealth - gottenDamage;
    }

  
    void Die()
    {
        anim.SetTrigger("Die");
    }

}
