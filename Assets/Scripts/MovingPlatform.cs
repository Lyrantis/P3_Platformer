using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    Vector2 startPos;
    Vector2 endPos;
    public Transform endPosObject;
    public float speed;

    int direction = 1;

    public float waitTime;
    bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = endPosObject.transform.position;

    }

    // Update is called once per frame

    IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        direction *= -1;
        waiting = false;
    }

    private void FixedUpdate()
    {
        if (!waiting)
        {
            if (direction == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, endPos, speed * Time.fixedDeltaTime);

                if (transform.position.x == endPos.x && transform.position.y == endPos.y)
                {
                    waiting = true;
                    StartCoroutine(Wait(waitTime));
                }

            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.fixedDeltaTime);

                if (transform.position.x == startPos.x && transform.position.y == startPos.y)
                {
                    waiting = true;
                    StartCoroutine(Wait(waitTime));
                }
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(this.gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
