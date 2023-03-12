using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobTheBlob : MonoBehaviour
{

    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    [SerializeField] bool facingRight;
    [SerializeField] float moveSpeed;
    [SerializeField] float timeBetweenTurn;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Transform attackBoxPos;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Vector2 edgeCheckOffset;

    Vector2 edgeCheckPos;
    Vector2 edgeCheckSize;
    Vector2 groundCheckSize;

    bool edgeCheck;
    bool groundCheck;
    float direction;
    float attackTime = 2.0f;

    bool moving = true;
    bool movingBeforeAttack = false;
    

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        
    
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!facingRight)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        edgeCheckSize = new Vector2(0.5f, 0.5f);
        groundCheckSize = new Vector2(0.25f, 0.25f);
        anim.SetBool("Moving", true);

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(moving);
        if (moving)
        {
            edgeCheckPos = new Vector2(transform.position.x + (direction * edgeCheckOffset.x), transform.position.y - edgeCheckOffset.y);

            edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
            groundCheck = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, collisionLayer) != null;

            if (groundCheck && !edgeCheck)
            {
                moving = false;
                anim.SetBool("Moving", false);

                rb.velocity = new Vector2(0.0f, 0.0f);
                StartCoroutine(WaitAtEdge(timeBetweenTurn));
            }
        }
        
    }

    private void FixedUpdate()
    {

        if (moving)
        {
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }
        
    }

    void ChangeDirection()
    {
        if (direction == 1)
        {
            direction = -1;
            attackBoxPos.transform.localPosition = new Vector2(-1, 0);
            if (facingRight)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        else
        {
            direction = 1;
            attackBoxPos.transform.localPosition = new Vector2(1, 0);
            if (facingRight)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }

        moving = true;
        anim.SetBool("Moving", true);
    }

    IEnumerator WaitAtEdge(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ChangeDirection();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("Attacking", true);
            anim.SetBool("Moving", false);

            if (moving)
            {
                Debug.Log("Stopping");
                moving = false;
                rb.velocity = new Vector2(0, 0);
                movingBeforeAttack = true;
            }

            other.GetComponent<BetterCharacterController>().TakeDamage(20.0f);
            StartCoroutine(Attack(attackTime));
        }
    }

    IEnumerator Attack(float time)
    {

        yield return new WaitForSeconds(time);

        anim.SetBool("Attacking", false);

        if (movingBeforeAttack)
        {
            moving = true;
            anim.SetBool("Moving", true);
            movingBeforeAttack = false;
        }

    }

    //private void OnDrawGizmos()
    //{
    //    if (edgeCheck)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
    //    }

    //    if (groundCheck)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    //    }
    //}

}
