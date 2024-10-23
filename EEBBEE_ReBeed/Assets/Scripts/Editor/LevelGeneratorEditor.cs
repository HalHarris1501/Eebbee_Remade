using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator)), CanEditMultipleObjects]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Save Obstacle"))
        {
            levelGenerator.SaveObstacle();
        }

        if (GUILayout.Button("Load Obstacle"))
        {
            levelGenerator.LoadCurrentObstacle();
        }

        if (GUILayout.Button("Clear Obstacle"))
        {
            levelGenerator.ClearObstacle();
        }
    }
}
