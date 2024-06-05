using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseGatherer))]
public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    private Base _newBase;
    private BaseSpawner _spawner;
    private BaseGatherer _gatherer;
    private Coroutine _buildCoroutine;
    private Unit _builderUnit;
    private int _baseCost = 5;
    private float _interactionDistance = 5f;

    public event Action BuildStarted;
    public event Action BuildFinished;
    public event Action<Base, int> UnitRequested;

    public bool IsSelected { get; private set; } = false;

    private void Awake()
    {
        _gatherer = GetComponent<BaseGatherer>();
    }

    public void SelectSpawner(BaseSpawner baseSpawner)
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

    public void SelectBuilderUnit(Unit unit)
    {
        _builderUnit = unit;
    }

    public void PlaceFlag(Vector3 position)
    {
        if (IsSelected)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = position;
            _builderUnit = null;
            _newBase = null;
            BuildStarted?.Invoke();
        }
    }

    public void BuildBase()
    {
        if (_gatherer.Items < _baseCost)
            return;

        _newBase = _spawner.SpawnObject(_flag.transform.position);

        UnitRequested?.Invoke(_newBase, _baseCost);

        _newBase.gameObject.SetActive(false);

        if (_builderUnit != null)
            _buildCoroutine = StartCoroutine(MonitorUnitPosition());
        

    }

    private void RemoveFlag()
    {
        _flag.gameObject.SetActive(false);
        _flag.transform.position = transform.position;
    }

    private void FinishBuild()
    {
        _newBase.gameObject.SetActive(true);
        BuildFinished?.Invoke();
        _newBase = null;
        IsSelected = false;
        RemoveFlag();


        if (_buildCoroutine != null)
        {
            StopCoroutine(_buildCoroutine);
            _buildCoroutine = null;
        }
    }

    private IEnumerator MonitorUnitPosition()
    {
        while (Vector3.Distance(_builderUnit.transform.position, _flag.transform.position) > _interactionDistance)
        {
            yield return null;
        }

        FinishBuild();
    }
}