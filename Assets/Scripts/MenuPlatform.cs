using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlatform : MonoBehaviour {

    [SerializeField] float jumpForce = 16.5f;
    [SerializeField] AudioClip jumpSound;

    void OnCollisionEnter2D(Collision2D collision) {
        // Add force when player falls from the top
        if (collision.relativeVelocity.y <= 0f) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            if (rb != null) {
                // TODO Add EDITOR if statement
                // Stop the player from dying before loading in Editor
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
                AudioManager.instance.PlaySoundEffect(jumpSound);

            }
        }
    }
}
