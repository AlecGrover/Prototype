using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour
{
    [SerializeField] float boostSpeed = 10f;
    [Tooltip("Multiplier applied to base rotation rate of 60 degrees per second")]
    [SerializeField] float rotateMultiplier = 1f;
    bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            ProcessBoost();
            ProcessRotation();
        }
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void ProcessRotation()
    {
        float rotationInput = CrossPlatformInputManager.GetAxis("Horizontal");
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = new Vector3(0, 0, rigidbody.angularVelocity.z);

        if (Mathf.Abs(rotationInput) > Mathf.Epsilon)
        {
            rigidbody.freezeRotation = true;
            transform.Rotate(Vector3.back * rotationInput * 60f * Time.deltaTime * rotateMultiplier);
            //rigidbody.rotation = (new Quaternion(0, 0, (rigidbody.rotation.z - (rotationInput * 0.7f * Time.deltaTime)) % 1, rigidbody.rotation.w));
            // Debug.Log("rotating " + rotationInput.ToString());
            rigidbody.freezeRotation = false;
        }
    }

    private void ProcessBoost()
    {
        bool boostInput = CrossPlatformInputManager.GetButton("Jump");
        if (boostInput && alive)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Vector3 boostVector = new Vector3(0, boostSpeed, 0);
            rigidbody.AddRelativeForce(boostVector * Time.deltaTime);
            //boostVector = transform.TransformDirection(boostVector);
            //rigidbody.velocity += (boostVector * Time.deltaTime);
            //Debug.Log(boostVector.ToString());
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.3f;
        }
        else
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Friendly"))
        {
            //Debug.Log("Safe impact, ignoring...");
            return;
        }
        else
        {
            StartCoroutine(Crash());
        }
    }

    IEnumerator Crash ()
    {
        alive = false;
        //Debug.Log("Bad impact");
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.0f;
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager)
        {
            levelManager.ReloadLevel();
        }
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(1f);
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager)
        {
            levelManager.ReloadLevel();
        }
    }

}
