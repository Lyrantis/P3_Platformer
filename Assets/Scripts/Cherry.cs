using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{

    public int scoreValue = 100;
    protected Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    IEnumerator Disappear(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.tag);
        
        if (other.tag == "Player")
        {
            other.GetComponent<BetterCharacterController>().score += scoreValue;
            anim.SetBool("Collected", true);
            StartCoroutine(Disappear(0.5f));
        }
       
    }
}
