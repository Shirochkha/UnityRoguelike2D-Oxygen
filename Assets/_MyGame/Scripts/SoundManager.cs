using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                    //Ссылка на источник звука, который будет воспроизводить звуковые эффекты.
    public AudioSource musicSource;                  //Ссылка на источник звука, который будет воспроизводить музыку.
    public static SoundManager instance = null;      //Позволяет другим сценариям вызывать функции из SoundManager.                
    public float lowPitchRange = .95f;               //Самый низкий звуковой эффект будет воспроизводиться случайным образом.
    public float highPitchRange = 1.05f;             //Самый высокий звуковой эффект будет воспроизводиться случайным образом.


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "MainMenu")
        {
            Destroy(instance.gameObject);
        }
    }

    //Используется для воспроизведения одного звукового эффекта.
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    //Используется для воспроизведения одной мелодии.
    public void PlaySingleMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    ///RandomizeSfx случайным образом выбирает между различными аудиоклипами и слегка меняет их высоту.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    //RandomizeBackgroundMusic случайным образом выбирает между различными аудиоклипами и слегка меняет их высоту.
    public void RandomizeBackgroundMusic(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        musicSource.pitch = randomPitch;
        musicSource.clip = clips[randomIndex];
        musicSource.Play();
    }
}