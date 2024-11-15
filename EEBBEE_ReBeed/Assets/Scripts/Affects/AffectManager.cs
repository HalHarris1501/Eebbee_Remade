using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectManager : MonoBehaviour
{
    public IEnumerator CurrentEffect;
    public int EffectTimeRemaining;

    //Singleton pattern
    #region Singleton
    private static AffectManager _instance;
    public static AffectManager Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AffectManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("AffectManager");
                _instance = go.AddComponent<AffectManager>();
            }
            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEffect()
    {
        if(CurrentEffect == null)
        {
            Debug.Log("No Affect set");
            return;
        }
        StartCoroutine(CurrentEffect);
    }
}
