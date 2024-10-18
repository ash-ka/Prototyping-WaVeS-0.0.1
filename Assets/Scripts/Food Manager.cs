using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float timeBeforeDisapear;
    private float speed = 40.0f;
    private float timePassed;

    // Audio
    private AudioSource audioSource;
    public AudioClip yummySound;
    public AudioClip crunchySound;

    private Renderer theRenderer;

    private bool toBeDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0.0f;

        audioSource = GetComponent<AudioSource>();

        theRenderer = GetComponent<Renderer>();
        theRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        timePassed += Time.deltaTime;

        if (timePassed > timeBeforeDisapear) 
        {


//            Destroy(gameObject, crunchySound.length * 5); // Destroy the food object
        }
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
    }
}
