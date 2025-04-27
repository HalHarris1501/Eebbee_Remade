using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//function to move objects horizontally within the scene
public abstract class ParallaxMover : MonoBehaviour, IObserver<Direction> //observer for when the direction of player movement changes
{
    [SerializeField] private float _length; //variable to locally store this object's length
    [SerializeField] private float _forwardParallaxSpeed, _backwordParallaxSpeed; //variable to determine how fast this object moves in either direction
    [SerializeField] private float _currentSpeed; 
    [SerializeField] private LayerMask _resetterLayer; //variable to control the layer of collision needed to reset the objects position
    [SerializeField] private float _resetPosition; //postition to set the object to on collision with resetter
    [SerializeField] private Vector3 _startPos; //variable to store the position this object started at
    public bool StopAtStart = false; //bool to control whether the object should stop moving when it reaches it's starting position

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterObserver(this); //set this as an observer to the GameManager to get updates about changes to player movement direction
        _startPos = this.transform.position; //set start position to be it's current position

        _currentSpeed = 0; //set current speed to 0
        _length = GetComponent<SpriteRenderer>().bounds.size.x; //get the length of the object based on it's sprite size
        _resetPosition = _length * 3; //set the reset position
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(); //function to move the object
        if(StopAtStart) //check if the object should stop at start, and if it should, run a function to handle that
        {
            StopAtStartFunc();
        }
    }
    
    //function to move the object
    private void Move()
    {
        transform.position = new Vector3(transform.position.x + _currentSpeed, transform.position.y, transform.position.z); //adjust the position based on the current speed of the object
    }

    //functio to detect and stop the object when it reaches it's start position
    private void StopAtStartFunc()
    {
        if(this.transform.position.x == _startPos.x) //if it's at the start position
        {
            _currentSpeed = 0; //stop it from moving
            GameManager.Instance.StopMovers(); //stop all other parallax mover objects from moving
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer) == _resetterLayer.value) //check for a collision with a resetter object
        {
            ResetterCollision(); //handle collision
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

    //function that triggers when the direction is changed
    public void ItemAltered(Direction type, int count)
    {
        if (type == Direction.Forward) //if player is going forward
        {
            _currentSpeed = _forwardParallaxSpeed; //set this object's current speed to be it's forward speed
        }
        if (type == Direction.Backward) //if player is going backwards
        {
            _currentSpeed = _backwordParallaxSpeed; //set this object's current speed to be it's backwards speed
            _resetPosition = -_resetPosition; //set reset position to be the opposite side of the scene
        }
        else if(type == Direction.Stop) //if player is stopped
        {
            _currentSpeed = 0; //stop object from moving
        }
    }

    //function to handle what happens when colliding with a resetter object
    public virtual void ResetterCollision()
    {
        transform.position = new Vector3(transform.position.x + _resetPosition, transform.position.y, transform.position.z); //move the object back to it's reset position based on it's current position
    }
}

