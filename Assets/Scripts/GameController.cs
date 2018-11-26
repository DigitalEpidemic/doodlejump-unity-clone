using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Vector3 topLeft = Vector3.zero;
    Vector3 cameraPos = Vector3.zero;
    
    void Awake () {
        Vector3 cameraPos = Camera.main.transform.position;

        // Disable unnecessary warning about topLeft not being used
#pragma warning disable 0219
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
#pragma warning restore 0219
    }

    public float GetDestroyDistance() {
        return cameraPos.y + topLeft.y -9.5f;
    }
}
