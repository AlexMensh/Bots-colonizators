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
    private ScoreCounter _scoreCounter;

    private List<Item> _itemsFound = new List<Item>();
    private List<Unit> _unitsAvailable = new List<Unit>();

    public event Action ScoreChanged;

    private void Awake()
    {
        _searcher = GetComponent<Searcher>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _scoreCounter = GetComponent<ScoreCounter>();
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

        if (_unitsAvailable.Count == 0)
            return;

        Unit unit = GetFreeUnit();
        Item item = GetFreeItem();

        if (unit != null || item != null)
            StartItemDelivery(unit, item, this);
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        AddUnit(unit);

        item.gameObject.SetActive(false);
        item.transform.parent = _container.transform;
        item.isFound = false;

        ScoreChanged?.Invoke();
    }

    private void StartItemDelivery(Unit unit, Item item, Base homeBase)
    {
        RemoveUnit(unit);
        RemoveFoundItem(item);

        unit.SetDeliveryTask(item);
        unit.SetHomeBase(homeBase);
    }

    private Unit GetFreeUnit()
    {
        if (_unitsAvailable.Count > 0)
        {
            return _unitsAvailable[0];
        }
        return null;
    }

    private Item GetFreeItem()
    {
        if (_itemsFound.Count > 0)
        {
            return _itemsFound[0];
        }
        return null;
    }

    private void AddFoundItem(Item item)
    {
        _itemsFound.Add(item);
        item.isFound = true;
    }

    private void RemoveFoundItem(Item item)
    {
        _itemsFound.Remove(item);
    }

    public void RemoveUnit(Unit unit)
    {
        _unitsAvailable.Remove(unit);
    }

    public void AddUnit(Unit unit)
    {
        _unitsAvailable.Add(unit);
    }

    private void CreateStartUnitAmount(int startAmount)
    {
        for (int i = 0; i < startAmount; i++)
        {
            _unitsAvailable.Add(_unitSpawner.SpawnObject());
        }
    }
}