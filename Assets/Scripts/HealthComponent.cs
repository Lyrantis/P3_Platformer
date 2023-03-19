using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{

    public int MaxHealth;
    private int currentHealth;

    bool takingDamage = false;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int Damage)
    {

        if (!takingDamage)
        {
            currentHealth -= Damage;
            takingDamage = true;
            Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                

                if (gameObject.tag == "Player")
                {
                    Debug.Log("HERE");
                    gameObject.GetComponent<BetterCharacterController>().TakeDamage();
                }
            }
        }
        
    }

    public void StopTakingDamage()
    {
        takingDamage=false;
    }

    void Die()
    {
        if (gameObject.tag == "Player")
        {
            gameObject.transform.position = gameObject.GetComponent<BetterCharacterController>().RespawnPoint.position;
            currentHealth = MaxHealth;
            takingDamage = false;
        }
        else
        {
            Destroy(gameObject);
            Destroy(this);
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
