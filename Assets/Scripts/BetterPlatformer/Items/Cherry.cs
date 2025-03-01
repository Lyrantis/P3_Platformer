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
        
        if (other.tag == "Player")
        {
            UIManager.Instance.AddScore(scoreValue);
            UIManager.Instance.AddHeart();
            anim.SetBool("Collected", true);
            StartCoroutine(Disappear(0.5f));
        }
       
    }
}
