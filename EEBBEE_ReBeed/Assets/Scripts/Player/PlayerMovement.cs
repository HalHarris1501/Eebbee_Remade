using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _beeRigidBody;
    private PlayerInputActions _playerInputActions;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;

    private void Awake()
    {
        _beeRigidBody = GetComponent<Rigidbody2D>();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Keyboard.Enable();
    }

    //movebee
    private void Movement()
    {
        Vector2 inputVector = _playerInputActions.Keyboard.Movement.ReadValue<Vector2>();
        _beeRigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * _acceleration, ForceMode2D.Force);
        if (_beeRigidBody.velocity.magnitude > _maxSpeed)
        {
            _beeRigidBody.velocity = Vector2.ClampMagnitude(_beeRigidBody.velocity, _maxSpeed);
        }

        if(_playerInputActions.Keyboard.GoBack.IsPressed())
        {
            GameManager.Instance.GoBack();
            this.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _speed = _beeRigidBody.velocity.magnitude;
        Movement();
    }
}
