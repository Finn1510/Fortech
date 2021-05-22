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
    [SerializeField] float NewPickDelayWhenEnemyIsObscured = 1;
    [SerializeField] float RaycastLengh = 50;
    [SerializeField] float RangeRadius = 50;
    [SerializeField] float shootDelay = 1;
    [SerializeField] float BulletForce = 1500;
    [SerializeField] float GunKnockbackForce = 2000;

    Sequence HeadPoseSequence;
    Sequence PolePoseSequence;
    RaycastHit2D AimRay;
    public GameObject target;
    public GameObject AimTarget;
    bool Gundelay = true;
    bool Posedelay = false;
    bool Spawndelay = false;
    public bool EnemySightdelay = false;
    public bool EnemyInSight = false;
    public bool EnemyInSightDelayStarted = false;
    public bool DefaultPoseDelayStarted = false;
    public bool inDefaultPose = false;
    public bool PoseInterrupted = false;
    public bool GettingNewTarget = false;
    public GameObject RaycastGO;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnDelay(SpawnFireDelay));
    }

    // Update is called once per frame
    void Update()
    {
        //Getting the Gameobject the Raycast currently hits for debug reasons
        try
        {
            RaycastGO = AimRay.transform.gameObject;
        }
        catch
        {
            RaycastGO = null;
        }

        //Picks a new Target if the Old one is obscured for a ceratain time
        if (EnemyInSight == false && EnemyInSightDelayStarted == false)
        {
            StartCoroutine(EnemySightDelay(NewPickDelayWhenEnemyIsObscured));
            Debug.Log("started delay");
        }
        if (EnemyInSight == true && EnemyInSightDelayStarted == true)
        {
            EnemySightdelay = false;
            Debug.Log("Disabled Delay coz Enemy is in sight");
        }
        if (EnemyInSight == false && EnemySightdelay == true)
        {
            Debug.Log("LEEEETS get new target");
            EnemySightdelay = false;
            EnemyInSightDelayStarted = false;
            GetNewTarget();
        }

        //return to default Pose if we dont see the Target
        if (EnemyInSight == false && DefaultPoseDelayStarted == false && inDefaultPose == false)
        {
            PoseInterrupted = false;
            StartCoroutine(PoseDelay(2));
            DefaultPoseDelayStarted = true;
        }
        if (EnemyInSight == true && DefaultPoseDelayStarted == true)
        {
            PoseInterrupted = true;
            inDefaultPose = false;
        }
        if (Posedelay == true && PoseInterrupted == false)
        {
            ReturnToDefaultPose();
            inDefaultPose = true;
            Posedelay = false;
            DefaultPoseDelayStarted = false;
        }
        if (Posedelay = true && PoseInterrupted == true)
        {
            DefaultPoseDelayStarted = false;
            inDefaultPose = false;
        }
        if (EnemyInSight == true)
        {
            inDefaultPose = false;
            HeadPoseSequence.Kill();
            PolePoseSequence.Kill();
        }

        if (target == null || target.tag == "Dead" || Vector2.Distance(transform.position, target.transform.position) >= RangeRadius)
        {
            target = null;
            AimTarget = null;
            GetNewTarget();
        }

        if (target != null)
        {
            //Raycast for spotting the Target
            AimRay = Physics2D.Raycast(TurretHead.transform.position, GetTowardsTargetRotation(AimTarget) * Vector2.left, RaycastLengh);
            Debug.DrawRay(TurretHead.transform.position, GetTowardsTargetRotation(AimTarget) * Vector2.left * RaycastLengh, Color.red);

            try
            {
                //test if the Raycast hit anything so we dont get an Exception when it doesnt
                GameObject go = AimRay.transform.gameObject;
            }
            catch
            {
                EnemyInSight = false;
                return;
            }


            //Only Aim if the Turret can "see" the Target
            if (target == AimRay.transform.gameObject)
            {
                if (Spawndelay == true)
                {
                    TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, GetTowardsTargetRotation(AimTarget), HeadRotationspeed * Time.deltaTime);

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

                if (Gundelay == true && Spawndelay == true)
                {
                    Shoot();
                }
            }
            else
            {
                EnemyInSight = false;
            }
        }
        else
        {
            //We cant see the enemy when target = null
            EnemyInSight = false;
        }
        
        
    }

    //Do some math magic to get the rotation we need if we want to face the target Object
    Quaternion GetTowardsTargetRotation(GameObject go)
    {
        Vector2 direction = go.transform.position - TurretHead.transform.position;
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

        StartCoroutine(GunDelay(shootDelay));
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
        GettingNewTarget = true;
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

        //Set Target if we got any
        if (CurrentchosenTarget != null)
        {
            target = CurrentchosenTarget;
        }

        //Set Aim Target if we got any
        if (CurrentchosenTarget != null)
        {
            //set the Target to the Target's senterpoint Gameobject for better aiming
            if(CurrentchosenTarget.transform.GetChild(0).gameObject.name == "CenterPoint")
            {
                AimTarget = CurrentchosenTarget.transform.GetChild(0).gameObject;
            }
            else
            {
                AimTarget = CurrentchosenTarget;
            }
            
        }

        GettingNewTarget = false;

    }

    //Waits for a certain time -Used for GunDelay
    IEnumerator GunDelay(float DelayTime)
    {
        Gundelay = false;
        yield return new WaitForSeconds(DelayTime);
        Gundelay = true;
    }

    //Waits for a certain time -Used for returntodefaultPose
    IEnumerator PoseDelay(float DelayTime)
    {
        Posedelay = false;
        yield return new WaitForSeconds(DelayTime);
        Posedelay = true;
    }

    //Waits for a certain time -Used for SpawnDelay
    IEnumerator SpawnDelay(float DelayTime)
    {
        Spawndelay = false;
        yield return new WaitForSeconds(DelayTime);
        Spawndelay = true;
    }

    //Waits for a certain time -Used for EnemyInSightDelay
    IEnumerator EnemySightDelay(float DelayTime)
    {
        EnemyInSightDelayStarted = true;
        EnemySightdelay = false;
        yield return new WaitForSeconds(DelayTime);
        EnemySightdelay = true;
    }

}
