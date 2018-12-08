using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float destroyTime = 1f;

    void Start() {
        Destroy(gameObject, destroyTime);
    }

    void Update () {
        transform.Translate(Vector2.up * Time.deltaTime * projectileSpeed);
	}

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Monster")) {
            //print("Hit: " + collision.name);
            Destroy(gameObject);
        }
    }
}
