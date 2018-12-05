﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] GameObject spinningStars;

    [HideInInspector] public bool enableControls = false;

    float movement = 0f;
    bool isFlipped = false;

    Rigidbody2D rb;
    SpriteRenderer doodler;
    //Vector3 playerLocalScale;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        //playerLocalScale = transform.localScale;
    }

    public bool GetIsFlipped() {
        return isFlipped;
    }

    public void ShowSpinningStars(bool decision) {
        if (decision) {
            spinningStars.SetActive(decision);
            print("Setting stars to: " + decision);
        }
    }

    void Update() {
        if (enableControls) {
            //movement = Mathf.Lerp(movement, Input.acceleration.x * movementSpeed, Time.deltaTime * 8f);
            movement = Input.GetAxis("Horizontal") * movementSpeed;

            if (movement >=0.5f) {
                doodler.flipX = true;
                //transform.localScale = new Vector3(playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
                isFlipped = false;

            } else if (movement <= -0.5f) {
                doodler.flipX = false;
                //transform.localScale = new Vector3(-playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
                isFlipped = true;
            }
        }
    }

    void FixedUpdate() {
        if (enableControls) {
            Vector2 velocity = rb.velocity;
            velocity.x = movement;
            rb.velocity = velocity;
        }

        // Doodler wall to wall teleport
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.5f;

        if (transform.position.x > -topLeft.x + offset) {
            transform.position = new Vector3(topLeft.x - offset, transform.position.y, transform.position.z);
        } else if (transform.position.x < topLeft.x - offset) {
            transform.position = new Vector3(-topLeft.x + offset, transform.position.y, transform.position.z);
        }
    }
}
