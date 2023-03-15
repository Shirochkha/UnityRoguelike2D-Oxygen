using UnityEngine;

//Enemy наследуется от MovingObject, базового класса для движущихся объектов, от него также наследуется Player.
public class Enemy : MovingObject
{
    //Количество очков еды, которое нужно отнять у игрока при атаке.
    public int playerDamage;
    //Переменная типа Animator для хранения ссылки на компонент Animator противника.
    private Animator animator;
    //Transform для изменения параметров объекта.
    private Transform target;
    //Логическое значение, определяющее, должен ли противник пропускать ход или двигаться на этом ходу.
    private bool skipMove;
    public AudioClip enemyAttack1; //Звук атаки врага 1.
    public AudioClip enemyAttack2; //Звук атаки врага 2.


    //Start переопределяет виртуальную функцию Start базового класса.
    protected override void Start()
    {
        //Регистрирует врага в экземпляре GameManager, добавив его в список объектов Enemy.
        GameManager.instance.AddEnemyToList(this);

        //Получает и сохраняет ссылку на присоединенный компонент Animator.
        animator = GetComponent<Animator>();

        //Находит GameObject Player, используя его тег, и сохраняет ссылку на его компонент преобразования.
        if (animator != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        //Вызов функции Start базового класса.
        base.Start();
    }


    //Переопределяет функцию AttemptMove объекта MovingObject, чтобы включить функции, необходимые врагу для пропуска ходов.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
      
        //Проверяет если skipMove true, то устанавливает для его false и пропускается ход.
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        //Вызов функции AttemptMove из MovingObject.
        base.AttemptMove<T>(xDir, yDir);
        if (animator != null)
        {
            if (xDir != 0 || yDir != 0)
                animator.SetBool("Walk", true);
            else
                animator.SetBool("Walk", false);
        }

        //Когда Враг переместился, устанавливает для параметра skipMove значение true, чтобы пропустить следующий ход.
        skipMove = true;
    }


    //MoveEnemy вызывается GameManger каждый ход, чтобы сказать каждому врагу, чтобы он попытался двигаться в сторону игрока.
    public void MoveEnemy()
    {
        //Объявляет переменные для направлений перемещения по осям X и Y, они могут принимать значения от -1 до 1.
        //Эти значения позволяют выбирать между основными направлениями: вверх, вниз, влево и вправо.
        int xDir = 0;
        int yDir = 0;

        //Если разница в позициях приблизительно равна нулю (эпсилон), выполняется следующее действие:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            //Если координата y положения цели (игрока) больше, чем координата y положения этого врага, устанавливает направление y 1 (для движения вверх). Если нет, устанавливает его на -1 (для перемещения вниз).
            yDir = target.position.y > transform.position.y ? 1 : -1;

        //Если разница в позициях не равна нулю (эпсилон), выполняется следующее действие:
        else         
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);      
            
    }


    //OnCantMove вызывается, если враг пытается переместиться в пространство, занятое игроком, он переопределяет функцию OnCantMove MovingObject
    //и принимает общий параметр T, который используется для передачи компонента, с которым ожидается столкновение, в данном случае Player.
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
