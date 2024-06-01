using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Item _prefab;

    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private float _spawnDelay;

    private ObjectPooler<Item> _pool;

    private void Awake()
    {
        _pool = new ObjectPooler<Item>(_prefab, _container);
    }

    private void Start()
    {
        StartCoroutine(GenerateItems());
    }

    private IEnumerator GenerateItems()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            SpawnObject();

            yield return wait;
        }
    }

    private void SpawnObject()
    {
        int randomValue = Random.Range(0, _spawnPoints.Count);

        var item = _pool.GetObject();

        item.gameObject.SetActive(true);
        item.transform.parent = _container;
        item.transform.position = _spawnPoints[randomValue].transform.position;
    }
}
