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
        GameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
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
