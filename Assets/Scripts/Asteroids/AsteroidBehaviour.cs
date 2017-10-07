using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    public GameObject asteroidPrefab;
    public float spawnTime = 2.0f;
    public float randForceXMin = -300.0f;
    public float randForceXMax = 300.0f;
    public float randForceYMin = -300.0f;
    public float randForceYMax = 300.0f;

    private float timeCounter = 0.0f;
    private int state = 1;
    private float nextRandForceX;
    private float nextRandForceY;

    private GameManager manager;

    // Use this for initialization
    void Start () {
        transform.localScale = new Vector3(transform.localScale.x / state, transform.localScale.y / state, transform.localScale.z / state);
        Transform tP = GetComponentInChildren<ParticleSystem>().transform;
        tP.localScale = new Vector3(tP.localScale.x / state, tP.localScale.y / state, tP.localScale.z / state);
        GetComponentInChildren<TrailRenderer>().widthMultiplier /= state;
        nextRandForceX = Random.Range(randForceXMin, randForceXMax);
        nextRandForceY = Random.Range(randForceYMin, randForceYMax);
        Impulse();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        timeCounter += Time.deltaTime;
	}

    public void Decrease(int oldState)
    {
        if (state < 3)
            state = oldState + 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlyingAsteroid") && timeCounter > spawnTime)
        {
            if(state < 3 && manager.astreoidCount < manager.asteroidMaxCount)
            {
                GameObject sonOne = GameObject.Instantiate(asteroidPrefab);
                GameObject sonTwo = GameObject.Instantiate(asteroidPrefab);
                sonOne.transform.position = transform.position;
                sonTwo.transform.position = transform.position;
                sonOne.GetComponent<AsteroidBehaviour>().Decrease(state);
                sonTwo.GetComponent<AsteroidBehaviour>().Decrease(state);
                manager.astreoidCount++;

                //Particle system collision
                Destroy(gameObject);
            }
            else
            {
                manager.astreoidCount--;

                //Particle system collision
                Destroy(gameObject);
            }
        }
    }
    
    public void Impulse()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(nextRandForceX, nextRandForceY));
    }
}
