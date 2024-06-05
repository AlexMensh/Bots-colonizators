using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base), typeof(Searcher), typeof(UnitSpawner))]
[RequireComponent(typeof(BaseBuilder))]
public class BaseGatherer : MonoBehaviour
{
    [SerializeField] private int _items;

    private Base _base;
    private BaseBuilder _builder;
    private Searcher _searcher;
    private UnitSpawner _unitSpawner;
    private int _unitCost = 3;
    private int _unitsAvailable;

    private List<Item> _itemsFound = new List<Item>();
    private List<Unit> _units = new List<Unit>();

    private bool _isHaveRequest;
    private Base _requestedBase;

    public int Items { get { return _items; } }

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        _base = GetComponent<Base>();
        _builder = GetComponent<BaseBuilder>();
        _searcher = GetComponent<Searcher>();
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void OnEnable()
    {
        _searcher.ItemFound += AddFoundItem;
        _builder.UnitRequested += SetRequestData;
    }

    private void OnDisable()
    {
        _searcher.ItemFound -= AddFoundItem;
        _builder.UnitRequested -= SetRequestData;
    }

    private void Update()
    {
        CollectItem();
    }

    public void CollectItem()
    {
        if (_itemsFound.Count == 0 || _units.Count == 0)
            return;

        Unit unit = GetFreeObject(_units);
        Item item = GetFreeObject(_itemsFound);

        if (unit != null || item != null)
            StartItemDelivery(unit, item);
    }

    public void CreateUnit()
    {
        if (_items < _unitCost)
            return;

        _items -= _unitCost;
        _unitsAvailable++;

        Unit unit = _unitSpawner.SpawnObject();
        _units.Add(unit);
        unit.SetHomeBase(_base);
        ScoreChanged?.Invoke(_items);
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        _units.Add(unit);
        _items++;
        ScoreChanged?.Invoke(_items);
        ReturnToPool(item);
    }

    private void StartItemDelivery(Unit unit, Item item)
    {
        if (_isHaveRequest && _unitsAvailable > 0)
        {
            _isHaveRequest = false;
            _builder.SelectBuilderUnit(unit);
            unit.SetHomeBase(_requestedBase);
            _unitsAvailable--;
            _requestedBase = null;
        }

        item.MarkAsFound();
        unit.SetDeliveryTask(item);
        _units.Remove(unit);
        _itemsFound.Remove(item);
    }

    private void ReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = null;
        item.ResetFoundStatus();
    }

    private T GetFreeObject<T>(List<T> list) where T : class
    {
        return list.Count > 0 ? list[0] : null;
    }

    private void AddFoundItem(Item item)
    {
        _itemsFound.Add(item);
    }

    private void SetRequestData(Base requestedBase, int baseCost)
    {
        _isHaveRequest = true;
        _requestedBase = requestedBase;
        _items -= baseCost;
        ScoreChanged?.Invoke(_items);
    }
}