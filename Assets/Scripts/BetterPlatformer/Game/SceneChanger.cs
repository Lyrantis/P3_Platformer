using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        canvas.gameObject.SetActive(false);
        SceneManager.LoadScene("ParallaxPlatformer", LoadSceneMode.Single);
        UIManager.Instance.ResetEverything();

    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
