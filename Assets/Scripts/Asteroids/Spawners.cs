using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour {

    public GameObject toSpawn;
    public float randTimeMin = 2.0f;
    public float randTimeMax = 5.0f;
    public float randForceXMin = -100.0f;
    public float randForceXMax = 100.0f;
    public float randForceYMin = -100.0f;
    public float randForceYMax = 100.0f;

    private float nextRand;
    private float nextRandForceX;
    private float nextRandForceY;

	// Use this for initialization
	void Start () {
        nextRand = Random.Range(randTimeMin, randTimeMax);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Spawn()
    {
        GameObject spawned;
        spawned = GameObject.Instantiate(toSpawn);
        spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(nextRandForceX, nextRandForceY));
    }
}
