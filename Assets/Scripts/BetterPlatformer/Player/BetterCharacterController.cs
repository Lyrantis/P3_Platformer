using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

//--------------------------------------------
/*Better Character Controller Includes:
     - Fixed Update / Update Input seperation
     - Better grounding using a overlap box
     - Basic Multi Jump
 */
//--------------------------------------------
public class BetterCharacterController : MonoBehaviour
{
    protected bool facingRight = true;
    protected bool jumped;
    bool jumping = false;
    public int maxJumps;
    protected int currentjumpCount;

    public Transform RespawnPoint;

    bool crouched;
    public float crouchSpeed;

    public float slideSpeed;
    public float slideDuration;
    bool sliding = false;

    bool takingDamage = false;
    float iFrames = 2.0f;

    public bool running;
    public float speed = 5.0f;
    public float jumpForce = 1000;
    float currentSpeed = 0.0f;

    private float horizInput;

    public bool grounded;

    public Rigidbody2D rb;

    public LayerMask groundedLayers;

    protected Collider2D charCollision;
    protected Vector2 playerSize, boxSize;
    protected Animator anim;

    PlayerInputActions playerInputActions;

    private int gemCount = 0;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charCollision = GetComponent<Collider2D>();
        playerSize = charCollision.bounds.extents;
        boxSize = new Vector2(playerSize.x, 0.05f);

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Crouch.performed += Crouch;
        playerInputActions.Player.Crouch.canceled += Crouch;
        playerInputActions.Player.Slide.performed += Slide;
        playerInputActions.Player.Slide.canceled += Slide;
    }

    void FixedUpdate()
    {

        horizInput = playerInputActions.Player.Movement.ReadValue<float>();

        //Box Overlap Ground Check
        Vector2 boxCenter = new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y);
        grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundedLayers) != null;
        anim.SetBool("Grounded", grounded);

        if (!grounded && rb.velocity.y < 0)
        {
            anim.SetBool("Falling", true);
            anim.SetBool("Jumping", false);
        }
        

        if (grounded)
        {
            anim.SetBool("Falling", false);
            
            if (jumping)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            } 
            
        }

        if (horizInput != 0)
        {
            running = true;
        }
        else
        {
            running = false;
        }

        anim.SetBool("Running", running);


        if (crouched)
        {
            //Move Character
            rb.velocity = new Vector2(horizInput * speed * crouchSpeed * Time.fixedDeltaTime, rb.velocity.y);
            currentSpeed = rb.velocity.x;
            anim.SetFloat("currentSpeed", currentSpeed);
        }
        else
        {
            //Move Character
            rb.velocity = new Vector2(horizInput * speed * Time.fixedDeltaTime, rb.velocity.y);
            currentSpeed = rb.velocity.x;
            anim.SetFloat("currentSpeed", currentSpeed);
        }
        
        //Jump
        if (jumped == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(new Vector2(0f, jumpForce));
            jumping = true;
            anim.SetBool("Jumping", true);

            jumped = false;
        }

        // Detect if character sprite needs flipping.
        if (horizInput > 0 && !facingRight)
        {
            FlipSprite();
        }
        else if (horizInput < 0 && facingRight)
        {
            FlipSprite();
        }
    }

    void Update()
    {
        if (grounded)
        {
            currentjumpCount = maxJumps;
        }

    }

    // Flip Character Sprite
    void FlipSprite()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    IEnumerator CancelSlide()
    {
        yield return new WaitForSeconds(slideDuration);
        sliding = false;
        anim.SetBool("Sliding", false);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizInput = context.ReadValue<float>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumping!");

        if (context.performed && currentjumpCount > 1)
        {
            jumped = true;
            currentjumpCount--;
        }
        
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            crouched = true;
            anim.SetBool("Crouched", true);
        }
        else if (context.canceled)
        {
            crouched = false;
            anim.SetBool("Crouched", false);
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {

        sliding = true;
        anim.SetBool("Sliding", true);

        if (facingRight)
        {
            rb.AddForce(Vector2.right * slideSpeed);
        }
        else
        {
            rb.AddForce(Vector2.left * slideSpeed);
        }
        StartCoroutine(CancelSlide());

    }

    public void TakeDamage()
    {

        takingDamage = true;
        anim.SetBool("Damaged", true);
        playerInputActions.Disable();

        StartCoroutine(EndIFrames(iFrames / 2));
    }

    IEnumerator EndIFrames(float iFrameTime)
    {

        yield return new WaitForSeconds(iFrameTime);

        playerInputActions.Enable();
        anim.SetBool("Damaged", false);

        StartCoroutine(BecomeVulnerable(iFrameTime));


    }

    IEnumerator BecomeVulnerable(float iFrameTime)
    {

        yield return new WaitForSeconds(iFrameTime);

        takingDamage = false;
    }

    public int GetGemCount()
    {
        return gemCount;
    }

    public void AddGem(int gemValue)
    {
        gemCount += gemValue;
    }

    private void OnDrawGizmos()
    {
        //if (grounded)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawCube(new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y), boxSize);
        //}
        //else
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawCube(new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y), boxSize);
        //}
    }

  

}

