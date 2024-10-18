using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDestroy : MonoBehaviour
{
    public ParticleSystem explosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        explosionParticles.Play();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {

    }
}
