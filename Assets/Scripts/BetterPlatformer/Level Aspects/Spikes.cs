using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private bool playerOnSpikes = false;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerOnSpikes)
        {
            player.GetComponent<HealthComponent>().TakeDamage(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
 
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerOnSpikes = true;
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        playerOnSpikes = false;
    }
}
