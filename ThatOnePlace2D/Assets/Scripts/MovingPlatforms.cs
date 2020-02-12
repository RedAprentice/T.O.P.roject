using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Vector3 velocity;
    public float platformSpeed;

    bool moving;

    private void OnCollisionEnter(Collision2D collision)
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (velocity * Time.deltaTime * platformSpeed);
    }
}
