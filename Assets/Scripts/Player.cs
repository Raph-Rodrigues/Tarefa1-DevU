using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private InputHandler _inputHandler;
    private Vector2 _moveInput;

    [Header("Configurações de movimento")]
    [SerializeField] private float _moveSpeed = 5f;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _inputHandler = GetComponent<InputHandler>();        
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveInputChanged += HandleMoveInput;
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveInputChanged -= HandleMoveInput;
        }
    }

    private void HandleMoveInput(Vector2 inputDirection)
    {
        _moveInput = inputDirection;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0, _moveInput.y) * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + movement);
    }
}
