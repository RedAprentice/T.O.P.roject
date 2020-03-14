using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // General Movement Variables
    [SerializeField] private Rigidbody2D player; // Player Object
    [SerializeField] private BoxCollider2D boxCollider2D; // Player Collider
    [SerializeField] private float movementFactor = 5; // movement speed modifier
    [SerializeField] private float jumpFactor = 30; // jump height modifier
    [SerializeField] private float gravity = 10;
    [SerializeField] private string xControl = "Horizontal";
    [SerializeField] private string yControl = "Vertical";

    // Ground Check Variables
    private bool isGrounded; // true if touching ground
    [SerializeField] private LayerMask m_GroundLayers; // Layers isGrounded interacts with
    [SerializeField] private float bufferSpace = 0.5f; // extra space below the player can jump

    // Grappling Hook Variables
    [SerializeField] private GameObject gHook;
    [SerializeField] private float hookSpeedFactor = 1f;
    [SerializeField] private float hookPullForceFactor = 1f;
    private Vector2 hookDirection;
    private bool hookOut;
    private bool hookGrappled;

    // Movement Buffering Variables
    [SerializeField] private string hookButton = "Fire1";
    private bool bufHook;
    [SerializeField] private string dodgeButton = "Fire2";
    private bool bufDodge;
    [SerializeField] private string otherThing;
    private bool bufOther;

    // Start is called before the first frame update
    void Start()
    {
        player.gravityScale = gravity;
        hookOut = false;
    }

    // Update is called once per frame
    void Update()
    {

        // NOTE FOR THE FUTURE
        // Implement buffers so that movement can be restricted during abilities

        basicMovement();

        if ( bufHook )
        {
            bufHook = false;
            shootGrapplingHook();
        }

        gHookMovement();

    }

    private void FixedUpdate()
    {
        characterChecks(); // idk if this should be here. As of here for cleaniness.
    }

    private void basicMovement()
    {

        Vector2 curVelocity = player.velocity;
        Vector2 pVelocity;

        float xInput = Input.GetAxis(xControl); // I don't know if this will work, but hey.
        float yInput = Input.GetAxis(yControl);

        if (xInput != 0)
        {
            pVelocity.x = xInput * movementFactor;
            pVelocity.y = curVelocity.y;
            player.velocity = pVelocity;
        }

        if (yInput > 0 && isGrounded == true)
        {
            isGrounded = false;
            pVelocity.y = jumpFactor;
            pVelocity.x = curVelocity.x;
            player.velocity = pVelocity;
        }

    }

    private void characterChecks()
    {
        RaycastHit2D collided = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, bufferSpace, m_GroundLayers);

        if (collided.collider != null)
        {
            // do something later
            isGrounded = true;
            // TEST
            Debug.Log(collided.collider);
        }
    }
    
    private void shootGrapplingHook()
    {
        if (hookOut == false)
        {
            if ( hookGrappled == false )
            {
                // activate the hook
                hookOut = true;
                // save the direction
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 curPos = transform.position;

                hookDirection = mouse - curPos;
                hookDirection = hookDirection / hookDirection.magnitude; // Intensive because sqrt. Maybe look for another way later?
            } else // when hookGrappled is true
            {
                // release the grapple
                grappleRelease();
            }
            
        }

    }

    private void gHookMovement ()
    {
        if (hookOut)
        {
            hookTravel();
        }

        if (hookGrappled)
        {
            // move the player to the hooked location
            // NOTE: Fly to the tile grabbed, but the last part of the
            //       movement might need to be done "strangely". Hopefully not

            transform.position = (Vector2)transform.position + hookDirection*hookSpeedFactor*Time.deltaTime;
            // or
            // player.AddForce(hookDirection * hookPullForceFactor);
        }
    }

    private void hookTravel()
    {
        // move the hook around until it hits something

        if (/* we hit something */ true) // probably going to be moved since it needs onTriggerEnter()
        {
            // when it hits
            hookOut = false;
            hookGrappled = true;
            player.gravityScale = 0; // turn off gravity while we are flying
            // hookDirection = transform.position - /* middle of the tile we are going to snap to*/ ;
        }

    }

    private void grappleRelease() // might need more steps?
    {
        hookGrappled = false;
        player.gravityScale = gravity;
    }
}
