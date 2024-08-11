using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("All Menu")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    public GameObject ObjectiveMenuUI;

    public static bool GameIsStopped = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsStopped)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if(Input.GetKeyDown("m"))
        {
            if(GameIsStopped)
            {
                removeObjectives();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                ShowObjectives();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ShowObjectives()
    {
        ObjectiveMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsStopped = true;
    }

    public void removeObjectives()
    {
        ObjectiveMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsStopped = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsStopped = true;
    }
}
