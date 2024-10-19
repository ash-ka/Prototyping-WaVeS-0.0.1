using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Camera
    public Camera topCamera;
    public Camera mainCamera;
    public Camera thirdCamera;

    private float speed = 10.0f;
    private float turnSpeed = 90.0f;
    private float horizontalInput;
    private float verticalInput;
    private float xRange = 30.0f;
    private float zRange = 30.0f;
    private float xStart;
    private float zStart;
    private Quaternion initialRotation;
    private float xMin, zMin, xMax, zMax;
    public GameObject projectilePrefab;
    public float heightOffset = 0.8f;

    private int defaultCameraNumber = 1;
    private int defaultLightNumber = 1;

    // Animation
    private Animator playerAnimator;

    // Audio
    private AudioSource playerAudioSource;
    public AudioClip jumpSound;

    public Light[] lights;

    private GameManager gameManager;

    bool isMusicChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        
        InitializePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOverStatus == 1 && !isMusicChanged) // Game over, player wins
        {
            // Background music change
            gameManager.GetComponent<AudioSource>().Stop();
            playerAudioSource.Play();
            isMusicChanged = true;

            SetLight(4); // Nighttime litght setup
        }
        else if (gameManager.gameOverStatus == 0) // Game not over yet
        {
            // This is where we get player input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            float s = speed * verticalInput;
            // Movinng the player forward or backward
            transform.Translate(Vector3.forward * Time.deltaTime * s);
            playerAnimator.SetFloat("Speed_f", Mathf.Abs(s)); // Walking aniation

            // Turning the player
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

            // Switching camera view
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetCamera(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SetCamera(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SetCamera(3);

            // Switching lights
            if (Input.GetKeyDown(KeyCode.Alpha4))
                SetLight(1);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                SetLight(2);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                SetLight(3);

            // Limiting the player withing a rectangluar domain
            LimitPlayer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Launch a projectile from the player
                Instantiate(projectilePrefab, transform.position + new Vector3(0, heightOffset, 0), transform.rotation);
                playerAnimator.SetBool("Jump_b", true);
                playerAudioSource.PlayOneShot(jumpSound, 1.0f);
            }
            else
                playerAnimator.SetBool("Jump_b", false);
        }
        else
        {
            playerAnimator.SetBool("Jump_b", false); // Stop jump animation
            playerAnimator.SetFloat("Speed_f", 0.0f); // Stop walking aniation
        }
    }

    public void InitializePlayer()
    {
        SetCamera(defaultCameraNumber); // Camera setup
        SetLight(defaultLightNumber); // Light setup

        // Obtaining player's position
        xStart = transform.position.x;
        zStart = transform.position.z;

        initialRotation  =  transform.rotation; // Obtaining initial rotation of the player

        // Definint player's boundary
        xMin = xStart - xRange;
        xMax = xStart + xRange;
        zMin = zStart - zRange;
        zMax = zStart + zRange;
    }

    private void LimitPlayer()
    {
        if (transform.position.x > xMax)
            transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
        if (transform.position.x < xMin)
            transform.position = new Vector3(xMin, transform.position.y, transform.position.z);

        if (transform.position.z > zMax)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
        if (transform.position.z < zMin)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMin);
    }

    private void SetLight(int lightNumber)
    {
        lights[0].enabled = false;
        lights[1].enabled = false;
        lights[2].enabled = false;
        lights[3].enabled = false;

        lights[lightNumber - 1].enabled = true;
    }

    private void SetCamera(int cameraNumber)
    {
        switch (cameraNumber)
        {
            case 1:
//            default:
                topCamera.enabled = false;
                mainCamera.enabled = false;
                thirdCamera.enabled = true;
                break;

            case 2:
                topCamera.enabled = false;
                mainCamera.enabled = true;
                thirdCamera.enabled = false;
                break;

            case 3:
                topCamera.enabled = true;
                mainCamera.enabled = false;
                thirdCamera.enabled = false;
                break;
        }
    }
}
