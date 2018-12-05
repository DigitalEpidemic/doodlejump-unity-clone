using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    [SerializeField] GameObject platformParent;

    [SerializeField] GameObject greenPlatform;
    [SerializeField] GameObject brownPlatform;
    [SerializeField] GameObject bluePlatform;
    [SerializeField] GameObject whitePlatform;

    [SerializeField] GameObject spring;
    [SerializeField] GameObject trampoline;
    [SerializeField] GameObject propeller;

    [SerializeField] GameObject monster;

    GameObject randomObject;

    [SerializeField] float currentY = 0;
    float previousY = 0;
    float offset;
    Vector3 topLeft;

    GameController gameController;

    void Start() {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        // Initialize boundaries
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        offset = 0.85f;

        // Initialize platforms
        GeneratePlatform(40);
    }

    public void GeneratePlatform(int numberOfPlatforms) {
        for (int i = 0; i < numberOfPlatforms; i++) {
            // Calculate platform's x and y coordinates
            float distX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
            float distY = Random.Range(0.5f, 2.25f);
            //float distY = 0f;
            
            // Create other platforms
            currentY += distY;

            // Separate platforms that are too close
            while (Mathf.Abs(currentY - previousY) < 1.5f) {
                currentY += 0.01f;
            }

            Vector3 platformPos = new Vector3(distX, currentY, 0); // Set the platform's position
            int randomPlatform = Random.Range(0, 9); // Randomly choose the next platform type
            GameObject platform;

            // Blue platform (Moving)
            if (randomPlatform == 2 && gameController.GetScore() >= 2500) { // Required score to start spawning blue platforms
                platform = Instantiate(bluePlatform, platformPos, Quaternion.identity);
            // White platform
            } else if (randomPlatform == 3 && gameController.GetScore() >= 10000) { // Required score to start spawning white platforms
                platform = Instantiate(whitePlatform, platformPos, Quaternion.identity);
            // Green platform
            } else {
                platform = Instantiate(greenPlatform, platformPos, Quaternion.identity);
            }

            platform.transform.SetParent(platformParent.transform, false); // Set parent of platform to empty GameObject for organization

            if (randomPlatform != 3) { // 3 Being white platforms
                int randomObjProb = Random.Range(0, 50);

                if (randomObjProb == 7) { // Create spring
                    Vector3 springPos = new Vector3(platformPos.x + 0.5f, platformPos.y + 0.2f, 0);
                    randomObject = Instantiate(spring, springPos, Quaternion.identity);
                    randomObject.transform.parent = platform.transform; // Make the object a child of the platform

                } else if (randomObjProb == 11) { // Create trampoline
                    Vector3 trampolinePos = new Vector3(platformPos.x + 0.13f, platformPos.y + 0.18f, 0);
                    randomObject = Instantiate(trampoline, trampolinePos, Quaternion.identity);
                    randomObject.transform.parent = platform.transform; // Make the object a child of the platform


                } else if (randomObjProb == 17 && gameController.GetScore() >= 3500) { // Create propeller
                    //print("creating propeller");
                    Vector3 propellerPos = new Vector3(platformPos.x + 0.13f, platformPos.y + 0.16f, 0);
                    randomObject = Instantiate(propeller, propellerPos, Quaternion.identity);
                    randomObject.transform.SetParent(platform.transform);

                } else if (randomObjProb == 19 && gameController.GetScore() >= 5000) { // Create Monster
                    Vector3 monsterPos = new Vector3(platformPos.x, platformPos.y, 0);
                    randomObject = Instantiate(monster, monsterPos, Quaternion.identity);
                    randomObject.transform.SetParent(platform.transform);
                    currentY += 1.25f; // Keep next platform 1.25f away to have distance from the monster
                }

            }

            // Create brown platforms (Breakable)
            int randomBrownPlatform = Random.Range(0, 6);
            //int randomBrownPlatform = 2;
            if (randomBrownPlatform == 2) {
                float brownDistX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
                float randomOffset = Random.Range(-0.3f, 0.3f);
                float brownDistY = currentY - Mathf.Abs((currentY - previousY) / 2) + randomOffset;
                Vector3 brownPlatformPos = new Vector3(brownDistX, brownDistY, 0);
                GameObject brownPlatformGO = Instantiate(brownPlatform, brownPlatformPos, Quaternion.identity);
                brownPlatformGO.transform.SetParent(platformParent.transform, false);
            }

            previousY = currentY; // Save the previous Y to base the next platform's Y off of

            //Debug.Log("Spawning a platform");
        }
    }

}
