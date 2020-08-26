using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //направление движения птички
    public Vector2      direction;
    //переменная для определения направления персонажа вправо/влево
    private bool        isFacingRight = true;
    //ссылка на компонент анимаций
    private Animator    anim;
    //звук проигрываемый при столкновении с ракетой
    public AudioClip    touchBird;
    
    void Start()
    {
        //задаем случайное направление и скорость
        direction.x = Random.Range(-1f, 1f);
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //перемещение птички по оси х
        transform.Translate(direction * Time.fixedDeltaTime);
        //поворот птички
        if (direction.x > 0 && !isFacingRight)
            //отражаем птичку вправо
            Flip();
        //обратная ситуация. отражаем птичку влево
        else if (direction.x < 0 && isFacingRight)
            Flip();
    }

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }

    //проигрывает звук когда ракета врезается в птичку
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var audio = GetComponent<AudioSource>();
            if (audio)
            {
                audio.PlayOneShot(this.touchBird);
            }
        }
    }
}
