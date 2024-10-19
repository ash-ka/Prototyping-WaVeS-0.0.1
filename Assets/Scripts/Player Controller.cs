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
    private float xMin, zMin, xMax, zMax;
    public GameObject projectilePrefab;
    public float heightOffset = 0.8f;

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

        // Initially the third person view camera behind the player is enabled
        topCamera.enabled = false;
        mainCamera.enabled = false;
        thirdCamera.enabled = true;

        // Initial light setup
        lights[0].enabled = true;

        xStart = transform.position.x;
        xMin = xStart - xRange;
        xMax = xStart + xRange;
        zStart = transform.position.z;
        zMin = zStart - zRange;
        zMax = zStart + zRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.spawnedAnimals != 0 && gameManager.spawnedAnimals == gameManager.fedAnimals && !isMusicChanged)
        {
            Debug.Log("Music change!");
            gameManager.GetComponent<AudioSource>().Stop();
            playerAudioSource.Play();
            isMusicChanged = true;
        }

        // This is where we get player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Movinng the player forward or backward
        float s = speed * verticalInput;
        transform.Translate(Vector3.forward * Time.deltaTime * s);
        playerAnimator.SetFloat("Speed_f", Mathf.Abs(s));

        // Turning the player
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // Switching camera view
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Top camera
        {
            topCamera.enabled = true;
            mainCamera.enabled = false;
            thirdCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            topCamera.enabled = false;
            mainCamera.enabled = true;
            thirdCamera.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            topCamera.enabled = false;
            mainCamera.enabled = false;
            thirdCamera.enabled = true;
        }

        // Switching light
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Top camera
        {
            lights[0].enabled = true;
            lights[1].enabled = false;
            lights[2].enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lights[0].enabled = false;
            lights[1].enabled = true;
            lights[2].enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            lights[0].enabled = false;
            lights[1].enabled = false;
            lights[2].enabled = true;
        }


        // Limiting the player withing a rectangluar domain
        if (transform.position.x > xMax)
            transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
        if (transform.position.x < xMin)
            transform.position = new Vector3(xMin, transform.position.y, transform.position.z);

        if (transform.position.z > zMax)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
        if (transform.position.z < zMin)
            transform.position = new Vector3(transform.position.x, transform.position.y, zMin);

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
}
