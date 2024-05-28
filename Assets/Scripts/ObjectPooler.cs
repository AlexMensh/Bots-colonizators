using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : MonoBehaviour
{
    private Transform _container;
    private T _prefab;

    private List<T> _pool = new List<T>();

    public ObjectPooler(T prefab, Transform container)
    {
        _prefab = prefab;
        _container = container;
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
            item.transform.parent = _container;
        }

        return item;
    }
}