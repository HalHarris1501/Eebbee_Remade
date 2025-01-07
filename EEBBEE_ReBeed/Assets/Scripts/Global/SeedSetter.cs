using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text _seedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSeed() //doesn't work because ObstacleManager isn't loaded
    {
        if(_seedText.text != null)
        {
            ObstacleManager.Instance.SetSeed(_seedText.text);
        }
        else
        {
            ObstacleManager.Instance.SetSeed(Random.Range(0, 999999999).ToString());
        }
    }
}
