using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    private bool hit = false;
    private void OnEnable()
    {
        hit = false;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hit)
        {
            collision.GetComponent<HealthComponent>().TakeDamage(1);
            hit = true;
        }
    }
}
