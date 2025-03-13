using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{


    //Singleton pattern
    #region Singleton
    private static SceneSwapper _instance;
    public static SceneSwapper Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneSwapper>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("SceneSwapper");
                _instance = go.AddComponent<SceneSwapper>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
