using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D _obstacleMap;
    [SerializeField] private ColourToPrefab[] _colourMappings;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateObstacle()
    {
        for (int x = 0; x < _obstacleMap.width; x++)
        {
            for (int y = 0; y < _obstacleMap.height; y++)
            {

            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColour = _obstacleMap.GetPixel(x, y);

        if(pixelColour.a == 0)
        {
            //transparent pixels are ignored
            return;
        }

        Debug.Log(pixelColour);
    }
}
