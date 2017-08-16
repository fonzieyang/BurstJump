using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform followTarget;
    public Vector3 cameraOffset;
	
    Transform trans;

    void Awake()
    {
        trans = transform;
    }

	void LateUpdate () {
        var pos = followTarget.position + cameraOffset;
        pos.y = cameraOffset.y;
        trans.position = pos;
	}
}
