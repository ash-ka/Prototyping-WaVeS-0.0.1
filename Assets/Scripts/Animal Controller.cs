using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public float speed;
    private float randomSeconds;
    private float minSeconds = 10.0f;
    private float maxSeconds = 60.0f;
    private float secondsMeasure = 0.0f;
    private float minSpeed, maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        randomSeconds = Random.Range(minSeconds,maxSeconds);
        minSpeed = speed / 2.0f;
        maxSpeed = speed * 2.0f;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

        secondsMeasure += Time.deltaTime;

        if (secondsMeasure > randomSeconds)
        {
            float angle = Random.Range(0.0f, 360.0f);
            transform.Rotate(Vector3.up, angle);

            speed = Random.Range(minSpeed,maxSpeed);

            randomSeconds = Random.Range(minSeconds, maxSeconds);
            secondsMeasure = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary Object")) // Anmials will take u-turns when they collide with the boundary
        {
            transform.forward = -transform.forward;
        }
        else if (other.gameObject.CompareTag("Animal")) // Animal behavior when encountering other animals
        {
            transform.Rotate(Vector3.up, 120.0f); // Animals rotate 120 degrees when they encounter other animals
            //            transform.forward = -transform.forward;  // Anmials will take u-turns when they collide
        }
        else if (other.gameObject.CompareTag("Human")) // Animals will take u-turn when collide with humans
        {
            transform.forward = -transform.forward;
        }
    }
}
