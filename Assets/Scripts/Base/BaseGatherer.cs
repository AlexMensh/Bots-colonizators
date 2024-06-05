using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base), typeof(BaseBuilder), typeof(UnitSpawner))]
public class BaseGatherer : MonoBehaviour
{
    [SerializeField] private int _items;

    private Searcher _searcher;
    private Base _base;
    private BaseBuilder _builder;
    private UnitSpawner _unitSpawner;
    private int _unitCost = 3;
    private int _unitsAvailable;

    private bool _isHaveRequest;
    private Base _requestedBase;

    private List<Unit> _units = new List<Unit>();

    public int Items => _items;

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        _base = GetComponent<Base>();
        _builder = GetComponent<BaseBuilder>();
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void OnEnable()
    {
        _builder.UnitRequested += SetRequestData;
    }

    private void OnDisable()
    {
        _builder.UnitRequested -= SetRequestData;
    }

    private void Update()
    {
        GatheringItems();
    }

    public void SetSeracher(Searcher searcher)
    {
        _searcher = searcher;
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

    private void GatheringItems()
    {
        Item item = _searcher.GetItem();

        if (item != null && _units.Count > 0)
            StartItemDelivery(GetFreeObject(_units), item);
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

        unit.SetDeliveryTask(item);
    }

    private void ReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = null;
        _searcher.ResetItemStatus(item);
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

    private void SetRequestData(Base requestedBase, int baseCost)
    {
        _isHaveRequest = true;
        _requestedBase = requestedBase;
        _items -= baseCost;
        ScoreChanged?.Invoke(_items);
    }
}