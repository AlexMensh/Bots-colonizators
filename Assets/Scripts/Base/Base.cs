using UnityEngine;

[RequireComponent(typeof(BaseGatherer), typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    private BaseGatherer _gatherer;
    private BaseBuilder _builder;

    private bool _isBuildPriority = false;
    public BaseGatherer Gatherer => _gatherer;

    private void Awake()
    {
        _gatherer = GetComponent<BaseGatherer>();
        _builder = GetComponent<BaseBuilder>();
    }

    private void OnEnable()
    {
        _builder.BuildTaskStarted += ChangeBuildPriority;
        _builder.BuildTaskFinished += ChangeBuildPriority;
    }

    private void OnDisable()
    {
        _builder.BuildTaskStarted -= ChangeBuildPriority;
        _builder.BuildTaskFinished -= ChangeBuildPriority;
    }

    private void Update()
    {
        if (_isBuildPriority == false)
            _gatherer.CreateUnit();
        else
            _builder.BuildBase();
    }

    private void ChangeBuildPriority()
    {
        _isBuildPriority = !_isBuildPriority;
    }
}