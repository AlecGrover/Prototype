using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementDirection = new Vector3(0, 0, 0);
    [SerializeField] float period = 1f;
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset;
        if (Mathf.Abs(period) < Mathf.Epsilon)
        {
            offset = new Vector3(0, 0, 0);
        }
        else
        {
            offset = movementDirection * (Mathf.Sin(Time.timeSinceLevelLoad))/period;
        }
        transform.position = offset + startingPosition;
    }
}
