using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{

    public int GemsRequired = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            if (other.GetComponent<BetterCharacterController>().GetGemCount() >= GemsRequired)
            {
                //Game won
            } 
            else
            {
                //Tell player to go find more gems
            }
        }
    }
}
