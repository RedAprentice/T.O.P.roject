using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    #region *Generic Variables
    // General Movement Variables
    [SerializeField] private Rigidbody2D player; // Player Object
    [SerializeField] private BoxCollider2D boxCollider2D; // Player Collider
    [SerializeField] private float movementFactor = 5; // movement speed modifier
    [SerializeField] private float jumpFactor = 30; // jump height modifier
    [SerializeField] private float gravity = 10;
    [SerializeField] private string xControl = "Horizontal";
    [SerializeField] private string yControl = "Vertical";
    #endregion

    #region *Ground Check Variables
    private bool isGrounded; // true if touching ground
    [SerializeField] private LayerMask m_GroundLayers; // Layers isGrounded interacts with
    [SerializeField] private float groundBufferSpace = 0.5f; // extra space below the player can jump
    #endregion

    #region *Grappling Hook Variables
    [SerializeField] private GameObject gHook;
    [SerializeField] private float hookMaxDistance = 30.0f;
    [SerializeField] private float hookSpeedFactor = 20f;
    [SerializeField] private float hookPullForceFactor = 15f;
    private float hookTravelDistance;
    [HideInInspector] public Vector2 hookDirection; // this will be changed by gHookTrigger.cs
    [HideInInspector] public bool hookOut; // this will be changed by gHookTrigger.cs
    [HideInInspector] public bool hookGrappled; // this will be changed by gHookTrigger.cs
                                                // NOTE: Should later rewrite into state machine probably.
    [HideInInspector] public bool endOfGrapple;
    [HideInInspector] public Vector2 hookHitLocation;
    #endregion

    #region *Input Buffering Variables
    // Input Buffering Variables
    [SerializeField] private string hookButton = "Fire1";
    private bool bufHook;
    [SerializeField] private string dodgeButton = "Fire2";
    private bool bufDodge;
    [SerializeField] private string otherThing;
    private bool bufOther;
    #endregion

    #region *Unity's Basic Stuff

    // Start is called before the first frame update
    void Start()
    {
        player.gravityScale = gravity;
        hookOut = false;
        hookGrappled = false;
        endOfGrapple = false;
        gHook.transform.position = Vector2.down * 1000;
    }

    // Update is called once per frame
    void Update()
    {

        // NOTE FOR THE FUTURE
        // Implement buffers so that movement can be restricted during abilities

        basicMovement();

        if ( Input.GetButtonDown(hookButton) ){
            bufHook = true;
        }

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

    #endregion

    #region *Basic Movement
    private void basicMovement()
    {

        Vector2 curVelocity = player.velocity;
        Vector2 pVelocity;

        float xInput = Input.GetAxis(xControl); // I don't know if this will work, but hey.
        float yInput = Input.GetAxis(yControl);

        // Horizontal Movement
        if (xInput != 0)
        {
            pVelocity.x = xInput * movementFactor;
            pVelocity.y = curVelocity.y;
            player.velocity = pVelocity;
        }

        // Jumping on positive vertical input
        if (yInput > 0)
        {
            // jumping will release the grapple if player is being dragged by grapple
            // NOTE: make it smoother and have an animation later
            if (hookGrappled)
            {
                gHook.transform.position = player.transform.position;
            }


            if (isGrounded)
            {
                // might want to snap player y location before starting jump
                isGrounded = false;
                pVelocity.y = jumpFactor;
                pVelocity.x = curVelocity.x;
                player.velocity = pVelocity;
            }

        }

    }

    private void characterChecks()
    {
        RaycastHit2D collided = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, groundBufferSpace, m_GroundLayers);

        if (collided.collider != null)
        {
            // do something later
            isGrounded = true;
            // TEST
            Debug.Log(collided.collider);
        }
    }

    #endregion

    #region *Grappling Hook

    private void shootGrapplingHook()
    {
        if (hookOut == false)
        {
            if ( hookGrappled == false )
            {
                // activate the hook
                hookOut = true;
                hookTravelDistance = 0;

                // set the hook location to our location
                gHook.transform.position = transform.position;

                // save the direction
                // NOTE: big sad!
                //       when buffering save the location of the mouse when the button was hit ???
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
            hookCharPull();
        }
    }

    // Needs to be simple to ensure predictability
    private void hookTravel()
    {
        // move the hook towards the direction it needs to go.
        gHook.transform.position = (Vector2) gHook.transform.position + hookDirection * hookSpeedFactor * Time.deltaTime;
        hookTravelDistance += hookSpeedFactor * Time.deltaTime;
        Debug.Log(hookTravelDistance);
        if (hookTravelDistance >= hookMaxDistance)
        {
            grappleRelease();
        }
    }

    // Pulls to hit location. During travel, needs to be able to navigate angled hits with walls smoothly.
    private void hookCharPull()
    {
        // move the player to the hooked location
        // If it is inside of the player, or at default location, we don't want to calculate the velocity.
        // This avoids errors.
        // NOTE: Fly to the tile grabbed, but the last part of the
        //       movement might need to be done "strangely". Hopefully not
        // NOTE 2: The last part might just instantly transition to a hooked
        //         wall grab.
        if (gHook.transform.position.y != -1000 && gHook.transform.position.y != transform.position.y)
        {
            hookDirection = (gHook.transform.position - transform.position);
            hookDirection /= hookDirection.magnitude;

            // transform.position = (Vector2)transform.position + hookDirection * hookPullForceFactor * Time.deltaTime;
            // or
            // player.AddForce(hookDirection * hookPullForceFactor);
            // or
            // player.velocity = /* MAXofMagnitude( Velocity + Hook Pull , Max Speed adjusted for direction ) NOTE: sqrMagnitude exists for cheap comparisons */
            player.velocity = hookDirection * hookPullForceFactor;

        }

        // when we reach the destination
        if ( endOfGrapple )
        {
            // NOTE: Will likely change drastically.
            grappleRelease();
        }
    }

    private void grappleRelease() // might need more steps?
    {
        hookOut = false;
        hookGrappled = false;
        endOfGrapple = false;
        player.gravityScale = gravity;
        gHook.transform.position = transform.position;
        gHook.transform.position = Vector2.down * 1000;
    }

    #endregion
}
