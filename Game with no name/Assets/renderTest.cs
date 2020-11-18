using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class renderTest : MonoBehaviour
{
    SpriteRenderer ourRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ourRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0, 0, -270);
        if (ourRenderer.isVisible == true)
        {
            Debug.Log("object visible");
            //Destroy(gameObject);
        }
    }
}
