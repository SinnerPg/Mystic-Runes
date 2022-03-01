using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerManager player;
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        player.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("000");
        SceneManager.LoadScene("001",LoadSceneMode.Additive);
        TimerManager.m_timer = 0;
    }

    public void invertedStatus()
    {
        PlayerManager.invertedAxis = !PlayerManager.invertedAxis;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
