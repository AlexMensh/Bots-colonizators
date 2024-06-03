using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    private ObjectPooler<Base> _pool;

    private void Awake()
    {
        _pool = new ObjectPooler<Base>(_prefab);
    }

    public Base SpawnObject(Vector3 position)
    {
        Base newBase = _pool.GetObject();
        newBase.gameObject.SetActive(true);
        newBase.transform.position = position;

        return newBase;
    }
}
