using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for spawning and tracking obstacles during runtime
public class ObstacleManager : MonoBehaviour, IObserver<Direction> //observer for when the direction of player movement changes (between going forward to collect point, and going back to bank them)
{
    #region Singleton
    private static ObstacleManager _instance;
    public static ObstacleManager Instance
    {
        get //making sure that a obstacle manager always exists
        {
            if (_instance == null) //if there isn't a instance of the obstacle manager...
            {
                _instance = FindObjectOfType<ObstacleManager>(); //find one in the scene
            }
            if (_instance == null) //if there isn't one in the scene..
            {
                GameObject go = new GameObject("ObstacleManager"); //make a new game object called ObstacleManager...
                _instance = go.AddComponent<ObstacleManager>(); //and give it this script...
            }
            return _instance; //set the instance
        }
    }
    #endregion

    [Header("Seed Generation")]
    [SerializeField] private int _currentSeed; //local variable the seed being used by the RandomNumberGenerators to have a consistent way of generating obstacle and collectables
    [SerializeField] private SeedStorage _seedStorage; //scriptable object for storing the seed

    [Header("Obstacles")]
    [SerializeField] private List<ObstacleBox> _obstacleBoxes; //a list of gameobjects that contain the spawned obstacles and collectables, and allow them to all move simultaneously
    [SerializeField] private List<Obstacle> _obstacles; //list of all created obstacles that can be spawned
    [SerializeField] private int _currentBox = 0; //id for the current ObstacleBox to spawn the obstacle in

    //obstacle saving info
    private Stack<List<SaveableObjectInfo>> _obstacleStack; //stack containing the information of the previous obstacles and any collectables spawned
    private Dictionary<int, List<ObjectType>> _activeCollectablesDictionary; //dictionary contain info about previously spawned collectables that haven't been collected by the player
    private int _obstacleNumTracker = 0; //counter for the total number of obstacles spawned
    private Direction _direction = Direction.Stop; //local variable for the current direction of player movement

    [Header("Collectables")]
    [SerializeField] private Collectable[] _collectables; //an array of all collectables that can be spawned
    [Range(0f, 100f)]
    [SerializeField] private float _collectableSpawnPercentage = 35.5f; //varaible controlling the chance of a collectable being spawned when an obstacle is spawned

    [Header("Background Objects")]
    [SerializeField] private List<Background> _backgroundObjects; //list of background objects to allow for control of their parallax movement

    [Header("Settings Data")]
    [SerializeField] private SettingsSO settingsSO; //a scriptable object for the game settings data


    private void Awake()
    {
        if (_instance != null && _instance != this) //if there is an instance of the obstacler manager and it isn't this object...
        {
            Destroy(this.gameObject); //destroy this object
        }
        else
        {
            _instance = this; //otherwise, make this object the current instance
        }
        SetSeed(); //function to set the seed for this run
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterObserver(this); //set this as an observer to the GameManager to get updates about changes to player movement direction

        _obstacleStack = new Stack<List<SaveableObjectInfo>>(); //make a new stack for the obstacles
        _activeCollectablesDictionary = new Dictionary<int, List<ObjectType>>(); //make a new dictionary for the collectables
        _obstacleNumTracker = 0; //reset the number of obstacles spawned
        _currentBox = 0; //set the current box to spawn an obstacle in to be the first box

        //add listeners to the ObjectPooler, so that when it is ready it spawns the first 3 obstacles
        ObjectPooler.Instance.objectPoolerReady.AddListener(() => GenerateSpecificObstacle(0, false));
        ObjectPooler.Instance.objectPoolerReady.AddListener(() => GenerateNewObstacle());
        ObjectPooler.Instance.objectPoolerReady.AddListener(() => GenerateNewObstacle());
    }

    //funtion to set the current seed
    public void SetSeed()
    {
        _currentSeed = _seedStorage.Seed; //get the seed saved in the seed storage scriptable object
        Random.InitState(_currentSeed); //set the state of Random using the current seed
    }

    //funtion to load an obstacle into the scene
    public void LoadObstacle()
    {
        if(_direction == Direction.Stop) //if the player isn't currently moving, dont spawn an obstacle
        {
            return;
        }
        else if(_direction == Direction.Forward) //if the player is moving forward, spawn a new obstacle
        {
            GenerateNewObstacle(); //function to spawn a new obstacle
        }
        else //if the player is moving backwards, spawn the last obstacle that was despawned
        {
            LoadPreviousObstacle(); //function to spawn a previous obstacle
        }
    }

