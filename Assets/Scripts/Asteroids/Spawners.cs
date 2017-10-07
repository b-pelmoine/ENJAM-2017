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

    private float elapsedTime;
    private float nextRandTime;
    private float nextRandForceX;
    private float nextRandForceY;

    private GameManager manager;

	// Use this for initialization
	void Start () {
        elapsedTime = 0.0f;
        nextRandTime = Random.Range(randTimeMin, randTimeMax);
        nextRandForceX = Random.Range(randForceXMin, randForceXMax);
        nextRandForceY = Random.Range(randForceYMin, randForceYMax);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= nextRandTime && manager.astreoidCount < manager.asteroidMaxCount)
        {
            Spawn();
        }
	}

    private void Spawn()
    {
        GameObject spawned;
        spawned = GameObject.Instantiate(toSpawn);
        spawned.transform.position = transform.position;
        spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(nextRandForceX, nextRandForceY));

        elapsedTime = 0.0f;
        nextRandTime = Random.Range(randTimeMin, randTimeMax);
        nextRandForceX = Random.Range(randForceXMin, randForceXMax);
        nextRandForceY = Random.Range(randForceYMin, randForceYMax);
        manager.astreoidCount++;
    }
}
