using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField] float jumpForce = 16.5f;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y <= 0.7f) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            if (rb != null) {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() != null) {
                    Player doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    Animator doodlerAnim = doodler.GetComponent<Animator>();

                    doodlerAnim.SetTrigger("Jump");
                }

                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;

                // Play jump sound
                //GetComponent<AudioSource>().Play();

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Rigidbody2D doodlerRB = collision.GetComponent<Rigidbody2D>();
            doodlerRB.isKinematic = true; // Remove gravity from Doodler
            Player doodler = collision.GetComponent<Player>();

            // Show spinning stars and disable controls
            doodler.ShowSpinningStars(true);
            doodler.enableControls = false;
            
            // Make Doodler fall off the screen
            Vector2 velocity = doodlerRB.velocity;
            velocity.y = -9.81f; // Apply gravity constant to kinematic Rigidbody
            doodlerRB.velocity = velocity;
        }
    }

}