    //function to load specific obstacles using the obstacles id in the list, and using a bool to determin if collectables can spawn
    private void GenerateSpecificObstacle(int obstacleID, bool spawnCollectable)
    {
        _obstacleNumTracker++; //increase the number of obstacles spawned
        ClearObstaclesBox(); //function to clear the current obstacle box

        ObstacleInfo currentObstacle = new ObstacleInfo(_obstacles[obstacleID]); //create a temporary variable with the obstacle info, so that the original obstacle isn't altered

        if (spawnCollectable == true) //if spawning collectables is allowed, randomly attempt to spawn one
        {
            GenerateCollectable(currentObstacle); //function that spawns a random collectable using a percentage chance
        }

        _obstacleStack.Push(currentObstacle.ObjectList); //add the current obstacle, and any collectable made, to the obstacle stack

        LoadObjects(currentObstacle.ObjectList); //load the objects within the current obstacle into the scene

        _currentBox++; //set the next box to spawn an obstacle into
        if (_currentBox > 2)
        {
            _currentBox = 0;
        }
    }

    //function to spawn a new random obstacle
    private void GenerateNewObstacle()
    {
        _obstacleNumTracker++; //increase the number of obstacles spawned
        ClearObstaclesBox(); //function to clear the current obstacle box

        int obstacleID = Random.Range(1, _obstacles.Count); //pick a random obstacle to spawn from the list (excluding the hive at 'ID = 0')
        ObstacleInfo currentObstacle = new ObstacleInfo(_obstacles[obstacleID]); //create a temporary variable with the obstacle info, so that the original obstacle isn't altered

        GenerateCollectable(currentObstacle); //function that spawns a random collectable using a percentage chance

        _obstacleStack.Push(currentObstacle.ObjectList); //add the current obstacle, and any collectable made, to the obstacle stack

        LoadObjects(currentObstacle.ObjectList); //load the objects within the current obstacle into the scene       

        _currentBox++; //set the next box to spawn an obstacle into
        if (_currentBox > 2)
        {
            _currentBox = 0;
        }
    }

    //function to load a previously spawned obstacle
    private void LoadPreviousObstacle()
    {       
        ClearObstaclesBox(); //function to clear the current obstacle box

        if (_obstacleStack.Count > 0) //if there are still obstacles in the stack, load the top of the stack
        {
            LoadObjects(_obstacleStack.Pop()); //load the objects within the stack's top obstacle into the scene
        }
        else if(_obstacleStack.Count == 0) //if the stack is empty, stop the obstacle and background movement
        {
            StopAllMove(); //function to stop the movement of all obstacles and background objects
        }
        _obstacleNumTracker--; //decrease the number of obstacles spawned

        _currentBox--; //set the next box to spawn an obstacle into
        if (_currentBox < 0)
        {
            _currentBox = 2;
        }
    }

    //function to stop the movement of all obstacles and background objects
    private void StopAllMove()
    {
        foreach (ObstacleBox box in _obstacleBoxes) //set a variable to stop the movement of all obstacles boxes when they reach they're starting position
        {
            box.StopAtStart = true;
        }
        foreach (Background background in _backgroundObjects) //set a variable to stop the movement of all background objects when they reach they're starting position
        {
            background.StopAtStart = true;
        }
    }

    //function to clear the current obstacle box
    private void ClearObstaclesBox()
    {
        int childrenCount = _obstacleBoxes[_currentBox].transform.childCount; //get the number of child objects within the current obstacle box
        for (int i = 0; i < childrenCount; i++) //for every child found
        {
            GameObject currentChild = _obstacleBoxes[_currentBox].transform.GetChild(0).gameObject; //set the current child being affected
            currentChild.transform.SetParent(null); //detach the child object from the obstacle box
            currentChild.SetActive(false); //make the object inactive
        }
    }

