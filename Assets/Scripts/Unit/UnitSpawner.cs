using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    private ObjectPooler<Unit> _pool;

    private void Awake()
    {
        _pool = new ObjectPooler<Unit>(_prefab);
    }

    public Unit SpawnObject()
    {
        Unit unit = _pool.GetObject();
        unit.gameObject.SetActive(true);
        unit.transform.position = transform.position;

        return unit;
    }
}