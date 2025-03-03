using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class SeedSetter : MonoBehaviour
{
    [Header("UI object references")]
    [SerializeField] private TMP_Text _seedText;
    [SerializeField] private TMP_InputField _previousSeedText;

    [Header("Seed Storage Scriptable Object")]
    [SerializeField] private SeedStorage _seedStorage;

    // Start is called before the first frame update
    void Start()
    {
        _previousSeedText.text = _seedStorage.UnhashedSeed;
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
            _seedStorage.SeedRandomised = true;
        }
        else
        {
            tempSeed = tempSeed.ToString();
            tempSeed = tempSeed.Substring(0, tempSeed.Length - 1);
            _seedStorage.SeedRandomised = false;
        }
        Debug.Log("Seed: " + tempSeed);
        _seedStorage.UnhashedSeed = tempSeed;
        _seedStorage.Seed = tempSeed.GetHashCode();        
    }
}
