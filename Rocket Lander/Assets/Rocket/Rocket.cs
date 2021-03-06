﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum RocketState { Alive, Dying, Transcending }
    RocketState state = RocketState.Alive;

    [SerializeField] float RCSThrust = 100f;
    [SerializeField] float MainThrust = 100f;

    [SerializeField] float LevelLoadDelay = 1f;

    [SerializeField] AudioClip Engine;
    [SerializeField] AudioClip Explosion;
    [SerializeField] AudioClip Success;

    [SerializeField] ParticleSystem ThrustParticles;
    [SerializeField] ParticleSystem ExplosionParticles;
    [SerializeField] ParticleSystem SuccessParticles;

    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == RocketState.Alive)
        {
            RespondToThrust();
            RespondToRotation();
            RespondToDebugKeys();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != RocketState.Alive || collisionsDisabled)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = RocketState.Transcending;
                audioSource.Stop();
                ThrustParticles.Stop();
                SuccessParticles.Play();
                audioSource.PlayOneShot(Success);
                Invoke("LoadNextScene", LevelLoadDelay);
                break;
            default:
                state = RocketState.Dying;
                ThrustParticles.Stop();
                ExplosionParticles.Play();
                audioSource.Stop();
                audioSource.PlayOneShot(Explosion);
                Invoke("LoadFirstScene", LevelLoadDelay);
                break;
        }
    }

    private void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * MainThrust);

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(Engine);
            ThrustParticles.Play();
        }
        else
        {
            audioSource.Stop();
            ThrustParticles.Stop();
        }
    }

    private void RespondToRotation()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationSpeed = RCSThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        rigidBody.freezeRotation = false; // resume manual control of rotation
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        state = RocketState.Alive;
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void LoadFirstScene()
    {
        state = RocketState.Alive;
        SceneManager.LoadScene(0);
    }

    void RespondToDebugKeys()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Invoke("LoadNextScene", 0f);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                collisionsDisabled = !collisionsDisabled; //toggle
            }
        }
    }


}
