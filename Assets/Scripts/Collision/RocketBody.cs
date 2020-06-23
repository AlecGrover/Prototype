using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBody : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Friendly"))
        {
            Debug.Log("Safe impact, ignoring...");
            return;
        }
        else
        {
            Debug.Log("Bad impact");
            Time.timeScale = 0;
        }
    }
}
