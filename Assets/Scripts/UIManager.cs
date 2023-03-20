using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public List<GameObject> hearts;
    private int score = 0;
    private int health = 3;

    private static UIManager instance;

    public static UIManager Instance
    {
      get { return instance; }
    }

     
    private void Awake()
    {

        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score.ToString();
    }

    public void RemoveHeart()
    {

        hearts[health - 1].SetActive(false);
        health -= 1;
    }

    public void AddHeart()
    {
        hearts[health].SetActive(true);
        health += 1;
    }

    public void ResetHearts()
    {
        health = 3;
        
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(true);
        }
    }

}
