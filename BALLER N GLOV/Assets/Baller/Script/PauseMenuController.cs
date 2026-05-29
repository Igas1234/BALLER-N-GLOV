using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuController : MonoBehaviour
{
    [Header("Panel Pause")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        // Saat game mulai, pause panel disembunyikan
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        // Tekan ESC untuk pause / lanjut
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                StartGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        // Membekukan game
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Melanjutkan game
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Pastikan waktu normal lagi sebelum reload scene
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Keluar dari game");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}