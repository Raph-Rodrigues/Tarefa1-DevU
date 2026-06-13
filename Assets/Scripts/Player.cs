using UnityEngine;

public class Player : MonoBehaviour
{
  private Rigidbody _rb;
  private InputHandler _inputHandler;
  private Vector2 _moveInput;

  [Header("Camera")]
  [SerializeField] private Transform _mainCamera;

  [Header("Configurações de movimento")]
  [SerializeField] private float _moveSpeed = 5f;
  [SerializeField] private float _rotationSpeed = 10f;

  [Header("Configurações de Pulo")]
  [SerializeField] private int _maxJumps = 2;
  [SerializeField] private float _jumpForce = 15f;
  private int _jumpsRemaining;

  [Header("Verficação de Chão")]
  [SerializeField] private Transform _groundCheck;
  [SerializeField] private float _groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask _groundLayer;
  private bool _isGrounded;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Awake()
  {
    _rb = GetComponent<Rigidbody>();
    _inputHandler = GetComponent<InputHandler>();

    if (_mainCamera == null)
    {
      _mainCamera = Camera.main.transform;
    }
  }

  private void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void OnEnable()
  {
    if (_inputHandler != null)
    {
      _inputHandler.OnMoveInputChanged += HandleMoveInput;
      _inputHandler.OnJumpPressed += HandleJump;
    }
  }

  private void OnDisable()
  {
    if (_inputHandler != null)
    {
      _inputHandler.OnMoveInputChanged -= HandleMoveInput;
      _inputHandler.OnJumpPressed -= HandleJump;
    }
  }

  private void HandleMoveInput(Vector2 inputDirection)
  {
    _moveInput = inputDirection;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    MoveAndRotate();
  }

  void Update()
  {
    _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);

    if (_isGrounded && _rb.linearVelocity.y <= 0.1f)
    {
      _jumpsRemaining = _maxJumps;
    }
  }

  private void MoveAndRotate()
  {
    Vector3 camFoward = _mainCamera.forward;
    Vector3 camRight = _mainCamera.right;

    camFoward.y = 0f;
    camRight.y = 0f;
    camFoward.Normalize();
    camRight.Normalize();

    Vector3 movementDirection = (camFoward * _moveInput.y + camRight * _moveInput.x).normalized;

    Vector3 movement = movementDirection * _moveSpeed;
    _rb.linearVelocity = new Vector3(movement.x, _rb.linearVelocity.y, movement.z);

    if (movementDirection != Vector3.zero)
    {
      Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
      _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
    }
  }

  private void HandleJump()
  {
    if (_jumpsRemaining > 0)
    {
      _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
      _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
      _jumpsRemaining--;
    }
  }

  private void OnDrawGizmosSelected()
  {
    if (_groundCheck != null)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
  }
}
