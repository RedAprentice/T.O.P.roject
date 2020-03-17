using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private Rigidbody2D player; // Player Object
    [SerializeField] private BoxCollider2D boxCollider2D; // Player Collider
    [SerializeField] private float movementFactor = 5; // movement speed modifier
    [SerializeField] private float jumpFactor = 30; // jump height modifier
    [SerializeField] private float bufferSpace = 0.5f; // extra space above the player can jump
    [SerializeField] private float gravity = 10;
    [SerializeField] private string xControl = "Horizontal";
    [SerializeField] private string yControl = "Vertical";
    [SerializeField] private LayerMask m_GroundLayers;
    private float xInput;
    private float yInput;
    private Vector2 pVelocity;
    private Vector2 curVelocity;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    const float groundRadius = 0.05f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player.gravityScale = gravity;

    }

    // Update is called once per frame
    void Update()
    {
        BasicMovement();



    }

    private void FixedUpdate()
    {
        CharacterChecks(); // idk if this should be here. As of here for cleaniness.
    }

    private void BasicMovement()
    {

        curVelocity = player.velocity;

        xInput = Input.GetAxis(xControl); // I don't know if this will work, but hey.
        yInput = Input.GetAxis(yControl);

        if (xInput != 0)
        {
            pVelocity.x = xInput * movementFactor;
            pVelocity.y = curVelocity.y;
            player.velocity = pVelocity;
        }

        if (yInput != 0 && isGrounded == true)
        {
            isGrounded = false;
            pVelocity.y = jumpFactor;
            pVelocity.x = curVelocity.x;
            player.velocity = pVelocity;
        }

    }

    void CharacterChecks()
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
    
}
