using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParallaxMover : MonoBehaviour, IObserver<Direction>
{
    [SerializeField] private float _length;
    [SerializeField] private float _forwardParallaxSpeed, _backwordParallaxSpeed;
    private float _currentSpeed;
    [SerializeField] private LayerMask _resetterLayer;
    [SerializeField] private float _resetPosition;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterObserver(this);

        _currentSpeed = _forwardParallaxSpeed;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _resetPosition = _length * 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBackground();
    }

    //Bad, need different approach
    private void MoveBackground()
    {
        transform.position = new Vector3(transform.position.x + _currentSpeed, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer) == _resetterLayer.value)
        {
            ResetterCollision();
        }
    }

    public void NewItemAdded(Direction type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemRemoved(Direction type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemAltered(Direction type, int count)
    {
        _currentSpeed = _backwordParallaxSpeed;
        _resetPosition = -_resetPosition;
    }

    public virtual void ResetterCollision()
    {
        transform.position = new Vector3(transform.position.x + _resetPosition, transform.position.y, transform.position.z);
    }
}

