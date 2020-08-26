using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Использует диспетчер ввода для приложения боковой силы
// к объекту. Используется для отклонения ракеты в сторону.
public class RocketDirection : MonoBehaviour
{
    void FixedUpdate()
    {
        // Если твердое тело отсутствует (уже), удалить
        // этот компонент
        if(GetComponent<Rigidbody2D>() == null)
        {
            Destroy(this);
            return;
        }

        // Получить величину наклона из InputManager
        float turn = InputManager.instance.sidewaysMotion;

        // Повернуть
        GetComponent<Rigidbody2D>().MoveRotation(turn*(-90));
    }
}
