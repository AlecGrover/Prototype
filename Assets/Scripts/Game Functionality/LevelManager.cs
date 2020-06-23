using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LevelManager[] levelManagers = FindObjectsOfType<LevelManager>();
        if (levelManagers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void ReloadLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Loading build index " + currentBuildIndex.ToString());
        SceneManager.LoadScene(currentBuildIndex);
    }

}
