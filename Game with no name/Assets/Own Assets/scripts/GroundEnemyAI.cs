using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform raystart;
    [SerializeField] private Transform rayend;
    [SerializeField] private Transform Centerpoint;
    PlayerStats PlayerStats;
    Renderer ourRenderer;

    [Header("Parameters")]
    [SerializeField] private float raydistance = 2f;
    [SerializeField] private float movespeed = 20f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float Damage = 2f;
    [SerializeField] private float Damagedelay = 2f;
    [SerializeField] private float maxDamageDistance = 2f;
    [SerializeField] private float EnemyHealth = 50f;
    [SerializeField] private float Knockbackstrengh = 10f;

    [Header("BodyParts")]
    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject Torso;
    [SerializeField] private GameObject rightLeg;
    [SerializeField] private GameObject leftLeg;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private GameObject leftArm;

    Animator anim;
    GameObject Player;
    Rigidbody2D rb;

    bool Jump = false;
    bool RIGHT;
    bool LEFT;

    bool delay = false;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, -270);
        ourRenderer = GetComponent<SpriteRenderer>();
        if (ourRenderer.isVisible == true)
        {
            Destroy(gameObject);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerStats = GameObject.FindGameObjectWithTag("PlayerStatBin").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealth <= 0f)
        {
            Die();
        }
        
        movement();
        DamagePlayer();
    }

    void FixedUpdate()
    {
        if (EnemyHealth > 0f)
        {
            turning();
        }
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
                Player.SendMessage("Damage", Damage);
                Hashtable KnockbackDATA = new Hashtable();
                KnockbackDATA.Add("Direction", Centerpoint.position);
                KnockbackDATA.Add("Strengh", Knockbackstrengh);
                Player.SendMessage("applyKnockback", KnockbackDATA);
                
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

    void EnemyDamage(float gottenDamage)
    {
        EnemyHealth = EnemyHealth - gottenDamage;
    }  

    void Die()
    {
        PlayerStats.ZombiesKilled = PlayerStats.ZombiesKilled + 1;
        
        anim.SetTrigger("Die");

        Head.AddComponent<Rigidbody2D>();
        Head.transform.parent = null;
        Head.GetComponent<CircleCollider2D>().enabled = true;
        Head.tag = "Dead";
        Head.layer = 17;
        Head.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        Head.SendMessage("Die");

        Torso.AddComponent<Rigidbody2D>();
        Torso.transform.parent = null;
        Torso.GetComponent<CapsuleCollider2D>().enabled = true;
        Torso.tag = "Dead";
        Torso.layer = 17;
        Torso.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        Torso.SendMessage("Die");

        rightArm.AddComponent<Rigidbody2D>();
        rightArm.transform.parent = null;
        rightArm.GetComponent<CapsuleCollider2D>().enabled = true;
        rightArm.tag = "Dead";
        rightArm.layer = 17;
        rightArm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        rightArm.SendMessage("Die");

        leftArm.AddComponent<Rigidbody2D>();
        leftArm.transform.parent = null;
        leftArm.GetComponent<CapsuleCollider2D>().enabled = true;
        leftArm.tag = "Dead";
        leftArm.layer = 17;
        leftArm.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        leftArm.SendMessage("Die");

        rightLeg.AddComponent<Rigidbody2D>();
        rightLeg.transform.parent = null;
        rightLeg.GetComponent<CapsuleCollider2D>().enabled = true;
        rightLeg.tag = "Dead";
        rightLeg.layer = 17;
        rightLeg.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        rightLeg.SendMessage("Die");

        leftLeg.AddComponent<Rigidbody2D>();
        leftLeg.transform.parent = null;
        leftLeg.GetComponent<CapsuleCollider2D>().enabled = true;
        leftLeg.tag = "Dead";
        leftLeg.layer = 17;
        leftLeg.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 10)));
        leftLeg.SendMessage("Die");

        Destroy(this.gameObject);

    }
}
