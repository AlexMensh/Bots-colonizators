using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base), typeof(Searcher), typeof(UnitSpawner))]
public class BaseGatherer : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private int _items;

    private Base _base;
    private BaseBuilder _builder;
    private Searcher _searcher;
    private UnitSpawner _unitSpawner;
    private List<Item> _itemsFound = new List<Item>();
    private List<Unit> _units = new List<Unit>();
    private int _unitCost = 3;

    public int Items { get { return _items; } }

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        _base = GetComponent<Base>();
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
        _units.Remove(unit);
        _itemsFound.Remove(item);
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

    private void AddFoundItem(Item item)
    {
        _itemsFound.Add(item);
        item.MarkAsFound();
    }
}