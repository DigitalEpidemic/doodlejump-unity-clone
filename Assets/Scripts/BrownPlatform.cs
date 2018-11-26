using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownPlatform : MonoBehaviour {


    private bool fallDown = false;

    void FixedUpdate() {
        

        if (fallDown) {
            transform.position -= new Vector3(0, 8.5f*Time.deltaTime, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Add force when player fall from top
        if (collision.relativeVelocity.y <= 0f) {
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<PlatformEffector2D>().enabled = false;

            if (GetComponent<Animator>()) {
                GetComponent<Animator>().SetTrigger("Break");
            }
            fallDown = true;


        }
    }
}