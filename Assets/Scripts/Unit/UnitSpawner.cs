using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Unit _prefab;

    private ObjectPooler<Unit> _pool;

    private void Awake()
    {
        _pool = new ObjectPooler<Unit>(_prefab, _container);
    }

    public Unit SpawnObject()
    {
        var unit = _pool.GetObject();
        unit.gameObject.SetActive(true);

        return unit;
    }
}