using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should only trigger when the Grappling Hook hits something.

public class gHookTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float hookRaycastExtend = 0.42f; // This number is round up from the square root of 2
    [SerializeField] private LayerMask hookMask;
    [SerializeField] private float colliderSize = 0.25f;
    [SerializeField] private float detachDistance = 0.05f;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = colliderSize;
    }

    // Can also be read "when hook hits something"
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.tag == "Other")
        {
            Debug.Log("We managed to hit: ", other);
            player.GetComponent<playerMovement>().hookOut = false;
            player.GetComponent<playerMovement>().hookGrappled = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;

            // player.GetComponent(playerMovement>().hookDirection = transform.position - /* middle of the tile we are going to snap to OR hit location */ ;
            // The direction will be from player's position to the snapping point.
            // The snapping point will be gotten by sending a raycast in a direction.
            // Hierarchy of direction: Direction the grappling is going, 30 degrees to each side, 60 degrees to each side, 90 degrees to each side.
            // This raycasts should only extend slightly ahead of the 

            RaycastHit2D wallEdge = findWallEdge(player.GetComponent<playerMovement>().hookDirection);
            player.GetComponent<playerMovement>().hookHitLocation = wallEdge.point;

            // Now we need to change hook direction to match with the new location.
            // NOTE: Yes, messy. Yes could be made much easier to read. No. I won't. I'm tired.
            player.GetComponent<playerMovement>().hookDirection = (wallEdge.point - player.GetComponent<Rigidbody2D>().position);
            player.GetComponent<playerMovement>().hookDirection /= player.GetComponent<playerMovement>().hookDirection.magnitude;

            // Make the collider of the hook inside of the wall
            transform.position = wallEdge.point;
            GetComponent<CircleCollider2D>().radius = detachDistance;
        }
        if(other.tag == "Player" && player.GetComponent<playerMovement>().hookGrappled)
        {
            player.GetComponent<playerMovement>().endOfGrapple = true;
            GetComponent<CircleCollider2D>().radius = colliderSize;
        }
    }

    private RaycastHit2D findWallEdge( Vector2 hookDirection )
    {
        RaycastHit2D hitInfo = new RaycastHit2D();
        for (int i = 0; i < 7; i++) // 7 is covers 180 degrees. 13 covers 360 degrees.
        {
            Vector2 localHookDir;
            int angleAdjust;

            if (i % 2 == 0) // Even
            {
                angleAdjust = 30 * (i / 2);
            }
            else // Odd
            {
                angleAdjust = -30 * ( (i + 1) / 2 );
            }

            float sin = Mathf.Sin(angleAdjust * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angleAdjust * Mathf.Deg2Rad);

            localHookDir.x = cos * hookDirection.x - sin * hookDirection.y;
            localHookDir.y = sin * hookDirection.y + cos * hookDirection.x;

            hitInfo = Physics2D.Raycast(transform.position, localHookDir, GetComponent<CircleCollider2D>().radius + hookRaycastExtend, hookMask);

            if (hitInfo)
            {
                return hitInfo;
            }
        }

        Debug.LogError("Hook's Raycasts didn't hit anything. False detection or Increase hookRaycastExtend.");
        return hitInfo;
    }
}
