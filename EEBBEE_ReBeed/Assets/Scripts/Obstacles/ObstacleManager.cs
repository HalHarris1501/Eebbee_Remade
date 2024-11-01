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
    [SerializeField] private List<GameObject> _obstacleBoxes = new List<GameObject>();
    [SerializeField] private List<Obstacle> _obstacles = new List<Obstacle>();
    [SerializeField] private int _currentBox = 0;

    private Stack<int> _obstacleStack = new Stack<int>();
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
        ClearObstaclesBox();

        int obstacleID = Random.Range(0, _obstacles.Count);
        _obstacleStack.Push(obstacleID);
        Obstacle currentObstacle = _obstacles[obstacleID];

        LoadObjects(currentObstacle);

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
            LoadObjects(_obstacles[_obstacleStack.Pop()]);
        }

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

    private void LoadObjects(Obstacle currentObstacle)
    {
        foreach (SaveableObjectInfo saveableObject in currentObstacle.ObjectList)
        {
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool(saveableObject.Type.ToString(), _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity); ;
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform);
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
        _currentBox--;
        LoadPreviousObstacle();
        LoadPreviousObstacle();
    }
}
