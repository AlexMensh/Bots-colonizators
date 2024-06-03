using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Selector : MonoBehaviour
{
    [SerializeField] private BaseSpawner _spawner;

    private Camera _camera;
    private Ray _ray;
    private BaseBuilder _targetBase;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Select();
    }

    private void Select()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit hit) == false)
            return;

        if (hit.collider.TryGetComponent(out BaseBuilder targetBase))
        {
            _targetBase = targetBase;
            _targetBase.SelecSpawner(_spawner);
            _targetBase.SelectBase();
        }

        if (hit.collider.TryGetComponent(out Ground targetGround))
        {
            if (_targetBase != null)
            {
                _targetBase.PlaceFlag(hit.point);
            }
        }
    }
}