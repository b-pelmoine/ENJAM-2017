﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    //self
    public static GameManager instance;

    [Header("Game Parameters")]
    public float shrinkDuration; //in seconds
    public float asteroidCircleStart; // how far do we start from the center
    public float asteroidCircleEnd; // how close from the center the cirlce ends
    public int astreoidCount;
    public int asteroidMaxCount;
    public int beltAsteroidCount;

    [Header("Players")]
    public float moveSpeed;
    public int maxHP;
    public float distanceFromPlayer;

    [Header("Dash")]
    public float dashDistance;
    public float dashSpeed;
    public float dashCooldown;

    [Header("Shield")]
    public float shieldCooldown;
    public float shieldDuration;

    [Header("Attack")]
    public float laserDuration;
    public float maxWidth;
    public float minWidth;
    public float maxLenght;
    [Tooltip("0 means instant laser")]
    public float laserSpeed;
    public int damagesToEnnemy;
    public float percentageHealthCost;
    public float attackCooldown;
    public float chargeTime;

    [Header("Asteroids")]
    public int damagesOnCollision;
    public float speedMultiplierOnCollision;
    public float speedMultiplierOnSpawn;

    [Header("CirclePop")]
    public float circleDuration = 1.5f;

    [Header("Game manager related variables")]
    public PlayerController[] players;
    public List<GameObject> ShowPlayers;
    private AreaManager areaGenerator;

    public Vector3 getPlayerPosition(int player)
    {
        return players[player].transform.position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            EventManager.TriggerEvent("Dash" + ((Input.GetKey(KeyCode.LeftShift)) ? "J2" : "J1") );
        if (Input.GetKeyDown(KeyCode.Z))
            EventManager.TriggerEvent("Shield" + ((Input.GetKey(KeyCode.LeftShift)) ? "J2" : "J1"));
        if (Input.GetKeyDown(KeyCode.E))
            EventManager.TriggerEvent("Attack" + ((Input.GetKey(KeyCode.LeftShift)) ? "J2" : "J1"));
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        players = FindObjectsOfType<PlayerController>();
        foreach(PlayerController c in players)
        {
            c.enabled = false;
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("start", GameStart);
        EventManager.StartListening("shrinkEnded", OnShrinkEnded);
    }

    void OnDisable()
    {
        EventManager.StopListening("start", GameStart);
        EventManager.StopListening("shrinkEnded", OnShrinkEnded);
    }

    void GameStart()
    {
        EventManager.TriggerEvent("generateArea");
        //spawn players

        //enable splitscreen
        SplitScreen.switchState();
        EventManager.TriggerEvent("splitscreen");
        //show screen and start the game for real
        EventManager.TriggerEvent("gameStart");
        foreach (PlayerController c in players)
        {
            c.enabled = true;
        }
    }


    void OnShrinkEnded()
    {
        for(int i=0; i<players.Length; ++i)
        {
            GameObject go = Instantiate(ShowPlayers[i]);
            go.transform.localScale = Vector3.one * 20;
            go.transform.position = players[i].transform.position;
        }
    }
}
