using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject TurretHead;
    [SerializeField] GameObject TurretPole;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject GunfireVFX;
    [SerializeField] Transform Firepoint;
    [SerializeField] AudioSource TurretAudio;
    
    [Header("Parameters")]
    [SerializeField] float HeadRotationspeed = 200;
    [SerializeField] float SpawnFireDelay = 1;
    [SerializeField] float RaycastLengh = 50;
    [SerializeField] float RangeRadius = 50;
    [SerializeField] float shootDelay = 1;
    [SerializeField] float BulletForce = 1500;
    [SerializeField] float GunKnockbackForce = 2000;

    Sequence HeadPoseSequence;
    Sequence PolePoseSequence;
    RaycastHit2D AimRay;
    public GameObject target;
    bool delay = true;
    bool delay2 = false;
    bool delay3 = false;
    bool EnemyInSight = false;
    bool DefaultPoseDelayStarted = false;
    bool inDefaultPose = false;
    bool PoseInterrupted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay3(SpawnFireDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            //Raycast for spotting the Target
            AimRay = Physics2D.Raycast(TurretHead.transform.position, GetTowardsTargetRotation() * Vector2.left, RaycastLengh);
            Debug.DrawRay(TurretHead.transform.position, GetTowardsTargetRotation() * Vector2.left * RaycastLengh, Color.red);

            //Only Aim if the Turret can "see" the Target
            if (target == AimRay.transform.gameObject)
            {
                if (delay3 == true)
                {
                    TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, GetTowardsTargetRotation(), HeadRotationspeed * Time.deltaTime);

                    if (transform.position.x < target.transform.position.x)
                    {
                        //rotate pole to left
                        TurretPole.transform.DORotate(new Vector3(0, 0, 40), 1);
                    }
                    else
                    {
                        //rotate pole to right
                        TurretPole.transform.DORotate(new Vector3(0, 0, -40), 1);
                    }
                }

                EnemyInSight = true;

                if (delay == true && delay3 == true)
                {
                    Shoot();
                }
            }
            else
            {
                EnemyInSight = false;
            }
        }
        
        if(target == null || target.tag == "Dead" || Vector2.Distance(transform.position, target.transform.position) >= RangeRadius)
        {
            GetNewTarget();
        }

        //return to default Pose if we dont see the Target
        if(EnemyInSight == false && DefaultPoseDelayStarted == false && inDefaultPose == false)
        {
            PoseInterrupted = false;
            StartCoroutine(Delay2(2));
            DefaultPoseDelayStarted = true;
        }
        if(EnemyInSight == true && DefaultPoseDelayStarted == true)
        {
            PoseInterrupted = true;
            inDefaultPose = false;
        }
        if(delay2 == true && PoseInterrupted == false)
        {
            ReturnToDefaultPose();
            inDefaultPose = true;
            delay2 = false;
            DefaultPoseDelayStarted = false;
        }
        if(delay2 = true && PoseInterrupted == true)
        {
            DefaultPoseDelayStarted = false;
            inDefaultPose = false;
        }
        if(EnemyInSight == true)
        {
            inDefaultPose = false;
            HeadPoseSequence.Kill();
            PolePoseSequence.Kill();
        }

    }

    //Do some math magic to get the rotation we need if we want to face the target Object
    Quaternion GetTowardsTargetRotation()
    {
        Vector2 direction = target.transform.position - TurretHead.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        return rotation;
        
    }

    void Shoot()
    {
        //Instantiate Bullet & add force
        GameObject Firedbullet = Instantiate(BulletPrefab, Firepoint.position, TurretHead.transform.rotation);
        Firedbullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(BulletForce * -1, 0));

        //Play GunShootSound 
        TurretAudio.Play();

        //Instanciate Gunfire VFX
        Instantiate(GunfireVFX, Firepoint.position, Quaternion.identity);

        StartCoroutine(Delay(shootDelay));
    }
    
    void ReturnToDefaultPose()
    {
        HeadPoseSequence = DOTween.Sequence();
        PolePoseSequence = DOTween.Sequence();
        HeadPoseSequence.Append(TurretHead.transform.DORotate(new Vector3(0, 0, -90), 1));
        PolePoseSequence.Append(TurretPole.transform.DORotate(new Vector3(0, 0, 0), 1));
    }

    void GetNewTarget()
    {
        GameObject CurrentchosenTarget = null;
        float PreviousDistance = 100;

        //Get the nearest Enemy
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, RangeRadius);
        foreach (Collider2D col in results)
        {
            if(col.tag == "Enemy")
            {
                if (Vector2.Distance(col.transform.position, transform.position) < PreviousDistance)
                {
                    CurrentchosenTarget = col.transform.gameObject;
                }
                PreviousDistance = Vector2.Distance(col.transform.position, transform.position);
            }
            
        }

        //Set new Target if we got any
        if(CurrentchosenTarget != null)
        {
            target = CurrentchosenTarget;
        }

    }

    //Waits for a certain time -Used for GunDelay
    IEnumerator Delay(float DelayTime)
    {
        delay = false;
        yield return new WaitForSeconds(DelayTime);
        delay = true;
    }

    //Waits for a certain time -Used for returntodefaultPose
    IEnumerator Delay2(float DelayTime)
    {
        delay2 = false;
        yield return new WaitForSeconds(DelayTime);
        delay2 = true;
    }

    //Waits for a certain time -Used for SpawnDelay
    IEnumerator Delay3(float DelayTime)
    {
        delay3 = false;
        yield return new WaitForSeconds(DelayTime);
        delay3 = true;
    }

}
