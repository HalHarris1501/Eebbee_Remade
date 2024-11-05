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
    private int _obstacleNumTracker;
    private Direction _direction = Direction.Forward;

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

    private void GenerateNewObstacle()
    {
        _obstacleNumTracker++;
        ClearObstaclesBox();

        int obstacleID = Random.Range(0, _obstacles.Count);
        Obstacle currentObstacle = _obstacles[obstacleID];
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
