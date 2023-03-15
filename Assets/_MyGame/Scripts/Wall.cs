using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    //Звук разрушения стены 1.
    public AudioClip chopSound1;
    //Звук разрушения стены 2.
    public AudioClip chopSound2;
    //Альтернативный спрайт, отображаемый после того, как игрок атаковал Стену.
    public Sprite dmgSprite;
    //Альтернативный спрайт, отображаемый после того, как игрок атаковал Стену.
    public Sprite dmgSprite2;
    //Здоровье стены.
    public int hp = 3;
    //Сохраняем ссылку компонента на прикрепленный SpriteRenderer.
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //DamageWall вызывается, когда игрок атакует стену.
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