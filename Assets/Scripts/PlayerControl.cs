using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _actionGrab, _actionJump;

    private Rigidbody2D _rigidbody;
    private SpringJoint2D _springJoint;
    private LineRenderer _lineRenderer;
    private SpriteRenderer _spriteRenderer;
    private GroundDetector _groundDetector;
    [SerializeField] private GameObject _tongueEnd;

    [SerializeField] private GameObject[] _bloodSplatters;
    [SerializeField] private GameObject[] _bodyParts;

    private Vector2 _cursorWorldPos = Vector2.zero;

    private bool _grabbed = false;
    private Vector2 _springJointAnchor = Vector2.zero;
    private float _springJointDistance = 0f;

    private LayerMask _maskLevel;

    [SerializeField] private float _maxJumpTime = 0.5f;
    [SerializeField] private float _maxJumpForce = 1000f;
    private float _jumpTimer = 0f;

    private bool _isGrounded = false;

    [SerializeField] private float _realInDuration = 0.3f;
    [SerializeField] private float _realInDistance = 4f;

    [SerializeField] private float _springDampingRatio = 1f;
    [SerializeField] private float _springFrequency = 1f;

    [SerializeField] private Sprite[] _spriteIdle;
    // [SerializeField] private Sprite[] _spriteIdleTongue;
    [SerializeField] private Sprite[] _spriteJumpFront;
    [SerializeField] private Sprite[] _spriteJumpRight;

    [SerializeField] private GameObject[] _tongueOrigin;
    [SerializeField] private GameObject _tongueOriginParent;

    private int _currentTongueOrigin = 0;
    private bool _flipTongueXAxis = false;

    private Coroutine _realInCoroutine = null;
    private Coroutine _animationCoroutine = null;

    // Sound effects
    [SerializeField] private SoundEffectSO _SOGrab;
    [SerializeField] private SoundEffectSO _SOBleh;
    [SerializeField] private SoundEffectSO _SOJump;
    [SerializeField] private SoundEffectSO _SOLand;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _springJoint = GetComponent<SpringJoint2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _groundDetector = GetComponentInChildren<GroundDetector>();
        _springJoint.enabled = false;

        _actionGrab = _playerInput.actions["Grab"];
        _actionJump = _playerInput.actions["Jump"];

        _maskLevel = LayerMask.GetMask("Level");
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
        bool previousFrameIsGrounded = _isGrounded;
        _isGrounded = _groundDetector.IsGrounded;

        if (!previousFrameIsGrounded && _isGrounded)
        {
            if (Time.timeSinceLevelLoad > 0.1) // To avoid landing sound playing on first frame
            {
                _SOLand.Play();
            }
        }

        if (_grabbed)
        {
            float distanceToConnectedAnchor = Vector2.Distance(transform.position, _springJointAnchor);

            // Keep minimum distance as new spring distance
            // if (distanceToConnectedAnchor < _springJointDistance)
            // {
            //     _springJoint.distance = distanceToConnectedAnchor;
            //     _springJointDistance = distanceToConnectedAnchor;
            // }

            // Disable spring if closer than minimum distance to avoid pushing back player
            if (distanceToConnectedAnchor - 0.0001f < _springJointDistance)
            {
                _springJoint.enabled = false;
            }
            else
            {
                _springJoint.enabled = true;
            }

            // update tongue position
            _tongueOriginParent.transform.localScale = new Vector3(_flipTongueXAxis ? -1 : 1, 1, 1);
            Vector2 tongueOrigin = _tongueOrigin[_currentTongueOrigin].transform.position;
            Vector3[] positions = { tongueOrigin, _springJointAnchor };
            _lineRenderer.SetPositions(positions);
            _lineRenderer.enabled = true;

            _tongueEnd.SetActive(true);
            _tongueEnd.transform.position = new Vector3(_springJointAnchor.x, _springJointAnchor.y, 0f);
        }
        else
        {
            _lineRenderer.enabled = false;
            _tongueEnd.SetActive(false);

        }

        if (!_isGrounded)
        {
            StopAnimation();
        }

        Vector2 playerVelocity = _rigidbody.linearVelocity;

        if (_isGrounded)
        {
            if (_playerInput.actions["Jump"].IsPressed())
            {
                _jumpTimer += Time.deltaTime;
                StopAnimation();
                _spriteRenderer.sprite = _spriteJumpFront[0];
                _currentTongueOrigin = 3;
            }
            else
            {
                StartAnimation(_spriteIdle);
                _currentTongueOrigin = 0;
            }
        }
        else
        {
            if (Mathf.Abs(playerVelocity.x) < 0.001f)
            {
                if (playerVelocity.y > 0f)
                {
                    if (playerVelocity.y > 10f)
                    {
                        _spriteRenderer.sprite = _spriteJumpFront[1];
                        _currentTongueOrigin = 1;
                    }
                    else
                    {
                        _spriteRenderer.sprite = _spriteJumpFront[2];
                        _currentTongueOrigin = 1;
                    }
                }
                else
                {
                    _spriteRenderer.sprite = _spriteJumpFront[3];
                    _currentTongueOrigin = 4;
                }
            }
            else
            {
                if (playerVelocity.y > 3f)
                {
                    _spriteRenderer.sprite = _spriteJumpRight[1];
                    _currentTongueOrigin = 4;
                }
                else if (playerVelocity.y > -3f)
                {
                    _spriteRenderer.sprite = _spriteJumpRight[2];
                    _currentTongueOrigin = 5;
                }
                else
                {
                    _spriteRenderer.sprite = _spriteJumpRight[3];
                    _currentTongueOrigin = 6;
                }
            }
        }

        if (Mathf.Abs(playerVelocity.x) > 0.001f)
        {
            _spriteRenderer.flipX = playerVelocity.x < 0f;
            _flipTongueXAxis = playerVelocity.x < 0f;
        }
    }

    void StartAnimation(Sprite[] sprites)
    {
        // StopAnimation();
        if (_animationCoroutine != null) return;
        _animationCoroutine = StartCoroutine(AnimationCoroutine());
    }

    void StopAnimation()
    {
        if (_animationCoroutine == null) return;
        StopCoroutine(_animationCoroutine);
        _animationCoroutine = null;
    }

    void PressJump(InputAction.CallbackContext context)
    {
        // _jumpTimer = 0f;
    }

    void ReleaseJump(InputAction.CallbackContext context)
    {
        // Debug.Log("ReleaseJump");
        Jump();
    }

    void Jump()
    {
        if (_isGrounded)
        {
            // Debug.Log("Jump");
            float timer = Mathf.Clamp(_jumpTimer, 0f, _maxJumpTime);
            _rigidbody.AddForceY(_maxJumpForce * Utilities.MapRange(timer, 0f, _maxJumpTime, 0f, 1f));
            _SOJump.Play();
        }
        else
        {
            // Debug.Log("cannot jump, not grounded!");
        }

        _jumpTimer = 0f;
    }

    void PressGrab(InputAction.CallbackContext context)
    {
        Vector2 dir = (_cursorWorldPos - transform.position.xy()).normalized;
        float maxDistance = Vector2.Distance(_cursorWorldPos, transform.position.xy()) + 4f;
        var hit = Physics2D.Raycast(transform.position.xy(), dir, maxDistance, _maskLevel);
        if (!hit) return;

        Vector2 grabPos = hit.point;

        // Debug.Log("start Grab");
        float distance = Mathf.Max(Vector2.Distance(transform.position, grabPos), 0f);

        _springJoint.enableCollision = true;
        _springJoint.autoConfigureDistance = false;
        _springJoint.dampingRatio = _springDampingRatio;
        _springJoint.frequency = _springFrequency;
        _springJoint.connectedAnchor = grabPos;
        _springJoint.distance = distance;
        _springJoint.enabled = true;

        _grabbed = true;
        _springJointAnchor = grabPos;
        _springJointDistance = distance;

        if (_realInCoroutine != null) StopCoroutine(_realInCoroutine);
        _realInCoroutine = StartCoroutine(RealInCoroutine());

        _SOGrab.Play();
    }

    void ReleaseGrab(InputAction.CallbackContext context)
    {
        // Debug.Log("Release Grab");
        _springJoint.enabled = false;
        _grabbed = false;
    }

    void OnLook(InputValue value)
    {
        Vector2 mousePosition = value.Get<Vector2>();
        _cursorWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    IEnumerator RealInCoroutine()
    {
        float timer = _realInDuration;

        while (true)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) yield break;

            _springJoint.distance -= _realInDistance * (Time.deltaTime / _realInDuration);
            _springJointDistance = _springJoint.distance;
            yield return null;
        }
    }

    IEnumerator AnimationCoroutine()
    {
        while (true)
        {
            foreach (Sprite sp in _spriteIdle)
            {
                _spriteRenderer.sprite = sp;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf) return;
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Danger")
        {
            print($"OnTriggerEnter2D {collision.gameObject.name}");
            Death();
        }
    }

    void SpawnBloodSplatters()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, _maskLevel);
        // if (hit)
        // {
        //     // To align sprite with tilemap pixels
        //     const float r = 1f / 16f; // hardcoded (tilemap tiles are 16x16)
        //     var p = new Vector2(hit.point.x - (hit.point.x % r), hit.point.y - (hit.point.y % r));
        //     Instantiate(_bloodSplatters[0], p, Quaternion.identity);
        // }
        Vector2 originP = transform.position.xy();

        if (hit)
        {
            originP = hit.point;
        }

        // foreach (int i in Enumerable.Range(0, 3))
        // {
        Vector2 p = originP;// + Random.insideUnitCircle * 0.3f;
        const float r = 1f / 16f; // hardcoded (tilemap tiles are 16x16)
        p.x -= p.x % r;
        p.y -= p.y % r;
        Instantiate(_bloodSplatters[0], p, Quaternion.identity);
        // }
    }

    void Death()
    {
        SpawnBloodSplatters();

        foreach (var bodypart in _bodyParts)
        {
            var part = Instantiate(bodypart, transform.position, Quaternion.identity);

            Vector2 outwardRandomForce = Random.insideUnitCircle * Random.Range(1f, 1.5f);
            part.GetComponent<Rigidbody2D>().linearVelocity = _rigidbody.linearVelocity * 0.7f + outwardRandomForce;
            DontDestroyOnLoad(part);
        }

        gameObject.SetActive(false);

        FindFirstObjectByType<AudioManager>().Invoke(
            () => { TransitionManager.Instance.TransitionToScene("Game", 0.2f); },
            1.5f
        );
    }
}
