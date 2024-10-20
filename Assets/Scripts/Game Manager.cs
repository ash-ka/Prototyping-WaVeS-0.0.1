using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI spawnedAnimalsText;
    public TextMeshProUGUI animalsTreatedText;
    public TextMeshProUGUI magicCookieWastedText;
    public TextMeshProUGUI spawnSourceActiveText;
    public TextMeshProUGUI animalsFailedToFeedText;
    public TextMeshProUGUI animalsVisitingCountText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameOverSubtext;
    public TextMeshProUGUI toleranceText;

    public bool isGameActive = false;

    private float timeElasped;

    private bool spawningStopped = false;

    public Button restartButton;

    public int gameLevel = 1; // 1=Easy, 2=Medium, 3=Hard 

    public int[] returnedAnimalTolerance;

    public ParticleSystem explosionParticles;

    public GameObject[] animalPrefabs;
    public float[,] zoneData = { {105.0f, 0.0f, 100.0f },
        {-65.0f, 0.0f, -5.0f},
        {5.0f, 0.0f, -5.0f},
        {100.0f, 0.0f, -10.0f},
        {5.0f, 0.0f, -80.0f},
    };

    // To do: create a Zone class
    public bool[] isZoneEnabled = { true, true, true, true, true };

    private int noOfZones = 5;
    private int noOfAnimalsPerZone = 2;

    private float startDelay = 2.0f;
    private float spawnInterval = 1.5f;

    public int maxAnimals;

    // Score
    public int spawnedAnimalsCount = 0;
    public int fedAnimalsCount = 0;
    public int magicCookieWastedCount = 0;
    public int spawnSourceActiveCount = 5;
    public int animalsFailedToFeedCount = 0;

    public int animalsInsideBaseStation = 0;

    // To do: replace with enum
    public int gameOverStatus = 0; // 0=Not yet over, 1=Player wins, 2=Player fails 

    public GameObject titleScreen;
    public GameObject infoPanel;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetLight(playerController.gameInactiveLight); // Light setup
    }

    // Update is called once per frame
    void Update()
    {
        timeElasped += Time.deltaTime;

        if (gameOverStatus == 0 && isGameActive)
        {
            if (spawnedAnimalsCount != 0 && spawnedAnimalsCount == fedAnimalsCount)
            {
                gameOverStatus = 1; // Player wins
                gameOverText.text = "You Win!";
                gameOverSubtext.text = "Less than " + (int)(animalsFailedToFeedCount + 1) + " animal(s) returned untreated.";
                GameOver();
            }
            else if (animalsFailedToFeedCount > returnedAnimalTolerance[gameLevel - 1])
            {
                gameOverStatus = 2; // Player fails
                gameOverText.text = "Game Over!";
                gameOverSubtext.text = animalsFailedToFeedCount + " animal(s) returned untreated.";
                GameOver();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                gameOverStatus = 1; // Player ends the game
                gameOverText.text = "Game Ended!";
                gameOverSubtext.text = "You played for " + timeElasped + " seconds.";
                GameOver();
            }

            spawnedAnimalsText.text = "#Spawned Animal(s) = " + spawnedAnimalsCount;
            animalsTreatedText.text = "#Animal(s) Treated = " + fedAnimalsCount;
            magicCookieWastedText.text = "#Magic Cookie(s) Wasted = " + magicCookieWastedCount;
            spawnSourceActiveText.text = "#Spawn Source(s) Left = " + spawnSourceActiveCount;
            animalsFailedToFeedText.text = "#Returned Untreated = " + animalsFailedToFeedCount;

            if (animalsInsideBaseStation == 0)
                animalsVisitingCountText.text = "";
            else
                animalsVisitingCountText.text = animalsInsideBaseStation + " Animal(s) Visiting";

            if (spawnedAnimalsCount >= maxAnimals) // Set animals limit reached or game is over
            {
                spawningStopped = true;
                CancelInvoke();
            }
        }
    }

    void SpawnRandomAnimal()
    {
        int zone = UnityEngine.Random.Range(0, noOfZones); // Randomly choose a zone from 
        if (!isZoneEnabled[zone])
            return;

        int coinToss = UnityEngine.Random.Range(0, noOfAnimalsPerZone); // One of the two animals in that zone
        int animalID = zone * 2 + coinToss;
        Vector3 spwanPos = new Vector3(zoneData[zone,0], zoneData[zone, 1], zoneData[zone, 2]);
        Quaternion initalRot = animalPrefabs[animalID].transform.rotation;

        Instantiate(animalPrefabs[animalID], spwanPos, initalRot);
        spawnedAnimalsCount++;
    }

    public void PlayParticleEffect(Vector3 particlePos)
    {
        explosionParticles.transform.position = particlePos;
        explosionParticles.Play();
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverSubtext.gameObject.SetActive(true);
        gameOverText.enabled = true;
        gameOverSubtext.enabled = true;
        animalsVisitingCountText.enabled = false;
        restartButton.gameObject.SetActive(true);

        if (!spawningStopped)
            CancelInvoke(); // Stop spawning


        playerController.SetLight(playerController.gameInactiveLight); // Nighttime litght setup
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        timeElasped = 0.0f;

        gameLevel = difficulty;

        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
        gameOverText.enabled = false;
        gameOverSubtext.enabled = false;

        playerController.SetCamera(playerController.defaultCameraNumber); // Camera setup
        playerController.SetLight(playerController.defaultLightNumber); // Light setup

        restartButton.gameObject.SetActive(false);
        toleranceText.text = "Max. Failure Tolerance = " + returnedAnimalTolerance[gameLevel - 1];

        titleScreen.gameObject.SetActive(false);
        infoPanel.gameObject.SetActive(true);

        isGameActive = true;
    }
}
