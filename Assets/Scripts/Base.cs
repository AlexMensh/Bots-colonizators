using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Searcher), typeof(UnitSpawner))]
public class Base : MonoBehaviour
{
    private Searcher _searcher;
    private UnitSpawner _spawnerUnit;
    private ItemSpawner _spawnerItem;

    private void Awake()
    {
        _searcher = GetComponent<Searcher>();
        _spawnerUnit = GetComponent<UnitSpawner>();
        _spawnerItem = GetComponent<ItemSpawner>();
    }

    private void Update()
    {
        Work();
    }

    private void Work()
    {
        if (_searcher.ItemsFound.Count == 0)
            return;

        if (_spawnerUnit.UnitsAvailable.Count == 0)
            return;

        Unit unit = GetFreeUnit();
        Item item = GetFreeItem();

        if (unit != null || item != null)
            StartItemDelivery(unit, item, this);
    }

    public void FinishItemDelivery(Unit unit)
    {
        _spawnerUnit.AddUnit(unit);
    }

    private void StartItemDelivery(Unit unit, Item item, Base homeBase)
    {
        _searcher.RemoveItem(item);
        _spawnerUnit.RemoveUnit(unit);

        unit.SetDeliveryTask(item);
        unit.SetHomeBase(homeBase);
    }

    private Unit GetFreeUnit()
    {
        if (_spawnerUnit.UnitsAvailable.Count > 0)
        {
            return _spawnerUnit.UnitsAvailable[0];
        }
        return null;
    }

    private Item GetFreeItem()
    {
        if (_searcher.ItemsFound.Count > 0)
        {
            return _searcher.ItemsFound[0];
        }
        return null;
    }
}