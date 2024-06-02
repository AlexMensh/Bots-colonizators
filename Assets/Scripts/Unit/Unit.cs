using UnityEngine;

[RequireComponent(typeof(UnitTaskHandler))]
public class Unit : MonoBehaviour
{
    private UnitTaskHandler _unitTaskHandler;

    public Base HomeBase { get; private set; }

    private void Start()
    {
        _unitTaskHandler = GetComponent<UnitTaskHandler>();
    }

    public void SetHomeBase(Base homeBase)
    {
        HomeBase = homeBase;
    }

    public void SetDeliveryTask(Item item)
    {
        _unitTaskHandler.SetTarget(item);
    }
}
