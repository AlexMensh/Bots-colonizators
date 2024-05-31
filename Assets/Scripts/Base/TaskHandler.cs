using UnityEngine;

[RequireComponent(typeof(Base))]
public class TaskHandler: MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private bool _isSelected;

    private Base _base;

    private void Awake()
    {
        _base = GetComponent<Base>();
    }

    private void OnEnable()
    {
        _base.BuildFinished += EndTask;
    }

    private void OnDisable()
    {
        _base.BuildFinished -= EndTask;
    }

    public void StartTask(Vector3 hitPoint)
    {
        _flag.transform.position = hitPoint;
        _flag.gameObject.SetActive(true);
        _base.StartBuildingTask(hitPoint);
    }

    public void SelectedChange()
    {
        _isSelected = true;
    }

    public bool GetSelectedState()
    {
        return _isSelected;
    }

    private void EndTask()
    {
        _flag.transform.position = _base.transform.position;
        _flag.gameObject.SetActive(false);
        _isSelected = false;
    }
}
