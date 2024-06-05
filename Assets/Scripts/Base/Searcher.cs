using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;
    [SerializeField] private LayerMask _itemLayerMask;

    private List<Item> _itemsFound = new List<Item>();
    private HashSet<Item> _foundMarker = new HashSet<Item>();

    private Coroutine _searchItems;

    private void OnEnable()
    {
        _searchItems = StartCoroutine(SearchItems());
    }

    private void OnDisable()
    {
        StopCoroutine(_searchItems);
    }

    public Item GetItem()
    {
        if (_itemsFound.Count > 0)
        {
            Item item = _itemsFound[0];
            _itemsFound.Remove(item);
            return item;
        }
        return null;
    }

    public void ResetItemStatus(Item item)
    {
        _foundMarker.Remove(item);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }

    private void AddFoundItem(Item item)
    {
        if (_foundMarker.Contains(item) == false)
        {
            _foundMarker.Add(item);
            _itemsFound.Add(item);
        }
    }

    private IEnumerator SearchItems()
    {
        WaitForSeconds wait = new WaitForSeconds(_searchDelay);

        while (enabled)
        {
            Collider[] detectedItems = Physics.OverlapSphere(transform.position, _searchRadius, _itemLayerMask);

            foreach (var detected in detectedItems)
            {
                if (detected.TryGetComponent(out BaseGatherer gatherer))
                {
                    gatherer.SetSeracher(this);
                }

                if (detected.TryGetComponent(out Item item))
                {
                    AddFoundItem(item);
                }
            }
            yield return wait;
        }
    }
}