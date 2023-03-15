using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Player ����������� �� MovingObject, ������ �������� ������ ��� ��������, ������� ����� ���������, Enemy ����� ����������� �� ����.
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //����� �������� � �������� �� ����������� ������.
    public int pointsPerBubble = 20;            //���������� �����, ����������� ������ � ����� �������� ��� ������� ���������.
    public int pointsPerWater = 30;             //���������� �����, ����������� ������ � ����� �������� ��� ������� ����.
    public int wallDamage = 1;                  //������� ����� ����� ������� �����, �������� ��.
    public TextMeshProUGUI resourcesText;       //����� ����������������� ���������� ��� ����������� ������ ���������� �������� ��� �������� ������.
    public AudioClip teleportSound1;            //���� ��������� 1.
	public AudioClip teleportSound2;            //���� ��������� 2.
	public AudioClip popSound1;                 //���� ����������� ������ 1.
	public AudioClip popSound2;                 //���� ����������� ������ 2.
    public AudioClip popSound3;                 //���� ����������� ������ 3.
    public AudioClip drinkSound1;               //���� ������� ���� 1.
	public AudioClip drinkSound2;               //���� ������� ���� 2.
    public AudioClip gameOverSound;             //���� ����� ������ ������.
    public AudioClip gameOverMusic;				//������� ������ ����� ����.

    private Animator animator;                  //������������ ��� �������� ������ �� ��������� ��������� �������������.
    private int resources;                      //������������ ��� �������� ������ ���������� ����� �������� ������ �� ����� ������.


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
        ///���� ������ �� ��� ������, ������� �� �������.
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
            //�������� AttemptMove, ��������� ����� �������� Wall, ��� ��� ��� ��, � ��� ����� ����������������� Player, ���� �� ���������� � ��� (������ ���)
            //��������� ����������� � ��������� � �������� ����������, ����� ������� ����������� ����������� ������.
            AttemptMove<Wall>(horizontal, vertical);
		}
	}

    //AttemptMove �������������� ������� AttemptMove � ������� ������ MovingObject
    //AttemptMove ��������� ����� �������� T, ������� ��� Player ����� ����� ��� Wall, � ����� ��������� ����� ����� ��� ����������� x � y ��� �����������.
    protected override void AttemptMove<T>(int xDir, int yDir)
	{
		resources--;
		resourcesText.text = "RESOURCES: " + resources;
		base.AttemptMove<T>(xDir, yDir);
		CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }


    //OnCantMove �������������� ����������� ������� OnCantMove � MovingObject.
    //�� ��������� ����� �������� T, ������� � ������ Player �������� ������, ������� ����� ����� ��������� � ���������.
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

            //�������� ������� Restart ��� ������� ���������� ������ � ��������� restartLevelDelay (�� ��������� 1 �������).
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

    //Restart ������������� ����� ��� ������.
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