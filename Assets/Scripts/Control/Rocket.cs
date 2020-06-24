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
    [SerializeField] AudioClip deathExplosion;
    [SerializeField] AudioClip victoryPing;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject victoryVFX;
    bool victory = false;
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
            SetParticleSystem(true);
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Vector3 boostVector = new Vector3(0, boostSpeed, 0);
            rigidbody.AddRelativeForce(boostVector * Time.deltaTime);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.3f;
        }
        else
        {
            SetParticleSystem(false);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.0f;
        }
    }

    private void SetParticleSystem(bool setActive)
    {
        BoostParticles boostParticles = FindObjectOfType<BoostParticles>();
        if (!boostParticles) { return; }
        boostParticles.SetBoostParticlesActive(setActive);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (!rigidbody)
            {
                return;
            }
            if (Mathf.Abs(rigidbody.velocity.y) <= 0.009 && Mathf.Abs(rigidbody.velocity.x) <= 0.009)
            {
                if (victory) { return; }
                victory = true;
                if (victoryPing)
                {
                    GameObject audioSourceObject = Instantiate(new GameObject(), transform.position, Quaternion.identity);
                    audioSourceObject.AddComponent<AudioSource>();
                    audioSourceObject.GetComponent<AudioSource>().PlayOneShot(victoryPing, 0.06f);
                }
                if (victoryVFX)
                {
                    Instantiate(victoryVFX, transform.position, Quaternion.identity);
                }
                StartCoroutine(ReloadLevel());
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Friendly"))
        {
            //Debug.Log("Safe impact, ignoring...");
            return;
        }
        else
        {
            StartCoroutine(Crash(collision.contacts[0].point));
        }
    }

    IEnumerator Crash(Vector3 collisionTransform)
    {
        alive = false;
        //Debug.Log("Bad impact");
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.0f;
        if (deathExplosion)
        {
            GameObject audioSourceObject = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
            audioSourceObject.AddComponent<AudioSource>();
            audioSourceObject.GetComponent<AudioSource>().PlayOneShot(deathExplosion, 0.1f);
        }
        SetParticleSystem(false);
        if (explosionVFX) {
            GameObject explosion = Instantiate(explosionVFX, collisionTransform, Quaternion.identity, transform);
            Destroy(explosion, 1f);
        }
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
