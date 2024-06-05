using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    public Base SpawnObject(Vector3 position)
    {
        Base newBase = Instantiate(_prefab, position, Quaternion.identity);

        return newBase;
    }
}
