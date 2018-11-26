using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    [SerializeField] GameObject platformParent;
    [SerializeField] GameObject greenPlatform;
    GameObject platform;

    [SerializeField] float currentY = 0;
    float offset;
    Vector3 topLeft;

    void Start() {
        // Initialize boundaries
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        offset = 0.66f;

        // Initialize platforms
        GeneratePlatform(20);
    }

    public void GeneratePlatform(int numberOfPlatforms) {
        for (int i = 0; i < numberOfPlatforms; i++) {
            // Calculate platform x and y coordinates
            float distX = Random.Range(topLeft.x + offset, -topLeft.x - offset);
            float distY = Random.Range(0.5f, 2.5f);

            // Create green platforms
            currentY += distY;
            Vector3 platformPos = new Vector3(distX, currentY, 0);
            //int randomPlatform = Random.Range(1, 10);
            int randomPlatform = 0;

            if (randomPlatform == 0) {
                platform = Instantiate(greenPlatform, platformPos, Quaternion.identity);
                platform.transform.SetParent(platformParent.transform, false);
            }


        }
    }

}
