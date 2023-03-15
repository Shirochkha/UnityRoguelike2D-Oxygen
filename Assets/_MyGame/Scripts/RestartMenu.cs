using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
    public GameObject restartMenu;
    public AudioClip buttonSound;

    void Start()
    {
        restartMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene("Scene");
    }

    public void LoadMenu()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        SoundManager.instance.PlaySingle(buttonSound);
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
}
