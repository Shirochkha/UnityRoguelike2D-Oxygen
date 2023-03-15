using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;       //��������� ������������ ������.
using TMPro;

public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;                      //����� �������� ����� ������� ������ � ��������.
    public float turnDelay = 0.5f;                          //�������� ����� ������ ����� ������.
    public int playerResourcesPoints = 100;                 //��������� �������� ����� �������� ������.
    public static GameManager instance = null;              //����������� ��������� GameManager, ������� ��������� ������ ������� ������� �������� � ���� ������.
    [HideInInspector] public bool playersTurn = true;       //���������� �������� ��� �������� ���� �������, ������� � ����������, �� �������������.


    private TextMeshProUGUI levelText;                      //����� ��� ����������� ������ �������� ������.
    private GameObject levelImage;                          //����������� ��� ���������� ������ ��� ��������� �������, ��� ��� levelText.
    private GameObject restartMenu;                         //����������� restart.
    private BoardManager boardScript;                       //��������� ������ �� ��� BoardManager, ������� �������� �������.
    private int level = 0;                                  //������� ����� ������, ���������� � ���� ��� "Day 1".
    private List<Enemy> enemies;                            //������ ���� ��������� ������, ������������ ��� ������ �� ������ ��������.
    private bool enemiesMoving;                             //���������� ��������, ����� ���������, ��������� �� �����.
    private bool doingSetup = true;                         //���������� ��������, ����� ���������, ������������� �� �� �����, ��������� ������ ��������� �� ����� ���������.

    //Awake ������ ���������� �����  ��������� Start.
    void Awake()
	{
        Cursor.visible = false;
        //���������, ���������� �� ��� ���������, ���� ��, �� ������� ��� (Singleton).
        if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();

        //InitGame();
    }

    //���������� ������ ���� ���, � �������� ���������, ��� ��� ����� �������� ������ ����� �������� �����.
    //(� ��������� ������ �������� ����� �������� ����� ����� ���������� ����� ������ ���������)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static public void CallbackInitialization()
	{
        //������������ callback, ������� ����� ���������� ������ ��� ��� �������� �����.
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//���������� ������ ��� ��� �������� �����.
	static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if(instance != null)
		{
            if (arg0.name == "MainMenu" || arg0.name == "CutScene")
                Destroy(instance.gameObject);
            else
            {
                instance.level++;
                instance.InitGame();
            }
       }
		
    }


	//������������� ���� �� ������ ������.
	void InitGame()
	{
        //��������� ������ ���������, ���� ������� �������� � ��������� ������.
        doingSetup = true;

		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        levelText.text = "Day " + level;
		levelImage.SetActive(true);

        restartMenu = GameObject.Find("RestartMenu");
        //�������� ������� HideLevelImage � ��������� levelStartDelay � ��������.
        Invoke(nameof(HideLevelImage), levelStartDelay);
		
		enemies.Clear();
		boardScript.SetupScene(level);

	}


	//�������� ������ ��������,������� �������� ����� ��������.
	void HideLevelImage()
	{
		levelImage.SetActive(false);
		doingSetup = false;
	}

	void Update()
	{
        //���������, ��� playerTurn ��� �����, ����������� ��� ����������� ���������, � ��������� ����� �� ������������� ����������������.
        if (playersTurn || enemiesMoving || doingSetup)
			return;

		//����� �������� ���������.
		StartCoroutine(MoveEnemies());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add(script);
	}


	public void GameOver()
	{
		levelText.text = "After " + level + " days, you starved.";
		levelText.transform.position = new Vector3(3150f, 1900f, 0f);
        levelImage.SetActive(true);
        enabled = false;
        restartMenu.SetActive(true);
    }

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);

		if (enemies.Count == 0)
			yield return new WaitForSeconds(turnDelay);

		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
        playersTurn = true;
		enemiesMoving = false;
	}
}