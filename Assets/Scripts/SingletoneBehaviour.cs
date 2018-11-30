using UnityEngine;

public class SingletoneBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static private T _instance;

    static public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
}
