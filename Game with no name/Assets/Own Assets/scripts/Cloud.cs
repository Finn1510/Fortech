using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();   
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(0.5f, 0);
    }
}
