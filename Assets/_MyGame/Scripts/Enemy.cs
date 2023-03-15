using UnityEngine;

//Enemy ����������� �� MovingObject, �������� ������ ��� ���������� ��������, �� ���� ����� ����������� Player.
public class Enemy : MovingObject
{
    //���������� ����� ���, ������� ����� ������ � ������ ��� �����.
    public int playerDamage;
    //���������� ���� Animator ��� �������� ������ �� ��������� Animator ����������.
    private Animator animator;
    //Transform ��� ��������� ���������� �������.
    private Transform target;
    //���������� ��������, ������������, ������ �� ��������� ���������� ��� ��� ��������� �� ���� ����.
    private bool skipMove;
    public AudioClip enemyAttack1; //���� ����� ����� 1.
    public AudioClip enemyAttack2; //���� ����� ����� 2.


    //Start �������������� ����������� ������� Start �������� ������.
    protected override void Start()
    {
        //������������ ����� � ���������� GameManager, ������� ��� � ������ �������� Enemy.
        GameManager.instance.AddEnemyToList(this);

        //�������� � ��������� ������ �� �������������� ��������� Animator.
        animator = GetComponent<Animator>();

        //������� GameObject Player, ��������� ��� ���, � ��������� ������ �� ��� ��������� ��������������.
        if (animator != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        //����� ������� Start �������� ������.
        base.Start();
    }


    //�������������� ������� AttemptMove ������� MovingObject, ����� �������� �������, ����������� ����� ��� �������� �����.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
      
        //��������� ���� skipMove true, �� ������������� ��� ��� false � ������������ ���.
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        //����� ������� AttemptMove �� MovingObject.
        base.AttemptMove<T>(xDir, yDir);
        if (animator != null)
        {
            if (xDir != 0 || yDir != 0)
                animator.SetBool("Walk", true);
            else
                animator.SetBool("Walk", false);
        }

        //����� ���� ������������, ������������� ��� ��������� skipMove �������� true, ����� ���������� ��������� ���.
        skipMove = true;
    }


    //MoveEnemy ���������� GameManger ������ ���, ����� ������� ������� �����, ����� �� ��������� ��������� � ������� ������.
    public void MoveEnemy()
    {
        //��������� ���������� ��� ����������� ����������� �� ���� X � Y, ��� ����� ��������� �������� �� -1 �� 1.
        //��� �������� ��������� �������� ����� ��������� �������������: �����, ����, ����� � ������.
        int xDir = 0;
        int yDir = 0;

        //���� ������� � �������� �������������� ����� ���� (�������), ����������� ��������� ��������:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            //���� ���������� y ��������� ���� (������) ������, ��� ���������� y ��������� ����� �����, ������������� ����������� y 1 (��� �������� �����). ���� ���, ������������� ��� �� -1 (��� ����������� ����).
            yDir = target.position.y > transform.position.y ? 1 : -1;

        //���� ������� � �������� �� ����� ���� (�������), ����������� ��������� ��������:
        else         
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);      
            
    }


    //OnCantMove ����������, ���� ���� �������� ������������� � ������������, ������� �������, �� �������������� ������� OnCantMove MovingObject
    //� ��������� ����� �������� T, ������� ������������ ��� �������� ����������, � ������� ��������� ������������, � ������ ������ Player.
    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);

        if (hitPlayer.transform.position == gameObject.transform.position)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1,
                gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (animator != null)
            animator.SetTrigger("Attack");
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }

}
