using UnityEngine;

[RequireComponent(typeof(BaseGatherer), typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    private BaseGatherer _gatherer;
    private BaseBuilder _builder;

    private bool _isBuildPriority = false;

    private void Awake()
    {
        _gatherer = GetComponent<BaseGatherer>();
        _builder = GetComponent<BaseBuilder>();
    }

    private void OnEnable()
    {
        _builder.BuildStarted += ChangeBuildPriority;
        _builder.BuildFinished += ChangeBuildPriority;
    }

    private void OnDisable()
    {
        _builder.BuildStarted -= ChangeBuildPriority;
        _builder.BuildFinished -= ChangeBuildPriority;
    }

    private void Update()
    {
        if (_isBuildPriority == false)
        {
            _gatherer.CreateUnit();
        }
        else
        {
            _builder.BuildBase();
        }
    }

    public BaseGatherer GetBaseGatherer()
    {
        return _gatherer;
    }

    private void ChangeBuildPriority()
    {
        _isBuildPriority = !_isBuildPriority;
    }
}