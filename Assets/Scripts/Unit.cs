using UnityEngine;

[RequireComponent(typeof(UnitAction))]
public class Unit : MonoBehaviour
{
    private UnitAction _unitMover;
    
    public Base HomeBase { get; private set; }

    private void Start()
    {
        _unitMover = GetComponent<UnitAction>();
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
