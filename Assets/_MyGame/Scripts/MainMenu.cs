using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource efxSource;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Cursor.visible = true;
    }
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    public void PlayGame()
    {
        StartCoroutine(ButtonStartCorutine());
        Cursor.visible = false;
    }

    IEnumerator ButtonStartCorutine()
    {
        efxSource.Play();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void ExitGame()
    {
        efxSource.Play();
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
}
