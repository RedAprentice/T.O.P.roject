using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Variables
    public Rigidbody2D player; // Player Object
    public float movementFactor = 5; // movement speed modifier
    public float jumpFactor = 30; // jump height modifier
    public float gravity = 10;
    public string xControl = "Horizontal";
    public string yControl = "Vertical";
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

    private void FixedUpdate()
    {
        CharacterChecks();
    }

    void CharacterChecks()
    {
        Collider2D[] collided = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, m_GroundLayers);
        for (int i = 0; i < collided.Length; i++)
        {
            if (collided[i].gameObject != gameObject)
            {
                // do something later
                isGrounded = true;
                // TEST
                Debug.Log("Hit");
            }
        }
    }
    
}
