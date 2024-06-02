using UnityEngine;

[RequireComponent(typeof(UnitMover), typeof(Unit))]
public class UnitTaskHandler : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;
    [SerializeField] private Vector3 _pickUpOffset;

    private Unit _unit;
    private UnitMover _unitMover;
    private Item _item;
    private bool _isActive = false;
    private bool _isEquipped = false;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _unitMover = GetComponent<UnitMover>();
    }

    private void Update()
    {
        if (_isActive == true && _isEquipped == false)
        {
            TryToCollect();
        }
        else if (_isActive == true && _isEquipped == true)
        {
            TryToStock();
        }
    }

    public void SetTarget(Item item)
    {
        _item = item;
        _isActive = true;
    }

    private void TryToCollect()
    {
        if (_item != null)
        {
            _unitMover.MoveTo(_item.transform);
            PickUpItem();
        }
    }

    private void TryToStock()
    {
        if (_unit.HomeBase != null)
        {
            _unitMover.MoveTo(_unit.HomeBase.transform);
            StockItem();
        }
    }

    private void PickUpItem()
    {
        float checkDistance = Vector3.SqrMagnitude(transform.position - _item.transform.position);

        if (checkDistance > _interactionDistance)
            return;

        _item.transform.parent = gameObject.transform;
        _item.transform.localPosition = Vector3.zero + _pickUpOffset;
        _isEquipped = true;
    }

    private void StockItem()
    {
        float checkDistance = Vector3.SqrMagnitude(transform.position - _unit.HomeBase.transform.position);

        if (checkDistance > _interactionDistance)
            return;

        _unit.HomeBase.GetBaseGatherer().FinishItemDelivery(_unit, _item);
        _item = null;
        _isEquipped = false;
        _isActive = false;
    }
}