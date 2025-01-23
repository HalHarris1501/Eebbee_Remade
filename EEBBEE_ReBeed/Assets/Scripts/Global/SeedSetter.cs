using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedSetter : MonoBehaviour
{
    [SerializeField] private TMP_Text _seedText;
    [SerializeField] private SeedStorage _seedStorage;

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
        Debug.Log("Seed entered: " + _seedText.text + ". Seed Length: " + _seedText.text.Length);
        if(_seedText.text.Length <= 1)
        {
            _seedStorage.Seed = Random.Range(0, 999999999);
            
        }
       // else if(_seedText.text.ToIntArray())
        else
        {
            _seedStorage.Seed = _seedText.text.GetHashCode();
        }
        Debug.Log("New Seed: (" +  _seedStorage.Seed + ")");
    }
}
