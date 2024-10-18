using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    private bool isActive = false;
    private Renderer theRenderer;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponent<Renderer>();
        theRenderer.enabled = isActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Top camera
        {
            isActive = !isActive;

            theRenderer.enabled = isActive;
        }
    }
}
