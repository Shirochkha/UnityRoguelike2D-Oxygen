using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    //���� ���������� ����� 1.
    public AudioClip chopSound1;
    //���� ���������� ����� 2.
    public AudioClip chopSound2;
    //�������������� ������, ������������ ����� ����, ��� ����� �������� �����.
    public Sprite dmgSprite;
    //�������������� ������, ������������ ����� ����, ��� ����� �������� �����.
    public Sprite dmgSprite2;
    //�������� �����.
    public int hp = 3;
    //��������� ������ ���������� �� ������������� SpriteRenderer.
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //DamageWall ����������, ����� ����� ������� �����.
    public void DamageWall(int loss)
    {
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        if(hp==2) spriteRenderer.sprite = dmgSprite2;
        else spriteRenderer.sprite = dmgSprite;

        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}