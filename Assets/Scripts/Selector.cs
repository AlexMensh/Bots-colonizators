using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Selector : MonoBehaviour
{
    private Camera _camera;
    private Ray _ray;

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

        if (hit.collider.TryGetComponent(out Base selectBase))
        {
            
        }
    }
}
