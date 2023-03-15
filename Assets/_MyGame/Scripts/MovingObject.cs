using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;       //Время, которое потребуется объекту для перемещения, в секундах.
    public LayerMask blockingLayer;     //Слой, на котором будет проверяться коллизия.


    private BoxCollider2D boxCollider;  //Компонент BoxCollider2D, прикрепленный к этому объекту.
    private Rigidbody2D rb2D;           //Компонент Rigidbody2D, прикрепленный к этому объекту.
    private float inverseMoveTime;      //Используется для повышения эффективности движения.
    //protected bool isMoving;              //Объект в настоящее время движется.


    protected virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}


    //Move возвращает true, если он может двигаться, и false, если нет.
    //Move принимает параметры для направления x, направления y и RaycastHit2D для проверки столкновения.
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);
		boxCollider.enabled = false;

		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null )
		{
			StartCoroutine(SmoothMovement(end));
			return true;
		}


		return false;
	}


    //Coroutine для перемещения юнитов из одной области в другую использует параметр end, чтобы указать, куда двигаться.
    protected IEnumerator SmoothMovement(Vector3 end)
	{


		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon)
		{			
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			rb2D.MovePosition(newPostion);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
		rb2D.MovePosition(end);
	
		if (GameManager.instance.playersTurn) GameManager.instance.playersTurn = false;
	}

    //AttemptMove принимает общий параметр T, чтобы указать тип компонента, с которым мы ожидаем,
	//что наш юнит будет взаимодействовать, если он заблокирован (игрок для врагов, стена для игрока).
    protected virtual void AttemptMove<T>(int xDir, int yDir)
		where T : Component
	{
		bool canMove = Move(xDir, yDir, out RaycastHit2D hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T>();

		if (!canMove && hitComponent != null)
			OnCantMove(hitComponent);
	}

    //Абстрактный модификатор указывает на то, что модифицируемая вещь имеет отсутствующую или неполную реализацию.
    //OnCantMove будет переопределен функциями в классах-наследниках.
    protected abstract void OnCantMove<T>(T component)
		where T : Component;
}
