using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI spawnedAnimalsText;
    public TextMeshProUGUI animalsTreatedText;
    public TextMeshProUGUI magicCookieWastedText;
    public TextMeshProUGUI spawnSourceActiveText;
    public TextMeshProUGUI animalsFailedToFeedText;
    public TextMeshProUGUI animalsVisitingCountText;

    public ParticleSystem explosionParticles;

    public GameObject[] animalPrefabs;
    public float[,] zoneData = { {105.0f, 0.0f, 100.0f },
        {-65.0f, 0.0f, -5.0f},
        {5.0f, 0.0f, -5.0f},
        {100.0f, 0.0f, -10.0f},
        {5.0f, 0.0f, -80.0f},
    };

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

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        spawnedAnimalsText.text = "#Spawned Animals = " + spawnedAnimalsCount;
        animalsTreatedText.text = "#Animals Treated = " + fedAnimalsCount;
        magicCookieWastedText.text = "#Magic Cookie Wasted = " + magicCookieWastedCount;
        spawnSourceActiveText.text = "#Spawn Sources = " + spawnSourceActiveCount;
        animalsFailedToFeedText.text = "#Returned Untreated = " + animalsFailedToFeedCount;

        if (animalsInsideBaseStation == 0)
            animalsVisitingCountText.text = "";
        else
            animalsVisitingCountText.text = animalsInsideBaseStation + " Animal(s) Visiting";

        if (spawnedAnimalsCount >= maxAnimals) 
        {
            CancelInvoke();
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
}
