using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Unit _prefab;
    [SerializeField] private int _startUnitsAmount;

    private List<Unit> _unitsAvailable = new List<Unit>();
    private ObjectPooler<Unit> _pool;

    public List<Unit> UnitsAvailable => new(_unitsAvailable);

    private void Awake()
    {
        _pool = new ObjectPooler<Unit>(_prefab, _container);
    }

    private void Start()
    {
        CreateStartUnitAmount(_startUnitsAmount);
    }

    public void RemoveUnit(Unit unit)
    {
        _unitsAvailable.Remove(unit);
    }

    public void AddUnit(Unit unit)
    {
        _unitsAvailable.Add(unit);
    }

    public Unit SpawnObject()
    {
        var unit = _pool.GetObject();
        unit.gameObject.SetActive(true);

        return unit;
    }

    private void CreateStartUnitAmount(int startAmount)
    {
        for (int i = 0; i < startAmount; i++)
        {
            _unitsAvailable.Add(SpawnObject());
        }
    }
}