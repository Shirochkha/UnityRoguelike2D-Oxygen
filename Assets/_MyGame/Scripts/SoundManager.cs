using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;                    //������ �� �������� �����, ������� ����� �������������� �������� �������.
    public AudioSource musicSource;                  //������ �� �������� �����, ������� ����� �������������� ������.
    public static SoundManager instance = null;      //��������� ������ ��������� �������� ������� �� SoundManager.                
    public float lowPitchRange = .95f;               //����� ������ �������� ������ ����� ���������������� ��������� �������.
    public float highPitchRange = 1.05f;             //����� ������� �������� ������ ����� ���������������� ��������� �������.


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

    //������������ ��� ��������������� ������ ��������� �������.
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    //������������ ��� ��������������� ����� �������.
    public void PlaySingleMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    ///RandomizeSfx ��������� ������� �������� ����� ���������� ������������ � ������ ������ �� ������.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    //RandomizeBackgroundMusic ��������� ������� �������� ����� ���������� ������������ � ������ ������ �� ������.
    public void RandomizeBackgroundMusic(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        musicSource.pitch = randomPitch;
        musicSource.clip = clips[randomIndex];
        musicSource.Play();
    }
}