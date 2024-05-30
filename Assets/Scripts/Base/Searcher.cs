using System;
using System.Collections;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;
    [SerializeField] private LayerMask _itemLayerMask;

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
        WaitForSeconds wait = new WaitForSeconds(_searchDelay);

        while (enabled)
        {
            Collider[] detectedItems = Physics.OverlapSphere(transform.position, _searchRadius, _itemLayerMask);

            foreach (var detected in detectedItems)
            {
                if (detected.TryGetComponent(out Item item) && item.IsFound == false)
                {
                    ItemFound?.Invoke(item);
                    item.MarkAsFound();
                }
            }

            yield return wait;
        }
    }
}
