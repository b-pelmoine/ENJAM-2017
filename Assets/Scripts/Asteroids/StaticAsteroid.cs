using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class StaticAsteroid : MonoBehaviour {
    [SerializeField]
    List<Sprite> sprites;
	void Start () {
        if (sprites.Count == 0) Debug.LogError("No textures found for static asteroids");
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[Random.Range(0, sprites.Count-1)];
        gameObject.AddComponent<PolygonCollider2D>();
    }
}
