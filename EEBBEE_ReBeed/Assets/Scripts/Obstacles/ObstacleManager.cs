using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _obstacleBoxes = new List<GameObject>();
    [SerializeField] private List<Obstacle> _obstacles = new List<Obstacle>();
    private int _currentBox = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void LoadObstacle()
    {
        foreach(Transform child in _obstacleBoxes[_currentBox].transform)
        {
            child.SetParent(null);
            child.gameObject.SetActive(false);            
        }

        int obstacleID = Random.Range(0, _obstacles.Count);
        Obstacle currentObstacle = _obstacles[obstacleID];

        foreach (SaveableObjectInfo saveableObject in currentObstacle.ObjectList)
        {
            GameObject currentObject = ObjectPooler.Instance.SpawnFromPool("Block", _obstacleBoxes[_currentBox].transform.position + saveableObject.Position, Quaternion.identity);
            currentObject.transform.SetParent(_obstacleBoxes[_currentBox].transform);            
        }

        _currentBox++;
        if(_currentBox > 2)
        {
            _currentBox = 0;
        }
    }
}
