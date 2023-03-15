using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;       //Позволяет использовать списки.
using TMPro;

public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;                      //Время ожидания перед началом уровня в секундах.
    public float turnDelay = 0.5f;                          //Задержка между каждым ходом игрока.
    public int playerResourcesPoints = 100;                 //Начальное значение очков ресурсов игрока.
    public static GameManager instance = null;              //Статический экземпляр GameManager, который позволяет любому другому скрипту получить к нему доступ.
    [HideInInspector] public bool playersTurn = true;       //Логическое значение для проверки хода игроков, скрытое в инспекторе, но общедоступное.


    private TextMeshProUGUI levelText;                      //Текст для отображения номера текущего уровня.
    private GameObject levelImage;                          //Изображение для блокировки уровня при настройке уровней, фон для levelText.
    private GameObject restartMenu;                         //Изображение restart.
    private BoardManager boardScript;                       //Сохраните ссылку на наш BoardManager, который настроит уровень.
    private int level = 0;                                  //Текущий номер уровня, выраженный в игре как "Day 1".
    private List<Enemy> enemies;                            //Список всех вражеских юнитов, используемый для отдачи им команд движения.
    private bool enemiesMoving;                             //Логическое значение, чтобы проверить, двигаются ли враги.
    private bool doingSetup = true;                         //Логическое значение, чтобы проверить, устанавливаем ли мы доску, запретить игроку двигаться во время установки.

    //Awake всегда вызывается перед  функциями Start.
    void Awake()
	{
        Cursor.visible = false;
        //Проверить, существует ли уже экземпляр, если да, то удаляет его (Singleton).
        if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();

        //InitGame();
    }

    //Вызывается только один раз, и параметр указывает, что его нужно вызывать только после загрузки сцены.
    //(в противном случае обратный вызов загрузки сцены будет вызываться самой первой загрузкой)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static public void CallbackInitialization()
	{
        //Подиписывает callback, который будет вызываться каждый раз при загрузке сцены.
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

	//Вызывается каждый раз при загрузке сцены.
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


	//Инициализация игры на каждом уровне.
	void InitGame()
	{
        //Запрещает игроку двигаться, пока открыта картинка с названием уровня.
        doingSetup = true;

		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        levelText.text = "Day " + level;
		levelImage.SetActive(true);

        restartMenu = GameObject.Find("RestartMenu");
        //Вызывает функцию HideLevelImage с задержкой levelStartDelay в секундах.
        Invoke(nameof(HideLevelImage), levelStartDelay);
		
		enemies.Clear();
		boardScript.SetupScene(level);

	}


	//Скрывает черную картинку,которая показана между уровнями.
	void HideLevelImage()
	{
		levelImage.SetActive(false);
		doingSetup = false;
	}

	void Update()
	{
        //Проверяет, что playerTurn или враги, двигающиеся или выполняющие установку, в настоящее время не соответствуют действительности.
        if (playersTurn || enemiesMoving || doingSetup)
			return;

		//Враги начинают двигаться.
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