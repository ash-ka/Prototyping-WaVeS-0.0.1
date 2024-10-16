using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float timeBeforeDisapear;
    private float speed = 40.0f;
    private float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        timePassed += Time.deltaTime;
        if(timePassed > timeBeforeDisapear) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Human"))
            return;

        Destroy(gameObject);
        if (other.gameObject.CompareTag("Animal"))
            Destroy(other.gameObject);
    }
}
