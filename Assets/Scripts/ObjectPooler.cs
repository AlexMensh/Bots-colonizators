using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : MonoBehaviour
{
    private T _prefab;

    private List<T> _pool = new List<T>();

    public ObjectPooler(T prefab)
    {
        _prefab = prefab;
    }

    public T GetObject()
    {
        T item = null;

        foreach (var checkItem in _pool)
        {
            if (checkItem.isActiveAndEnabled == false)
            {
                item = checkItem;
                break;
            }
        }

        if (item == null)
        {
            item = Object.Instantiate(_prefab);
            _pool.Add(item);
        }

        return item;
    }
}