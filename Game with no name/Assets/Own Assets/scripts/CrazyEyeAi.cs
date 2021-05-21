using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CrazyEyeAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform Centerpoint;

    [Header("BodyParts")]
    [SerializeField] private GameObject LeftWing;
    [SerializeField] private GameObject RightWing;
    [SerializeField] private GameObject Eye;

    [Header("Parameters")]
    [SerializeField] private float EnemyHealth = 30f;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float PathUpdateDelay = 0.5f;
    [SerializeField] private float maxDamageDistance = 2f;
    [SerializeField] private float Damage = 3;
    [SerializeField] private float Knockbackstrengh = 200;
    [SerializeField] private float DamageDelay = 0.5f ;

    Path path;
    GameObject Player;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    public bool Dead = false;
    bool DeadStarted = false;
    bool delay;
    Seeker seeker;
    Rigidbody2D rb;
    SpriteRenderer ourRenderer;
    PlayerStats PlayerStats;

    
    // Start is called before the first frame update
    void Start()
    {
        ourRenderer = GetComponent<SpriteRenderer>();
        if (ourRenderer.isVisible == true)
        {
            Destroy(gameObject);
        }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("CORE");
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats = GameObject.FindGameObjectWithTag("PlayerStatBin").GetComponent<PlayerStats>();

        InvokeRepeating("UpdatePath", 0f, PathUpdateDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealth <= 0f && DeadStarted == false)
        {
            Dead = true;
            Die();
            //TODO: find another solution for disableling pathfinding
            PathUpdateDelay = 9999999999;
        }
        if(Dead == false)
        {
            AttackFoe();
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
        PlayerStats.CrazyEyesKilled = PlayerStats.CrazyEyesKilled + 1;

        DeadStarted = true;
        anim.SetTrigger("Die");

        LeftWing.AddComponent<Rigidbody2D>();
        LeftWing.transform.parent = null;
        LeftWing.GetComponent<PolygonCollider2D>().enabled = true;
        LeftWing.tag = "Dead";
        LeftWing.layer = 17;


        RightWing.AddComponent<Rigidbody2D>();
        RightWing.transform.parent = null;
        RightWing.GetComponent<PolygonCollider2D>().enabled = true;
        RightWing.tag = "Dead";
        RightWing.layer = 17;


        Eye.GetComponent<Rigidbody2D>().gravityScale = 1;
        Eye.GetComponent<Animator>().enabled = false;
        Eye.GetComponent<CircleCollider2D>().enabled = true;
        Eye.tag = "Dead";
        Eye.layer = 17;
        Eye.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)));

        Destroy(GetComponent<SimpleSmoothModifier>());
        Destroy(GetComponent<Seeker>());

    } 

    void AttackFoe()
    {
        if(Vector2.Distance(transform.position,target.transform.position) <= maxDamageDistance)
        {
            

            if (target.tag == "Player")
            {
                
                if (delay == false)
                {
                    float dist = Vector3.Distance(Player.transform.position, transform.position);

                        //Apply damage to Player
                        Player.SendMessage("Damage", Damage);
                        
                        //Send Knockbag Data
                        Hashtable KnockbackDATA = new Hashtable();
                        KnockbackDATA.Add("Direction", Centerpoint.position);
                        KnockbackDATA.Add("Strengh", Knockbackstrengh);
                        Player.SendMessage("applyKnockback", KnockbackDATA);

                        StartCoroutine(Damagedelay());
                    
                }
            }  
            if(target.tag == "CORE")
            {
                if(delay == false)
                {
                    //Apply damage to Core
                    target.SendMessage("Damage", Damage);
                    StartCoroutine(Damagedelay());
                }
                
            }
        }
    }

    IEnumerator Damagedelay()
    {
        delay = true;
        yield return new WaitForSeconds(DamageDelay);
        delay = false;
    }

}
