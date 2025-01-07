using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _beeRigidBody;
    private static PlayerInputActions _playerInputActions;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private LayerMask _obstacleLayer, _collectableLayer, _hiveLayer;

    private void Awake()
    {
        _beeRigidBody = GetComponent<Rigidbody2D>();

        _playerInputActions = new PlayerInputActions();
        
        SetDefaultControls();
    }

    //movebee
    private void Movement()
    {
        Vector2 inputVector = _playerInputActions.FindAction("Movement").ReadValue<Vector2>();
        _beeRigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * _acceleration, ForceMode2D.Force);
        if (_beeRigidBody.velocity.magnitude > _maxSpeed)
        {
            _beeRigidBody.velocity = Vector2.ClampMagnitude(_beeRigidBody.velocity, _maxSpeed);
        }

        if(_playerInputActions.FindAction("Go Back").IsPressed())
        {
            _playerInputActions.FindAction("Go Back").Disable();
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

    public static void SetDefaultControls()
    {
        _playerInputActions.Disable();
        _playerInputActions.Keyboard.Enable();
    }

    public static void InvertControls()
    {
        _playerInputActions.Disable();
        _playerInputActions.InvertedKeyboard.Enable();
    }

    private void FixedUpdate()
    {
        _speed = _beeRigidBody.velocity.magnitude;
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if ((1 << collision.gameObject.layer) == _obstacleLayer.value)
        {
            GameManager.Instance.ManageLose();
        }
        if ((1 << collision.gameObject.layer) == _collectableLayer.value)
        {
            collision.GetComponent<Collectable>().CollectableData.OnCollect(collision.gameObject);
        }
        if ((1 << collision.gameObject.layer) == _hiveLayer.value)
        {
            GameManager.Instance.ManageWin();
        }
    }
}
