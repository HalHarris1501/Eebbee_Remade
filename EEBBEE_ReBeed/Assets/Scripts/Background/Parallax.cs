using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _length, _startPosition;
    [SerializeField] private float _forwardParallaxSpeed, _backwordParallaxSpeed;
    [SerializeField] private LayerMask _resetterLayer;
    [SerializeField] private float _resetPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position.x;
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
        transform.position = new Vector3(transform.position.x + _forwardParallaxSpeed, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer) == _resetterLayer.value)
        {
            transform.position = new Vector3(transform.position.x + _resetPosition, transform.position.y, transform.position.z);
        }
    }
}
