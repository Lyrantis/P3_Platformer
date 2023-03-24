using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

    public int gemValue = 1;
    private int scoreValue = 1000;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BetterCharacterController>().AddGem(gemValue);
            UIManager.Instance.AddScore(scoreValue);
            Destroy(this.gameObject);
            Destroy(this);
        }
    }
}
