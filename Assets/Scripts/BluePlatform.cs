using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlatform : MonoBehaviour {

    [SerializeField] float speed = 2.25f;
    bool movingRight = true;
    float offset = 0.85f;

	void FixedUpdate () {
		Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        // Moving right
        if (movingRight) {
            if (transform.position.x < -topLeft.x - offset) {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            } else {
                movingRight = false;
            }

        // Moving left
        } else {
            if (transform.position.x > topLeft.x + offset) {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            } else {
                movingRight = true;
            }
        }
    }
}
