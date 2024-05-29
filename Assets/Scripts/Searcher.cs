using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;

    private Coroutine _searchItems;

    public event Action<Item> ItemFound;

    private void OnEnable()
    {
        _searchItems = StartCoroutine(SearchItems());
    }

    private void OnDisable()
    {
        StopCoroutine(_searchItems);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }

    private IEnumerator SearchItems()
    {
        while (enabled)
        {
            Collider[] detectedItems = Physics.OverlapSphere(transform.position, _searchRadius);

            foreach (var detected in detectedItems)
            {
                if (detected.TryGetComponent(out Item item) && item.isFound == false)
                {
                    ItemFound?.Invoke(item);
                }
            }

            yield return _searchDelay;
        }
    }
}
