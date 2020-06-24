using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AudioManager[] audioManagers = FindObjectsOfType<AudioManager>();
        if (audioManagers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
