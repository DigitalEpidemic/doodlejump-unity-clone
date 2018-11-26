using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField] float jumpForce = 20f;
    [SerializeField] Collider2D[] colliders;
    [SerializeField] float boxX=2f, boxY=1f;

    float destroyDistance;
    bool createNewPlatform = false;

    GameObject gameController;
    
    void Start() {
        gameController = GameObject.Find("GameController");

        // Set distance to destroy the platforms out of screen
        destroyDistance = gameController.GetComponent<GameController>().GetDestroyDistance();
    }

    void FixedUpdate() {

        // Prevent spawn overlapping (COMMENT OUT BEFORE DEBUGGING WITH LOTS OF PLATFORMS)
        colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(boxX, boxY), 0);
        if (colliders.Length > 1 && colliders[0].CompareTag("Platform") && colliders[1].CompareTag("Platform")) {
            Debug.Log("Collided with " + colliders[0].name + " and " + colliders[1].name);
            gameController.GetComponent<PlatformGenerator>().GeneratePlatform(1);
            Destroy(gameObject);
        }

        // Platform out of screen
        if (transform.position.y - Camera.main.transform.position.y < destroyDistance) {
            // Create new platform
            if (name != "BrownPlatform(Clone)" && name != "Spring(Clone)" && name != "Trampoline(Clone)" && !createNewPlatform) {
                gameController.GetComponent<PlatformGenerator>().GeneratePlatform(1);
                createNewPlatform = true;
            }

            // Deactive Collider and effector
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<PlatformEffector2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Deactive collider and effector if gameobject has child
            if (transform.childCount > 0) {
                if (transform.GetChild(0).GetComponent<Platform>()) // if child is platform
                {
                    transform.GetChild(0).GetComponent<EdgeCollider2D>().enabled = false;
                    transform.GetChild(0).GetComponent<PlatformEffector2D>().enabled = false;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                }

                // Destroy this platform if sound has finished
                //if (!GetComponent<AudioSource>().isPlaying && !transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
                Destroy(gameObject);
            } else {
                // Destroy this platform if sound has finished
                //if (!GetComponent<AudioSource>().isPlaying)
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Add force when player fall from top
        if (collision.relativeVelocity.y <= 0f) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            if (rb != null) {
                // TODO Add EDITOR if statement
                // Stop the player from dying before loading in Editor
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() != null) {
                    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    player.enableControls = true;
                }

                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;

                // Play jump sound
                //GetComponent<AudioSource>().Play();

                // if gameobject has animation; Like spring, trampoline and etc...
                //if (GetComponent<Animator>())
                //    GetComponent<Animator>().SetBool("Active", true);

                // Check platform type

            }
        }
    }

}
