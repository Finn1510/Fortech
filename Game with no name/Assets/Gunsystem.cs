using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunsystem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] KeyCode FireKey = KeyCode.Mouse0;

    [Header("References")]
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform Firepoint;
    [SerializeField] GameObject Muzzleflash;

    [Header("WeaponState")]
    [SerializeField] bool pickedUp;
    [SerializeField] bool Shooting;

    bool Playerdied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Playerdied != true && pickedUp == true)
        {
            aimtwrdsmouse();
        }   
    } 

    void aimtwrdsmouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    } 

    public void PlayerDied()
    {
        Playerdied = true;
    }
}
