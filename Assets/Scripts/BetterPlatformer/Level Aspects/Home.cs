using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{

    public int GemsRequired = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            if (other.GetComponent<BetterCharacterController>().GetGemCount() >= GemsRequired)
            {
                SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
            } 
            else
            {
                UIManager.Instance.DisplayMessage();
            }
        }
    }
}
