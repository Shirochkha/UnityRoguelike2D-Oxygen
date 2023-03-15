using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;             //������ GameManager ��� �������� ����������.
    public GameObject soundManager;            //������ SoundManager ��� �������� ����������.


    void Awake()
    {
        // ���������, ��� �� GameManager ��� �������� ����������� ���������� GameManager.instance ��� �� ��� ��� ����� ����
        if (GameManager.instance == null)

            //������� ������ gameManager
            Instantiate(gameManager);

        //���������, ��� �� SoundManager ��� �������� ����������� ���������� SoundManager.instance ��� �� ��� ��� ����� ����
        if (SoundManager.instance == null)

            //������� ������ SoundManager
            Instantiate(soundManager);
    }
}
