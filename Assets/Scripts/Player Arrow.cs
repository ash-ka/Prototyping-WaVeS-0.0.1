using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    private bool isActive = false;
    private Renderer theRenderer;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponent<Renderer>();
        theRenderer.enabled = isActive;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameManager.gameOverStatus == 0 && gameManager.isGameActive) // Controlling player help arrow if game is not over
        {
            isActive = !isActive;

            theRenderer.enabled = isActive;
        }
    }
}
