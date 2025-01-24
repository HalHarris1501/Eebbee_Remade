using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

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

    public void SetSeed()
    {
        string tempSeed;
        tempSeed = _seedText.text;
        if (tempSeed.Length <= 1)
        {
            tempSeed = Random.Range(0, 999999999).ToString();
        }
        else
        {
            tempSeed = tempSeed.ToString();
            tempSeed = tempSeed.Substring(0, tempSeed.Length - 1);
        }
        Debug.Log("Seed: " + tempSeed);
        _seedStorage.Seed = tempSeed.GetHashCode();
        
    }
}
