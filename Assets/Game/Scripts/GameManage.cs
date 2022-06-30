using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public GameObject GameWinPanel;
    public GameObject GameOverPanel;
    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        GameWinPanel.SetActive(true);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }
}
