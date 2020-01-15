using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public CharacterController2D controller;
    float Horizontalmove = 0f;
    public float runSpeed = 40f;
    bool Jump = false;
    
    // Update is called once per frame
    void Update()
    {
        Horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed; 
        if (Input.GetButtonDown("Jump"))
        {
            Jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(Horizontalmove * Time.fixedDeltaTime , false, Jump);
        Jump = false;
    }

}
