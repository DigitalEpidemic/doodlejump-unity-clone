using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    [SerializeField] float jumpForce = 16.5f;
    [SerializeField] Collider2D[] colliders;
    [SerializeField] float boxX = 2f, boxY = 1f;

    float destroyDistance;
    bool createNewPlatform = false;

    GameObject gameController;

    void Start() {
        gameController = GameObject.Find("GameController");

        // Set distance to destroy the platforms out of screen
        destroyDistance = gameController.GetComponent<GameController>().GetDestroyDistance();
    }

    void DestroyOverlap() {
        // Prevent spawn overlapping (COMMENT OUT BEFORE DEBUGGING WITH LOTS OF PLATFORMS)
        colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(boxX, boxY), 0);

        //DebugDrawBox(transform.position, new Vector2(boxX, boxY), 0, Color.red);

        if (gameObject.name == "Spring(Clone)" && colliders.Length >= 2 && transform.position.y >= Camera.main.transform.position.y + 12f) {
            foreach (Collider2D col in colliders) {
                if (col.name == "BrownPlatform(Clone)") {
                    //print("Destroying brown platform because of spring");
                    Destroy(col.gameObject);
                }
            }
        }

        if (gameObject.name == "Trampoline(Clone)" && colliders.Length >= 2 && transform.position.y >= Camera.main.transform.position.y + 12f) {
            foreach (Collider2D col in colliders) {
                if (col.name == "BrownPlatform(Clone)") {
                    //print("Destroying brown platform because of trampoline");
                    Destroy(col.gameObject);
                }
            }
        }
    }

    void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color) {
        var orientation = Quaternion.Euler(0, 0, angle);

        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomLeft = point - up - right;
        var bottomRight = point - up + right;

        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
    }

    void Update() {
        DestroyOverlap();
    }

    void FixedUpdate() {
        // Platform out of screen
        if (transform.position.y - Camera.main.transform.position.y < destroyDistance) {
            // Create new platform
            if (gameObject.name != "BrownPlatform(Clone)" && gameObject.name != "Spring(Clone)" && gameObject.name != "Trampoline(Clone)" && !createNewPlatform) {
                gameController.GetComponent<PlatformGenerator>().GeneratePlatform(1);
                createNewPlatform = true;
            }

            // Disable Collider and effector
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<PlatformEffector2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Disable collider and effector if gameobject has child
            if (transform.childCount > 0) {
                if (transform.GetChild(0).GetComponent<Platform>()) // If child is platform
                {
                    //transform.GetChild(0).GetComponent<EdgeCollider2D>().enabled = false;
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
        // Add force when player falls from the top
        if (collision.relativeVelocity.y <= 0f) {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();

            if (rb != null) {
                // TODO Add EDITOR if statement
                // Stop the player from dying before loading in Editor
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() != null) {
                    Player doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    Animator doodlerAnim = doodler.GetComponent<Animator>();

                    doodler.enableControls = true;
                    if (gameObject.name != "BrownPlatform(Clone)") {
                        doodlerAnim.SetTrigger("Jump");
                    }

                }

                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;

                // Play jump sound
                //GetComponent<AudioSource>().Play();

                // If current gameObject has an animation (Spring, trampoline, etc.)
                if  (gameObject.GetComponent<Animator>() != null) { // (gameObject.name != "BrownPlatform(Clone)" &&
                    boxX = 0f;
                    boxY = 0f;
                    GetComponent<Animator>().SetTrigger("Active");
                }

            }
        }
    }

}
