using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour, IObserver<Direction>
{
    #region Singleton
    private static ObstacleManager _instance;
    public static ObstacleManager Instance
    {
        get //making sure that a obstacle manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObstacleManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("ObstacleManager");
                _instance = go.AddComponent<ObstacleManager>();
            }
            return _instance;
        }
    }
    #endregion

    [Header("Seed Generation")]
    [SerializeField] private string _gameSeed = "Normal";
    [SerializeField] private int _currentSeed = 0;

    [Header("Obstacles")]
    [SerializeField] private List<ObstacleBox> _obstacleBoxes;
    [SerializeField] private List<Obstacle> _obstacles;
    [SerializeField] private int _currentBox = 0;

    //obstacle saving info
    private Stack<List<SaveableObjectInfo>> _obstacleStack;
    private Dictionary<int, List<ObjectType>> _activeCollectablesDictionary;
    private int _obstacleNumTracker = 0;
    private Direction _direction = Direction.Forward;

    [Header("Collectables")]
    [SerializeField] private Collectable[] _collectables;
    [Range(0f, 100f)]
    [SerializeField] private float _collectableSpawnPercentage = 35.5f;

    private void Awake()
    {
        _currentSeed = _gameSeed.GetHashCode();
        Random.InitState(_currentSeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterObserver(this);
        _obstacleStack = new Stack<List<SaveableObjectInfo>>();
        _activeCollectablesDictionary = new Dictionary<int, List<ObjectType>>();
        _obstacleNumTracker = 0;
        ObjectPooler.objectPoolerReady += () => GenerateSpecificObstacle(3, false); //i dont understand entirely why this works
        ObjectPooler.objectPoolerReady += () => GenerateNewObstacle();
        ObjectPooler.objectPoolerReady += () => GenerateNewObstacle();
    }


    // Update is called once per frame
    void Update()
    {
       
    }

    public void LoadObstacle()
    {
        if(_direction == Direction.Forward)
        {
            GenerateNewObstacle();
        }
        else
        {
            LoadPreviousObstacle();
        }
    }

    private void GenerateSpecificObstacle(int obstacleID, bool spawnCollectable)
    {
        _obstacleNumTracker++;
        ClearObstaclesBox();

        ObstacleInfo currentObstacle = new ObstacleInfo(_obstacles[obstacleID]);

        if (spawnCollectable == true)
        {
            GenerateCollectable(currentObstacle);
        }

        _obstacleStack.Push(currentObstacle.ObjectList);

        LoadObjects(currentObstacle.ObjectList);


        _currentBox++;
        if (_currentBox > 2)
        {
            _currentBox = 0;
        }
    }

    private void GenerateNewObstacle()
    {
        _obstacleNumTracker++;
        ClearObstaclesBox();

        int obstacleID = Random.Range(1, _obstacles.Count);
        ObstacleInfo currentObstacle = new ObstacleInfo(_obstacles[obstacleID]);

        GenerateCollectable(currentObstacle);

        _obstacleStack.Push(currentObstacle.ObjectList);

        LoadObjects(currentObstacle.ObjectList);
        

        _currentBox++;
        if (_currentBox > 2)
        {
            _currentBox = 0;
        }
    }

    private void LoadPreviousObstacle()
    {       
        ClearObstaclesBox();

        if (_obstacleStack.Count > 0)
        {
            LoadObjects(_obstacleStack.Pop());
        }
        _obstacleNumTracker--;


        _currentBox--;
        if (_currentBox < 0)
        {
            _currentBox = 2;
        }
    }

    private void ClearObstaclesBox()
    {
        int childrenCount = _obstacleBoxes[_currentBox].transform.childCount;
        for (int i = 0; i < childrenCount; i++)
        {
            GameObject currentChild = _obstacleBoxes[_currentBox].transform.GetChild(0).gameObject;
            currentChild.transform.SetParent(null);
            currentChild.SetActive(false);
        }
    }

    private void LoadObjects(List<SaveableObjectInfo> objectList)
    {
        _obstacleBoxes[_currentBox].ObstacleNumber = _obstacleNumTracker;
        List<ObjectType> currentObstacleCollectableTypeList = new List<ObjectType>();
        foreach (SaveableObjectInfo saveableObject in objectList)
        {
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool(saveableObject.Type.ToString(), _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity);
            CheckIfCollectable(currentObject, currentObstacleCollectableTypeList);
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform);
        }
        //only add new entry if going forward
        if (_direction == Direction.Forward)
        {
            _activeCollectablesDictionary.Add(_obstacleNumTracker, currentObstacleCollectableTypeList);
        }
    }

    private void CheckIfCollectable(GameObject currentObject, List<ObjectType> currentList)
    {
        //ignore if not collectable
        if (currentObject.GetComponent<Collectable>() == null)     
            return;
        
        if(_direction == Direction.Forward)
        {
            //add to dictionary if going forwards
            currentList.Add(currentObject.GetComponent<SaveableObject>().ObjectType);
            return;
        }

        //ignore if no more obstacles
        if (_obstacleNumTracker <= 0)
            return;

        //default deactivate collectables going back
        currentObject.SetActive(false);
        if(_activeCollectablesDictionary[_obstacleNumTracker].Contains(currentObject.GetComponent<SaveableObject>().ObjectType))
        {
            currentObject.SetActive(true);
        }
    }

    public void UpdateObstacle(int obstacleNumber, SaveableObject objectToAffect)
    {
        if(_activeCollectablesDictionary[obstacleNumber].Contains(objectToAffect.ObjectType))
        {
            _activeCollectablesDictionary[obstacleNumber].Remove(objectToAffect.ObjectType);
        }
    }

    private void GenerateCollectable(ObstacleInfo obstacle)
    {
        float spawnCheck = Random.Range(0f, 100f);
        if (spawnCheck > _collectableSpawnPercentage)
            return; //cancel if it does meet spawn percentage requirements

        int collectableToSpawn = Random.Range(0, _collectables.Length); //decide collectable to spawn from array
        int placeToSpawn = Random.Range(0, obstacle.FreeSpace.Count); //get a free space to spawn in
        Vector3 spawnPos = new Vector3(obstacle.FreeSpace[placeToSpawn].x, obstacle.FreeSpace[placeToSpawn].y, 0); //set collectable position

        //set object data
        SaveableObjectInfo collectableInfo = new SaveableObjectInfo(_collectables[collectableToSpawn].GetComponent<SaveableObject>());
        collectableInfo.Position = spawnPos;
        obstacle.ObjectList.Add(collectableInfo); //add collectable to obstacle object list
        obstacle.FreeSpace.RemoveAt(placeToSpawn);
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
        _direction = type;
        if (_currentBox > 0)
        {
            _currentBox--;
        }
        else
        {
            _currentBox = 2;
        }
        LoadPreviousObstacle();
        LoadPreviousObstacle();
        LoadPreviousObstacle();
    }
}
