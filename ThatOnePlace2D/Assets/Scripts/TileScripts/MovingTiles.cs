using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTiles : MonoBehaviour
{
    public Vector3 velocity; //The direction at which tile moves
    public float tileSpeed;
    public float moveBackTimer;

    public bool moving = true; //Enable or Disable moving before player touchs tile
    public bool moveBackAndForth = false;
    bool moveBack = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) //Checks to see if it is player layer
        {
            moving = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) //Checks to see if it is player layer
        {
            collision.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
    }

    void Move()
    {
        if (Time.time % moveBackTimer == 0 && moveBackAndForth)
        {
            Debug.Log("Time at " + Time.time);
            moveBack = !moveBack;
        }
        if (moveBack)
            transform.position += (-velocity * Time.deltaTime * tileSpeed);
        else
            transform.position += (velocity * Time.deltaTime * tileSpeed);
    }
}
