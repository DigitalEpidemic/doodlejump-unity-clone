using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePlatform : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision) {
        // Add force when player falls from the top
        if (collision.relativeVelocity.y <= 0f) {
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<PlatformEffector2D>().enabled = false;
        }
    }
}
