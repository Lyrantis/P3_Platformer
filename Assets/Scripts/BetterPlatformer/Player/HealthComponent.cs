using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{

    public int MaxHealth;
    private int currentHealth;
    public float iFrameTime = 2.0f;
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
            StartCoroutine(IFrames(iFrameTime));

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
 
            }
            else
            {
                

                if (gameObject.tag == "Player")
                {
                    UIManager.Instance.RemoveHeart();
                    gameObject.GetComponent<BetterCharacterController>().TakeDamage();
                }
            }
        }
        
    }

    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            gameObject.transform.position = gameObject.GetComponent<BetterCharacterController>().RespawnPoint.position;
            currentHealth = MaxHealth;
            UIManager.Instance.ResetHearts();
            UIManager.Instance.RemoveLife();
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
        UIManager.Instance.AddHeart();

        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
            
        }
    }

    IEnumerator IFrames(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentHealth != 0)
        {
            takingDamage = false;
        }
        

    }

}
