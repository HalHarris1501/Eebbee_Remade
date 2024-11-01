using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
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

    private Stack<int> _obstacleStack;

    private void Awake()
    {
        _currentSeed = _gameSeed.GetHashCode();
        Random.InitState(_currentSeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void LoadObstacle()
    {
        int childrenCount = _obstacleBoxes[_currentBox].transform.childCount;
        for(int i = 0; i < childrenCount; i++)
        {
            GameObject currentChild = _obstacleBoxes[_currentBox].transform.GetChild(0).gameObject;
            currentChild.transform.SetParent(null);
            currentChild.SetActive(false);
        };

        int obstacleID = Random.Range(0, _obstacles.Count);
        _obstacleStack.Push(obstacleID);
        Obstacle currentObstacle = _obstacles[obstacleID];

        foreach (SaveableObjectInfo saveableObject in currentObstacle.ObjectList)
        {
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool(saveableObject.Type.ToString(), _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity); ;
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform);
        }

        _currentBox++;
        if(_currentBox > 2)
        {
            _currentBox = 0;
        }
    }

    public void DisableEndingNectar()
    {

    }
}
