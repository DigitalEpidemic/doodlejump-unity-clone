using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    [SerializeField] GameObject platformParent;

    [SerializeField] GameObject greenPlatform;
    [SerializeField] GameObject brownPlatform;
    [SerializeField] GameObject bluePlatform;

    [SerializeField] GameObject spring;

    GameObject randomObject;

    [SerializeField] float currentY = 0;
    float offset;
    Vector3 topLeft;

    GameController gameController;

    void Start() {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        // Initialize boundaries
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        offset = 0.85f;

        // Initialize platforms
        GeneratePlatform(20);
    }

    public void GeneratePlatform(int numberOfPlatforms) {
        for (int i = 0; i < numberOfPlatforms; i++) {
            // Calculate platform x and y coordinates
            float distX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
            float distY = Random.Range(0.5f, 2.5f);
            //float distY = 0f;

            // Create brown platforms (Breakable)
            int randomBrownPlatform = Random.Range(0, 9);
            if (randomBrownPlatform == 2) {
                float brownDistX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
                float brownDistY = Random.Range(currentY + 1, currentY + distY - 1);
                Vector3 brownPlatformPos = new Vector3(brownDistX, brownDistY, 0);
                GameObject brownPlatformGO = Instantiate(brownPlatform, brownPlatformPos, Quaternion.identity);
                brownPlatformGO.transform.SetParent(platformParent.transform, false);
            }

            // Create other platforms
            currentY += distY;
            Vector3 platformPos = new Vector3(distX, currentY, 0);
            int randomPlatform = Random.Range(0, 9);
            GameObject platform;

            // Blue platform (Moving)
            if (randomPlatform == 2 && gameController.score >= 1000) { // Required score to start spawning blue platforms
                platform = Instantiate(bluePlatform, platformPos, Quaternion.identity);

            // Green platform
            } else {
                platform = Instantiate(greenPlatform, platformPos, Quaternion.identity);
            }

            platform.transform.SetParent(platformParent.transform, false); // Set parent of platform to empty GameObject for organization

            if (randomPlatform != 4) { // 4 Being white platforms
                int randomObjProb = Random.Range(0, 50);

                if (randomObjProb == 7) { // Create spring
                    Vector3 springPos = new Vector3(platformPos.x + 0.5f, platformPos.y + 0.2f, 0);
                    randomObject = Instantiate(spring, springPos, Quaternion.identity);
                    randomObject.transform.parent = platform.transform; // Make the object a child of the platform
                }

            }


        }
    }

}
