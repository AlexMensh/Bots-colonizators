using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public Unit SpawnObject()
    {
        Unit unit = Instantiate(_prefab);

        return unit;
    }
}