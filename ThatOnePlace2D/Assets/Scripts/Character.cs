using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Variables
    Rigidbody2D rb; //Setting the character's Rigidbody component to rb, can call with rb.etc
    bool bleftReq, bRightReq; //Setting a bool for movement with GetKey
    float fSpeed = 20f; //Character movement speed
    Vector2 vDirModifier; //Character Vector2


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 1; //Hardcode for character mass, adjust through this variable only, not through inspector.
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //Can remove 2nd Input if we don't want arrow key control.
        {
            bleftReq = true;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // Can remove 2nd Input if we don't want arrow key control.
        {
            bRightReq = true;
        }


    }

    void FixedUpdate()
    {
        vDirModifier.x = 0;

        if (bleftReq)
        {
            bleftReq = false;
            vDirModifier.x -= fSpeed;
        }

        if (bRightReq)
        {
            bRightReq = false;
            vDirModifier.x += fSpeed;
        }
        rb.velocity = vDirModifier;
    }

}
