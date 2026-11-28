using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("All Menu")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    public GameObject ObjectiveMenuUI;

    public static bool GameIsStopped = false;

    private bool isPaused;
    private bool showingObjectives;

    private void Start()
    {
        Time.timeScale = 1f;
        GameIsStopped = false;
        isPaused = false;
        showingObjectives = false;
        SetCursor(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused || showingObjectives) Resume();
            else Pause();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPaused) return;

            if (showingObjectives) removeObjectives();
            else ShowObjectives();
        }
    }

    public void ShowObjectives()
    {
        if (isPaused) return;

        showingObjectives = true;
        if (ObjectiveMenuUI != null) ObjectiveMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsStopped = true;
        SetCursor(true);
    }

    public void removeObjectives()
    {
        showingObjectives = false;
        if (ObjectiveMenuUI != null) ObjectiveMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsStopped = false;
        SetCursor(false);
    }

    public void Resume()
    {
        isPaused = false;
        showingObjectives = false;

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (ObjectiveMenuUI != null) ObjectiveMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsStopped = false;
        SetCursor(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameIsStopped = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsStopped = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void Pause()
    {
        isPaused = true;

        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (ObjectiveMenuUI != null) ObjectiveMenuUI.SetActive(false);

        Time.timeScale = 0f;
        GameIsStopped = true;
        SetCursor(true);
    }

    private void SetCursor(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        GameIsStopped = false;
    }
}