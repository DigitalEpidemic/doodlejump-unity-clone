using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {
    GameObject doodler;
    Animator doodlerAnim;

    void Start() {
        doodler = GameObject.FindGameObjectWithTag("Player");
        doodlerAnim = doodler.GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y <= 0f && doodler.transform.localScale.x < 0) {
            doodlerAnim.SetTrigger("FlipLeft");
        } else if (collision.relativeVelocity.y <= 0f && doodler.transform.localScale.x > 0) {
            doodlerAnim.SetTrigger("FlipRight");
        }
    }
}
