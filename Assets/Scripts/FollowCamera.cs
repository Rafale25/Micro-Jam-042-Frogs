using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private Transform _transformTarget;

    void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, _transformTarget.position.y, -10);
    }
}
