using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;       //�����, ������� ����������� ������� ��� �����������, � ��������.
    public LayerMask blockingLayer;     //����, �� ������� ����� ����������� ��������.


    private BoxCollider2D boxCollider;  //��������� BoxCollider2D, ������������� � ����� �������.
    private Rigidbody2D rb2D;           //��������� Rigidbody2D, ������������� � ����� �������.
    private float inverseMoveTime;      //������������ ��� ��������� ������������� ��������.
    //protected bool isMoving;              //������ � ��������� ����� ��������.


    protected virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}


    //Move ���������� true, ���� �� ����� ���������, � false, ���� ���.
    //Move ��������� ��������� ��� ����������� x, ����������� y � RaycastHit2D ��� �������� ������������.
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


    //Coroutine ��� ����������� ������ �� ����� ������� � ������ ���������� �������� end, ����� �������, ���� ���������.
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

    //AttemptMove ��������� ����� �������� T, ����� ������� ��� ����������, � ������� �� �������,
	//��� ��� ���� ����� �����������������, ���� �� ������������ (����� ��� ������, ����� ��� ������).
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

    //����������� ����������� ��������� �� ��, ��� �������������� ���� ����� ������������� ��� �������� ����������.
    //OnCantMove ����� ������������� ��������� � �������-�����������.
    protected abstract void OnCantMove<T>(T component)
		where T : Component;
}
