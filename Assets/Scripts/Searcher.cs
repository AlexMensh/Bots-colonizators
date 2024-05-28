using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;

    private List<Item> _itemsFound = new List<Item>();
    private Coroutine _searchItems;

    public List<Item> ItemsFound => new(_itemsFound);

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

    public void RemoveItem(Item item)
    {
        _itemsFound.Remove(item);
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
                    item.isFound = true;
                    _itemsFound.Add(item);
                }
            }
            yield return _searchDelay;
        }
    }
}
