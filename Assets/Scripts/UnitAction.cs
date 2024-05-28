using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitAction : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactionDistance;
    [SerializeField] private Vector3 _pickUpOffset;

    private bool _isActive = false;
    private bool _isPickedUp = false;
    private Unit _unit;
    private Item _item;

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        Action();
    }

    private void Action()
    {
        if (_isActive == true && _isPickedUp == false)
        {
            TryToCollect();
        }
        else if (_isActive == true && _isPickedUp == true)
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
            transform.position = Vector3.MoveTowards(transform.position, _item.transform.position, _speed * Time.deltaTime);
            PickUpItem();
        }
    }

    private void TryToStock()
    {
        if (_unit.HomeBase != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _unit.HomeBase.transform.position, _speed * Time.deltaTime);
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

        _isPickedUp = true;
    }

    private void StockItem()
    {
        float checkDistance = Vector3.SqrMagnitude(transform.position - _unit.HomeBase.transform.position);

        if (checkDistance > _interactionDistance)
            return;

        _unit.HomeBase.FinishItemDelivery(_unit);

        _isPickedUp = false;
        _isActive = false;
    }
}
