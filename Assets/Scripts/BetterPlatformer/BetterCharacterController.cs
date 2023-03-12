using UnityEngine;
using System.Collections;


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


    bool crouched;
    public float crouchSpeed;

    public float slideSpeed;
    public float slideDuration;
    bool sliding;

    public float health = 100;
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

    public int score = 0;

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
    }

    void FixedUpdate()
    {
        //Box Overlap Ground Check
        Vector2 boxCenter = new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y);
        grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundedLayers) != null;
        anim.SetBool("Grounded", grounded);

        Debug.Log(grounded);
        if (grounded && jumping)
        {
            jumping = false;
            anim.SetBool("Jumping", false);
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
            Debug.Log(Time.fixedDeltaTime);
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && sliding == false) 
            {

                if (currentSpeed > 0 || currentSpeed < 0)
                {
                    CharacterSlide();
                }
            } 

            if (Input.GetKeyDown(KeyCode.S))
            {
                crouched = true;
                anim.SetBool("Crouched", true);
            }
        }

        if (crouched)
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                crouched = false;
                anim.SetBool("Crouched", false);
            }
        }

        //Input for jumping ***Multi Jumping***
        if (Input.GetButtonDown("Jump") && currentjumpCount > 1)
        {
            jumped = true;
            currentjumpCount--;
        }

        //Get Player input 
        horizInput = Input.GetAxis("Horizontal");     
    }

    // Flip Character Sprite
    void FlipSprite()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void CharacterSlide()
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

    IEnumerator CancelSlide()
    {
        yield return new WaitForSeconds(slideDuration);
        sliding = false;
        anim.SetBool("Sliding", false);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }


    private void OnDrawGizmos()
    {
        if (grounded)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y), boxSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y), boxSize);
        }
    }

}