    //function to load objects into the scene using a given object list
    private void LoadObjects(List<SaveableObjectInfo> objectList)
    {
        _obstacleBoxes[_currentBox].ObstacleNumber = _obstacleNumTracker; //set the obstacle box's obstacle number to be the current number of obstacles spawned
        List<ObjectType> currentObstacleCollectableTypeList = new List<ObjectType>(); //create a temporary list for the collectables within this obstacle
        foreach (SaveableObjectInfo saveableObject in objectList) //loop through each object in the object list
        {
            //set the current object to a temporary variable, and spawn it at the correct location
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool(saveableObject.Type.ToString(), _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity);
            ActivateBackground(currentObject); //function to activate the background for the current object (for increased visibility)
            CheckIfCollectable(currentObject, currentObstacleCollectableTypeList); //function to check if the current object is a collectable, and control what is done with it
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform); //set the parent of the current object to the obstacle box, so they move together
        }
        //only add new entry if going forward or stopped
        if (_direction == Direction.Forward || _direction == Direction.Stop)
        {
            _activeCollectablesDictionary.Add(_obstacleNumTracker, currentObstacleCollectableTypeList); //add collectables from the current obstacle to the dictionary with the current obstacle's number
        }
    }


    //function to activate the background for the current object (for increased visibility)
    private void ActivateBackground(GameObject currentObject)
    {
        if(settingsSO.SettingsData.IncreasedVisibility) //if the setting for background visibility is on, activate the background on the object
        {
            currentObject.GetComponent<SaveableObject>().Background.SetActive(true);
        }
        else //if the setting is off, make the background inactive
        {
            currentObject.GetComponent<SaveableObject>().Background.SetActive(false);
        }
    }

    //function to check if the current object is a collectable, and control what is done with it
    private void CheckIfCollectable(GameObject currentObject, List<ObjectType> currentList)
    {
        //ignore if not collectable
        if (currentObject.GetComponent<Collectable>() == null)     
            return;
        
        if(_direction == Direction.Forward || _direction == Direction.Stop) //if it is collectable, and player is going forwards or stopped
        {
            currentList.Add(currentObject.GetComponent<SaveableObject>().ObjectType); //add collectable to the list
            return;
        }

        //ignore if no more obstacles
        if (_obstacleNumTracker <= 0)
            return;
        
        currentObject.SetActive(false); //by default, deactivate collectables going back
        if (_activeCollectablesDictionary[_obstacleNumTracker].Contains(currentObject.GetComponent<SaveableObject>().ObjectType)) //if the collectable wasn't collected going forwards, set it active instead
        {
            currentObject.SetActive(true);
        }
    }

    //function to update and alter data in a previously spawned obstacle
    public void UpdateObstacle(int obstacleNumber, SaveableObject objectToAffect)
    {
        if(_activeCollectablesDictionary[obstacleNumber].Contains(objectToAffect.ObjectType)) //if the given object is in the active collectables dictionary, remove it from the dictionary
        {
            _activeCollectablesDictionary[obstacleNumber].Remove(objectToAffect.ObjectType);
        }
    }

    //function to spawn a collectable using a given obstacle
    private void GenerateCollectable(ObstacleInfo obstacle)
    {
        float spawnCheck = Random.Range(0f, 100f); //get a random number between 1 and 100
        if (spawnCheck > _collectableSpawnPercentage)
            return; //cancel if it does meet spawn percentage requirements

        int collectableToSpawn = Random.Range(0, _collectables.Length); //decide collectable to spawn from array
        int placeToSpawn = Random.Range(0, obstacle.FreeSpace.Count); //get a free space to spawn in
        Vector3 spawnPos = new Vector3(obstacle.FreeSpace[placeToSpawn].x, obstacle.FreeSpace[placeToSpawn].y, 0); //set collectable position

        //set object data
        SaveableObjectInfo collectableInfo = new SaveableObjectInfo(_collectables[collectableToSpawn].GetComponent<SaveableObject>());
        collectableInfo.Position = spawnPos; 
        obstacle.ObjectList.Add(collectableInfo); //add collectable to obstacle object list
        obstacle.FreeSpace.RemoveAt(placeToSpawn); //remove the free space where the collectable was spawned
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
        _direction = type; //set local variable for direction
        if (_direction == Direction.Stop || _direction == Direction.Forward) //if direction is forward or stop, do nothing
        {
            return;
        }

        //set the box to spawn an obstacle to be the previous box used
        if (_currentBox > 0) 
        {
            _currentBox--;
        }
        else
        {
            _currentBox = 2;
        }
        //load the last 3 obstacles
        LoadPreviousObstacle();
        LoadPreviousObstacle();
        LoadPreviousObstacle();
    }
}
