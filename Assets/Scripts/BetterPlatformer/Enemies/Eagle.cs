using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{

    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    [SerializeField] bool facingRight;
    [SerializeField] float moveSpeed;
    [SerializeField] float timeBetweenTurn;
    [SerializeField] int damage;
    [SerializeField] Transform edgeCheckBox;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Vector2 edgeCheckOffset;

    Vector2 edgeCheckSize;
    Vector2 edgeCheckPos;
    Vector2 wallCheckPos;
    Vector2 wallCheckSize;
    float wallCheckOffset = 0.5f;

    bool edgeCheck;
    bool wallCheck;
    float direction;
    bool angry = false;
    public float angrySpeedMultiplier;
    public float AttackCooldown;
    bool ableToAttack = true;

    bool moving = true;
    bool swooping = false;

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
        wallCheckSize = new Vector2(0.5f, 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            edgeCheckPos = edgeCheckBox.position;
            wallCheckPos = new Vector2(transform.position.x + (direction * wallCheckOffset), transform.position.y);

            edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
            wallCheck = Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0, collisionLayer);

            if (!edgeCheck || wallCheck)
            {
                moving = false;

                rb.velocity = new Vector2(0.0f, 0.0f);
                StartCoroutine(WaitAtEdge(timeBetweenTurn));
            }
        }
    }

    private void FixedUpdate()
    {

        if (moving)
        {
            if (angry)
            {
                rb.velocity = new Vector2(direction * moveSpeed * angrySpeedMultiplier, rb.velocity.y);
            }
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }

    }

    void ChangeDirection()
    {
        if (direction == 1)
        {
            direction = -1;
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
            if (facingRight)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }

        edgeCheckBox.localPosition = new Vector2(direction * edgeCheckOffset.x, edgeCheckOffset.y);

        moving = true;
    }

    IEnumerator WaitAtEdge(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ChangeDirection();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        angry = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        angry = false;
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
            Gizmos.DrawCube(edgeCheckBox.position, edgeCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(edgeCheckBox.position, edgeCheckSize);
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
