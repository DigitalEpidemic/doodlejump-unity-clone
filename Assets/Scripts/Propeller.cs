using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour {

    [SerializeField] float flightLength = 2.5f;

    Player doodler;
    Rigidbody2D doodlerRB;

    float timer;
    float animationTimer;
    float destroyDistance;

    bool startTimer = false;
    bool fallOff = false;

    void Start() {
        timer = 0f;
        destroyDistance = GameObject.Find("GameController").GetComponent<GameController>().GetDestroyDistance();
        doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate() {
        if (startTimer) {
            doodlerRB.isKinematic = true;

            Vector2 velocity = doodlerRB.velocity;
            velocity.y = 16.5f;
            doodlerRB.velocity = velocity;

            timer += Time.deltaTime;

            if (timer >= flightLength) {
                startTimer = false;
                fallOff = true;
            }
        }

        if (fallOff && doodlerRB) {
            animationTimer += Time.deltaTime;
            doodlerRB.isKinematic = false;

            // Play propeller animation based on if Doodler sprite is flipped
            if (doodler.GetIsFlipped() == false) {
                GetComponent<Animator>().SetTrigger("FallRight");
            } else if (doodler.GetIsFlipped() == true) {
                GetComponent<Animator>().SetTrigger("FallLeft");
            }

            if (transform.position.y - Camera.main.transform.position.y < destroyDistance) {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.transform.childCount == 0) {
                transform.parent = collision.transform;
                transform.localPosition = new Vector3(0, -0.2f, 0);
                GetComponent<BoxCollider2D>().enabled = false;

                doodlerRB = collision.GetComponent<Rigidbody2D>();
                if (doodlerRB != null && !startTimer) {
                    startTimer = true;

                    doodlerRB.isKinematic = true;

                    // Play sound

                    // Play animation
                    GetComponent<Animator>().SetTrigger("Active");

                    // Bring sprite to the front layer
                    GetComponent<SpriteRenderer>().sortingOrder = 13;
                }
            }
        }
    }

    // Animation Event
    public void UnparentPropeller() {
        //Debug.Log("Unparenting");
        transform.parent = null;
    }

}
