using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public RectTransform mainPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            mainPanel.localScale = new Vector3(0, 0, 0);
            mainPanel.LeanScale(new Vector3(1, 1, 1), 1.5f).setEaseInOutElastic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToScore()
    {
        SceneManager.LoadScene("ScoreMenu");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
