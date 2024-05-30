using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Searcher), typeof(UnitSpawner), typeof(ScoreCounter))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _startUnitsAmount;

    private Searcher _searcher;
    private UnitSpawner _unitSpawner;
    private int _resourses;

    private List<Item> _itemsFound = new List<Item>();
    private List<Unit> _units = new List<Unit>();

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        _searcher = GetComponent<Searcher>();
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void OnEnable()
    {
        _searcher.ItemFound += AddFoundItem;
    }

    private void OnDisable()
    {
        _searcher.ItemFound -= AddFoundItem;
    }

    private void Start()
    {
        CreateStartUnitAmount(_startUnitsAmount);
    }

    private void Update()
    {
        Work();
    }

    private void Work()
    {
        if (_itemsFound.Count == 0)
            return;

        if (_units.Count == 0)
            return;

        Unit unit = GetFreeObject(_units);
        Item item = GetFreeObject(_itemsFound);

        if (unit != null || item != null)
            StartItemDelivery(unit, item);
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        AddUnit(unit);
        ReturnToPool(item);
        AddResourse();
    }

    private void StartItemDelivery(Unit unit, Item item)
    {
        RemoveUnit(unit);
        RemoveFoundItem(item);

        unit.SetDeliveryTask(item);
    }

    private void ReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = _container.transform;
        item.ResetFoundStatus();
    }

    private T GetFreeObject<T>(List<T> list)
    {
        if (list.Count > 0)
        {
            return list[0];
        }
        return default;
    }

    private void AddFoundItem(Item item)
    {
        _itemsFound.Add(item);
        item.MarkAsFound();
    }

    private void RemoveFoundItem(Item item)
    {
        _itemsFound.Remove(item);
    }

    private void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }

    private void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    private void AddResourse()
    {
        _resourses++;
        ScoreChanged?.Invoke(_resourses);
    }

    private void CreateStartUnitAmount(int startAmount)
    {
        for (int i = 0; i < startAmount; i++)
        {
            Unit unit = _unitSpawner.SpawnObject();
            unit.SetHomeBase(this);
            _units.Add(unit);
        }
    }
}