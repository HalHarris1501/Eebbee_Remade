using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//creates a custom editor for the level generator class
[CustomEditor(typeof(LevelGenerator)), CanEditMultipleObjects]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Save Obstacle")) //adds button to editor to call SaveObstacle function
        {
            levelGenerator.SaveObstacle();
        }

        if (GUILayout.Button("Load Obstacle")) //adds button to editor to call LoadCurrentObstacle function
        {
            levelGenerator.LoadCurrentObstacle();
        }

        if (GUILayout.Button("Clear Obstacle")) //adds button to editor to call ClearObstacle function
        {
            levelGenerator.ClearObstacle();
        }
    }
}
