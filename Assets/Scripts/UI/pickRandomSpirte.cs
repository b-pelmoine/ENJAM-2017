using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickRandomSpirte : MonoBehaviour {

    public Sprite[] sprites;

    void OnEnable()
    {
        EventManager.StartListening("loading", loading);
    }

    void OnDisable()
    {
        EventManager.StopListening("loading", loading);
    }

    void loading()
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        img.preserveAspect = true;
    }
}
