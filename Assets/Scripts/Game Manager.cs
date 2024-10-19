using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    public int spawnedAnimals = 0;
    public int fedAnimals = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnedAnimals >= maxAnimals) 
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
        //        int animalIndex = Random.Range(0, animalPrefabs.Length);
        //       float spwanX = Random.Range(-spwanRangeX, spwanRangeX);
        int animalID = zone * 2 + coinToss;
        Vector3 spwanPos = new Vector3(zoneData[zone,0], zoneData[zone, 1], zoneData[zone, 2]);
//        float initialAngle = UnityEngine.Random.Range(0.0f, 360.0f);
        Quaternion initalRotation = animalPrefabs[animalID].transform.rotation;

        Instantiate(animalPrefabs[animalID], spwanPos, initalRotation);
        spawnedAnimals++;
    }

    public void PlayParticleEffect(Vector3 particlePos)
    {
        explosionParticles.transform.position = particlePos;
        explosionParticles.Play();
    }
}
