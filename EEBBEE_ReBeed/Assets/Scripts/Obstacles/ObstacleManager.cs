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

    private Stack<List<SaveableObjectInfo>> _obstacleStack = new Stack<List<SaveableObjectInfo>>();
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
        foreach (SaveableObjectInfo saveableObject in objectList)
        {
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool(saveableObject.Type.ToString(), _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity);
            CheckIfCollectable(currentObject, saveableObject);
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform);
        }
        _obstacleBoxes[_currentBox].CurrentObstacle = objectList;
    }

    private void CheckIfCollectable(GameObject currentObject, SaveableObjectInfo saveableObject)
    {
        if(currentObject.GetComponent<Collectable>() != null)
        {
            currentObject.GetComponent<Collectable>().CollectableData.StartPosition = saveableObject.Position;
        }
    }

    public void UpdateObstacle(GameObject objectToRemove, List<SaveableObjectInfo> obstacleToUpdate)
    {
        Debug.Log("Updating obstacle");
        if(!_obstacleStack.Contains(obstacleToUpdate))
        {
            Debug.Log("Obstacle not in stack");
            return;
        }

        Stack<List<SaveableObjectInfo>> temp = new Stack<List<SaveableObjectInfo>>();
        while(_obstacleStack.Count > 0)
        {
            List<SaveableObjectInfo> obstacleToCheck = _obstacleStack.Pop();
            if (obstacleToCheck == obstacleToUpdate)
            {
                Debug.Log("obstacle found");
                SaveableObjectInfo objectInfoToRemove = new SaveableObjectInfo(objectToRemove.GetComponent<SaveableObject>());
                objectInfoToRemove.Position = objectToRemove.GetComponent<Collectable>().CollectableData.StartPosition;
                foreach (SaveableObjectInfo objectToFind in obstacleToCheck)
                {
                    if(objectToFind == objectInfoToRemove)
                    {
                        Debug.Log("Found object info to remove");
                        obstacleToCheck.Remove(objectInfoToRemove);
                        break;
                    }
                }
                
            }
            temp.Push(obstacleToCheck);
        }
        while (temp.Count > 0)
        {
            
            _obstacleStack.Push(temp.Pop());
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
    }
}
