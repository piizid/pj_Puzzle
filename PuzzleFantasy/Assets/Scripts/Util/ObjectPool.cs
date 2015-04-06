using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : Singleton_Scene< ObjectPool >
{
    Dictionary<GameObject, Queue<GameObject>> _objectPool = new Dictionary<GameObject, Queue<GameObject>>();
    Dictionary<GameObject, GameObject> _managedObjectPrefab = new Dictionary<GameObject, GameObject>();

    public void clearObjectPool()
    {
        _objectPool.Clear();
        _managedObjectPrefab.Clear();
    }

    public GameObject GetObj( GameObject prefab )
    {
        if (prefab == null)
            return null;
        Queue< GameObject > queue = null;
        if (!_objectPool.TryGetValue(prefab, out queue))
        {
            queue = new Queue<GameObject>();
            _objectPool.Add(prefab, queue);
        }

        if (queue.Count > 0)
        {
            return queue.Dequeue();
        }

        GameObject newObj = Instantiate(prefab) as GameObject;
        _managedObjectPrefab.Add(newObj, prefab);
        return newObj;        
    }

    public void ReleaseObj(GameObject obj)
    {
        if (obj == null) return;
        GameObject preFab = null;

        if (_managedObjectPrefab.TryGetValue(obj, out preFab) == false)
        {
            Destroy(obj);
            return;
        }

        
    }
}
