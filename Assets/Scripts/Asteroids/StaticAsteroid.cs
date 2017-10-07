using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class StaticAsteroid : MonoBehaviour {

    float rotationSpeed = 0.0f;


    void Awake () {
        List<Sprite> asteroidSprites = AreaManager.instance.asteroidSprites;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Count-1)];
        gameObject.AddComponent<PolygonCollider2D>();
        rotationSpeed = Random.value * 20;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
