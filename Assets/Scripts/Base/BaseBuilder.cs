using System;
using UnityEngine;

[RequireComponent(typeof(BaseSpawner), typeof(BaseGatherer))]
public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private BaseSpawner _spawner;
    private BaseGatherer _gatherer;
    private int _baseCost = 5;

    public event Action BuildStarted;
    public event Action BuildFinished;

    public bool IsSelected { get; private set; } = false;

    private void Start()
    {
        _spawner = GetComponent<BaseSpawner>();
        _gatherer = GetComponent<BaseGatherer>();
    }

    public void SelectBase()
    {
        IsSelected = true;
    }

    public void PlaceFlag(Vector3 position)
    {
        if (IsSelected)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = position;
            BuildStarted?.Invoke();
        }
    }

    public void BuildBase()
    {
        if (_gatherer.Items < _baseCost)
            return;

        _spawner.SpawnObject(_flag.transform.position);

        FinishBuild();
    }

    private void FinishBuild()
    {
        _flag.gameObject.SetActive(false);
        _flag.transform.position = transform.position;
        IsSelected = false;
        BuildFinished?.Invoke();
    }
}
