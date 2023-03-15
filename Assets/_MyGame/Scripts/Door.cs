using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    //Проверка взаимодействия триггеров.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Загрузка игровой сцены.
        SceneManager.LoadScene("Scene");
    }
}