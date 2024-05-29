using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    private UnitMover _unitMover;

    public Base HomeBase { get; private set; }

    private void Start()
    {
        _unitMover = GetComponent<UnitMover>();
    }

    public void SetHomeBase(Base homeBase)
    {
        HomeBase = homeBase;
    }

    public void SetDeliveryTask(Item item)
    {
        _unitMover.SetTarget(item);
    }
}
