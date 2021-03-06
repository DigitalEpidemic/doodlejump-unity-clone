﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Cleanup code
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    [SerializeField] float movementSpeed = 10f;

    [SerializeField] GameObject spinningStars;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject nose;

    [SerializeField] RuntimeAnimatorController normalController;
    [SerializeField] RuntimeAnimatorController shootingController;

    [SerializeField] AudioClip shootSound1;
    [SerializeField] AudioClip shootSound2;
    [SerializeField] AudioClip monsterShot;

    [HideInInspector] public bool enableControls = false; // TODO Encapsulate

    float movement = 0f;
    bool isFlipped = false;
    public bool canShoot = true;
    public bool usingPropeller = false;
    public bool flipping = false;

    Rigidbody2D rb;
    SpriteRenderer doodler;
    Animator anim;
    GameController gameController;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (GameObject.Find("GameController") != null) { // Fixes null reference on main menu
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }
    }

    public void TemporarilyMakeKinematic() {
        StartCoroutine(MakeKinematic());
    }

    IEnumerator MakeKinematic() {
        rb.isKinematic = true;
        Vector2 velocity = rb.velocity;
        velocity.y = -9.81f; // Apply gravity constant to kinematic Rigidbody
        rb.velocity = velocity;

        yield return new WaitForSeconds(0.001f);

        rb.isKinematic = false;
    }

    public bool GetIsFlipped() {
        return isFlipped;
    }

    public void ResetDoodlerAnimController() {
        nose.SetActive(false);
        anim.runtimeAnimatorController = normalController;
    }

    public void ResetDoodlerBools() {
        canShoot = true;
        flipping = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void ShowSpinningStars(bool decision) {
        if (decision) {
            spinningStars.SetActive(decision);
        }
    }

    void Update() {
        if (enableControls) {
            movement = Mathf.Lerp(movement, Input.acceleration.x * movementSpeed, Time.deltaTime * 8f); // For mobile
            //movement = Input.GetAxis("Horizontal") * movementSpeed; // For Editor

            if (movement >= 0.5f) {
                doodler.flipX = true;
                isFlipped = false;
            } else if (movement <= -0.5f) {
                doodler.flipX = false;
                isFlipped = true;
            }

            if (Input.touchCount > 0) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    if (!RectTransformUtility.RectangleContainsScreenPoint(gameController.GetPauseRect(), Input.GetTouch(0).position, Camera.main) && canShoot) { // If pause button's area is not touched
                        StopAllCoroutines();
                        StartCoroutine(RotateAndShoot());
                    }
                }
            }

        }
    }

    void FixedUpdate() {
        if (enableControls) { // Move Doodler if controls are enabled
            Vector2 velocity = rb.velocity;
            velocity.x = movement;
            rb.velocity = velocity;
        }

        // Doodler wall to wall teleport
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.45f;
        if (transform.position.x > -topLeft.x + offset) {
            transform.position = new Vector3(topLeft.x - offset, transform.position.y, transform.position.z);
        } else if (transform.position.x < topLeft.x - offset) {
            transform.position = new Vector3(-topLeft.x + offset, transform.position.y, transform.position.z);
        }
    }

    IEnumerator RotateAndShoot() {
        nose.SetActive(true); // Show doodler nose gameobject
        anim.runtimeAnimatorController = shootingController; // Switch to shooting animator controller
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)); // Calculate difference between middle of screen and touch position
        difference.Normalize();

        float rotZ = Mathf.Abs(Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg); // Convert difference to positive rotation in degrees
        float clampedRotZ = Mathf.Clamp(rotZ - 90, -25, 25); // Clamp rotation from -25 degrees to +25 degrees
        nose.transform.rotation = Quaternion.Euler(0f, 0f, clampedRotZ); // Set nose rotation

        // TODO Optimize with object pooling
        Instantiate(projectile, firePoint.position, Quaternion.Euler(0f, 0f, clampedRotZ)); // Instantiate projectile at firePoint position attached to Doodler
        ChooseShootSound(); // Randomly pick shooting sound effects

        yield return new WaitForSeconds(1f);

        nose.SetActive(false); // Hide doodler nose gameobject
        anim.runtimeAnimatorController = normalController; // Switch back to original animator controller
    }

    void ChooseShootSound() {
        int randomNumber = Random.Range(0, 49);

        if (randomNumber % 2 == 0) {
            AudioManager.instance.PlaySoundEffect(shootSound1);
        } else {
            AudioManager.instance.PlaySoundEffect(shootSound2);
        }
    }

}
