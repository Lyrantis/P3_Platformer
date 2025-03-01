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
    [SerializeField] int damage;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Transform attackBoxPos;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Vector2 edgeCheckOffset;

    Vector2 edgeCheckPos;
    Vector2 edgeCheckSize;
    Vector2 groundCheckSize;

    Vector2 wallCheckPos;
    Vector2 wallCheckSize;
    float wallCheckOffset = 0.5f;

    bool edgeCheck;
    bool groundCheck;
    bool wallCheck;
    float direction;
    float attackTime = 0.2f;
    bool playerInRange = false;
    bool ableToAttack = true;
    public float AttackCooldown;

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
        wallCheckSize = new Vector2(0.5f, 0.5f);

        anim.SetBool("Moving", true);

    }

    // Update is called once per frame
    void Update()
    {

        if (moving)
        {
            edgeCheckPos = new Vector2(transform.position.x + (direction * edgeCheckOffset.x), transform.position.y - edgeCheckOffset.y);
            wallCheckPos = new Vector2(transform.position.x + (direction * wallCheckOffset), transform.position.y);

            edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
            groundCheck = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, collisionLayer) != null;
            wallCheck = Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0, collisionLayer);

            if (groundCheck && (!edgeCheck || wallCheck))
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
        if (other.tag == "Player" && ableToAttack)
        {
            anim.SetBool("Attacking", true);
            anim.SetBool("Moving", false);
            playerInRange = true;

            if (moving)
            {
                moving = false;
                rb.velocity = new Vector2(0, 0);
                movingBeforeAttack = true;
            }

            ableToAttack = false;
            StartCoroutine(Attack(other, attackTime));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;  
        }
    }

    IEnumerator Attack(Collider2D other, float time)
    {

        yield return new WaitForSeconds(time);

        anim.SetBool("Attacking", false);
        if (playerInRange)
        {
            other.GetComponent<HealthComponent>().TakeDamage(damage);
            Attack(other, time);
        }

        if (movingBeforeAttack)
        {
            moving = true;
            anim.SetBool("Moving", true);
            movingBeforeAttack = false;
            StartCoroutine(Cooldown(AttackCooldown));
        }

        ableToAttack = true;

    }

    IEnumerator Cooldown(float cooldown)
    {

        yield return new WaitForSeconds(cooldown);

        ableToAttack = true;
    }

    private void OnDrawGizmos()
    {
        if (edgeCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
        }

        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
        }
        if (wallCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(wallCheckPos, wallCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(wallCheckPos, wallCheckSize);
        }

    }

    private void OnDestroy()
    {
        if (this.gameObject.scene.isLoaded)
        {
            UIManager.Instance.AddScore(100);
        }
        
    }

}
