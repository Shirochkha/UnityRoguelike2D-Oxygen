using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Player наследуется от MovingObject, нашего базового класса для объектов, которые могут двигаться, Enemy также наследуется от него.
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //Время задержки в секундах до перезапуска уровня.
    public int pointsPerBubble = 20;            //Количество очков, добавляемых игроку к очкам ресурсов при подборе пузырьков.
    public int pointsPerWater = 30;             //Количество очков, добавляемых игроку к очкам ресурсов при подборе воды.
    public int wallDamage = 1;                  //Сколько урона игрок наносит стене, разрушая ее.
    public TextMeshProUGUI resourcesText;       //Текст пользовательского интерфейса для отображения общего количества ресурсов для текущего игрока.
    public AudioClip teleportSound1;            //Звук телепорта 1.
	public AudioClip teleportSound2;            //Звук телепорта 2.
	public AudioClip popSound1;                 //Звук лопающегося пузыря 1.
	public AudioClip popSound2;                 //Звук лопающегося пузыря 2.
    public AudioClip popSound3;                 //Звук лопающегося пузыря 3.
    public AudioClip drinkSound1;               //Звук объекта воды 1.
	public AudioClip drinkSound2;               //Звук объекта воды 2.
    public AudioClip gameOverSound;             //Звук после смерти игрока.
    public AudioClip gameOverMusic;				//Фоновая музыка конца игры.

    private Animator animator;                  //Используется для хранения ссылки на компонент аниматора проигрывателя.
    private int resources;                      //Используется для хранения общего количества очков ресурсов игрока во время уровня.


    protected override void Start()
	{
        animator = GetComponent<Animator>();
		resources = GameManager.instance.playerResourcesPoints;
		resourcesText.text = "RESOURCES: " + resources;
		base.Start();
	}

	private void OnDisable()
	{
		GameManager.instance.playerResourcesPoints = resources;
	}


	private void Update()
	{
        ///Если сейчас не ход игрока, выходим из функции.
        if (!GameManager.instance.playersTurn) return;

		int horizontal = (int)(Input.GetAxisRaw("Horizontal"));
		int vertical = (int)(Input.GetAxisRaw("Vertical"));

		if (horizontal != 0)
		{
			vertical = 0;
		}
		if(horizontal == -1)
		{
			gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, -180, gameObject.transform.rotation.z);
		}
		else gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);

		if (horizontal != 0 || vertical != 0)
		{
            //Вызываем AttemptMove, передавая общий параметр Wall, так как это то, с чем может взаимодействовать Player, если он встретится с ним (атакуя его)
            //Передайте горизонталь и вертикаль в качестве параметров, чтобы указать направление перемещения игрока.
            AttemptMove<Wall>(horizontal, vertical);
		}
	}

    //AttemptMove переопределяет функцию AttemptMove в базовом классе MovingObject
    //AttemptMove принимает общий параметр T, который для Player будет иметь тип Wall, а также принимает целые числа для направления x и y для перемещения.
    protected override void AttemptMove<T>(int xDir, int yDir)
	{
		resources--;
		resourcesText.text = "RESOURCES: " + resources;
		base.AttemptMove<T>(xDir, yDir);
		CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }


    //OnCantMove переопределяет абстрактную функцию OnCantMove в MovingObject.
    //Он принимает общий параметр T, который в случае Player является стеной, которую игрок может атаковать и разрушить.
    protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Exit"))
		{
			animator.SetTrigger("Teleport");
			SoundManager.instance.RandomizeSfx(teleportSound1, teleportSound2);

            //Вызываем функцию Restart для запуска следующего уровня с задержкой restartLevelDelay (по умолчанию 1 секунда).
            Invoke(nameof(Restart), restartLevelDelay);
			enabled = false;
		}
		else if (other.CompareTag("Bubbles"))
		{
			resources += pointsPerBubble;
			resourcesText.text = "+" + pointsPerBubble + " RESOURCES: " + resources;
			SoundManager.instance.RandomizeSfx(popSound1, popSound2, popSound3);
			other.gameObject.SetActive(false);
		}
		else if (other.CompareTag("Water"))
		{
			resources += pointsPerWater;
			resourcesText.text = "+" + pointsPerWater + " RESOURCES: " + resources;
			SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
			other.gameObject.SetActive(false);
		}

	}

    //Restart перезагружает сцену при вызове.
    private void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}


	public void LoseFood(int loss)
	{
		animator.SetTrigger("Hurt");
		resources -= loss;
		resourcesText.text = "-" + loss + " RESOURCES: " + resources;
		CheckIfGameOver();
	}

	private void CheckIfGameOver()
	{
		if (resources <= 0)
		{
			//SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.musicSource.Stop();
			GameManager.instance.GameOver();
            Cursor.visible = true;
            SoundManager.instance.PlaySingleMusic(gameOverMusic);
        }
    }
}