using System;
using UnityEngine;

[RequireComponent(typeof(BaseGatherer))]
public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private Base _newBase;
    private BaseSpawner _spawner;
    private BaseGatherer _gatherer;
    private int _baseCost = 5;

    public event Action BuildStarted;
    public event Action BuildFinished;
    public event Action<Base, int> UnitRequested;

    public bool IsSelected { get; private set; } = false;

    private void Start()
    {
        _gatherer = GetComponent<BaseGatherer>();
    }

    public void SelecSpawner(BaseSpawner baseSpawner)
    {
        if (baseSpawner != null)
        {
            _spawner = baseSpawner;
        }
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

        _newBase = _spawner.SpawnObject(_flag.transform.position);
        UnitRequested?.Invoke(_newBase, _baseCost);

        FinishBuild();
    }

    private void FinishBuild()
    {
        _flag.gameObject.SetActive(false);
        _flag.transform.position = transform.position;
        _newBase = null;
        IsSelected = false;
        BuildFinished?.Invoke();
    }
}