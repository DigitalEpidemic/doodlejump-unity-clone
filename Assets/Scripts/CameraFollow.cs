using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform target;
    [SerializeField] float smoothTime = 1f;
    Vector3 velocity;
	
	void LateUpdate () {
		if (target.position.y > transform.position.y) {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z);
            //Vector3 smoothPos = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
            Vector3 smoothPos = Vector3.Lerp(transform.position, newPos, smoothTime * Time.deltaTime);
            transform.position = smoothPos;
        }
	}
}
