using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

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

    public ParticleSystem explosionParticles;

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
        if (other.gameObject.CompareTag("Animal") && !toBeDestroyed)
        {
            gameManager.fedAnimalsCount++;

            if(other.gameObject.GetComponent<AnimalController>().IsInsideBaseStation())
                gameManager.animalsInsideBaseStation--;

            Destroy(other.gameObject); // Destroy the animal object

            theRenderer.enabled = false;
            toBeDestroyed = true;

            audioSource.PlayOneShot(yummySound, 1.0f); // Play joyful sound
            Destroy(gameObject, yummySound.length * 5); // Destroy the food object
        }
        else if (other.gameObject.CompareTag("Boundary Object") && !toBeDestroyed)
        {
            theRenderer.enabled = false;

            gameManager.magicCookieWastedCount++;

            audioSource.PlayOneShot(crunchySound, 1.0f); // Play joyful sound
            Destroy(gameObject, crunchySound.length * 5); // Destroy the food object
        }
        // Destroy a spawn potion source
        else if (other.gameObject.CompareTag("Stop Spawn (1)"))
            DestroySpawnSource(1, other);
        else if (other.gameObject.CompareTag("Stop Spawn (2)"))
            DestroySpawnSource(2, other);
        else if (other.gameObject.CompareTag("Stop Spawn (3)"))
            DestroySpawnSource(3, other);
        else if (other.gameObject.CompareTag("Stop Spawn (4)"))
            DestroySpawnSource(4, other);
        else if (other.gameObject.CompareTag("Stop Spawn (5)"))
            DestroySpawnSource(5, other);
    }

    private void DestroySpawnSource(int sourceID, Collider other)
    {
        int sourceIndex = sourceID - 1;
        gameManager.PlayParticleEffect(other.gameObject.transform.position);

        gameManager.spawnSourceActiveCount--;

        gameManager.isZoneEnabled[sourceIndex] = false;
        Destroy(other.gameObject);

        theRenderer.enabled = false;
        toBeDestroyed = true;

        audioSource.PlayOneShot(potionCrash, 1.0f); // Play joyful sound
        Destroy(gameObject, potionCrash.length * 5); // Destroy the food object
    }
}
