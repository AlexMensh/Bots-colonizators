using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Searcher), typeof(UnitSpawner), typeof(ScoreCounter))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Base _prefab;
    [SerializeField] private int _items;
    [SerializeField] private bool _buildingTask;

    private Vector3 _buildPosition;
    private Searcher _searcher;
    private UnitSpawner _unitSpawner;
    private int _unitCost = 3;
    private int _baseCost = 5;

    private List<Item> _itemsFound = new List<Item>();
    private List<Unit> _units = new List<Unit>();

    public event Action<int> ScoreChanged;
    public event Action BuildFinished;

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

    private void Update()
    {
        CollectingTask();

        if (_buildingTask == false)
            CreateUnit();

        if (_buildingTask == true)
            BuildingTask(_buildPosition);
    }

    public void StartBuildingTask(Vector3 builtPosition)
    {
        _buildingTask = true;
        _buildPosition = builtPosition;
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        AddUnit(unit);
        AddItem();
        ReturnToPool(item);
    }

    private void Initialize(Transform container, Base prefab)
    {
        _container = container;
        _prefab = prefab;
    }

    private void CollectingTask()
    {
        if (_itemsFound.Count == 0 || _units.Count == 0)
            return;

        Unit unit = GetFreeObject(_units);
        Item item = GetFreeObject(_itemsFound);

        if (unit != null || item != null)
            StartItemDelivery(unit, item);
        else
            return;
    }

    private void BuildingTask(Vector3 buildPosition)
    {
        if (_items < _baseCost)
            return;

        _items -= _baseCost;

        Base newBase = Instantiate(_prefab, buildPosition, Quaternion.identity);
        newBase.Initialize(_container, _prefab);
        newBase.ScoreChanged?.Invoke(_items);

        BuildFinished?.Invoke();

        _buildingTask = false;
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

    private T GetFreeObject<T>(List<T> list) where T : class
    {
        return list.Count > 0 ? list[0] : null;
    }

    private void CreateUnit()
    {
        if (_items < _unitCost)
            return;

        _items -= _unitCost;

        Unit unit = _unitSpawner.SpawnObject();
        AddUnit(unit);
        unit.SetHomeBase(this);

        ScoreChanged?.Invoke(_items);
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

    private void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    private void RemoveUnit(Unit unit)
    {
        _units.Remove(unit);
    }

    private void AddItem()
    {
        _items++;
        ScoreChanged?.Invoke(_items);
    }
}