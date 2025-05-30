using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _beeRigidBody;
    private BoxCollider2D _beeCollider;
    private static PlayerInputActions _playerInputActions;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private LayerMask _obstacleLayer, _collectableLayer, _hiveLayer;
    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;
    public delegate void OnPlayerWin();
    public static OnPlayerWin onPlayerWin;
    private Vector2 inputVector = new Vector2();

    private void Awake()
    {
        _beeRigidBody = GetComponent<Rigidbody2D>();
        _beeCollider = GetComponent<BoxCollider2D>();

        _playerInputActions = new PlayerInputActions();
        
        SetDefaultControls();
        SetColliderActive(false);
        SetControlsActive(false);
        SetRigidbodyActive(false);
    }

    //movebee
    private void Movement()
    {
        inputVector = _playerInputActions.FindAction("Movement").ReadValue<Vector2>();
        _beeRigidBody.AddForce(new Vector3(inputVector.x, inputVector.y, 0) * _acceleration, ForceMode2D.Force);

        if (_playerInputActions.FindAction("Movement").ReadValue<Vector2>().magnitude != 0f)
        {
            AudioManager.Instance.PlaySoundAffect(AudioTag.BuzzSound, false);
        }

        if (_beeRigidBody.velocity.magnitude > _maxSpeed)
        {
            _beeRigidBody.velocity = Vector2.ClampMagnitude(_beeRigidBody.velocity, _maxSpeed);
        }

        if(_playerInputActions.FindAction("Go Back").IsPressed())
        {
            _playerInputActions.FindAction("Go Back").Disable();
            GameManager.Instance.GoBack();
            this.gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
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

    public void SetColliderActive(bool activeState)
    {
        _beeCollider.enabled = activeState;
    }

    public void SetRigidbodyActive(bool activeState)
    {
        switch(activeState)
        {
            case true:
                _beeRigidBody.bodyType = RigidbodyType2D.Dynamic;
                break;
            case false:
                _beeRigidBody.bodyType = RigidbodyType2D.Kinematic;
                _beeRigidBody.velocity = new Vector2(0f, 0f);
                break;
        }
    }

    public void SetControlsActive(bool activeState)
    {
        switch(activeState)
        {
            case false:
                _playerInputActions.Disable();
                break;
            case true:
                _playerInputActions.Keyboard.Enable();
                break;
        }
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
        if (_beeRigidBody != null)
        {
            _speed = _beeRigidBody.velocity.magnitude;
            Movement();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if ((1 << collision.gameObject.layer) == _obstacleLayer.value)
        {
            onPlayerDeath?.Invoke();
        }
        if ((1 << collision.gameObject.layer) == _collectableLayer.value)
        {
            collision.GetComponent<Collectable>().CollectableData.OnCollect(collision.gameObject);
        }
        if ((1 << collision.gameObject.layer) == _hiveLayer.value)
        {
            onPlayerWin?.Invoke();
        }
    }
}
