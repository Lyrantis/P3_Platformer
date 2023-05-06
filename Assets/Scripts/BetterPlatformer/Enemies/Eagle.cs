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
    [SerializeField] GameObject player;
    [SerializeField] GameObject visionBox;

    private Vector2 startPos;

    Vector2 edgeCheckSize;
    Vector2 edgeCheckPos;
    Vector2 wallCheckPos;
    Vector2 wallCheckSize;
    float wallCheckOffset = 0.5f;

    bool edgeCheck;
    bool wallCheck;
    float direction;
    bool angry = false;
    bool returningToStart = false;
    public float angrySpeedMultiplier;
    public float AttackCooldown;
    private float attackRange = 1.0f;
    bool ableToAttack = true;

    bool moving = true;
    bool swooping = false;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        startPos = transform.position;
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
            if (angry)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * angrySpeedMultiplier * Time.deltaTime);

                float dist = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y).magnitude;

                Debug.Log(dist);
                if (dist <= attackRange)
                {
                    player.GetComponent<HealthComponent>().TakeDamage(1);
                    SetAngry(false);
                }
            }
            else if (returningToStart)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);

                if (transform.position.x - startPos.x > 0 && facingRight)
                {
                    ChangeDirection();
                }
                else if (transform.position.x - startPos.x <  0 && !facingRight)
                {

                }

                if (transform.position.x == startPos.x && transform.position.y == startPos.y)
                {
                    returningToStart = false;
                }
            }
            else
            {
                edgeCheckPos = edgeCheckBox.position;
                wallCheckPos = new Vector2(transform.position.x + (direction * wallCheckOffset), transform.position.y);

                edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
                wallCheck = Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0, collisionLayer);

                if (!edgeCheck || wallCheck)
                {
                    moving = false;
;
                    StartCoroutine(WaitAtEdge(timeBetweenTurn));
                }
                else
                {
                    transform.position = new Vector2(transform.position.x + (moveSpeed * direction * Time.deltaTime), transform.position.y);
                }
            }
        }
    }

    private void FixedUpdate()
    {

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

        visionBox.transform.localPosition = new Vector2(visionBox.transform.position.x * -1, visionBox.transform.position.y);
        edgeCheckBox.localPosition = new Vector2(direction * edgeCheckOffset.x, edgeCheckOffset.y);

        moving = true;
    }

    IEnumerator WaitAtEdge(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ChangeDirection();

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

    public void SetAngry(bool newValue)
    {
        angry = newValue;

        if (!angry)
        {
            returningToStart = true;
        }
    }

}
