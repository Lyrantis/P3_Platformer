using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{

    public int MaxHealth;
    private int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (gameObject.tag == "Player")
        {
            gameObject.transform.position = gameObject.GetComponent<BetterCharacterController>().RespawnPoint.position;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void Heal(int Healing)
    {
        currentHealth += Healing;

        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
    }
}
