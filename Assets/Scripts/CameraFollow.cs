using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Целевой объект, с позицией Y и X которого будет
    // синхронизироваться положение камеры.
    public Transform target;

    // Низшая точка, где может находиться камера.
    public float bottomLimit = 0f;

    // Скорость следования за целевым объектом.
    public float followSpeed = 0.5f;

    public float offset = 3f;

    // Определяет положение камеры после установки
    // позиций всех объектов
    private void LateUpdate()
    {
        // Если целевой объект определен...
        if(target != null)
        {
            // Получить его позицию
            Vector3 newPosition = this.transform.position;

            // Определить, где камера должна находиться
            newPosition.x = ( target.position.x );
            newPosition.y = ( target.position.y+offset );

            // Предотвратить выход позиции за граничные точки
            newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

            // Обновить местоположение
            transform.position = newPosition;
        }
    }


}
