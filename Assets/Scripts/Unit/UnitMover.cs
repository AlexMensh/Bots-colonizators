using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void MoveTo(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }
}