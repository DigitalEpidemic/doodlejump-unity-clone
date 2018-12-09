using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {
    Player doodler;
    Animator doodlerAnim;

    void Start() {
        doodler = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        doodlerAnim = doodler.GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.relativeVelocity.y <= 0f && doodler.GetIsFlipped() == true) { // If the Doodler is looking left
            doodler.canShoot = false;
            doodler.flipping = true;
            doodler.ResetDoodlerAnimController();
            doodler.GetComponent<BoxCollider2D>().isTrigger = true;
            doodlerAnim.SetTrigger("FlipLeft");
        } else if (collision.relativeVelocity.y <= 0f && doodler.GetIsFlipped() == false) { // If the Doodler is looking right
            doodler.canShoot = false;
            doodler.flipping = true;
            doodler.ResetDoodlerAnimController();
            doodler.GetComponent<BoxCollider2D>().isTrigger = true;
            doodlerAnim.SetTrigger("FlipRight");
        }
    }
}
