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

    private int _baseCost = 5;
    private float _interactionDistance = 5f;
    private bool _isSelected = false;

    public event Action BuildTaskStarted;
    public event Action BuildTaskFinished;
    public event Action<Base, int> UnitRequested;

    private void Awake()
    {
        _gatherer = GetComponent<BaseGatherer>();
    }

    public void SelectSpawner(BaseSpawner baseSpawner)
    {
        _spawner = baseSpawner;
    }

    public void SelectBase()
    {
        _isSelected = true;
    }

    public void PlaceFlag(Vector3 position)
    {
        if (_isSelected)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = position;

            _newBase = null;

            BuildTaskStarted?.Invoke();
        }
    }

    public void BuildBase()
    {
        if (_gatherer.Items < _baseCost)
            return;

        _newBase = _spawner.SpawnObject(_flag.transform.position);

        UnitRequested?.Invoke(_newBase, _baseCost);

        _newBase.gameObject.SetActive(false);

        BuildTaskFinished?.Invoke();
    }

    public void SentUnit(Unit unit)
    {
        _buildCoroutine = StartCoroutine(MonitorUnitPosition(unit));
    }

    private void RemoveFlag()
    {
        _flag.gameObject.SetActive(false);
        _flag.transform.position = transform.position;
    }

    private void FinishBuild()
    {
        _newBase.gameObject.SetActive(true);
        _newBase = null;
        _isSelected = false;

        RemoveFlag();

        if (_buildCoroutine != null)
        {
            StopCoroutine(_buildCoroutine);
            _buildCoroutine = null;
        }
    }

    private IEnumerator MonitorUnitPosition(Unit unit)
    {
        while (Vector3.Distance(unit.transform.position, _flag.transform.position) > _interactionDistance)
        {
            yield return null;
        }

        FinishBuild();
    }
}