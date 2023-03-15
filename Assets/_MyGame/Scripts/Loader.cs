using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;             //Префаб GameManager для создания экземпляра.
    public GameObject soundManager;            //Префаб SoundManager для создания экземпляра.


    void Awake()
    {
        // Проверяем, был ли GameManager уже назначен статической переменной GameManager.instance или он все еще равен нулю
        if (GameManager.instance == null)

            //Создаем префаб gameManager
            Instantiate(gameManager);

        //Проверяем, был ли SoundManager уже назначен статической переменной SoundManager.instance или он все еще равен нулю
        if (SoundManager.instance == null)

            //Создаем префаб SoundManager
            Instantiate(soundManager);
    }
}
