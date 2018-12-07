using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] GameObject spinningStars;
    
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 12.5f;
    [SerializeField] GameObject nose;

    [SerializeField] RuntimeAnimatorController normalController;
    [SerializeField] RuntimeAnimatorController shootingController;

    [HideInInspector] public bool enableControls = false;

    float movement = 0f;
    bool isFlipped = false;
    public bool canShoot = true;

    Rigidbody2D rb;
    SpriteRenderer doodler;
    Animator anim;
    //Vector3 playerLocalScale;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //playerLocalScale = transform.localScale;
    }

    public bool GetIsFlipped() {
        return isFlipped;
    }

    public void ShowSpinningStars(bool decision) {
        if (decision) {
            spinningStars.SetActive(decision);
            //print("Setting stars to: " + decision);
        }
    }

    void Update() {
        if (enableControls) {
            movement = Mathf.Lerp(movement, Input.acceleration.x * movementSpeed, Time.deltaTime * 8f);
            //movement = Input.GetAxis("Horizontal") * movementSpeed;

            if (movement >=0.5f) {
                doodler.flipX = true;
                //transform.localScale = new Vector3(playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
                isFlipped = false;

            } else if (movement <= -0.5f) {
                doodler.flipX = false;
                //transform.localScale = new Vector3(-playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
                isFlipped = true;
            }

            if (Input.touchCount > 0) {
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    if (canShoot) {
                        StopAllCoroutines();
                        StartCoroutine(RotateAndShoot());
                    }
                }

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

    IEnumerator RotateAndShoot() {
        nose.SetActive(true);
        anim.runtimeAnimatorController = shootingController;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2));
        difference.Normalize();

        float rotZ = Mathf.Abs(Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg);
        float clampedRotZ = Mathf.Clamp(rotZ - 90, -25, 25);
        nose.transform.rotation = Quaternion.Euler(0f, 0f, clampedRotZ);

        Vector2 touchPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.Euler(0f, 0f, clampedRotZ));

        yield return new WaitForSeconds(1f);

        nose.SetActive(false);
        //doodler.sprite = normalSprite;
        anim.runtimeAnimatorController = normalController;
    }
}
