﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    [SerializeField] float movementSpeed = 10f;

    BoxCollider2D boxCollider;
    float movement = 0f;

    Rigidbody2D rb;
    Vector3 playerLocalScale;

    public bool enableControls = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerLocalScale = transform.localScale;
    }

    void Update() {
        if (enableControls) {
            movement = Mathf.RoundToInt(Input.acceleration.x * movementSpeed);
            //movement = Input.GetAxis("Horizontal") * movementSpeed;

            if (movement >= 1) {
                transform.localScale = new Vector3(-playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
            } else if (movement <= -1) {
                transform.localScale = new Vector3(playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
            }
        }
    }

    void FixedUpdate() {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        // Player wall to wall teleport
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float offset = 0.5f;

        if (transform.position.x > -topLeft.x + offset) {
            transform.position = new Vector3(topLeft.x - offset, transform.position.y, transform.position.z);
        } else if (transform.position.x < topLeft.x - offset) {
            transform.position = new Vector3(-topLeft.x + offset, transform.position.y, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("RightWall")) {
            Vector3 newLocation = transform.position;
            newLocation.x = 357.1f;
            transform.position = newLocation;

        } else if (collision.CompareTag("LeftWall")) {
            Vector3 newLocation = transform.position;
            newLocation.x = 362.9f;
            transform.position = newLocation;

        } else if (collision.CompareTag("Killer")) {
            Debug.Log("You have died!");
            boxCollider.isTrigger = true;
        }
    }
}