using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//class to save and load obstacles being made in the editor
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Obstacle _obstacle; //obstacle being edited
    [SerializeField] private List<SaveableObjectPrefab> _prefabList = new List<SaveableObjectPrefab>(); //list to hold all SaveableObjects prefabs that have been made

    //function to save data for this obstacle
    public void SaveObstacle()
    {
        if(_obstacle == null) //makes sure there is an obstacle to save to
        {
            Debug.LogError("No Obstacle object found!");
            return; //if theres no obstacle, dont attempt save
        }

        _obstacle.ClearObjectList(); //empty current saved data for obstacle
        _obstacle.SetFreeSpace(); //reset free space so it can be set using new obstacle data

        SaveableObject[] saveableObjects = FindObjectsOfType<SaveableObject>(); //find all SaveableObjects in the scene
        foreach(SaveableObject saveableObject in saveableObjects) //loop through all objects found in scene
        {
            _obstacle.AddObjectInfo(saveableObject); //add SaveableObject to the obstacle data
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_obstacle); //if being done in editor, set dirty so the editors undo record isn't affected.
#endif
    }

    //function to load and display obstacle using previously made obstacle
    public void LoadCurrentObstacle()
    {
        if(_obstacle == null) //makes sure there is an obstacle to load
        {
            Debug.LogError("No Obstacle object found!");
            return; //if theres no obstacle, dont attempt load
        }

        ClearObstacle(); //function to remove all currently exsisting SaveableObjects in the scene

        foreach (SaveableObjectInfo saveableObject in _obstacle.ObjectList) //loop through each SaveableObject in the obstacle's ObjectList
        {
            GameObject prefab = null; //temp variable called prefab, used for getting the correct SaveableObject to instantiate
            foreach (SaveableObjectPrefab saveableObjectPrefab in _prefabList) //loop through each prefab SaveableObject in the prefab list, to find the correct type
            {
                if(saveableObject.Type == saveableObjectPrefab.Type) //check if the SaveableObject is the same as the current object being looked at
                {
                    prefab = saveableObjectPrefab.Prefab;
                    break; //if it is, set it to the prefab variable and break loop
                }
            }

            if(prefab == null) //check to make sure the prefab found isn't null
            {
                Debug.LogWarning("Couldnt find prefab of type: " + saveableObject.Type.ToString());
                continue; //if it is, move to next SaveableObject in the list
            }

            GameObject newInstance = Instantiate(prefab); //instantiate a new object in the scene using the found prefab

            newInstance.transform.position = saveableObject.Position; //set the new object's position to be the same position as the current SaveableObject's position data
        }
    }

    //function to load a specific obstacle
    public void LoadObstacle(Obstacle obstacle)
    {
        this._obstacle = obstacle; //sets the _obstacle field to be the obstacle specified by the function
        LoadCurrentObstacle(); //function to load and display obstacle using previously made obstacle
    }

    //function to remove all currently exsisting SaveableObjects in the scene
    public void ClearObstacle()
    {
        SaveableObject[] saveableObjects = FindObjectsOfType<SaveableObject>(); //find all SaveableObjects in the scene
        foreach (SaveableObject saveableObject in saveableObjects) //loop through all objects in scene
        {
            if (saveableObject == null) //check that the current SaveableObject isn't null
                continue; //if it is null, skip to next SaveableObject
            if (Application.isEditor) //check if in editor mode
                DestroyImmediate(saveableObject.gameObject); //destroy the current SaveableObject gameobject
            else //if in play mode instead
                Destroy(saveableObject.gameObject); //destroy the current SaveableObject gameobject
        }
    }
}
