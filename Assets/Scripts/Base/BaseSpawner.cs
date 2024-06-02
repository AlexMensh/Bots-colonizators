using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Base _prefab;

    private ObjectPooler<Base> _pool;

    private void Awake()
    {
        _pool = new ObjectPooler<Base>(_prefab, _container);
    }

    public Base SpawnObject()
    {
        Base newBase = _pool.GetObject();
        newBase.gameObject.SetActive(true);
        newBase.transform.position = transform.position;

        return newBase;
    }
}
