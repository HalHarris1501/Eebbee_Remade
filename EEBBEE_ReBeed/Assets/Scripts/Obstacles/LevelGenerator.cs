using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private Obstacle _obstacle;
    private List<SaveableObjectPrefab> _prefabList = new List<SaveableObjectPrefab>();

    private void SaveLevel()
    {
        if(_obstacle == null)
        {
            Debug.LogError("No Obstacle object found!");
            return;
        }

        _obstacle.ClearObjectList();

        SaveableObject[] saveableObjects = FindObjectsOfType<SaveableObject>();
        foreach(SaveableObject saveableObject in saveableObjects)
        {
            _obstacle.AddObjectInfo(saveableObject);
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_obstacle);
#endif
    }

    public void LoadCurrentObstacle()
    {
        if(_obstacle == null)
        {
            Debug.LogError("No Obstacle object found!");
            return;
        }

        ClearObstacle();

        foreach(SaveableObjectInfo saveableObject in _obstacle.ObjectList)
        {
            GameObject prefab = null;
            foreach(SaveableObjectPrefab saveableObjectPrefab in _prefabList)
            {
                if(saveableObject.Type == saveableObjectPrefab.Type)
                {
                    prefab = saveableObjectPrefab.Prefab;
                    break;
                }
            }

            if(prefab == null)
            {
                Debug.LogWarning("Couldnt find prefab of type: " + saveableObject.Type.ToString());
                continue;
            }

            GameObject newInstance = Instantiate(prefab);

            newInstance.transform.position = saveableObject.Position;
        }
    }

    public void LoadObstacle(Obstacle obstacle)
    {
        this._obstacle = obstacle;
        LoadCurrentObstacle();
    }

    private void ClearObstacle()
    {
        SaveableObject[] saveableObjects = FindObjectsOfType<SaveableObject>();
        foreach (SaveableObject saveableObject in saveableObjects)
        {
            if (saveableObject == null)
                continue;
            if (Application.isEditor)
                DestroyImmediate(saveableObject.gameObject);
            else
                Destroy(saveableObject.gameObject);
        }
    }
}
