// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// public class InGameUI : MonoBehaviour
// {
//     [Header("Health UI")]
//     public PlayerController player;
//     public Image[] heartImages;
//     public Sprite fullHeart;
//     public Sprite emptyHeart;

//     [Header("Pause UI")]
//     public GameObject pauseMenuUI;
//     private bool isPaused = false;

//     void Start()
//     {
//         Time.timeScale = 1f;

//         if (pauseMenuUI != null)
//         {
//             pauseMenuUI.SetActive(false);
//         }

//         if (player == null)
//         {
//             GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

//             if (playerObj != null)
//             {
//                 player = playerObj.GetComponent<PlayerController>();
//             }
//         }

//         UpdateHealthUI();
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             TogglePause();
//         }

//         UpdateHealthUI();
//     }

//     private void UpdateHealthUI()
//     {
//         if (player == null) return;

//         for (int i = 0; i < heartImages.Length; i++)
//         {
//             if (heartImages[i] == null) continue;

//             if (i < player.currentHealth)
//             {
//                 heartImages[i].enabled = true;

//                 if (fullHeart != null)
//                 {
//                     heartImages[i].sprite = fullHeart;
//                 }
//             }
//             else
//             {
//                 if (emptyHeart != null)
//                 {
//                     heartImages[i].enabled = true;
//                     heartImages[i].sprite = emptyHeart;
//                 }
//                 else
//                 {
//                     heartImages[i].enabled = false;
//                 }
//             }
//         }
//     }

//     public void Pause()
//     {
//         if (pauseMenuUI != null)
//         {
//             pauseMenuUI.SetActive(true);
//         }

//         Time.timeScale = 0f;
//         isPaused = true;
//     }

//     public void Resume()
//     {
//         if (pauseMenuUI != null)
//         {
//             pauseMenuUI.SetActive(false);
//         }

//         Time.timeScale = 1f;
//         isPaused = false;
//     }

//     public void TogglePause()
//     {
//         if (isPaused)
//         {
//             Resume();
//         }
//         else
//         {
//             Pause();
//         }
//     }

//     public void RestartLevel()
//     {
//         Time.timeScale = 1f;
//         isPaused = false;

//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }

//     public void QuitGame()
//     {
//         Time.timeScale = 1f;
//         isPaused = false;

// #if UNITY_EDITOR
//         EditorApplication.isPlaying = false;
// #else
//         Application.Quit();
// #endif
//     }
// }