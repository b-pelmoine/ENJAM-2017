using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    public float force = 1.0f;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(force, force), ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
