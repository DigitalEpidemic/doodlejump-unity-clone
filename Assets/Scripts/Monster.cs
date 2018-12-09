using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField] float jumpForce = 16.5f;
    [SerializeField] AudioClip monsterJumpedOn;
    [SerializeField] AudioClip monsterCrashedInto;
    [SerializeField] AudioClip monsterShot;
    [SerializeField] AudioClip monsterInRange;

    AudioSource audioSource;

    GameController gameController;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void PauseAudio() {
        audioSource.Pause();
    }

    void Update() {
        if (transform.parent.position.y - Camera.main.transform.position.y <= 15f) {
            PlayAudioLoop();
        }

        if (gameController.GetIsPaused() == true) {
            print("Pausing");
            if (audioSource.isPlaying) {
                audioSource.Pause();
            }
        } else if (gameController.GetIsPaused() == false) {
            if (!audioSource.isPlaying) {
                audioSource.UnPause();
            }
        }

        if (gameController.GetGameOver() == true) {
            audioSource.Stop();
        }
    }

    void PlayAudioLoop() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

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
                audioSource.Stop(); // Stop loop
                AudioManager.instance.PlaySoundEffect(monsterJumpedOn);
                // TODO Create audiosource class

                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !collision.gameObject.GetComponent<Player>().usingPropeller) {
            Rigidbody2D doodlerRB = collision.GetComponent<Rigidbody2D>();
            doodlerRB.isKinematic = true; // Remove gravity from Doodler
            Player doodler = collision.GetComponent<Player>();

            // Show spinning stars and disable controls
            doodler.ShowSpinningStars(true);
            doodler.enableControls = false;

            audioSource.Stop(); // Stop loop
            AudioManager.instance.PlaySoundEffect(monsterCrashedInto);

            // Make Doodler fall off the screen
            Vector2 velocity = doodlerRB.velocity;
            velocity.y = -9.81f; // Apply gravity constant to kinematic Rigidbody
            doodlerRB.velocity = velocity;

        } else if (collision.CompareTag("Projectile") || (collision.CompareTag("Player") && collision.gameObject.GetComponent<Player>().usingPropeller)) {
            AudioManager.instance.PlaySoundEffect(monsterShot);
            Destroy(gameObject);
        }
    }

}
