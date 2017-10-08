using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOverTime : MonoBehaviour {

    public float rate = 1.0f;

	void Update () {
        transform.Rotate(Vector3.forward, Time.deltaTime * rate);
	}
}
