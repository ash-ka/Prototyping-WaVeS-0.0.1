using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float timeBeforeDisapear;
    private float speed = 40.0f;

    // Audio
    private AudioSource audioSource;
    public AudioClip yummySound;
    public AudioClip crunchySound;
    public AudioClip potionCrash;

    private Renderer theRenderer;

    private bool toBeDestroyed = false;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        theRenderer = GetComponent<Renderer>();
        theRenderer.enabled = true;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
//        if (other.gameObject.CompareTag("Human"))
//            return;

        if (other.gameObject.CompareTag("Animal") && !toBeDestroyed)
        {
            Destroy(other.gameObject); // Destroy the animal object

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(yummySound, 1.0f); // Play joyful sound
            Destroy(gameObject, yummySound.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Boundary Object") && !toBeDestroyed)
        {
            theRenderer.enabled = false;

            audioSource.PlayOneShot(crunchySound, 1.0f); // Play joyful sound
            Destroy(gameObject, crunchySound.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Stop Spawn (1)"))
        {
            gameManager.isZoneEnabled[0] = false;
            Destroy(other.gameObject);

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
            Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Stop Spawn (2)"))
        {
            gameManager.isZoneEnabled[1] = false;
            Destroy(other.gameObject);

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
            Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Stop Spawn (3)"))
        {
            gameManager.isZoneEnabled[2] = false;
            Destroy(other.gameObject);

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
            Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Stop Spawn (4)"))
        {
            gameManager.isZoneEnabled[3] = false;
            Destroy(other.gameObject);

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
            Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Stop Spawn (5)"))
        {
            gameManager.isZoneEnabled[4] = false;
            Destroy(other.gameObject);

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
            Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
        }
    }
}
