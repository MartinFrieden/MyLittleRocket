using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
where T : MonoBehaviour
{
    // Единственный экземпляр класса.
    private static T _instance;
    // Метод доступа. В первом вызове настроит свойство _instance.
    // Если требуемый объект не найден, выводит сообщение об ошибке.
    public static T instance
    {
        get
        {
            // Если свойство _instance еще не настроено ...
            if (_instance == null)
            {
                // Попытаться найти объект.
                _instance = FindObjectOfType<T>();
                // Вывести собщение в случае неудачи.
                if (_instance == null)
                {
                    Debug.LogError("Can't find " +
                    typeof(T) + "!");
                }
            }
            // Вернуть экземпляр для использования!
            return _instance;
        }
    }
}