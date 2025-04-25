using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to store data for obstacles, so that they can be created at runtime
[CreateAssetMenu(fileName = "Obstacles", menuName = "ScriptableObjects/Obstacles", order = 1)] //allows multiple instances of the Obstacle script to be made as scriptable objects
public class Obstacle : ScriptableObject
{
    public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>(); //a list of all the saveable objects within the obstacle
    public List<Vector2> FreeSpace = new List<Vector2>(); //a list of all the free space in the obstacle so that collectables can be spawned in free spaces at runtime

    public void ClearObjectList() //a funtion to clear the list so the obstacle and be edited
    {
        ObjectList.Clear();
    }

    //funtion to add a SaveableObject to this obstacles object list
    public void AddObjectInfo(SaveableObject saveableObject)
    {
        SaveableObjectInfo saveableObjectInfo = new SaveableObjectInfo(saveableObject); //creates and stores information for the given SaveableObject
        if(FreeSpace.Contains(saveableObjectInfo.Position)) //checks if the objects position is still within the FreeSpace list
        {
            FreeSpace.Remove(saveableObjectInfo.Position); //if it is, it removes the position from the list
        }
        ObjectList.Add(saveableObjectInfo); //adds the created information to the ObjectList
    }

    //a function to reset the free space for an object, using given bounds
    public void SetFreeSpace()
    {
        FreeSpace.Clear(); //clear the current FreeSpace list
        for(int x = -13; x < 14; x++) //loop through the x axis with given bounds
        {
            for(int y = -6; y < 7; y++) //loop through the y axis with given bounds
            {
                FreeSpace.Add(new Vector2(x, y)); //add current x,y position to the list
            }
        }
    }
}
