using UnityEngine;

[RequireComponent(typeof(Searcher), typeof(UnitSpawner), typeof(ScoreCounter))]
public class Base : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private Searcher _searcher;
    private UnitSpawner _unitSpawner;
    private ScoreCounter _scoreCounter;

    private void Awake()
    {
        _searcher = GetComponent<Searcher>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _scoreCounter = GetComponent<ScoreCounter>();
    }

    private void Update()
    {
        Work();
    }

    private void Work()
    {
        if (_searcher.ItemsFound.Count == 0)
            return;

        if (_unitSpawner.UnitsAvailable.Count == 0)
            return;

        Unit unit = GetFreeUnit();
        Item item = GetFreeItem();

        if (unit != null || item != null)
            StartItemDelivery(unit, item, this);
    }

    public void FinishItemDelivery(Unit unit, Item item)
    {
        _unitSpawner.AddUnit(unit);

        item.gameObject.SetActive(false);
        item.transform.parent = _container.transform;
        item.isFound = false;

        _scoreCounter.AddPoint();
    }

    private void StartItemDelivery(Unit unit, Item item, Base homeBase)
    {
        _searcher.RemoveItem(item);
        _unitSpawner.RemoveUnit(unit);

        unit.SetDeliveryTask(item);
        unit.SetHomeBase(homeBase);
    }

    private Unit GetFreeUnit()
    {
        if (_unitSpawner.UnitsAvailable.Count > 0)
        {
            return _unitSpawner.UnitsAvailable[0];
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