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

    private bool _isHaveRequest;
    private Base _requestedBase;

    private List<Unit> _units = new List<Unit>();
    private List<Item> _itemsFound = new List<Item>();
    private HashSet<Item> _foundMarker = new HashSet<Item>();
    private HashSet<Item> _assignedItems = new HashSet<Item>();

    public int Items => _items;

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
        if (_itemsFound.Count > 0 && _units.Count > 0)
            StartItemDelivery(GetFreeObject(_units), GetFreeObject(_itemsFound));
    }

    public void CreateUnit()
    {
        if (_items < _unitCost)
            return;

        _items -= _unitCost;
        _unitsAvailable++;

        Unit unit = _unitSpawner.SpawnObject();
        unit.SetHomeBase(_base);
        _units.Add(unit);
        ScoreChanged?.Invoke(_items);
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        _units.Add(unit);
        _items++;

        ReturnToPool(item);
        ScoreChanged?.Invoke(_items);
    }

    private void StartItemDelivery(Unit unit, Item item)
    {
        if (_isHaveRequest && _unitsAvailable > 0)
        {
            _isHaveRequest = false;
            _builder.SentUnit(unit);
            unit.SetHomeBase(_requestedBase);
            _unitsAvailable--;
            _requestedBase = null;
        }

        _assignedItems.Add(item);
        unit.SetDeliveryTask(item);
    }

    private void ReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = null;
        _foundMarker.Remove(item);
        _assignedItems.Remove(item);
    }

    private T GetFreeObject<T>(List<T> list) where T : class
    {
        if (list.Count > 0)
        {
            T freeObject = list[0];
            list.Remove(freeObject);
            return freeObject;
        }
        return null;
    }

    private void AddFoundItem(Item item)
    {
        if (_foundMarker.Contains(item) == false)
        {
            _foundMarker.Add(item);
            _itemsFound.Add(item);
        }
    }

    private void SetRequestData(Base requestedBase, int baseCost)
    {
        _isHaveRequest = true;
        _requestedBase = requestedBase;
        _items -= baseCost;
        ScoreChanged?.Invoke(_items);
    }
}