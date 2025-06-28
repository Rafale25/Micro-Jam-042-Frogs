using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _actionGrab, _actionJump;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private SpringJoint2D _springJoint;
    private Vector2 _cursorWorldPos = Vector2.zero;

    private bool _grabbed = false;
    private Vector2 _springJointAnchor = Vector2.zero;
    private float _springJointDistance = 0f;

    private const float _MaxJumpTime = 0.5f;
    private const float _MaxJumpForce = 1000f;
    private float _jumpTimer = 0f;

    // TODO: real in the tongue a little bit over the duration of n second/s after you start grabbing

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _springJoint = GetComponent<SpringJoint2D>();
        _springJoint.enabled = false;

        _actionGrab = _playerInput.actions["Grab"];
        _actionJump = _playerInput.actions["Jump"];
    }

    void OnEnable()
    {
        _actionGrab.performed += PressGrab;
        _actionGrab.canceled += ReleaseGrab;
        _actionJump.performed += PressJump;
        _actionJump.canceled += ReleaseJump;
    }

    void OnDisable()
    {
        _actionGrab.performed -= PressGrab;
        _actionGrab.canceled -= ReleaseGrab;
        _actionJump.performed -= PressJump;
        _actionJump.canceled -= ReleaseJump;
    }

    void Update()
    {
        if (_grabbed)
        {
            float distanceToConnectedAnchor = Vector2.Distance(_transform.position, _springJointAnchor);
            if (distanceToConnectedAnchor - 0.0001f < _springJointDistance)
            {
                _springJoint.enabled = false;
            }
            else
            {
                _springJoint.enabled = true;
            }
        }

        if (_playerInput.actions["Jump"].IsPressed())
        {
            _jumpTimer += Time.deltaTime;
        }
    }

    void PressJump(InputAction.CallbackContext context)
    {
        // _jumpTimer = 0f;
    }

    void ReleaseJump(InputAction.CallbackContext context)
    {
        Debug.Log("ReleaseJump");
        Jump();
    }

    void Jump()
    {
        LayerMask mask = LayerMask.GetMask("Level");
        if (Physics2D.Raycast(_transform.position, Vector2.down, 0.8f / 2f, mask))
        {
            Debug.Log("Jump");
            float timer = Mathf.Clamp(_jumpTimer, 0f, _MaxJumpTime);
            _rigidbody.AddForceY(_MaxJumpForce * Utilities.MapRange(timer, 0f, _MaxJumpTime, 0f, 1f));
        }
        else
        {
            Debug.Log("cannot jump, not grounded!");
        }

        _jumpTimer = 0f;
    }

    void PressGrab(InputAction.CallbackContext context)
    {
        Debug.Log("start Grab");
        float distance = Mathf.Max(Vector2.Distance(_transform.position, _cursorWorldPos), 0f);

        _springJoint.enableCollision = true;
        _springJoint.autoConfigureDistance = false;
        _springJoint.dampingRatio = 0.6f;
        _springJoint.frequency = 1.75f;
        _springJoint.connectedAnchor = _cursorWorldPos;
        _springJoint.distance = distance;
        _springJoint.enabled = true;

        _grabbed = true;
        _springJointAnchor = _cursorWorldPos;
        _springJointDistance = distance;
    }

    void ReleaseGrab(InputAction.CallbackContext context)
    {
        Debug.Log("Release Grab");
        _springJoint.enabled = false;
        _grabbed = false;
    }

    void OnLook(InputValue value)
    {
        Vector2 mousePosition = value.Get<Vector2>();
        _cursorWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        // Debug.Log(_cursorWorldPos);
    }
}
