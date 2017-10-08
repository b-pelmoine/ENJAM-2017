using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public GameObject laserObject;

    private GameManager gameManager;
    private float maxWidth;
    private float minWidth;
    private float maxLenght;
    private float speed;
    private int laserDamages;
    private float percentageHealthCost;
    private bool touched;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        maxWidth = gameManager.maxWidth;
        minWidth = gameManager.minWidth;
        maxLenght = gameManager.maxLenght;
        speed = gameManager.laserSpeed;
        laserDamages = gameManager.damagesToEnnemy;
        percentageHealthCost = gameManager.percentageHealthCost;
        touched = false;
	}

    private void OnEnable()
    {
        touched = false;
    }

    public void Fire()
    {
        PlayerController p = GetComponentInParent<PlayerController>();
        Debug.Log(p.health);
        Debug.Log(p.GetHealth());
        //Faire cas speed
        if (speed == 0)
        {
            laserObject.transform.localScale = new Vector3(maxLenght, (maxWidth - minWidth) * p.GetHealth() + minWidth, 1);
            laserObject.SetActive(true);
        }
        else
        {
            // ?????
            laserObject.SetActive(true);
        }
        p.DamagePlayer((int)(p.health * percentageHealthCost / 100.0f));
        Debug.Log(p.health);
        Debug.Log(p.GetHealth());
    }

    public void Stop()
    {
        laserObject.SetActive(false);
    }
}
