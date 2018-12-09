using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownPlatform : MonoBehaviour {
    bool fallDown = false;

    void FixedUpdate() {
        
        if (fallDown) {
            transform.position -= new Vector3(0, 8f*Time.deltaTime, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Add force when player falls from the top
        if (collision.relativeVelocity.y <= 0f) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TemporarilyMakeKinematic();
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PlatformEffector2D>().enabled = false;

            


            fallDown = true;
        }
    }
}