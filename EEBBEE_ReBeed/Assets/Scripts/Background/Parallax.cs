using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _length, _startPosition;
    [SerializeField] private float _parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBackground();
    }

    //Bad, need different approach
    private void MoveBackground()
    {
        transform.position = new Vector3(transform.position.x + _parallaxSpeed, transform.position.y, transform.position.z);

        if (transform.position.x < _startPosition - _length)
        {
            transform.position = new Vector3(_startPosition, transform.position.y, transform.position.z);
        }
    }
}
