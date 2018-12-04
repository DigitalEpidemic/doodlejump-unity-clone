using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField] float jumpForce = 16.5f;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y <= 0f) {
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

        } else {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            rb.isKinematic = true;
            Vector2 velocity = rb.velocity;
            velocity.y = -9.81f;
            rb.velocity = velocity;
        }
    }
}
