using UnityEngine;
using System.Collections;

public abstract class Singleton< T > : MonoBehaviour  where T : MonoBehaviour
{
    static bool _isShutdown = false;
    static T _instance = null;
    public static T _Instance
    {
        get
        {
            if (_isShutdown)
                return null;

            if (_instance == null)
                _instance = GameObject.FindObjectOfType<T>();

            if (_instance == null)
            {
                GameObject newObj = new GameObject("[ Temp SingletonObj " + typeof(T).ToString() + " ]");
                _instance = newObj.AddComponent<T>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if( _instance != null )
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this as T;
        onAwakeEnd();
    }

    virtual protected void onAwakeEnd()
    {
    }


    void OnApplicationQuit()
    {
        _isShutdown = true;
        onApplicationQuitEnd();
    }

    virtual protected void onApplicationQuitEnd()
    {
    }
}
