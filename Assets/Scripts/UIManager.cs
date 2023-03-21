using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public List<GameObject> hearts;
    public List<GameObject> lives;
    private int score = 0;
    private int health = 3;
    private int lifeCount = 3;

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

    public void RemoveLife()
    {
        lives[lifeCount - 1].SetActive(false);
        lifeCount -= 1;

        if (lifeCount <= 0)
        {
            GameOver();
        }
    }

    public void AddLife()
    {
        if (lifeCount !> 3)
        {
            lives[lifeCount].SetActive(true);
            lifeCount += 1;
        }
        
    }

    public void ResetLives()
    {

        lifeCount = 3;
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].SetActive(true);
        }
    }

    public void StartGame()
    {
        Debug.Log("Should Start");
        SceneManager.LoadScene("Scenes/ParallaxPlatformer", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Scenes/ParallaxPlatformer", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
