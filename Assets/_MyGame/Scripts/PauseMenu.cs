using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenu;
    public AudioClip buttonSound;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        gameIsPaused = false;
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        gameIsPaused = true;
    }
    public void LoadMenu()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
}
